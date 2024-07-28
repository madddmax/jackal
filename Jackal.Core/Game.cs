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
            CoinsLeft = Board.CoinsOnMap;

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

        private void AddMoveAndActions(Move move, IGameAction action)
        {
            if (_availableMoves.Exists(x => x == move)) return;
            _availableMoves.Add(move);
            _actions.Add(action);
        }

        public bool IsGameOver => (CoinsLeft == 0 && !Board.AllTiles(x => x.Type == TileType.Unknown).Any()) 
                                  || TurnNo - 200 > LastActionTurnNo;

        public int TurnNo { get; private set; }
        public int SubTurnNo { get; private set; }
        public int LastActionTurnNo { get; internal set; }

        public int CurrentTeamId
        {
            get { return TurnNo % _players.Length; }
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
