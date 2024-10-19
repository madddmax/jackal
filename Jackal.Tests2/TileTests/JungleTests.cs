using System.Collections.Generic;
using System.Linq;
using Jackal.Core;
using Jackal.Core.Domain;
using Jackal.Core.MapGenerator;
using Xunit;

namespace Jackal.Tests2.TileTests;

public class JungleTests
{
    [Fact]
    public void OneJungle_GetAvailableMoves_ReturnNearestMoves()
    {
        // Arrange
        var jungleOnlyMap = new OneTileMapGenerator(
            new TileParams(TileType.Jungle)
        );
        var game = new TestGame(jungleOnlyMap);
        
        // Act - высадка с корабля на джунгли
        game.Turn();
        
        var moves = game.GetAvailableMoves();
        
        // Assert - доступно 4 хода на соседние клетки с клетки джунгли в месте высадки
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
    
    [Fact]
    public void JungleThenGrassWithCoin_GetAvailableMoves_ReturnAllMoveWithoutCoin()
    {
        const int coinsOnMap = 1;
        
        // Arrange
        var jungleGrassLineMap = new TwoTileMapGenerator(
            new TileParams(TileType.Jungle),
            new TileParams(TileType.Grass),
            coinsOnMap
        );
        var game = new TestGame(jungleGrassLineMap);
        
        // Act - высадка с корабля на джунгли
        game.Turn();
        
        // выбираем ход - вперед на пустую клетку
        game.SetMoveAndTurn(2,2);
        
        // добавляем монету - на текущую позицию нашего пирата
        game.AddCoin(new TilePosition(2, 2));
        
        var moves = game.GetAvailableMoves();
        
        // Assert - доступно 4 хода без монеты на соседние клетки из цента карты
        Assert.Equal(4, moves.Count);
        Assert.True(moves.All(m => !m.WithCoin));
        Assert.Equal(2, game.TurnNo);
    }
    
    [Fact]
    public void JungleThenGrassThenJungleAgainWithEnemy_MoveOnEnemyTurn_ReturnAllPiratesInOneJungle()
    {
        // Arrange
        var jungleGrassLineMap = new TwoTileMapGenerator(
            new TileParams(TileType.Jungle),
            new TileParams(TileType.Grass)
        );
        var game = new TestGame(jungleGrassLineMap);
        
        // Act - высадка с корабля на джунгли
        game.Turn();
        
        // выбираем ход - вперед на пустую клетку
        game.SetMoveAndTurn(2,2);
        
        // добавляем пирата противника - в месте высадки нашего пирата на джунгли
        game.AddEnemyTeamAndPirate(new TilePosition(2, 1));
        
        // выбираем ход - обратно в джунгли в место нашей высадки
        game.SetMoveAndTurn(2,1);
        
        // Assert - все пираты стоят на одной клетке джунгли
        Assert.Equal(2, game.Board.Teams.Length);
        Assert.Single(game.Board.Teams[0].Pirates);
        Assert.Single(game.Board.Teams[1].Pirates);
        
        var ownPirate = game.Board.Teams[0].Pirates[0];
        Assert.Equal(new TilePosition(2, 1), ownPirate.Position);
        
        var enemyPirate = game.Board.Teams[1].Pirates[0];
        Assert.Equal(new TilePosition(2, 1), enemyPirate.Position);

        Assert.Equal(3, game.TurnNo);
    }
}