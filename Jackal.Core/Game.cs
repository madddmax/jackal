using System;
using System.Collections.Generic;
using System.Linq;
using Jackal.Core.Actions;

namespace Jackal.Core
{
    public class Game
    {
        private readonly IPlayer[] _players;

        public readonly Board Board;

        public Dictionary<int, int> Scores; // TeamId->Total couns
        
        /// <summary>
        /// Нерастасканное золотишко
        /// </summary>
        public int CoinsLeft;

        private readonly List<Move> _availableMoves;
        private readonly List<IGameAction> _actions;

        public readonly Guid GameId = Guid.NewGuid();

        public Game(IPlayer[] players, Board board)
        {
            _players = players;

            Board = board;
            Scores = new Dictionary<int, int>();
            foreach (var team in Board.Teams)
            {
                Scores[team.Id] = 0;
            }
            CoinsLeft = MapGenerator.TotalCoins;

            _availableMoves = new List<Move>();
            _actions = new List<IGameAction>();

            foreach (var player in _players)
            {
                player.OnNewGame();
            }
        }

        public void Turn()
        {
            GetAvailableMoves(CurrentTeamId);

            this.NeedSubTurnPirate = null;
            this.PreviosSubTurnDirection = null;

            if (_availableMoves.Count > 0) //есть возможные ходы
            {
                int moveNo;
                if (_availableMoves.Count == 1) //только один ход, сразу выбираем его
                {
                    moveNo = 0;
                }
                else //запрашиваем ход у игрока
                {
                    GameState gameState = new GameState();
                    gameState.AvailableMoves = _availableMoves.ToArray();
                    gameState.Board = Board;
                    gameState.GameId = GameId;
                    gameState.TurnNumber = TurnNo;
                    gameState.SubTurnNumber = SubTurnNo;
                    gameState.TeamId = CurrentTeamId;
                    moveNo = CurrentPlayer.OnMove(gameState);
                }

                IGameAction action = _actions[moveNo];
                Pirate pirate = Board.Teams[CurrentTeamId].Pirates.First(x => x.Position == _availableMoves[moveNo].From);
                action.Act(this, pirate);
            }
            else //у нас нет возможных ходов - тогда если все трезвые, то все гибнут
            {
                var allPirates = Board.Teams[CurrentTeamId].Pirates.ToList();
                bool allNotDrunkPirates = allPirates.All(x => x.IsDrunk == false);
                if (allNotDrunkPirates)
                {
                    foreach (var pirate in allPirates)
                    {
                        KillPirate(pirate);
                    }
                }
            }

            if (this.NeedSubTurnPirate == null)
            {
                //также протрезвляем всех пиратов, которые начали бухать раньше текущего хода
                foreach (Pirate pirate in Board.Teams[CurrentTeamId].Pirates.Where(x => x.IsDrunk && x.DrunkSinceTurnNo < TurnNo))
                {
                    pirate.DrunkSinceTurnNo = null;
                    pirate.IsDrunk = false;
                }

                TurnNo++;
                SubTurnNo = 0;
            }
            else
            {
                SubTurnNo++;
            }
        }

        public Pirate NeedSubTurnPirate { private get; set; }

        public List<Move> GetPrevAvailableMoves()
        {
            return _availableMoves;
        }

        public List<Move> GetAvailableMoves()
        {
            GetAvailableMoves(CurrentTeamId);
            return _availableMoves;
        }

