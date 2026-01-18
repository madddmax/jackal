using System;
using System.Collections.Generic;
using System.Linq;
using Jackal.Core.Domain;
using Jackal.Core.MapGenerator;
using Newtonsoft.Json;

namespace Jackal.Core;

public class Board
{
    [JsonIgnore]
    public readonly IMapGenerator Generator;
        
    /// <summary>
    /// Размер стороны карты с учетом воды
    /// </summary>
    public readonly int MapSize;
        
    public readonly Map Map;

    public Team[] Teams;

    [JsonIgnore]
    public List<Pirate> AllPirates
    {
        get
        {
            var allPirates = new List<Pirate>();
            foreach (var teamPirates in Teams.Select(item => item.Pirates.ToList<Pirate>()))
            {
                allPirates.AddRange(teamPirates);
            }

            return allPirates;
        }
    }
        
    [JsonIgnore]
    public List<Pirate>? DeadPirates { get; set; }

    public IEnumerable<Tile> AllTiles(Predicate<Tile> selector)
    {
        for (int i = 0; i < MapSize; i++)
        {
            for (int j = 0; j < MapSize; j++)
            {
                var tile = Map[i, j];
                if (selector(tile))
                    yield return tile;
            }
        }
    }

    [JsonConstructor]
    public Board(int mapSize, Map map, Team[] teams)
    {
        MapSize = mapSize;
        Map = map;
        Teams = teams;
    }

    public Board(GameRequest request)
    {
        Generator = request.MapGenerator;
        MapSize = request.MapSize;
        Teams = TeamsFactory.Create(request);
        Map = MapFactory.Create(MapSize, Teams);
    }

