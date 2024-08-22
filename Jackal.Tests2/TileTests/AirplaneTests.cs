using System.Collections.Generic;
using System.Linq;
using Jackal.Core;
using Xunit;

namespace Jackal.Tests2.TileTests;

public class AirplaneTests
{
    [Fact]
    public void OneAirplane_GetAvailableMoves_ReturnWholeMapAndOwnShip()
    {
        // Arrange
        var airplaneOnlyMap = new OneTileMapGenerator(new TileParams(TileType.Airplane));
        var game = new TestGame(airplaneOnlyMap);
        
        // Act - высадка с корабля на самолет
        game.Turn();
        List<Move> moves = game.GetAvailableMoves();
        
        // Assert - все поле 5 клеток + свой корабль
        Assert.Equal(6, moves.Count);
        Assert.Equal(new TilePosition(2, 1), moves.First().From);
        Assert.Equivalent(new List<TilePosition>
            {
                new(1, 2), 
                new(2, 0), // свой корабль
                new(2, 1), // клетка с самолетом
                new(2, 2),
                new(2, 3),
                new(3, 2)
            },
            moves.Select(m => m.To)
        );
    }
    
    [Fact]
    public void AirplaneThenArrowThenCrocodile_Turn_ReturnDeadPirate()
    {
        // Arrange
        var airplaneArrowCrocodileLineMap = new ThreeTileMapGenerator(
            new TileParams(TileType.Airplane),
            new TileParams(TileType.Arrow) { ArrowsCode = ArrowsCodesHelper.GetCodeFromString("10000000") },
            new TileParams(TileType.Crocodile));
        
        var game = new TestGame(airplaneArrowCrocodileLineMap);
        
        // Act - высадка с корабля на самолет
        game.Turn();
        List<Move> moves = game.GetAvailableMoves();
        
        // выбираем ход - вперед на одинарную стрелку перпендикулярно вверх
        int moveNum = moves.FindIndex(a => a.To == new TilePosition(2, 2));
        game.SetHumanMove(moveNum);
        game.Turn();
        
        // единственный ход - вперед по стрелке на крокодила
        game.SetHumanMove(0);
        game.Turn();
        
        // Assert - пират помер
        Assert.Empty(game.Board.AllPirates);
        Assert.NotNull(game.Board.DeadPirates);
        Assert.Single(game.Board.DeadPirates);
    }
}