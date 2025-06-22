using System.Linq;
using Jackal.Core.Domain;
using Jackal.Core.MapGenerator;
using Xunit;

namespace Jackal.Tests2.TileTests;

public class RumBottlesTests
{
    [Fact]
    public void OneRum_GetAvailableMoves_ReturnNearestMovesAndOneRumBottel()
    {
        // Arrange
        var oneRumOnlyMap = new OneTileMapGenerator(
            new TileParams(TileType.RumBottles, 1)
        );
        var game = new TestGame(oneRumOnlyMap);
        
        // Act - высадка с корабля бутылку с ромом
        game.Turn();
        var moves = game.GetAvailableMoves();
        
        // Assert - доступно 4 хода на соседние клетки в месте высадки и одна бутылка с ромом
        Assert.Equal(4, moves.Count);
        Assert.Equal(1, game.Board.Teams.Single().RumBottles);
        Assert.Equal(1, game.TurnNumber);
    }
    
    [Fact]
    public void TwoRum_GetAvailableMoves_ReturnNearestMovesAndTwoRumBottels()
    {
        // Arrange
        var twoRumOnlyMap = new OneTileMapGenerator(
            new TileParams(TileType.RumBottles, 2)
        );
        var game = new TestGame(twoRumOnlyMap);
        
        // Act - высадка с корабля на две бутылки с ромом
        game.Turn();
        var moves = game.GetAvailableMoves();
        
        // Assert - доступно 4 хода на соседние клетки в месте высадки и две бутылки с ромом
        Assert.Equal(4, moves.Count);
        Assert.Equal(2, game.Board.Teams.Single().RumBottles);
        Assert.Equal(1, game.TurnNumber);
    }
    
    [Fact]
    public void ThreeRum_GetAvailableMoves_ReturnNearestMovesAndThreeRumBottels()
    {
        // Arrange
        var threeRumOnlyMap = new OneTileMapGenerator(
            new TileParams(TileType.RumBottles, 3)
        );
        var game = new TestGame(threeRumOnlyMap);
        
        // Act - высадка с корабля на три бутылки с ромом
        game.Turn();
        var moves = game.GetAvailableMoves();
        
        // Assert - доступно 4 хода на соседние клетки в месте высадки и три бутылки с ромом
        Assert.Equal(4, moves.Count);
        Assert.Equal(3, game.Board.Teams.Single().RumBottles);
        Assert.Equal(1, game.TurnNumber);
    }
}