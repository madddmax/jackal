using System.Collections.Generic;
using System.Linq;
using Jackal.Core;
using Xunit;

namespace Jackal.Tests2.TileTests;

public class CannonTests
{
    [Fact]
    public void OneCannonUp_GetAvailableMoves_ReturnOnlyWaterMoves()
    {
        // Arrange
        var cannonOnlyMap = new OneTileMapGenerator(new TileParams(TileType.Cannon) { Direction = DirectionType.Up });
        var game = new TestGame(cannonOnlyMap);
        
        // Act - высадка с корабля на пушку
        game.Turn();
        
        var moves = game.GetAvailableMoves();
        
        // Assert - следующий ход, оказываемся на верху в воде
        // доступно передвижение только по воде
        Assert.Equal(4, moves.Count);
        Assert.Equal(new TilePosition(2, 4), moves.First().From);
        Assert.Equivalent(new List<TilePosition>
            {
                new(1, 4), // влево
                new(1, 3), // влево вниз
                new(3, 4), // вправо
                new(3, 3), // вправо вниз
            },
            moves.Select(m => m.To)
        );
        Assert.Equal(1, game.TurnNo);
    }
    
    [Fact]
    public void OneCannonUp_MoveOnEnemyShipByCannon_ReturnDeadOwnPirate()
    {
        // Arrange
        var cannonOnlyMap = new OneTileMapGenerator(new TileParams(TileType.Cannon) { Direction = DirectionType.Up });
        var game = new TestGame(cannonOnlyMap);
        
        // добавляем пирата противника на корабль противника в место куда прилетает наш пират на пушке
        game.AddEnemyTeamAndSetPirate(new TilePosition(2, 4));
        
        // Act - высадка с корабля на пушку
        game.Turn();
        
        // Assert - наш пират помер
        Assert.NotNull(game.Board.DeadPirates);
        Assert.Single(game.Board.DeadPirates);
        Assert.Equal(0, game.Board.DeadPirates.Single().TeamId);
        Assert.Equal(1, game.TurnNo);
    }
    
    [Fact]
    public void OneCannonRight_GetAvailableMoves_ReturnOnlyWaterMoves()
    {
        // Arrange
        var cannonOnlyMap = new OneTileMapGenerator(new TileParams(TileType.Cannon) {Direction = DirectionType.Right});
        var game = new TestGame(cannonOnlyMap);
        
        // Act - высадка с корабля на пушку
        game.Turn();
        
        var moves = game.GetAvailableMoves();
        
        // Assert - следующий ход, оказываемся справа в воде
        // доступно передвижение только по воде
        Assert.Equal(3, moves.Count);
        Assert.Equal(new TilePosition(4, 1), moves.First().From);
        Assert.Equivalent(new List<TilePosition>
            {
                new(4, 2), // вверх
                new(3, 1), // влево
                new(3, 0), // влево вниз

            },
            moves.Select(m => m.To)
        );
        Assert.Equal(1, game.TurnNo);
    }
    
    [Fact]
    public void OneCannonRight_MoveOnEnemyPirateByCannon_ReturnDeadEnemyPirate()
    {
        // Arrange
        var cannonOnlyMap = new OneTileMapGenerator(new TileParams(TileType.Cannon) {Direction = DirectionType.Right});
        var game = new TestGame(cannonOnlyMap);
        
        // добавляем пирата противника в место куда прилетает наш пират на пушке
        game.AddEnemyTeamAndSetPirate(new TilePosition(4, 1));
        
        // Act - высадка с корабля на пушку
        game.Turn();
        
        // Assert - пират противника помер
        Assert.NotNull(game.Board.DeadPirates);
        Assert.Single(game.Board.DeadPirates);
        Assert.Equal(1, game.Board.DeadPirates.Single().TeamId);
        Assert.Equal(1, game.TurnNo);
    }
    
    [Fact]
    public void OneCannonDown_GetAvailableMoves_ReturnSingleMoveFromShip()
    {
        // Arrange
        var cannonOnlyMap = new OneTileMapGenerator(new TileParams(TileType.Cannon) { Direction = DirectionType.Down });
        var game = new TestGame(cannonOnlyMap);
        
        // Act - высадка с корабля на пушку
        game.Turn();
        
        var moves = game.GetAvailableMoves();
        
        // Assert - следующий ход, оказываемся на своем корабле
        // доступнен один ход - высадка на открытую пушку
        Assert.Single(moves);
        Assert.Equal(new TilePosition(2, 0), moves.Single().From);
        Assert.Equal(new TilePosition(2, 1), moves.Single().To);
        Assert.Equal(1, game.TurnNo);
    }
 
    [Fact]
    public void OneCannonDownRepeatMove_GetAvailableMoves_ReturnSingleMoveFromShip()
    {
        // Arrange
        var cannonOnlyMap = new OneTileMapGenerator(new TileParams(TileType.Cannon) { Direction = DirectionType.Down });
        var game = new TestGame(cannonOnlyMap);
        
        // Act - высадка с корабля на пушку
        game.Turn();
        
        // высадка с корабля на открытую пушку
        game.Turn();
        
        var moves = game.GetAvailableMoves();
        
        // Assert - следующий ход, оказываемся на своем корабле
        // доступнен один ход - высадка на открытую пушку
        Assert.Single(moves);
        Assert.Equal(new TilePosition(2, 0), moves.Single().From);
        Assert.Equal(new TilePosition(2, 1), moves.Single().To);
        Assert.Equal(2, game.TurnNo);
    }
    
    [Fact]
    public void OneCannonLeft_GetAvailableMoves_ReturnOnlyWaterMoves()
    {
        // Arrange
        var cannonOnlyMap = new OneTileMapGenerator(new TileParams(TileType.Cannon) { Direction = DirectionType.Left });
        
        var game = new TestGame(cannonOnlyMap);
        
        // Act - высадка с корабля на пушку
        game.Turn();
        
        var moves = game.GetAvailableMoves();
        
        // Assert - следующий ход, оказываемся слева в воде
        // доступно передвижение только по воде
        Assert.Equal(3, moves.Count);
        Assert.Equal(new TilePosition(0, 1), moves.First().From);
        Assert.Equivalent(new List<TilePosition>
            {
                new(0, 2), // вверх
                new(1, 1), // вправо
                new(1, 0) // вправо вниз
            },
            moves.Select(m => m.To)
        );
        Assert.Equal(1, game.TurnNo);
    }
}