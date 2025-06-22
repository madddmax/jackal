using System.Collections.Generic;
using Jackal.Core.Domain;

namespace Jackal.Core.Actions;

public class GameActionList(params IGameAction[] actions) : IGameAction
{
    private readonly List<IGameAction> _actions = [..actions];

    public void AddAction(IGameAction action)
    {
        _actions.Add(action);
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
}