using System.Collections.Generic;

namespace Jackal.Actions
{
    public class GameActionList : IGameAction
    {
        private readonly List<IGameAction> _actions;

        public GameActionList(params IGameAction[] actions)
        {
            _actions = new List<IGameAction>(actions);
        }

        public GameActionResult Act(Game game,Pirate pirate)
        {
            foreach (var action in _actions)
            {
                var rez = action.Act(game, pirate);
                if (rez == GameActionResult.Die)
                    return GameActionResult.Die;
            }
            return GameActionResult.Live;
        }

        public static GameActionList Create(params IGameAction[] actions)
        {
            return new GameActionList(actions);
        }
    }
}