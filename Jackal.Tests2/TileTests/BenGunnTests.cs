using System.Collections.Generic;
using System.Linq;
using Jackal.Core;
using Xunit;

namespace Jackal.Tests2.TileTests;

public class BenGunnTests
{
    [Fact]
    public void OneBenGunn_Turn_ReturnNewPirate()
    {
        // Arrange
        var benGunnOnlyMap = new OneTileMapGenerator(new TileParams(TileType.BenGunn));
        var game = new TestGame(benGunnOnlyMap);
        
        // Act - высадка с корабля на Бен Ганна
        game.Turn();
        
        // Assert - пиратов стало больше: 1 обычный и 1 Бен Ганн
        Assert.Equal(2, game.Board.AllPirates.Count);
        Assert.Single(game.Board.AllPirates.Where(p => p.Type == PirateType.Usual));
        Assert.Single(game.Board.AllPirates.Where(p => p.Type == PirateType.BenGunn));
        Assert.Equal(1, game.TurnNo);
    }    
    
    [Fact]
    public void OneBenGunn_GetAvailableMoves_ReturnNearestMoves()
    {
        // Arrange
        var benGunnOnlyMap = new OneTileMapGenerator(new TileParams(TileType.BenGunn));
        var game = new TestGame(benGunnOnlyMap);
        
        // Act - высадка с корабля на Бен Ганна
        game.Turn();
        var moves = game.GetAvailableMoves();
        
        // Assert - доступно 4 хода на соседние клетки с Бен Ганна в месте высадки
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
        Assert.Equal(1, game.TurnNo);
    }
}