    /// <summary>
    /// Возвращаем список всех полей, в которые можно попасть из исходного поля
    /// </summary>
    public List<AvailableMove> GetAllAvailableMoves(
        AvailableMovesTask task, 
        TilePosition source, 
        TilePosition prev, 
        SubTurnState subTurn,
        Position[] allyShips)
    {
        var sourceTile = Map[source.Position];

        var ourTeamId = task.TeamId;
        var ourTeam = Teams[ourTeamId];

        if (sourceTile.Type is TileType.Arrow or TileType.Horse or TileType.Ice or TileType.Crocodile)
        {
            var prevMoveDelta = sourceTile.Type == TileType.Ice
                ? Position.GetDelta(prev.Position, source.Position)
                : null;

            // запоминаем, что в текущую клетку уже не надо возвращаться
            task.AlreadyCheckedList.Add(new CheckedPosition(source, prevMoveDelta));
        }

        var goodTargets = new List<AvailableMove>();
            
        // доступно воскрешение
        if (sourceTile.Type == TileType.RespawnFort && 
            sourceTile.Pirates.Any(p => p.Type == PirateType.Usual) &&
            ourTeam.Pirates.Count(p => p.Type == PirateType.Usual) < 3)
        {
            var respawnMove = AvailableMoveFactory.RespawnMove(task.Source, source);
            goodTargets.Add(respawnMove);
            task.AlreadyCheckedList.Add(new CheckedPosition(source));
        }
        
        // места всех возможных ходов
        IEnumerable<TilePosition> positionsForCheck = GetAllTargetsForSubTurn(
            source, prev, ourTeam, subTurn, allyShips
        );

        foreach (var newPosition in positionsForCheck)
        {
            if (task.AlreadyCheckedList.Count > 0)
            {
                var incomeDelta = Position.GetDelta(source.Position, newPosition.Position);
                var currentCheck = new CheckedPosition(newPosition, incomeDelta);

                if (WasCheckedBefore(task.AlreadyCheckedList, currentCheck))
                {
                    // попали по рекурсии в ранее просмотренную клетку
                    continue;
                }
            }
            
            if (subTurn.QuakePhase > 0)
            {
                // разыгрываем ход разлома
                var quakeMove = AvailableMoveFactory.QuakeMove(task.Source, newPosition, prev);
                goodTargets.Add(quakeMove);
                continue;
            }
            
            var usualMove = AvailableMoveFactory.UsualMove(task.Source, newPosition, source);
            var coinMove = AvailableMoveFactory.CoinMove(task.Source, newPosition, source);
            var bigCoinMove = AvailableMoveFactory.BigCoinMove(task.Source, newPosition, source);
            
            // проверяем, что на этой клетке
            var newPositionTile = Map[newPosition.Position];
                
            switch (newPositionTile.Type)
            {
                case TileType.Unknown:
                    usualMove.Prev = !subTurn.AirplaneFlying ? source.Position : null;
                    usualMove.MoveType = subTurn.LighthouseViewCount > 0
                        ? MoveType.WithLighthouse
                        : MoveType.Usual;
                    
                    goodTargets.Add(usualMove);
                    break;

                case TileType.Water:
                    if (allyShips.Contains(newPosition.Position))
                    {
                        // заходим на свой корабль
                        goodTargets.Add(usualMove);
                        
                        if (Map[task.Source].Coins > 0)
                            goodTargets.Add(coinMove);
                        
                        if (Map[task.Source].BigCoins > 0)
                            goodTargets.Add(bigCoinMove);
                    }
                    else if (sourceTile.Type == TileType.Water)
                    {
                        // пират плавает на корабле или брасом
                        goodTargets.Add(usualMove);
                    }
                    else if (sourceTile.Type is TileType.Arrow or TileType.Cannon or TileType.Ice)
                    {
                        // ныряем с земли в воду
                        goodTargets.Add(usualMove);

                        if (Map[task.Source].Coins > 0)
                            goodTargets.Add(coinMove);
                        
                        if (Map[task.Source].BigCoins > 0)
                            goodTargets.Add(bigCoinMove);
                    }

                    break;

                case TileType.Fort:
                case TileType.RespawnFort:
                    if (newPositionTile.HasNoEnemy(ourTeam.EnemyTeamIds))
                    {
                        // форт не занят противником
                        goodTargets.Add(usualMove);
                    }
                    break;
                    
                case TileType.Ice:
                case TileType.Horse:
                case TileType.Arrow:
                case TileType.Crocodile:
                    goodTargets.AddRange(
                        GetAllAvailableMoves(
                            task,
                            newPosition,
                            source,
                            subTurn,
                            allyShips
                        )
                    );
                    break;
                    
                case TileType.Jungle:
                    goodTargets.Add(usualMove);
                    break;
                
                default:
                    goodTargets.Add(usualMove);

                    var newPositionTileLevel = Map[newPosition];
                    if (Map[task.Source].Coins > 0 && 
                        newPositionTileLevel.HasNoEnemy(ourTeam.EnemyTeamIds))
                    {
                        goodTargets.Add(coinMove);
                    }
                    
                    if (Map[task.Source].BigCoins > 0 && 
                        newPositionTileLevel.HasNoEnemy(ourTeam.EnemyTeamIds))
                    {
                        goodTargets.Add(bigCoinMove);
                    }
                    break;
            }
        }
            
        return goodTargets;
    }