        private void GetAvailableMoves(int teamId)
        {
            _availableMoves.Clear();
            _actions.Clear();
            
            Team team = Board.Teams[teamId];
            Ship ship = team.Ship;

            IEnumerable<Pirate> activePirates;
            Direction previosDirection = null;
            if (NeedSubTurnPirate != null)
            {
                activePirates = new[] {NeedSubTurnPirate};
                previosDirection = PreviosSubTurnDirection;
            }
            else
            {
                activePirates = team.Pirates.Where(x => x.IsDrunk == false && x.IsInTrap == false);
            }

            var targets = new List<AvaliableMove>();

            foreach (var pirate in activePirates)
            {
                var position = pirate.Position;

                GetAllAvaliableMovesTask task=new GetAllAvaliableMovesTask();
                task.TeamId = teamId;
                task.FirstSource = position;
                task.PreviosSource = (previosDirection != null) ? previosDirection.From : null;

                List<AvaliableMove> temp = Board.GetAllAvaliableMoves(task);
                targets.AddRange(temp);
            }

            //если есть ходы, которые не приводят к прыжку в воду, то выбираем только их
            if (targets.Any(x => x.WithJumpToWater == false))
                targets = targets.Where(x => x.WithJumpToWater == false).ToList();

            foreach (AvaliableMove avaliableMove in targets)
            {
                Move move = new Move(avaliableMove.Source, avaliableMove.Target, avaliableMove.MoveType);
                GameActionList actionList = avaliableMove.ActionList;
                AddMoveAndActions(move, actionList);
            }
        }

#if false
        private void Step(TilePosition target, Pirate pirate, Ship ship, Team team)
        {
            //var moves = _availableMoves;
            //var actions = _actions;

            Tile targetTile = Board.Map[target.Position];

            var source = pirate.Position.Position;
            Tile sourceTile = Board.Map[source];

            bool onShip = (ship.Position == pirate.Position.Position);

            Direction direction=new Direction(pirate.Position,target);

            switch (targetTile.Type)
            {
                case TileType.Unknown:
                {
                    // exploration
                    if (onShip)
                    {
                        if (CanLanding(pirate, target))
                        {
                            // landing
                            AddMoveAndActions(new Move(pirate.Position.Position, target),
                                GameActionList.Create(
                                   new Explore(target,pirate,direction),
                                    new Landing(pirate, ship),
                                    new Moving(pirate, target)));
                        }
                    }
                    else
                    {
                        AddMoveAndActions(new Move(pirate.Position.Position, target),
                            GameActionList.Create(
                                new Explore(target, pirate, direction),
                                new Moving(pirate, target)));
                    }

                    break;
                }
                case TileType.Water:
                {
                    if (target == ship.Position) //на свой корабль
                    {
                        if (sourceTile.Coins > 0)
                        {
                            // shipping with coins
                            AddMoveAndActions(new Move(pirate.Position.Position, target, MoveType.WithCoin),
                                GameActionList.Create(
                                    new TakeCoinToShip(pirate,ship),
                                    new Shipping(pirate, ship)));
                        }

                        // shipping without coins
                        AddMoveAndActions(new Move(pirate.Position.Position, target),
                            GameActionList.Create(
                                //new DropCoin(pirate),
                                new Shipping(pirate, ship)));
                    }
                    else if (pirate.Position.Position == ship.Position) //двигаем корабль
                    {
                        if (((ship.Position.X == 0 || ship.Position.X == Board.Size - 1) &&
                             (target.Y <= 1 || target.Y >= Board.Size - 2))
                            ||
                            ((ship.Position.Y == 0 || ship.Position.Y == Board.Size - 1) &&
                             (target.X <= 1 || target.X >= Board.Size - 2)))
                        {
                            break; // enemy water territories
                        }

                        // navigation
                        AddMoveAndActions(new Move(pirate.Position.Position, target),
                            GameActionList.Create(
                                new Navigation(ship, target)));
                    }
                    else if (Board.Teams.Where(x => x.Id != CurrentTeamId).Select(x => x.Ship.Position).Contains(target))
                    {
                        //столкновение с чужим кораблем
                        AddMoveAndActions(new Move(pirate.Position.Position, target),
                              GameActionList.Create(
                                  new Dying(pirate)));
                    }
                    else //падение пирата в воду и плавание в воде
                    {
                        AddMoveAndActions(new Move(pirate.Position.Position, target),
                            GameActionList.Create(
                                 new Moving(pirate, target)));
                    }
                    break;
                }
				case TileType.Chest1:
                case TileType.Chest2:
                case TileType.Chest3:
                case TileType.Chest4:
                case TileType.Chest5:
                case TileType.Grass:
                case TileType.Fort:
                case TileType.RespawnFort:
                case TileType.RumBarrel:
                case TileType.Canibal:
                case TileType.Spinning:
                case TileType.Trap:
                {
                    var attack = targetTile.OccupationTeamId.HasValue && targetTile.OccupationTeamId.Value != pirate.TeamId;
                    bool targetIsFort = targetTile.Type.IsFort();

                    //Respawn
                    if (targetTile.Type == TileType.RespawnFort && sourceTile.Type == TileType.RespawnFort)
                    {
                        if (team.Pirates.Count() < 3)
                        {
                            AddMoveAndActions(new Move(pirate.Position.Position, target, MoveType.WithRespawn),
                                GameActionList.Create(
                                    new Respawn(team, target)));
                        }
                    }

                    if (attack)
                    {
                        if (targetIsFort == false)
                        {
                            // attack
                            if (onShip)
                            {
                                if (CanLanding(pirate, target))
                                {
                                    AddMoveAndActions(new Move(pirate.Position.Position, target),
                                        GameActionList.Create(
                                            new Attack(target),
                                            new Landing(pirate, ship),
                                            new Moving(pirate, target)));
                                }
                            }
                            else
                            {
                                AddMoveAndActions(new Move(pirate.Position.Position, target),
                                    GameActionList.Create(
                                        new Attack(target),
                                        new Moving(pirate, target)));
                            }
                        }
                    }
                    else
                    {
                        if (onShip)
                        {
                            if (CanLanding(pirate, target))
                            {
                                AddMoveAndActions(new Move(pirate.Position.Position, target),
                                    GameActionList.Create(
                                        new Landing(pirate, ship),
                                        new Moving(pirate, target)));
                            }
                        }
                        else
                        {
                            AddMoveAndActions(new Move(pirate.Position.Position, target),
                                GameActionList.Create(
                                    new Moving(pirate, target)));

                            if (sourceTile.Coins > 0 && targetIsFort == false)
                            {
                                AddMoveAndActions(new Move(pirate.Position.Position, target, MoveType.WithCoin),
                                    GameActionList.Create(
                                       new Moving(pirate, target,true)));
                            }
                        }
                    }
                    break;
                }
            }
        }
#endif

