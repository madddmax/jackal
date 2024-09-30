using Jackal.Core;
using Jackal.Core.MapGenerator;
using Xunit;

namespace Jackal.Tests2.TileTests;

public class HoleTests
{
    [Fact]
    public void OneHole_GetAvailableMoves_ReturnNoAvailableMoves()
    {
        // Arrange
        var holeOnlyMap = new OneTileMapGenerator(
            new TileParams(TileType.Hole)
        );
        var game = new TestGame(holeOnlyMap);
        
        // Act - высадка с корабля на дыру
        game.Turn();
        
        var moves = game.GetAvailableMoves();
        
        // Assert - не доступно ни одного хода, т.к. застряли в дыре
        Assert.Empty(moves);
        Assert.Equal(1, game.TurnNo);
    }
    
    [Fact]
    public void GrassThenHoleThenNextPirateGrassAndSameHole_GetAvailableMoves_ReturnNoAvailableMoves()
    {
        // Arrange
        var grassHoleLineMap = new TwoTileMapGenerator(
            new TileParams(TileType.Grass),
            new TileParams(TileType.Hole)
        );
        const int mapSize = 5;
        const int piratesPerPlayer = 2;
        var game = new TestGame(grassHoleLineMap, mapSize, piratesPerPlayer);
        
        // Act - высадка с корабля на пустое поле
        game.Turn();
        
        // выбираем ход - вперед на дыру
        game.SetMoveAndTurn(2, 2);
        
        // высадка с корабля вторым пиратом на пустое поле
        game.Turn();
        
        // выбираем ход - вперед на дыру
        game.SetMoveAndTurn(2, 2);
        
        var moves = game.GetAvailableMoves();
        
        // Assert - не доступно ни одного хода, т.к. оба пирата застряли в дыре
        Assert.Empty(moves);
        Assert.Equal(4, game.TurnNo);
    }
    
    [Fact]
    public void GrassThenHoleThenNextPirateGrassAndOtherHole_GetAvailableMoves_ReturnAvailableMoves()
    {
        // Arrange
        var grassHoleLineMap = new TwoTileMapGenerator(
            new TileParams(TileType.Grass),
            new TileParams(TileType.Hole)
        );
        const int mapSize = 5;
        const int piratesPerPlayer = 2;
        var game = new TestGame(grassHoleLineMap, mapSize, piratesPerPlayer);
        
        // Act - высадка с корабля на пустое поле
        game.Turn();
        
        // выбираем ход - вперед на дыру
        game.SetMoveAndTurn(2, 2);
        
        // высадка с корабля вторым пиратом на пустое поле
        game.Turn();
        
        // выбираем ход - вперед и вправо на другую дыру
        game.SetMoveAndTurn(3, 2);
        
        var moves = game.GetAvailableMoves();
        
        // Assert - доступны новые ходы
        Assert.True(moves.Count > 0);
        Assert.Equal(4, game.TurnNo);
    }
}