    /// <summary>
    /// Возвращаем все позиции, в которые в принципе достижимы из заданной клетки за один подход
    /// (не проверяется, допустим ли такой ход)
    /// </summary>
    private List<TilePosition> GetAllTargetsForSubTurn(
        TilePosition source, 
        TilePosition prev, 
        Team ourTeam, 
        SubTurnState subTurn,
        Position[] allyShips)
    {
        var rez = GetNearDeltas(source.Position)
            .Where(IsValidMapPosition)
            .Where(x =>
                Map[x].Type != TileType.Water ||
                allyShips.Contains(x)
            )
            .Select(IncomeTilePosition);
            
        var sourceTile = Map[source.Position];
        switch (sourceTile.Type)
        {
            case TileType.Hole:
                var holeTiles = AllTiles(x => x.Type == TileType.Hole).ToList();
                if (holeTiles.Count == 1)
                {
                    rez = new List<TilePosition>();
                }
                else if (subTurn.FallingInTheHole)
                {
                    var freeHoleTiles = holeTiles
                        .Where(x => x.Position != source.Position && x.HasNoEnemy(ourTeam.EnemyTeamIds))
                        .ToList();

                    if (freeHoleTiles.Count > 0)
                    {
                        rez = freeHoleTiles.Select(x => IncomeTilePosition(x.Position));
                    }
                }
                break;
            case TileType.Horse:
                rez = GetHorseDeltas(source.Position)
                    .Where(IsValidMapPosition)
                    .Where(x =>
                        Map[x].Type != TileType.Water || Teams.Select(t => t.ShipPosition).Contains(x)
                    )
                    .Select(IncomeTilePosition);
                break;
            case TileType.Arrow:
                rez = GetArrowsDeltas(sourceTile.Code, source.Position)
                    .Select(IncomeTilePosition);
                break;
            case TileType.Airplane:
                if (sourceTile.Used == false)
                {
                    rez = GetAirplaneMoves(allyShips);
                }
                break;
            case TileType.Crocodile:
                if (subTurn.AirplaneFlying)
                {
                    rez = GetAirplaneMoves(allyShips);
                    break;
                }
                    
                rez = new[] { IncomeTilePosition(prev.Position) };
                break;
            case TileType.Ice:
                if (subTurn.AirplaneFlying)
                {
                    rez = GetAirplaneMoves(allyShips);
                    break;
                }
                
                var prevDelta = Position.GetDelta(prev.Position, source.Position);
                var target = Position.AddDelta(source.Position, prevDelta);
                rez = new[] { IncomeTilePosition(target) };
                break;
            case TileType.Spinning:
                if (source.Level > 0 && !subTurn.DrinkRumBottle)
                {
                    rez = new[] { new TilePosition(source.Position, source.Level - 1) };
                }
                break;
            case TileType.Water:
                if (allyShips.Contains(source.Position))
                {
                    // со своего корабля
                    rez = GetPossibleShipMoves(source.Position, MapSize)
                        .Concat(new[] {GetShipLanding(source.Position)})
                        .Select(IncomeTilePosition);
                }
                else
                {
                    // пират плавает в воде
                    rez = GetPossibleSwimming(source.Position)
                        .Select(IncomeTilePosition);
                }
                break;
        }

        if (subTurn.LighthouseViewCount > 0)
        {
            // просмотр карты с маяка
            rez = AllTiles(x => x.Type == TileType.Unknown)
                .Select(x => IncomeTilePosition(x.Position));
        }

        if (subTurn.QuakePhase > 0)
        {
            // перемещение клеток землетрясением
            rez = AllTiles(x =>
                    x.Coins == 0 &&
                    x.BigCoins == 0 &&
                    x.Type != TileType.Water &&
                    !x.Levels.SelectMany(l => l.Pirates).Any() &&
                    (x.Position != prev.Position || subTurn.QuakePhase == 2)
                )
                .Select(x => IncomeTilePosition(x.Position));
        }
            
        return rez.Where(x => IsValidMapPosition(x.Position)).ToList();
    }

    public void ShowUnknownTiles()
    {
        var unknownTiles = AllTiles(x => x.Type == TileType.Unknown);
        foreach (var tile in unknownTiles)
        {
            OpenTile(tile.Position);
        }
    }

    public Tile OpenTile(Position position)
    {
        var tile = Generator.GetNext(position);
        Map[position] = tile;

        return tile;
    }

