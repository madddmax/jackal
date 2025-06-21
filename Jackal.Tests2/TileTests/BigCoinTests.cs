using System.Collections.Generic;
using System.Linq;
using Jackal.Core.Domain;
using Jackal.Core.MapGenerator;
using Xunit;

namespace Jackal.Tests2.TileTests;

public class BigCoinTests
{
    [Fact]
    public void OneBigCoin_GetAvailableMoves_ReturnNearestMovesAndMoveWithBigCoin()
    {
        // Arrange
        var bigCoinOnlyMap = new OneTileMapGenerator(
            new TileParams(TileType.BigCoin)
        );
        var game = new TestGame(bigCoinOnlyMap);
        
        // Act - высадка с корабля на большую монету
        game.Turn();
        var moves = game.GetAvailableMoves();
        
        // Assert - доступно 5 ходов: 3 на соседние клетки в месте высадки
        // + 2 на свой корабль с большой монетой и без неё
        Assert.Equal(5, moves.Count);
        Assert.Equal(new TilePosition(2, 1), moves.First().From);
        
        Assert.Equivalent(new List<MoveType>
            {
                MoveType.Usual,
                MoveType.WithBigCoin
            },
            moves.Where(m => m.To == new TilePosition(2, 0)).Select(m => m.Type)
        );
        
        Assert.Equal(1, game.TurnNumber);
    }
    
    [Fact]
    public void OneBigCoin_TakeBigCoinOnShip_AddThreeCoinsToTeamScore()
    {
        // Arrange
        var bigCoinOnlyMap = new OneTileMapGenerator(
            new TileParams(TileType.BigCoin)
        );
        var game = new TestGame(bigCoinOnlyMap);
        
        // Act - высадка с корабля на большую монету
        game.Turn();
        
        // обратно на корабль с большой монетой
        game.SetMoveAndTurn(2, 0, withBigCoin: true);
        var moves = game.GetAvailableMoves();
        
        // Assert - доступен один ход - высадка с корабля
        Assert.Single(moves);
        Assert.Equal(new TilePosition(2, 0), moves.Single().From);
        
        Assert.Equal(3, game.Board.Teams.Single().Coins);
        Assert.Equal(2, game.TurnNumber);
    }

    [Fact]
    public void OneEmptyBigCoin_GetAvailableMoves_ReturnNearestMovesWithoutBigCoin()
    {
        // Arrange
        var bigCoinOnlyMap = new OneTileMapGenerator(
            new TileParams(TileType.BigCoin)
        );
        var game = new TestGame(bigCoinOnlyMap);
        
        // Act - высадка с корабля на большую монету
        game.Turn();
        
        // обратно на корабль с большой монетой
        game.SetMoveAndTurn(2, 0, withBigCoin: true);
        
        // высадка с корабля
        game.Turn();
        var moves = game.GetAvailableMoves();
        
        // Assert - доступно 4 хода на соседние клетки в месте высадки
        Assert.Equal(4, moves.Count);
        Assert.Equal(new TilePosition(2, 1), moves.First().From);

        Assert.Equivalent(new List<MoveType>
            {
                MoveType.Usual,
            },
            moves.Select(m => m.Type)
        );
        
        Assert.Equal(3, game.TurnNumber);
    }
}