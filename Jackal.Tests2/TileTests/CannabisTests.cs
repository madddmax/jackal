using System.Collections.Generic;
using System.Linq;
using Jackal.Core.Domain;
using Jackal.Core.MapGenerator;
using Xunit;

namespace Jackal.Tests2.TileTests;

public class CannabisTests
{
    [Fact]
    public void OneCannabis_GetAvailableMoves_ReturnNearestMoves()
    {
        // Arrange
        var cannabisOnlyMap = new OneTileMapGenerator(TileParams.Cannabis());
        var game = new TestGame(cannabisOnlyMap);
        
        // Act - высадка с корабля на хи-хи траву
        game.Turn();
        var moves = game.GetAvailableMoves();
        
        // Assert - доступно 4 хода на соседние клетки с хи-хи травы в месте высадки
        Assert.Equal(4, moves.Count);
        Assert.Equal(new TilePosition(2, 1), moves.First().From);
        Assert.Equivalent(new List<TilePosition>
            {
                new(1, 2),
                new(2, 0), // свой корабль
                new(2, 2),
                new(3, 2)
            },
            moves.Select(m => m.To)
        );
        Assert.Equal(1, game.TurnNumber);
    }
}