    private IEnumerable<TilePosition> GetAirplaneMoves(Position[] allyShips) =>
        AllTiles(x =>
                x.Type != TileType.Ice &&
                x.Type != TileType.Crocodile &&
                x.Type != TileType.Horse &&
                (
                    x.Type != TileType.Water ||
                    allyShips.Contains(x.Position)
                )
            )
            .Select(x => IncomeTilePosition(x.Position));

    private TilePosition IncomeTilePosition(Position pos)
    {
        return IsValidMapPosition(pos) && Map[pos].Type == TileType.Spinning
            ? new TilePosition(pos, Map[pos].SpinningCount - 1)
            : new TilePosition(pos);
    }

    private static IEnumerable<Position> GetHorseDeltas(Position pos)
    {
        for (int x = -2; x <= 2; x++)
        {
            if (x == 0) continue;
            int deltaY = (Math.Abs(x) == 2) ? 1 : 2;
            yield return new Position(pos.X + x, pos.Y - deltaY);
            yield return new Position(pos.X + x, pos.Y + deltaY);
        }
    }

    private static IEnumerable<Position> GetNearDeltas(Position pos)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                yield return new Position(pos.X + x, pos.Y + y);
            }
        }
    }

    private bool IsValidMapPosition(Position pos)
    {
        return (
            pos.X >= 0 && pos.X < MapSize
                       && pos.Y >= 0 && pos.Y < MapSize //попадаем в карту
                       && Utils.InCorners(pos, 0, MapSize - 1) == false //не попадаем в углы карты
        );
    }

    public static IEnumerable<Position> GetPossibleShipMoves(Position shipPosition, int mapSize)
    {
        if (shipPosition.X == 0 || shipPosition.X == mapSize - 1)
        {
            if (shipPosition.Y > 2)
                yield return new Position(shipPosition.X, shipPosition.Y - 1);
            if (shipPosition.Y < mapSize - 3)
                yield return new Position(shipPosition.X, shipPosition.Y + 1);
        }
        else if (shipPosition.Y == 0 || shipPosition.Y == mapSize - 1)
        {
            if (shipPosition.X > 2)
                yield return new Position(shipPosition.X - 1, shipPosition.Y);
            if (shipPosition.X < mapSize - 3)
                yield return new Position(shipPosition.X + 1, shipPosition.Y);
        }
        else
        {
            throw new Exception("Wrong ship position");
        }
    }

    private Position GetShipLanding(Position pos)
    {
        if (pos.X == 0)
            return new Position(1, pos.Y);

        if (pos.X == MapSize - 1)
            return new Position(MapSize - 2, pos.Y);

        if (pos.Y == 0)
            return new Position(pos.X, 1);

        if (pos.Y == MapSize - 1)
            return new Position(pos.X, MapSize - 2);

        throw new Exception("Wrong ship position");
    }

    private static IEnumerable<Position> GetArrowsDeltas(int arrowsCode, Position source)
    {
        foreach (var delta in ArrowsCodesHelper.GetExitDeltas(arrowsCode))
        {
            yield return new Position(source.X + delta.X, source.Y + delta.Y);
        }
    }

    /// <summary>
    /// Все возможные цели для плавающего пирата
    /// </summary>
    private IEnumerable<Position> GetPossibleSwimming(Position pos)
    {
        return GetNearDeltas(pos).Where(IsValidMapPosition).Where(x => Map[x].Type == TileType.Water);
    }

    private static bool WasCheckedBefore(List<CheckedPosition> alreadyCheckedList, CheckedPosition currentCheck)
    {
        foreach (var info in alreadyCheckedList)
        {
            if (info.Position == currentCheck.Position)
            {
                if (info.IncomeDelta == null || info.IncomeDelta == currentCheck.IncomeDelta) 
                    return true;
            }
        }
            
        return false;
    }
    
    public static int Distance(Position pos1, Position pos2)
    {
        int deltaX = Math.Abs(pos1.X - pos2.X);
        int deltaY = Math.Abs(pos1.Y - pos2.Y);
        return Math.Max(deltaX, deltaY);
    }
}