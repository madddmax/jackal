using System.Collections.Generic;
using System.Linq;
using Jackal.Core;
using Xunit;

namespace Jackal.Tests2.TileTests;

public class AirplaneTests
{
    [Fact]
    public void AvailableMovesTest()
    {
        // Arrange
        var airplaneOnlyMap = new OneTileMapGenerator(new TileParams(TileType.Airplane));
        var game = new TestGame(airplaneOnlyMap);
        
        // Act - высадка с корабля на самолет
        game.Turn();
        List<Move> moves = game.GetAvailableMoves();
        
        // Assert - все поле 5 клеток + свой корабль
        Assert.Equal(6, moves.Count);
        Assert.Equivalent(new List<TilePosition>
            {
                new(1, 2), // клетка с самолетом
                new(2, 0), // свой корабль
                new(2, 1),
                new(2, 2),
                new(2, 3),
                new(3, 2)
            },
            moves.Select(m => m.To)
        );
    }
}