using System;
using System.Collections.Generic;
using System.Linq;
using Jackal.Core.Actions;
using Newtonsoft.Json;

namespace Jackal.Core
{
    public class Board
    {
        /// <summary>
        /// Размер стороны поля с учетом воды, min = 5 max = 13
        /// </summary>
        public readonly int Size;

        [JsonIgnore]
        internal MapGenerator Generator;

        [JsonIgnore]
        public int MapId
        {
            get { return Generator.MapId; }
        }

        public Map Map;

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

        public IEnumerable<Tile> AllTiles(Predicate<Tile> selector)
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    var tile = Map[i, j];
                    if (selector(tile))
                        yield return tile;
                }
            }
        }

        /// <summary>
        /// Используется для десериализации
        /// </summary>
        public Board()
        {
        }

        public Board(IPlayer[] players, int mapId, int mapSize)
        {
            if (mapSize is < 5 or > 13)
                throw new ArgumentException("mapSize is >= 5 and <= 13");

            Size = mapSize;
            Generator = new MapGenerator(mapId, mapSize);
            Map = new Map(mapSize);
            InitMap();
            InitTeams(players);
        }

        private void InitTeams(IPlayer[] players)
        {
            Teams = new Team[players.Length];
            switch (players.Length)
            {
                case 1:
                    InitTeam(0, players[0].GetType().Name, (Size - 1) / 2, 0);
                    Teams[0].Enemies = [];
                    break;
                case 2:
                    InitTeam(0, players[0].GetType().Name, (Size - 1) / 2, 0);
                    InitTeam(1, players[1].GetType().Name, (Size - 1) / 2, (Size - 1));
                    Teams[0].Enemies = [1];
                    Teams[1].Enemies = [0];
                    break;
                case 4:
                    InitTeam(0, players[0].GetType().Name, (Size - 1) / 2, 0);
                    InitTeam(1, players[1].GetType().Name, 0, (Size - 1) / 2);
                    InitTeam(2, players[2].GetType().Name, (Size - 1) / 2, (Size - 1));
                    InitTeam(3, players[3].GetType().Name, (Size - 1), (Size - 1) / 2);
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
            for (int i = 0; i < Size; i++)
            {
                SetWater(i, 0);
                SetWater(0, i);
                SetWater(i, Size - 1);
                SetWater(Size - 1, i);
            }

            for (int x = 1; x < Size - 1; x++)
            {
                for (int y = 1; y < Size - 1; y++)
                {
                    if ((x==1 || x==Size-2) && (y==1||y==Size-2) )
                        SetWater(x, y);
                    else
                        SetUnknown(x, y);
                }
            }
        }

        void SetWater(int x, int y)
        {
            var tile = new Tile(new TileParams {Type = TileType.Water, Position = new Position(x, y)});
            Map[x, y] = tile;
        }

        private void SetUnknown(int x, int y)
        {
            var tile = new Tile(new TileParams {Type = TileType.Unknown, Position = new Position(x, y)});
            Map[x, y] = tile;
        }

        private void InitTeam(int teamId, string name, int x, int y)
        {
            var startPosition = new Position(x, y);
            var pirates = new Pirate[3];
            for (int i = 0; i < pirates.Length; i++)
            {
                pirates[i] = new Pirate(teamId, new TilePosition( startPosition));
            }
            var ship = new Ship(teamId, startPosition);
            foreach (var pirate in pirates)
            {
                Map[ship.Position].Pirates.Add(pirate);
            }
            Teams[teamId] = new Team(teamId, name, ship, pirates);
        }
        
        public List<AvaliableMove> GetAllAvaliableMoves(GetAllAvaliableMovesTask task)
        {
            return GetAllAvaliableMoves(task, task.FirstSource, task.PreviosSource);
        }

        /// <summary>
        /// Возвращаем список всех полей, в которые можно попасть из исходного поля
        /// </summary>
        /// <param name="task"></param>
        /// <param name="source"></param>
        /// <param name="previos"></param>
        /// <returns></returns>
        public List<AvaliableMove> GetAllAvaliableMoves(GetAllAvaliableMovesTask task, TilePosition source, TilePosition previos)
        {
            Direction previosDirection;
            if (previos != null)
                previosDirection = new Direction(previos, source);
            else
                previosDirection = new Direction(source, source);

            var sourceTile = Map[source.Position];

            var ourTeamId = task.TeamId;
            var ourTeam = Teams[ourTeamId];
            var ourShip = ourTeam.Ship;

            List<AvaliableMove> goodTargets = new List<AvaliableMove>();

            IEnumerable<TilePosition> positionsForCheck;

            if (sourceTile.Type.RequreImmediateMove()) //для клеток с редиректами запоминаем, что в текущую клетку уже не надо возвращаться
            {
                Position previosMoveDelta = null;
                if (sourceTile.Type == TileType.Ice)
                    previosMoveDelta = previosDirection.GetDelta();
                task.alreadyCheckedList.Add(new CheckedPosition(source, previosMoveDelta)); //запоминаем, что эту клетку просматривать уже не надо
            }

            //места всех возможных ходов
            positionsForCheck = GetAllTargetsForSubturn(source, previosDirection, ourTeam);

            foreach (var newPosition in positionsForCheck)
            {
                //проверяем, что на этой клетке
                var newPositionTile = Map[newPosition.Position];

                if (task.alreadyCheckedList.Count > 0 && previosDirection != null)
                {
                    Position incomeDelta = Position.GetDelta(previosDirection.To.Position, newPosition.Position);
                    CheckedPosition currentCheck = new CheckedPosition(newPosition, incomeDelta);

                    if (WasCheckedBefore(task.alreadyCheckedList, currentCheck)) //мы попали по рекурсии в ранее просмотренную клетку
                    {
                        if (newPositionTile.Type == TileType.Airplane && Map.AirplaneUsed == false) { 
                            // даем возможность не использовать самолет сразу!
                            goodTargets.Add(new AvaliableMove(task.FirstSource, newPosition, new Moving(task.FirstSource, newPosition)));
                        }
                        continue;
                    }
                }

                switch (newPositionTile.Type)
                {
                    case TileType.Water:
                        if (ourShip.Position == newPosition.Position) //заходим на свой корабль
                        {
                            goodTargets.Add(new AvaliableMove(task.FirstSource, newPosition, new Moving(task.FirstSource, newPosition))); //всегда это можем сделать
                            if (task.NoCoinMoving == false && Map[task.FirstSource].Coins > 0)
                                goodTargets.Add(new AvaliableMove(task.FirstSource, newPosition, new Moving(task.FirstSource, newPosition, true))
                                {
                                    MoveType = MoveType.WithCoin
                                });
                        }
                        else if (sourceTile.Type == TileType.Water) //из воды в воду 
                        {
                            if (source.Position != ourShip.Position && GetPosibleSwimming(task.FirstSource.Position).Contains(newPosition.Position))
                            {
                                //пират плавает
                                var action = new Moving(task.FirstSource, newPosition);
                                var move = new AvaliableMove(task.FirstSource, newPosition, action);
                                goodTargets.Add(move);
                            }
                            if (source.Position == ourShip.Position && GetShipPosibleNavaigations(task.FirstSource.Position).Contains(newPosition.Position))
                            {
                                //корабль плавает
                                var action = new Moving(task.FirstSource, newPosition);
                                var move = new AvaliableMove(task.FirstSource, newPosition, action);
                                goodTargets.Add(move);
                            }
                        }
                        else //с земли в воду мы можем попасть только если ранее попали на клетку, требующую действия
                        {
                            if (task.NoJumpToWater == false && sourceTile.Type.RequreImmediateMove())
                            {
                                goodTargets.Add(new AvaliableMove(task.FirstSource, newPosition, new Moving(task.FirstSource, newPosition)));
                                
                                if (task.NoCoinMoving == false && Map[task.FirstSource].Coins > 0)
                                    goodTargets.Add(new AvaliableMove(task.FirstSource, newPosition, new Moving(task.FirstSource, newPosition))
                                    {
                                        MoveType = MoveType.WithCoin
                                    });
                            }
                        }
                        break;
                    case TileType.RespawnFort:
                        if (task.FirstSource == newPosition)
                        {
                            if (task.NoRespawn==false && ourTeam.Pirates.Count() < 3)
                                goodTargets.Add(new AvaliableMove(task.FirstSource, newPosition, new Moving(task.FirstSource, newPosition), new Respawn(ourTeam, newPosition.Position))
                                {
                                    MoveType = MoveType.WithRespawn
                                });
                        }
                        else if (newPositionTile.OccupationTeamId.HasValue == false || newPositionTile.OccupationTeamId == ourTeamId) //только если форт не занят
                            goodTargets.Add(new AvaliableMove(task.FirstSource, newPosition, new Moving(task.FirstSource, newPosition)));
                        break;
                    case TileType.Fort:
                        if (task.NoFort == false && newPositionTile.OccupationTeamId.HasValue == false || newPositionTile.OccupationTeamId == ourTeamId) //только если форт не занят
                            goodTargets.Add(new AvaliableMove(task.FirstSource, newPosition, new Moving(task.FirstSource, newPosition)));
                        break;

                    case TileType.Canibal:
                        if (task.NoCanibal==false)
                            goodTargets.Add(new AvaliableMove(task.FirstSource, newPosition, new Moving(task.FirstSource, newPosition)));
                        break;


                    case TileType.Trap:
                        if (task.NoTrap == false)
                        {
                            goodTargets.Add(new AvaliableMove(task.FirstSource, newPosition, new Moving(task.FirstSource, newPosition)));
                            if (task.NoCoinMoving == false && Map[task.FirstSource].Coins > 0
                                && (newPositionTile.OccupationTeamId == null || newPositionTile.OccupationTeamId == ourTeamId))
                                goodTargets.Add(new AvaliableMove(task.FirstSource, newPosition, new Moving(task.FirstSource, newPosition, true))
                                {
                                    MoveType = MoveType.WithCoin
                                });
                        }
                        break;

                    case TileType.Grass:
                    case TileType.Chest1:
                    case TileType.Chest2:
                    case TileType.Chest3:
                    case TileType.Chest4:
                    case TileType.Chest5:
                    case TileType.RumBarrel:
                    case TileType.Spinning:
                        goodTargets.Add(new AvaliableMove(task.FirstSource, newPosition, new Moving(task.FirstSource, newPosition)));
                        if (task.NoCoinMoving==false && Map[task.FirstSource].Coins > 0
                            && (newPositionTile.OccupationTeamId == null || newPositionTile.OccupationTeamId == ourTeamId))
                            goodTargets.Add(new AvaliableMove(task.FirstSource, newPosition, new Moving(task.FirstSource, newPosition, true))
                            {
                                MoveType = MoveType.WithCoin
                            });
                        break;
                    case TileType.Unknown:
                        goodTargets.Add(new AvaliableMove(task.FirstSource, newPosition, new Moving(task.FirstSource, newPosition)));
                        break;
                    case TileType.Horse:
                    case TileType.Arrow:
                    case TileType.Balloon:
                    case TileType.Ice:
                    case TileType.Croc:
					case TileType.Cannon:
                        goodTargets.AddRange(GetAllAvaliableMoves(task, newPosition, source));
                        break;
                    case TileType.Airplane:
                        if (Map.AirplaneUsed == false)
                        {
                            goodTargets.AddRange(GetAllAvaliableMoves(task, newPosition, source));
                        }
                        else {
                            // если нет самолета, то клетка работает как пустое поле
                            goodTargets.Add(new AvaliableMove(task.FirstSource, newPosition, new Moving(task.FirstSource, newPosition)));
                            if (task.NoCoinMoving == false && Map[task.FirstSource].Coins > 0
                                && (newPositionTile.OccupationTeamId == null || newPositionTile.OccupationTeamId == ourTeamId))
                                goodTargets.Add(new AvaliableMove(task.FirstSource, newPosition, new Moving(task.FirstSource, newPosition, true))
                                {
                                    MoveType = MoveType.WithCoin
                                });
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
        /// <param name="source"></param>
        /// <param name="previosMove"></param>
        /// <returns></returns>
        public List<TilePosition> GetAllTargetsForSubturn(TilePosition source, Direction previosMove, Team ourTeam)
        {
            var sourceTile = Map[source.Position];
            var ourShip = ourTeam.Ship;

            IEnumerable<TilePosition> rez;
            switch (sourceTile.Type)
            {
                case TileType.Horse:
                    rez = GetHorseDeltas(source.Position)
                        .Select(x => IncomeTilePosition(x));
                    break;
				case TileType.Cannon:
					rez = new []{IncomeTilePosition(GetCannonFly(sourceTile.CannonDirection, source.Position))};
					break;
                case TileType.Arrow:
                    rez = GetArrowsDeltas(sourceTile.ArrowsCode, source.Position)
                        .Select(x => IncomeTilePosition(x));
                    break;
                case TileType.Balloon:
                    rez = Teams.Select(x => x.Ship.Position)
                        .Select(x => IncomeTilePosition(x)); //на корабль
                    break;
                case TileType.Airplane:
                    if (Map.AirplaneUsed == false)
                    {
                        var shipTargets = Teams.Select(x => x.Ship.Position)
                            .Select(x => IncomeTilePosition(x)); //на корабль
                        var airplaneTargets = AllTiles(x => x.Type != TileType.Water
                                                            && x.Type.RequreImmediateMove() == false
                                                            && x.Type != TileType.Airplane)
                            .Select(x => x.Position)
                            .Select(x => IncomeTilePosition(x));
                        rez = shipTargets.Concat(airplaneTargets);
                        if (previosMove.From != source)
                            rez = rez.Concat(new []{source}); //ход "остаемся на месте"
                    }
                    else
                    {
                        rez = GetNearDeltas(source.Position)
                            .Where(x => IsValidMapPosition(x))
                            .Where(x => Map[x].Type != TileType.Water || x == ourShip.Position)
                            .Select(x => IncomeTilePosition(x));
                    }
                    break;
                case TileType.Croc:
                    rez = new[] {previosMove.From}; //возвращаемся назад
                    break;
                case TileType.Ice:
                    if (Map[previosMove.From.Position].Type == TileType.Horse)
                    {
                        //повторяем ход лошади
                        rez = GetHorseDeltas(source.Position)
                            .Select(x => IncomeTilePosition(x));
                    }
                    else //повторяем предыдущий ход
                    {
                        //TODO - проверка на использование самолета на предыдущем ходу - тогда мы должны повторить ход самолета
                        var previosDelta = previosMove.GetDelta();
                        Position target = Position.AddDelta(source.Position, previosDelta);
                        rez = new[] { target }.Select(x => IncomeTilePosition(x));
                    }
                    break;
                case TileType.RespawnFort:
                    rez = GetNearDeltas(source.Position)
                        .Where(x => IsValidMapPosition(x))
                        .Where(x => Map[x].Type != TileType.Water || x==ourShip.Position)
                        .Select(x => IncomeTilePosition(x))
                        .Concat(new[] {source});
                    break;
                case TileType.Spinning:
                    if (source.Level == 0)
                    {
                        rez = GetNearDeltas(source.Position)
                            .Where(x => IsValidMapPosition(x))
                            .Where(x => Map[x].Type != TileType.Water || x == ourShip.Position)
                            .Select(x => IncomeTilePosition(x));
                    }
                    else
                    {
                        rez = new[] {new TilePosition(source.Position, source.Level - 1)};
                    }
                    break;
                case TileType.Water:
                    if (source.Position == ourShip.Position) //с своего корабля
                    {
                        rez = GetShipPosibleNavaigations(source.Position)
                            .Concat(new[] {GetShipLanding(source.Position)})
                            .Select(x => IncomeTilePosition(x));
                    }
                    else //пират плавает в воде
                    {
                        rez = GetPosibleSwimming(source.Position)
                            .Select(x => IncomeTilePosition(x));
                    }
                    break;
                default:
                    rez = GetNearDeltas(source.Position)
                        .Where(x => IsValidMapPosition(x))
                        .Where(x => Map[x].Type != TileType.Water || x == ourShip.Position)
                        .Select(x => IncomeTilePosition(x));
                    break;
            }
            return rez.Where(x => IsValidMapPosition(x.Position)).ToList();
        }

        TilePosition IncomeTilePosition(Position pos)
        {
            if (IsValidMapPosition(pos) && Map[pos].Type==TileType.Spinning)
                return new TilePosition(pos,Map[pos].SpinningCount-1);
            else
                return new TilePosition(pos);
        }

        public static IEnumerable<Position> GetHorseDeltas(Position pos)
        {
            for (int x = -2; x <= 2; x++)
            {
                if (x == 0) continue;
                int deltaY = (Math.Abs(x) == 2) ? 1 : 2;
                yield return new Position(pos.X + x, pos.Y - deltaY);
                yield return new Position(pos.X + x, pos.Y + deltaY);
            }
        }
        
        public static IEnumerable<Position> GetNearDeltas(Position pos)
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

        public bool IsValidMapPosition(Position pos)
        {
            return (
                pos.X >= 0 && pos.X < Size
                           && pos.Y >= 0 && pos.Y < Size //попадаем в карту
                           && Utils.InCorners(pos, 0, Size - 1) == false //не попадаем в углы карты
            );
        }

        public IEnumerable<Position> GetShipPosibleNavaigations(Position pos)
        {
            if (pos.X == 0 || pos.X == Size - 1)
            {
                if (pos.Y > 2)
                    yield return new Position(pos.X, pos.Y - 1);
                if (pos.Y < Size - 3)
                    yield return new Position(pos.X, pos.Y + 1);
            }
            else if (pos.Y == 0 || pos.Y == Size - 1)
            {
                if (pos.X > 2)
                    yield return new Position(pos.X - 1, pos.Y);
                if (pos.X < Size - 3)
                    yield return new Position(pos.X + 1, pos.Y);
            }
            else
            {
                throw new Exception("wrong ship position");
            }
        }

        public Position GetShipLanding(Position pos)
        {
            if (pos.X == 0)
                return new Position(1, pos.Y);

            if (pos.X == Size - 1)
                return new Position(Size - 2, pos.Y);

            if (pos.Y == 0)
                return new Position(pos.X, 1);

            if (pos.Y == Size - 1)
                return new Position(pos.X, Size - 2);

            throw new Exception("wrong ship position");
        }

        public Position GetCannonFly(int arrowsCode, Position pos) =>
            arrowsCode switch
            {
                // вверх
                0 => new Position(pos.X, Size - 1),
                // вправо
                1 => new Position(Size - 1, pos.Y),
                // вниз
                2 => new Position(pos.X, 0),
                // влево
                _ => new Position(0, pos.Y)
            };

        public IEnumerable<Position> GetArrowsDeltas(int arrowsCode, Position source)
        {
            foreach (var delta in ArrowsCodesHelper.GetExitDeltas(arrowsCode))
            {
                yield return new Position(source.X + delta.X, source.Y + delta.Y);
            }
        }

        /// <summary>
        /// Все возможные цели для плавающего пирата
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public IEnumerable<Position> GetPosibleSwimming(Position pos)
        {
            return GetNearDeltas(pos).Where(x => IsValidMapPosition(x)).Where(x => Map[x].Type == TileType.Water);
        }

        public bool WasCheckedBefore(List<CheckedPosition> alreadyCheckedList, CheckedPosition currentCheck)
        {
            foreach (var info in alreadyCheckedList)
            {
                if (info.Position == currentCheck.Position)
                {
                    if (info.IncomeDelta == null || info.IncomeDelta == currentCheck.IncomeDelta) return true;
                }
            }
            return false;
        }
    }
}