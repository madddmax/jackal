using System.Collections.Generic;
using System.Linq;
using Jackal.Core;
using Xunit;

namespace Jackal.Tests2.TileTests;

public class SpinningTests
{
    [Fact]
    public void SpinningFinishThenCrocodile_GetAvailableMoves_ReturnSpinningStart()
    {
        // Arrange
        var spinningCrocodileLineMap = new TwoTileMapGenerator(
            new TileParams(TileType.Spinning) { SpinningCount = 2 },
            new TileParams(TileType.Crocodile));
        var game = new TestGame(spinningCrocodileLineMap);
        
        // Act - высадка с корабля на джунгли-вертушку
        game.Turn();
        
        // доходим до конца клетки джунгли-вертушка
        game.Turn();
        List<Move> moves = game.GetAvailableMoves();
        
        // выбираем ход - вперед на крокодила
        int moveNum = moves.FindIndex(a => a.To == new TilePosition(2, 2));
        game.SetMove(moveNum);
        game.Turn();
        moves = game.GetAvailableMoves();
        
        // Assert - возврат на начало клетки джунгли-вертушка
        Assert.Single(moves);
        Assert.Equal(new TilePosition(2, 2), moves.Single().From);
        Assert.Equal(new TilePosition(2, 1, 1), moves.Single().To);
        Assert.Equal(2, game.TurnNo);
    }
}