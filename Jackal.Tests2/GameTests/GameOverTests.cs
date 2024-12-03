using Jackal.Core.Domain;
using Jackal.Core.MapGenerator;
using Xunit;

namespace Jackal.Tests2.GameTests;

public class GameOverTests
{
    [Fact]
    public void OnePlayerLighthouse_TurnSearchAllMap_ReturnGameOver()
    {
        // Arrange
        var lighthouseSpinningLineMap = new TwoTileMapGenerator(
            new TileParams(TileType.Lighthouse), 
            new TileParams(TileType.Spinning) {SpinningCount = 2}
        );
        var game = new TestGame(lighthouseSpinningLineMap);
        
        // Act - высадка с корабля на маяк
        game.Turn();
        
        // по очереди смотрим маяком неизвестные клетки
        game.Turn();
        game.Turn();
        game.Turn();
        game.Turn();
        
        // Assert - один игрок, вся карта открыта, золота нет = конец игры
        Assert.True(game.IsGameOver);
        Assert.Equal(1, game.TurnNo);
    }
    
    [Fact]
    public void OnePlayerChest1_MoveAllCoinsToTheShip_ReturnNoGameOver()
    {
        // Arrange
        var totalCoins = 1;
        var chest1GrassLineMap = new TwoTileMapGenerator(
            new TileParams(TileType.Chest1), 
            new TileParams(TileType.Grass),
            totalCoins
        );
        var game = new TestGame(chest1GrassLineMap);
        
        // Act - высадка с корабля на сундук с одной монетой
        game.Turn();
        
        // переносим монету на корабль
        game.SetMoveAndTurn(2, 0, true);
        
        // Assert - один игрок, карта не открыта, перенесли большую часть золота (всё золото) <> конец игры
        Assert.False(game.IsGameOver);
        Assert.Equal(2, game.TurnNo);
    }
    
    [Fact]
    public void TwoPlayersChest1_MoveAllCoinsToTheShip_ReturnGameOver()
    {
        // Arrange
        var totalCoins = 1;
        var chest1GrassLineMap = new TwoTileMapGenerator(
            new TileParams(TileType.Chest1), 
            new TileParams(TileType.Grass),
            totalCoins
        );
        var game = new TestGame(chest1GrassLineMap);
        game.AddEnemyTeamAndPirate(new TilePosition(2, 4));
        
        // Act - высадка с корабля на сундук с одной монетой
        game.Turn();
        
        // переносим монету на корабль
        game.SetMoveAndTurn(2, 0, true);
        
        // Assert - два игрока, карта не открыта, перенесли большую часть золота (всё золото) = конец игры
        Assert.True(game.IsGameOver);
        Assert.Equal(2, game.TurnNo);
    }
}