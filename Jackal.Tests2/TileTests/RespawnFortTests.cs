using System.Collections.Generic;
using System.Linq;
using Jackal.Core;
using Jackal.Core.Domain;
using Jackal.Core.MapGenerator;
using Xunit;

namespace Jackal.Tests2.TileTests;

public class RespawnFortTests
{
    [Fact]
    public void OneRespawnFort_GetAvailableMoves_ReturnNearestMovesAndMoveWithRespawn()
    {
        // Arrange
        var respawnFortOnlyMap = new OneTileMapGenerator(new TileParams(TileType.RespawnFort));
        var game = new TestGame(respawnFortOnlyMap);
        
        // Act - высадка с корабля на воскрешающий форт
        game.Turn();
        var moves = game.GetAvailableMoves();
        
        // Assert - 3 поля рядом + воскрешающий ход на месте + свой корабль
        Assert.Equal(5, moves.Count);
        Assert.Equal(new TilePosition(2, 1), moves.First().From);
        Assert.Equivalent(new List<TilePosition>
            {
                new(1, 2), 
                new(2, 0), // свой корабль
                new(2, 1), // воскрешающий ход на месте
                new(2, 2),
                new(3, 2)
            },
            moves.Select(m => m.To)
        );
        Assert.Equal(1, game.TurnNumber);
    }
    
    [Fact]
    public void OneRespawnFort_MoveWithRespawn_ReturnTwoOwnPirates()
    {
        // Arrange
        var respawnFortOnlyMap = new OneTileMapGenerator(new TileParams(TileType.RespawnFort));
        var game = new TestGame(respawnFortOnlyMap);
        
        // Act - высадка с корабля на воскрешающий форт
        game.Turn();
        
        // воскрешающий ход
        game.SetMoveAndTurn(2, 1);
        
        // Assert - появилось 2 наших пирата
        Assert.Equal(2, game.Board.AllPirates.Count);
        Assert.Equal(0, game.Board.AllPirates[0].TeamId);
        Assert.Equal(0, game.Board.AllPirates[1].TeamId);
        Assert.Equal(2, game.TurnNumber);
    }
    
    [Fact]
    public void OneRespawnFort_DoubleMoveWithRespawn_ReturnThreeOwnPirates()
    {
        // Arrange
        var respawnFortOnlyMap = new OneTileMapGenerator(new TileParams(TileType.RespawnFort));
        var game = new TestGame(respawnFortOnlyMap);
        
        // Act - высадка с корабля на воскрешающий форт
        game.Turn();
        
        // воскрешающий ход
        game.SetMoveAndTurn(2, 1);
        
        // воскрешающий ход
        game.SetMoveAndTurn(2, 1);
        
        // Assert - появилось 2 наших пирата
        Assert.Equal(3, game.Board.AllPirates.Count);
        Assert.Equal(0, game.Board.AllPirates[0].TeamId);
        Assert.Equal(0, game.Board.AllPirates[1].TeamId);
        Assert.Equal(0, game.Board.AllPirates[2].TeamId);
        Assert.Equal(3, game.TurnNumber);
    }
    
    [Fact]
    public void OneRespawnFort_DoubleMoveWithRespawn_ReturnNearestMovesAndNoMoveWithRespawn()
    {
        // Arrange
        var respawnFortOnlyMap = new OneTileMapGenerator(new TileParams(TileType.RespawnFort));
        var game = new TestGame(respawnFortOnlyMap);
        
        // Act - высадка с корабля на воскрешающий форт
        game.Turn();
        
        // воскрешающий ход
        game.SetMoveAndTurn(2, 1);
        
        // воскрешающий ход
        game.SetMoveAndTurn(2, 1);
        
        var moves = game.GetAvailableMoves();
        
        // Assert - 3 поля рядом + свой корабль
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
        Assert.Equal(3, game.TurnNumber);
    }
    
    [Fact]
    public void OneRespawnFortWithBenGunn_GetAvailableMoves_ReturnNearestMovesAndNoMoveWithRespawn()
    {
        // Arrange
        const int piratesPerPlayer = 0;
        var respawnFortOnlyMap = new OneTileMapGenerator(new TileParams(TileType.RespawnFort));
        var game = new TestGame(respawnFortOnlyMap, 5, piratesPerPlayer);
        
        // добавляем Бена Ганна на свой корабль
        game.AddOwnTeamPirate(new TilePosition(2, 0), PirateType.BenGunn);
        
        // Act - высадка с корабля на воскрешающий форт
        game.Turn();
        
        var moves = game.GetAvailableMoves();
        
        // Assert - 3 поля рядом + свой корабль
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
        Assert.Equal(1, game.TurnNumber);
    }
    
