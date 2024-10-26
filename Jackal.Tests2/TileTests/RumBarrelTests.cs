using System.Collections.Generic;
using System.Linq;
using Jackal.Core.Domain;
using Jackal.Core.MapGenerator;
using Xunit;

namespace Jackal.Tests2.TileTests;

public class RumBarrelTests
{
    [Fact]
    public void OneRumBarrel_GetAvailableMoves_ReturnNoAvailableMoves()
    {
        // Arrange
        var rumBarrelOnlyMap = new OneTileMapGenerator(
            new TileParams(TileType.RumBarrel)
        );
        var game = new TestGame(rumBarrelOnlyMap);
        
        // Act - высадка с корабля на бочку с ромом
        game.Turn();
        
        var moves = game.GetAvailableMoves();
        
        // Assert - не доступно ни одного хода, т.к. напились на бочке с ромом
        Assert.Empty(moves);
        Assert.Single(game.Board.Teams);
        Assert.Single(game.Board.Teams[0].Pirates);
        
        var ownPirate = game.Board.Teams[0].Pirates[0];
        Assert.False(ownPirate.IsActive);
        Assert.True(ownPirate.IsDrunk);
        
        Assert.Equal(1, game.TurnNo);
    }
    
    [Fact]
    public void OneRumBarrelWaitTurn_GetAvailableMoves_ReturnNearestMoves()
    {
        // Arrange
        var rumBarrelOnlyMap = new OneTileMapGenerator(
            new TileParams(TileType.RumBarrel)
        );
        var game = new TestGame(rumBarrelOnlyMap);
        
        // Act - высадка с корабля на бочку с ромом
        game.Turn();
        
        // ждем следующий ход
        game.Turn();
        
        var moves = game.GetAvailableMoves();
        
        // Assert - доступно 4 хода на соседние клетки с клетки бочки с ромом в месте высадки
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
        Assert.Equal(2, game.TurnNo);
    }
    
    [Fact]
    public void RumBarrelWaitTurnThenNextRumBarrel_GetAvailableMoves_ReturnNoAvailableMoves()
    {
        // Arrange
        var rumBarrelOnlyMap = new OneTileMapGenerator(
            new TileParams(TileType.RumBarrel)
        );
        var game = new TestGame(rumBarrelOnlyMap);
        
        // Act - высадка с корабля на бочку с ромом
        game.Turn();
        
        // ждем следующий ход
        game.Turn();
        
        // выбираем ход - вперед на следующую бочку с ромом
        game.SetMoveAndTurn(2, 2);
        
        var moves = game.GetAvailableMoves();
        
        // Assert - не доступно ни одного хода, т.к. напились на бочке с ромом
        Assert.Empty(moves);
        Assert.Equal(3, game.TurnNo);
    }
}