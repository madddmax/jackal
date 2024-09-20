using System.Linq;
using Jackal.Core;
using Xunit;

namespace Jackal.Tests2.TileTests;

public class CannonTests
{
    [Fact]
    public void OneCannonUpDirection_GetAvailableMoves_ReturnOneMoveUpToWater()
    {
        // Arrange
        var cannonOnlyMap = new OneTileMapGenerator(new TileParams(TileType.Cannon) {Direction = 0});
        var game = new TestGame(cannonOnlyMap);
        
        // Act - высадка с корабля на пушку
        game.Turn();
        var moves = game.GetAvailableMoves();
        
        // Assert - с пушки летим вверх в воду
        Assert.Single(moves);
        Assert.Equal(new TilePosition(2, 1), moves.First().From);
        Assert.Equal(new TilePosition(2, 4), moves.First().To);
        Assert.Equal(0, game.TurnNo);
    }
}