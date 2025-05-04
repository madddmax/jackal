using Jackal.Core.Domain;

namespace Jackal.Core;

public class GameState
{
    public Board Board;
    public Move[] AvailableMoves;
    public int TeamId;
    public int TurnNumber;
}