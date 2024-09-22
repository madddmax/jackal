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
            PrevSubTurnPosition = null;

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
                SubTurnAirplaneFlying = false;
            }
        }

        /// <summary>
        /// TODO-MAD является результатом действия -
        /// Пират которому требуется дополнительный ход 
        /// </summary>
        public Pirate? NeedSubTurnPirate { private get; set; }
        
        /// <summary>
        /// TODO-MAD является результатом действия -
        /// Предыдущая позиция пирата которому требуется дополнительный ход
        /// </summary>     
        public TilePosition? PrevSubTurnPosition { get; set; }

        /// <summary>
        /// TODO-MAD является результатом действия -
        /// Полет на самолете
        /// </summary>
        public bool SubTurnAirplaneFlying { get; set; }
        
        /// <summary>
        /// TODO-MAD является результатом действия -
        /// Количество просмотров карты с маяка
        /// </summary>
        public int SubTurnLighthouseViewCount { get; set; }

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

            IEnumerable<Pirate> activePirates = NeedSubTurnPirate != null 
                ? new[] {NeedSubTurnPirate} 
                : team.Pirates.Where(x => x is { IsDrunk: false, IsInTrap: false });

            var targets = new List<AvailableMove>();
            foreach (var pirate in activePirates)
            {
                TilePosition prev = PrevSubTurnPosition ?? pirate.Position;
                AvailableMovesTask task = new AvailableMovesTask(teamId, pirate.Position, prev);
                List<AvailableMove> moves = Board.GetAllAvailableMoves(task, task.Source, task.Prev, SubTurnAirplaneFlying);
                targets.AddRange(moves);
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

        /// <summary>
        /// Конец игры
        /// </summary>
        public bool IsGameOver => (CoinsLeft == 0 && !Board.AllTiles(x => x.Type == TileType.Unknown).Any())
                                  || TurnNo - 50 * _players.Length > LastActionTurnNo;
        
        /// <summary>
        /// Текущий ход - определяет какая команда ходит
        /// </summary>
        public int TurnNo { get; private set; }
        
        /// <summary>
        /// Последний ход когда производилось действие:
        /// открытие новой клетки или перенос монеты 
        /// </summary>
        public int LastActionTurnNo { get; internal set; }

        /// <summary>
        /// ИД команды которая ходит
        /// </summary>
        public int CurrentTeamId => TurnNo % _players.Length;

        /// <summary>
        /// Игрок который ходит
        /// </summary>
        public IPlayer CurrentPlayer => _players[CurrentTeamId];

        /// <summary>
        /// Убрать пирата с карты
        /// </summary>
        public void KillPirate(Pirate pirate)
        {
            int teamId = pirate.TeamId;
            Board.Teams[teamId].Pirates = Board.Teams[teamId].Pirates.Where(x => x != pirate).ToArray();
            var tile = Board.Map[pirate.Position];
            tile.Pirates.Remove(pirate);

            Board.DeadPirates ??= [];
            Board.DeadPirates.Add(pirate);
        }

        /// <summary>
        /// Добавить нового пирата на карту
        /// </summary>
        public void AddPirate(int teamId, TilePosition position, PirateType type)
        {
            var newPirate = new Pirate(teamId, position, type);
            Board.Teams[teamId].Pirates = Board.Teams[teamId].Pirates.Concat(new[] { newPirate }).ToArray();
            
            var tile = Board.Map[position];
            tile.Pirates.Add(newPirate);
        }

        /// <summary>
        /// Переместить пирата на его корабль
        /// </summary>
        public void MovePirateToTheShip(Pirate pirate)
        {
            var team = Board.Teams[pirate.TeamId];
            var pirateTileLevel = Board.Map[pirate.Position];
            
            pirate.Position = new TilePosition(team.Ship.Position);
            Board.Map[team.Ship.Position].Pirates.Add(pirate);
            pirateTileLevel.Pirates.Remove(pirate);
            
            pirate.IsInTrap = false;
            pirate.IsDrunk = false;
            pirate.DrunkSinceTurnNo = null;
        }
    }
}
