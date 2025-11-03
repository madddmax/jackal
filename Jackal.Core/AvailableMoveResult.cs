using System.Collections.Generic;
using Jackal.Core.Actions;
using Jackal.Core.Domain;

namespace Jackal.Core;

public class AvailableMoveResult
{
    public List<Move> AvailableMoves { get; } = new();
    
    public List<IGameAction> Actions { get; } = new();
}