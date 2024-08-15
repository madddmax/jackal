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
            CoinsLeft = Board.Generator.CoinsOnMap;

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

            NeedSubTurnPirate = null;
            PrevSubTurnDirection = null;

            if (_availableMoves.Count > 0)
            {
                //есть возможные ходы
                var gameState = new GameState
                {
                    AvailableMoves = _availableMoves.ToArray(),
                    Board = Board,
                    GameId = GameId,
                    TurnNumber = TurnNo,
                    TeamId = CurrentTeamId
                };
                var (moveNum, pirateId) = CurrentPlayer.OnMove(gameState);
                
                var from = _availableMoves[moveNum].From;
                var currentTeamPirates = Board.Teams[CurrentTeamId].Pirates;
                var pirate = 
                    currentTeamPirates.FirstOrDefault(x => x.Id == pirateId && x.Position == from) 
                    ?? currentTeamPirates.First(x => x.Position == from);
                
                IGameAction action = _actions[moveNum];
                action.Act(this, pirate);
            }

            if (NeedSubTurnPirate == null)
            {
                //также протрезвляем всех пиратов, которые начали бухать раньше текущего хода
                foreach (Pirate pirate in Board.Teams[CurrentTeamId].Pirates.Where(x => x.IsDrunk && x.DrunkSinceTurnNo < TurnNo))
                {
                    pirate.DrunkSinceTurnNo = null;
                    pirate.IsDrunk = false;
                }

                TurnNo++;
            }
        }

        public Pirate? NeedSubTurnPirate { private get; set; }

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

            IEnumerable<Pirate> activePirates;
            Direction? prevDirection = null;
            if (NeedSubTurnPirate != null)
            {
                activePirates = new[] {NeedSubTurnPirate};
                prevDirection = PrevSubTurnDirection;
            }
            else
            {
                activePirates = team.Pirates.Where(x => x.IsDrunk == false && x.IsInTrap == false);
            }

            var targets = new List<AvailableMove>();

            foreach (var pirate in activePirates)
            {
                TilePosition? prev = prevDirection != null ? prevDirection.From : null;
                AvailableMovesTask task = new AvailableMovesTask(teamId, pirate.Position, prev);

                List<AvailableMove> temp = Board.GetAllAvailableMoves(task);
                targets.AddRange(temp);
            }

            foreach (AvailableMove availableMove in targets)
            {
                Move move = new Move(availableMove.Source, availableMove.Target, availableMove.MoveType);
                GameActionList actionList = availableMove.ActionList;
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
        public int LastActionTurnNo { get; internal set; }

        public int CurrentTeamId => TurnNo % _players.Length;

        public IPlayer CurrentPlayer => _players[CurrentTeamId];

        public Direction? PrevSubTurnDirection;

        public void KillPirate(Pirate pirate)
        {
            int teamId = pirate.TeamId;
            Board.Teams[teamId].Pirates = Board.Teams[teamId].Pirates.Where(x => x != pirate).ToArray();
            var tile = Board.Map[pirate.Position];
            tile.Pirates.Remove(pirate);
        }
    }
}
