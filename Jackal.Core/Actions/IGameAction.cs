using Jackal.Core.Domain;

namespace Jackal.Core.Actions;

public interface IGameAction
{
    void Act(Game game, Pirate pirate);
}