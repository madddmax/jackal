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
            new TileParams(TileType.Grass)
        );
        var game = new TestGame(lighthouseSpinningLineMap);
        
        // Act - высадка с корабля на маяк
        game.Turn();
        
        // по очереди смотрим маяком неизвестные клетки: все джунгли-вертушки
        game.Turn();
        game.Turn();
        game.Turn();
        game.Turn();
        
        // Assert - все поле открыто, золота нет = конец игры
        Assert.True(game.IsGameOver);
        Assert.Equal(1, game.TurnNo);
    }
    
    [Fact]
    public void TwoPlayersLighthouse_TurnSearchAllMap_ReturnGameOver()
    {
        // Arrange
        var totalCoins = 0;
        var lighthouseSpinningLineMap = new TwoTileMapGenerator(
            new TileParams(TileType.Lighthouse), 
            new TileParams(TileType.Grass),
            totalCoins
        );
        var game = new TestGame(lighthouseSpinningLineMap);
        game.AddEnemyTeamAndPirate(new TilePosition(2, 4));
        
        // Act - высадка с корабля на маяк
        game.Turn();
        
        // по очереди смотрим маяком неизвестные клетки: все джунгли-вертушки
        game.Turn();
        game.Turn();
        game.Turn();
        game.Turn();
        
        // Assert - все поле открыто, золота нет = конец игры
        Assert.True(game.IsGameOver);
        Assert.Equal(1, game.TurnNo);
    }
}