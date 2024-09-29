using System.Collections.Generic;
using System.Linq;
using Jackal.Core;
using Jackal.Core.MapGenerator;
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
        
        var moves = game.GetAvailableMoves();
        
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
        Assert.Equal(0, game.TurnNo);
    }
    
    [Fact]
    public void AirplaneThenNextAirplane_GetAvailableMoves_ReturnWholeMapAndOwnShip()
    {
        // Arrange
        var airplaneOnlyMap = new OneTileMapGenerator(new TileParams(TileType.Airplane));
        var game = new TestGame(airplaneOnlyMap);
        
        // Act - высадка с корабля на самолет
        game.Turn();
        
        // выбираем ход - вперед на следующий самолет
        game.SetMoveAndTurn(2, 2);

        var moves = game.GetAvailableMoves();
        
        // Assert - все поле 5 клеток + свой корабль 
        Assert.Equal(6, moves.Count);
        Assert.Equal(new TilePosition(2, 2), moves.First().From);
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
        Assert.Equal(0, game.TurnNo);
    }
    
    // TODO-TEST AirplaneThenNextAirplaneThenPrevAirplane_GetAvailableMoves_ReturnAroundFirstAirplaneTiles()
    
    [Fact]
    public void AirplaneThenIce_GetAvailableMoves_ReturnWholeMapAndOwnShipExceptOpenIce()
    {
        // Arrange
        var airplaneIceLineMap = new TwoTileMapGenerator(
            new TileParams(TileType.Airplane),
            new TileParams(TileType.Ice));
        
        var game = new TestGame(airplaneIceLineMap);
        
        // Act - высадка с корабля на самолет
        game.Turn();
        
        // выбираем ход - вперед на лед
        game.SetMoveAndTurn(2, 2);

        var moves = game.GetAvailableMoves();
        
        // Assert - все поле 5 клеток + свой корабль, кроме открытого льда 
        Assert.Equal(5, moves.Count);
        Assert.Equal(new TilePosition(2, 2), moves.First().From);
        Assert.Equivalent(new List<TilePosition>
            {
                new(1, 2), 
                new(2, 0), // свой корабль
                new(2, 1), // клетка с самолетом
                new(2, 3),
                new(3, 2)
            },
            moves.Select(m => m.To)
        );
        Assert.Equal(0, game.TurnNo);
    }
    
    [Fact]
    public void AirplaneThenCrocodile_GetAvailableMoves_ReturnWholeMapAndOwnShipExceptOpenCrocodile()
    {
        // Arrange
        var airplaneIceLineMap = new TwoTileMapGenerator(
            new TileParams(TileType.Airplane),
            new TileParams(TileType.Crocodile));
        
        var game = new TestGame(airplaneIceLineMap);
        
        // Act - высадка с корабля на самолет
        game.Turn();
        
        // выбираем ход - вперед на крокодила
        game.SetMoveAndTurn(2, 2);

        var moves = game.GetAvailableMoves();
        
        // Assert - все поле 5 клеток + свой корабль, кроме открытого крокодила
        Assert.Equal(5, moves.Count);
        Assert.Equal(new TilePosition(2, 2), moves.First().From);
        Assert.Equivalent(new List<TilePosition>
            {
                new(1, 2), 
                new(2, 0), // свой корабль
                new(2, 1), // клетка с самолетом
                new(2, 3),
                new(3, 2)
            },
            moves.Select(m => m.To)
        );
        Assert.Equal(0, game.TurnNo);
    }
    
    [Fact]
    public void AirplaneThenArrowUpOnCrocodile_Turn_ReturnDeadPirate()
    {
        // Arrange
        var airplaneArrowUpOnCrocodileLineMap = new ThreeTileMapGenerator(
            new TileParams(TileType.Airplane),
            new TileParams(TileType.Arrow) { ArrowsCode = ArrowsCodesHelper.OneArrowUp },
            new TileParams(TileType.Crocodile));
        
        var game = new TestGame(airplaneArrowUpOnCrocodileLineMap);
        
        // Act - высадка с корабля на самолет
        game.Turn();
        
        // выбираем ход - вперед на одинарную стрелку перпендикулярно вверх
        game.SetMoveAndTurn(2, 2);
        
        // единственный ход - вперед по стрелке на крокодила
        game.Turn();
        
        // Assert - пират помер
        Assert.Empty(game.Board.AllPirates);
        Assert.NotNull(game.Board.DeadPirates);
        Assert.Single(game.Board.DeadPirates);
        Assert.Equal(1, game.TurnNo);
    }
    
    [Fact]
    public void AirplaneThenCrocodileThenArrowUpOnCrocodile_Turn_ReturnDeadPirate()
    {
        // Arrange
        var airplaneArrowUpOnCrocodileLineMap = new ThreeTileMapGenerator(
            new TileParams(TileType.Airplane),
            new TileParams(TileType.Arrow) { ArrowsCode = ArrowsCodesHelper.OneArrowUp },
            new TileParams(TileType.Crocodile));
        
        var game = new TestGame(airplaneArrowUpOnCrocodileLineMap);
        
        // Act - высадка с корабля на самолет
        game.Turn();
        
        // выбираем ход - вперед через клетку на крокодила
        game.SetMoveAndTurn(2, 3);
        
        // выбираем ход - на стрелку перед открытым крокодилом
        game.SetMoveAndTurn(2, 2);
        
        // Assert - пират помер
        Assert.Empty(game.Board.AllPirates);
        Assert.NotNull(game.Board.DeadPirates);
        Assert.Single(game.Board.DeadPirates);
        Assert.Equal(1, game.TurnNo);
    }
    
    [Fact]
    public void OneAirplaneWait_GetAvailableMoves_ReturnWholeMapAndOwnShip()
    {
        // Arrange
        var airplaneOnlyMap = new OneTileMapGenerator(new TileParams(TileType.Airplane));
        var game = new TestGame(airplaneOnlyMap);
        
        // Act - высадка с корабля на самолет
        game.Turn();

        // пропускаем ход самолета
        game.SetMoveAndTurn(2, 1);
        
        var moves = game.GetAvailableMoves();
        
        // Assert - следующий ход, доступен ход самолета
        // все поле 5 клеток + свой корабль
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
        Assert.Equal(1, game.TurnNo);
    }
    
    [Fact]
    public void OneAirplaneWaitThenSameAirplaneMoveByNextPirate_GetAvailableMoves_ReturnWholeMapAndOwnShip()
    {
        // Arrange
        const int piratesPerPlayer = 2;
        var airplaneOnlyMap = new OneTileMapGenerator(new TileParams(TileType.Airplane));
        var game = new TestGame(airplaneOnlyMap, 5, piratesPerPlayer);
        
        // Act - высадка с корабля на самолет
        game.Turn();

        // пропускаем ход самолета
        game.SetMoveAndTurn(2, 1);

        // ходим вторым пиратом с корабля на самолет
        var from = new TilePosition(2, 0);
        var to = new TilePosition(2, 1);
        game.SetMoveAndTurn(from, to);
        
        var moves = game.GetAvailableMoves();
        
        // Assert - продолжается ход второго пирата, доступен ход самолета
        // все поле 5 клеток + свой корабль
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
        Assert.Equal(1, game.TurnNo);
    }
}