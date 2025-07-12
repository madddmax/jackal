using System.Collections.Generic;
using System.Linq;
using Jackal.Core.Domain;
using Jackal.Core.MapGenerator;
using Xunit;

namespace Jackal.Tests2.TileTests;

public class RumBottleTests
{
    [Fact]
    public void OneRum_GetAvailableMoves_ReturnNearestMovesAndOneRumBottel()
    {
        // Arrange
        var oneRumOnlyMap = new OneTileMapGenerator(
            new TileParams(TileType.RumBottle, 1)
        );
        var game = new TestGame(oneRumOnlyMap);
        
        // Act - высадка с корабля на бутылку с ромом
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
            new TileParams(TileType.RumBottle, 2)
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
            new TileParams(TileType.RumBottle, 3)
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
    
    [Fact]
    public void RumBottleThenTrap_GetAvailableMoves_ReturnNearestMovesWithRumBottle()
    {
        // Arrange
        var rumBottleTrapLineMap = new TwoTileMapGenerator(
            new TileParams(TileType.RumBottle, 1),
            new TileParams(TileType.Trap)
        );
        var game = new TestGame(rumBottleTrapLineMap);
        
        // Act - высадка с корабля на бутылку с ромом
        game.Turn();
        
        // выбираем ход - вперед на ловушку
        game.SetMoveAndTurn(2, 2);
        
        var moves = game.GetAvailableMoves();
        
        // Assert - доступно 4 хода из ловушки за бутылку рома на соседние клетки
        Assert.Equal(4, moves.Count);
        Assert.Equal(new TilePosition(2, 2), moves.First().From);
        Assert.Equivalent(new List<TilePosition>
            {
                new(1, 2),
                new(2, 1),
                new(2, 3),
                new(3, 2)
            },
            moves.Select(m => m.To)
        );
        Assert.Equal(4, moves.Count(m => m.WithRumBottle));
        Assert.Equal(2, game.TurnNumber);
    }
    
    [Fact]
    public void RumBottleThenSpinning_GetAvailableMoves_ReturnNearestMovesWithRumBottleAndOneSpinningMove()
    {
        // Arrange
        var rumBottleSpinningLineMap = new TwoTileMapGenerator(
            new TileParams(TileType.RumBottle, 1),
            TileFactory.SpinningMount()
        );
        var game = new TestGame(rumBottleSpinningLineMap);
        
        // Act - высадка с корабля на бутылку с ромом
        game.Turn();
        
        // выбираем ход - вперед на гору
        game.SetMoveAndTurn(2, 2);
        
        var moves = game.GetAvailableMoves();
        
        // Assert - доступно 5 ходов: 4 из горы за бутылку рома на соседние клетки и 1 ход по горе 
        Assert.Equal(5, moves.Count);
        Assert.Equal(new TilePosition(2, 2, 4), moves.First().From);
        Assert.Equivalent(new List<TilePosition>
            {
                new(1, 2),
                new(2, 1),
                new(2, 3),
                new(3, 2),
                new(2,2, 3)
            },
            moves.Select(m => m.To)
        );
        Assert.Equal(4, moves.Count(m => m.WithRumBottle));
        Assert.Equal(1, moves.Count(m => !m.WithRumBottle));
        Assert.Equal(2, game.TurnNumber);
    }
    
    [Fact]
    public void LighthouseThenSearchRumBottles_GetAvailableMoves_ReturnNearestMovesAndNoRumBottles()
    {
        // Arrange
        var rumBottleTrapLineMap = new TwoTileMapGenerator(
            new TileParams(TileType.Lighthouse),
            new TileParams(TileType.RumBottle, 1)
        );
        var game = new TestGame(rumBottleTrapLineMap);
        
        // Act - высадка с корабля на маяк
        game.Turn();
        
        // по очереди смотрим неизвестные клетки с бутылками
        game.Turn(); // 1
        game.Turn(); // 2
        game.Turn(); // 3
        game.Turn(); // 4
        
        var moves = game.GetAvailableMoves();
        
        // Assert - доступно 4 хода на соседние клетки в месте высадки и 0 бутылок с ромом
        Assert.Equal(4, moves.Count);
        Assert.Equal(new TilePosition(2, 1), moves.First().From);
        Assert.Equal(0, game.Board.Teams.Single().RumBottles);
        Assert.Equal(1, game.TurnNumber);
    }
}