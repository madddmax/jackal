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
    
    public void Act(Game game,Pirate pirate)
    {
        foreach (var action in _actions)
        {
            action.Act(game, pirate);
        }
    }
}