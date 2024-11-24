using System;
using System.Collections.Generic;
using System.Linq;
using Jackal.Core.Domain;
using Jackal.Core.MapGenerator;
using Jackal.Core.Players;
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

    public Board(IPlayer[] players, IMapGenerator mapGenerator, int mapSize, int piratesPerPlayer)
    {
        Generator = mapGenerator;
        MapSize = mapSize;
        Map = new Map(mapSize);
        InitMap();
        InitTeams(players, piratesPerPlayer);
    }

    private void InitTeams(IPlayer[] players, int piratesPerPlayer)
    {
        Teams = new Team[players.Length];
        switch (players.Length)
        {
            case 1:
                InitTeam(0, players[0].GetType().Name, (MapSize - 1) / 2, 0, piratesPerPlayer);
                Teams[0].Enemies = [];
                break;
            case 2:
                InitTeam(0, players[0].GetType().Name, (MapSize - 1) / 2, 0, piratesPerPlayer);
                InitTeam(1, players[1].GetType().Name, (MapSize - 1) / 2, (MapSize - 1), piratesPerPlayer);
                Teams[0].Enemies = [1];
                Teams[1].Enemies = [0];
                break;
            case 4:
                InitTeam(0, players[0].GetType().Name, (MapSize - 1) / 2, 0, piratesPerPlayer);
                InitTeam(1, players[1].GetType().Name, 0, (MapSize - 1) / 2, piratesPerPlayer);
                InitTeam(2, players[2].GetType().Name, (MapSize - 1) / 2, (MapSize - 1), piratesPerPlayer);
                InitTeam(3, players[3].GetType().Name, (MapSize - 1), (MapSize - 1) / 2, piratesPerPlayer);
                Teams[0].Enemies = [1, 2, 3];
                Teams[1].Enemies = [0, 2, 3];
                Teams[2].Enemies = [0, 1, 3];
                Teams[3].Enemies = [0, 1, 2];
                break;
            default:
                throw new NotSupportedException("Only one player, two players or four");
        }
    }

    private void InitMap()
    {
        for (int i = 0; i < MapSize; i++)
        {
            SetWater(i, 0);
            SetWater(0, i);
            SetWater(i, MapSize - 1);
            SetWater(MapSize - 1, i);
        }

        for (int x = 1; x < MapSize - 1; x++)
        {
            for (int y = 1; y < MapSize - 1; y++)
            {
                if ((x==1 || x==MapSize-2) && (y==1||y==MapSize-2) )
                    SetWater(x, y);
                else
                    SetUnknown(x, y);
            }
        }
    }

    private void SetWater(int x, int y)
    {
        var tile = new Tile(new TileParams {Type = TileType.Water, Position = new Position(x, y)});
        Map[x, y] = tile;
    }

    private void SetUnknown(int x, int y)
    {
        var tile = new Tile(new TileParams {Type = TileType.Unknown, Position = new Position(x, y)});
        Map[x, y] = tile;
    }

    private void InitTeam(int teamId, string teamName, int x, int y, int piratesPerPlayer)
    {
        var startPosition = new Position(x, y);
        var pirates = new Pirate[piratesPerPlayer];
        for (int i = 0; i < pirates.Length; i++)
        {
            pirates[i] = new Pirate(teamId, new TilePosition(startPosition), PirateType.Usual);
        }
        var ship = new Ship(teamId, startPosition);
        foreach (var pirate in pirates)
        {
            Map[ship.Position].Pirates.Add(pirate);
        }
        Teams[teamId] = new Team(teamId, teamName, ship, pirates);
    }

    /// <summary>
    /// Возвращаем список всех полей, в которые можно попасть из исходного поля
    /// </summary>
    public List<AvailableMove> GetAllAvailableMoves(
        AvailableMovesTask task, 
        TilePosition source, 
        TilePosition prev, 
        SubTurnState subTurn)
    {
        var sourceTile = Map[source.Position];

        var ourTeamId = task.TeamId;
        var ourTeam = Teams[ourTeamId];
        var ourShip = ourTeam.Ship;

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
            source, prev, ourTeam, subTurn
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
                    if (ourShip.Position == newPosition.Position)
                    {
                        // заходим на свой корабль
                        goodTargets.Add(usualMove);
                        
                        if (Map[task.Source].Coins > 0)
                            goodTargets.Add(coinMove);
                    }
                    else if (sourceTile.Type == TileType.Water)
                    {
                        if (source.Position != ourShip.Position &&
                            GetPossibleSwimming(task.Source.Position).Contains(newPosition.Position))
                        {
                            // пират плавает
                            goodTargets.Add(usualMove);
                        }

                        if (source.Position == ourShip.Position &&
                            GetPossibleShipMoves(task.Source.Position, MapSize).Contains(newPosition.Position))
                        {
                            // корабль плавает
                            goodTargets.Add(usualMove);
                        }
                    }
                    else if (sourceTile.Type is TileType.Arrow or TileType.Cannon or TileType.Ice)
                    {
                        // ныряем с земли в воду
                        goodTargets.Add(usualMove);

                        if (Map[task.Source].Coins > 0)
                            goodTargets.Add(coinMove);
                    }
                    break;

                case TileType.Fort:
                case TileType.RespawnFort:
                    if (newPositionTile.HasNoEnemy(ourTeamId))
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
                            subTurn
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
                        newPositionTileLevel.HasNoEnemy(ourTeamId))
                    {
                        goodTargets.Add(coinMove);
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
        SubTurnState subTurn)
    {
        var sourceTile = Map[source.Position];
        var ourShip = ourTeam.Ship;

        IEnumerable<TilePosition> rez = GetNearDeltas(source.Position)
            .Where(IsValidMapPosition)
            .Where(x => Map[x].Type != TileType.Water || x == ourShip.Position)
            .Select(IncomeTilePosition);
            
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
                        .Where(x => x.Position != source.Position && x.HasNoEnemy(ourTeam.Id))
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
                        Map[x].Type != TileType.Water || Teams.Select(t => t.Ship.Position).Contains(x)
                    )
                    .Select(IncomeTilePosition);
                break;
            case TileType.Arrow:
                rez = GetArrowsDeltas(sourceTile.ArrowsCode, source.Position)
                    .Select(IncomeTilePosition);
                break;
            case TileType.Airplane:
                if (sourceTile.Used == false)
                {
                    rez = GetAirplaneMoves(ourShip);
                }
                break;
            case TileType.Crocodile:
                if (subTurn.AirplaneFlying)
                {
                    rez = GetAirplaneMoves(ourShip);
                    break;
                }
                    
                rez = new[] { IncomeTilePosition(prev.Position) };
                break;
            case TileType.Ice:
                if (subTurn.AirplaneFlying)
                {
                    rez = GetAirplaneMoves(ourShip);
                    break;
                }
                
                var prevDelta = Position.GetDelta(prev.Position, source.Position);
                var target = Position.AddDelta(source.Position, prevDelta);
                rez = new[] { IncomeTilePosition(target) };
                break;
            case TileType.Spinning:
                if (source.Level > 0)
                {
                    rez = new[] {new TilePosition(source.Position, source.Level - 1)};
                }
                break;
            case TileType.Water:
                if (source.Position == ourShip.Position)
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
    
    private IEnumerable<TilePosition> GetAirplaneMoves(Ship ourShip) =>
        AllTiles(x =>
                x.Type != TileType.Ice &&
                x.Type != TileType.Crocodile &&
                x.Type != TileType.Horse &&
                (x.Type != TileType.Water || x.Position == ourShip.Position)
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
}