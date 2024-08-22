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
        List<Move> moves = game.GetAvailableMoves();
        
        // выбираем ход - вперед на следующий самолет
        int moveNum = moves.FindIndex(a => a.To == new TilePosition(2, 2));
        game.SetMove(moveNum);
        game.Turn();
        moves = game.GetAvailableMoves();
        
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
        List<Move> moves = game.GetAvailableMoves();
        
        // выбираем ход - вперед на лед
        int moveNum = moves.FindIndex(a => a.To == new TilePosition(2, 2));
        game.SetMove(moveNum);
        game.Turn();
        moves = game.GetAvailableMoves();
        
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
        List<Move> moves = game.GetAvailableMoves();
        
        // выбираем ход - вперед на крокодила
        int moveNum = moves.FindIndex(a => a.To == new TilePosition(2, 2));
        game.SetMove(moveNum);
        game.Turn();
        moves = game.GetAvailableMoves();
        
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
    public void AirplaneThenArrowOnCrocodile_Turn_ReturnDeadPirate()
    {
        // Arrange
        var airplaneArrowOnCrocodileLineMap = new ThreeTileMapGenerator(
            new TileParams(TileType.Airplane),
            new TileParams(TileType.Arrow) { ArrowsCode = ArrowsCodesHelper.GetCodeFromString("10000000") },
            new TileParams(TileType.Crocodile));
        
        var game = new TestGame(airplaneArrowOnCrocodileLineMap);
        
        // Act - высадка с корабля на самолет
        game.Turn();
        List<Move> moves = game.GetAvailableMoves();
        
        // выбираем ход - вперед на одинарную стрелку перпендикулярно вверх
        int moveNum = moves.FindIndex(a => a.To == new TilePosition(2, 2));
        game.SetMove(moveNum);
        game.Turn();
        
        // единственный ход - вперед по стрелке на крокодила
        game.SetMove(0);
        game.Turn();
        
        // Assert - пират помер
        Assert.Empty(game.Board.AllPirates);
        Assert.NotNull(game.Board.DeadPirates);
        Assert.Single(game.Board.DeadPirates);
        Assert.Equal(1, game.TurnNo);
    }
    
    [Fact]
    public void AirplaneThenCrocodileThenArrowOnCrocodile_Turn_ReturnDeadPirate()
    {
        // Arrange
        var airplaneArrowOnCrocodileLineMap = new ThreeTileMapGenerator(
            new TileParams(TileType.Airplane),
            new TileParams(TileType.Arrow) { ArrowsCode = ArrowsCodesHelper.GetCodeFromString("10000000") },
            new TileParams(TileType.Crocodile));
        
        var game = new TestGame(airplaneArrowOnCrocodileLineMap);
        
        // Act - высадка с корабля на самолет
        game.Turn();
        List<Move> moves = game.GetAvailableMoves();
        
        // выбираем ход - вперед через клетку на крокодила
        int moveNum = moves.FindIndex(a => a.To == new TilePosition(2, 3));
        game.SetMove(moveNum);
        game.Turn();
        moves = game.GetAvailableMoves();
        
        // выбираем ход - на стрелку перед открытым крокодилом
        moveNum = moves.FindIndex(a => a.To == new TilePosition(2, 2));
        game.SetMove(moveNum);
        game.Turn();
        
        // Assert - пират помер
        Assert.Empty(game.Board.AllPirates);
        Assert.NotNull(game.Board.DeadPirates);
        Assert.Single(game.Board.DeadPirates);
        Assert.Equal(1, game.TurnNo);
    }
}