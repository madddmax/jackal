using System.Collections.Generic;
using Jackal.Core.Domain;

namespace Jackal.Core.Actions;

public class GameActionList(params IGameAction[] actions) : IGameAction
{
    private readonly List<IGameAction> _actions = [..actions];

    public void AddFirstAction(IGameAction action)
    {
        _actions.Insert(0, action);
    }
    
    public void Act(Game game,Pirate pirate)
    {
        foreach (var action in _actions)
        {
            action.Act(game, pirate);
        }
    }
}