    [Fact]
    public void OneRespawnFortWithBenGunnAndUsualPirate_DoubleMoveWithRespawn_ReturnFourOwnPirates()
    {
        // Arrange
        var respawnFortOnlyMap = new OneTileMapGenerator(new TileParams(TileType.RespawnFort));
        var game = new TestGame(respawnFortOnlyMap);
        
        // добавляем Бена Ганна на свой корабль
        game.AddOwnTeamPirate(new TilePosition(2, 0), PirateType.BenGunn);
        
        // Act - высадка с корабля на воскрешающий форт
        game.Turn();
        
        // воскрешающий ход или высадка с корабля Бена/Пирата
        game.SetMoveAndTurn(2, 1);
        
        // воскрешающий ход или высадка с корабля Бена
        game.SetMoveAndTurn(2, 1);
        
        // воскрешающий ход или высадка с корабля Бена
        game.SetMoveAndTurn(2, 1);
        
        // Assert - появилось 3 наших пирата и Бен Ганн
        Assert.Equal(4, game.Board.AllPirates.Count);
        Assert.Equal(0, game.Board.AllPirates[0].TeamId);
        Assert.Equal(0, game.Board.AllPirates[1].TeamId);
        Assert.Equal(0, game.Board.AllPirates[2].TeamId);
        Assert.Equal(0, game.Board.AllPirates[3].TeamId);
        Assert.Equal(4, game.TurnNumber);
    }
    
    [Fact]
    public void ArrowUpOnRespawnFort_GetAvailableMoves_ReturnNearestMovesAndMoveWithRespawn()
    {
        // Arrange
        var arrowUpOnRespawnFortLineMap = new TwoTileMapGenerator(
            TileFactory.OneArrowUp(),
            new TileParams(TileType.RespawnFort)
        );
        var game = new TestGame(arrowUpOnRespawnFortLineMap);
        
        // Act - высадка с корабля на стрелку вперед
        game.Turn();
        
        // автоматом идем вперед на воскрешающий форт
        game.Turn();
        
        var moves = game.GetAvailableMoves();
        
        // Assert - 3 поля рядом + воскрешающий ход на месте
        Assert.Equal(4, moves.Count);
        Assert.Equal(new TilePosition(2, 2), moves.First().From);
        Assert.Equivalent(new List<TilePosition>
            {
                new(1, 2), // влево
                new(2, 2), // воскрешающий ход на месте
                new(2, 3), // вперед
                new(3, 2) // вправо
            },
            moves.Select(m => m.To)
        );
        Assert.Single(moves.Where(m => m.WithRespawn));
        Assert.Equal(1, game.TurnNumber);
    }
    
    [Fact]
    public void ArrowUpOnRespawnFort_GetAvailableMoves_ReturnNearestMovesAndWaitMove()
    {
        // Arrange
        const int mapSize = 5;
        const int piratesPerPlayer = 3;
        
        var arrowUpOnRespawnFortLineMap = new TwoTileMapGenerator(
            TileFactory.OneArrowUp(),
            new TileParams(TileType.RespawnFort)
        );
        var game = new TestGame(arrowUpOnRespawnFortLineMap, mapSize, piratesPerPlayer);
        
        // Act - высадка с корабля на стрелку вперед
        game.Turn();
        
        // автоматом идем вперед на воскрешающий форт
        game.Turn();
        
        var moves = game.GetAvailableMoves();
        
        // Assert - 3 поля рядом + ход на месте через стрелку
        Assert.Equal(5, moves.Count);
        Assert.Equivalent(new List<TilePosition>
            {
                new (2, 0), // пираты на корабле
                new(2, 2) // пират на форте
            },
            moves.Select(m => m.From)
        );
        Assert.Equivalent(new List<TilePosition>
            {
                new (2, 2), // высадка с корабля
                new(1, 2), // влево
                new(2, 2), // ход на месте через стрелку
                new(2, 3), // вперед
                new(3, 2) // вправо
            },
            moves.Select(m => m.To)
        );
        Assert.Empty(moves.Where(m => m.WithRespawn));
        Assert.Equal(1, game.TurnNumber);
    }
}