        private void AddMoveAndActions(Move move, IGameAction action)
        {
            if (_availableMoves.Exists(x => x == move)) return;
            _availableMoves.Add(move);
            _actions.Add(action);
        }

        private static bool CanLanding(Pirate pirate, Position to)
        {
            return ((pirate.Position.Position.Y == 0 || pirate.Position.Position.Y == Board.Size - 1) &&
                    pirate.Position.Position.X - to.X == 0) ||
                   ((pirate.Position.Position.X == 0 || pirate.Position.Position.X == Board.Size - 1) &&
                    pirate.Position.Position.Y - to.Y == 0);
        }

        public bool IsGameOver
        {
            get { return CoinsLeft == 0 || TurnNo - 4*50 > LastActionTurnNo; }
        }

        public int TurnNo { get; private set; }
        public int SubTurnNo { get; private set; }
        public int LastActionTurnNo { get; internal set; }

        public int CurrentTeamId
        {
            get { return TurnNo%4; }
        }

        public IPlayer CurrentPlayer
        {
            get { return _players[CurrentTeamId]; }
        }

        public Direction PreviosSubTurnDirection;

        public void KillPirate(Pirate pirate)
        {
            int teamId = pirate.TeamId;
            Board.Teams[teamId].Pirates = Board.Teams[teamId].Pirates.Where(x => x != pirate).ToArray();
            var tile = Board.Map[pirate.Position];
            tile.Pirates.Remove(pirate);
        }
    }
}
