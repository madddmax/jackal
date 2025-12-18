using Jackal.Core.Domain;
using Jackal.Core.MapGenerator;
using Xunit;

namespace Jackal.Tests2.GameTests;

public class GameOverTests
{
    [Fact]
    public void OnePlayerHole_MoveIntoTheHole_ReturnGameOverByAllPiratesEnd()
    {
        // Arrange
        var holeOnlyMap = new OneTileMapGenerator(
            TileParams.Hole()
        );
        var game = new TestGame(holeOnlyMap);
        
        // Act - высадка с корабля на дыру
        game.Turn();
        
        // Assert - все пираты (один) застряли в дыре, карта не открыта = конец игры
        Assert.True(game.IsGameOver);
        Assert.Equal("Победа HumanPlayer путём конца всех пиратов!", game.GameMessage);
        Assert.Equal(1, game.TurnNumber);
    }
    
    [Fact]
    public void OnePlayerLighthouse_TurnSearchAllMap_ReturnGameOverByMapExploration()
    {
        // Arrange
        var lighthouseSpinningLineMap = new TwoTileMapGenerator(
            TileParams.Lighthouse(), 
            TileParams.SpinningForest()
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
        Assert.Equal("Победа HumanPlayer путём исследования карты!", game.GameMessage);
        Assert.Equal(1, game.TurnNumber);
    }
    
    [Fact]
    public void OnePlayerCoin1_MoveAllCoinsToTheShip_ReturnNoGameOver()
    {
        // Arrange
        var totalCoins = 1;
        var coin1EmptyLineMap = new TwoTileMapGenerator(
            TileParams.Coin(), 
            TileParams.Empty(),
            totalCoins
        );
        var game = new TestGame(coin1EmptyLineMap);
        
        // Act - высадка с корабля на сундук с одной монетой
        game.Turn();
        
        // переносим монету на корабль
        game.SetMoveAndTurn(2, 0, true);
        
        // Assert - один игрок, карта не открыта, перенесли большую часть золота (всё золото) <> конец игры
        Assert.False(game.IsGameOver);
        Assert.Equal(2, game.TurnNumber);
    }
    
    [Fact]
    public void TwoPlayersCoin1_MoveAllCoinsToTheShip_ReturnGameOverByGoldDomination()
    {
        // Arrange
        var totalCoins = 1;
        var coin1EmptyLineMap = new TwoTileMapGenerator(
            TileParams.Coin(), 
            TileParams.Empty(),
            totalCoins
        );
        var game = new TestGame(coin1EmptyLineMap);
        game.AddEnemyTeamAndPirate(new TilePosition(2, 4));
        
        // Act - высадка с корабля на сундук с одной монетой
        game.Turn();
        
        // переносим монету на корабль
        game.SetMoveAndTurn(2, 0, true);
        
        // Assert - два игрока, карта не открыта, перенесли большую часть золота (всё золото) = конец игры
        Assert.True(game.IsGameOver);
        Assert.Equal("Победа HumanPlayer путём доминирования по золоту!", game.GameMessage);
        Assert.Equal(2, game.TurnNumber);
    }
    
    [Fact]
    public void TwoPlayersCoin1_EqualAmountOfCoinsNoCoinsOnMap_ReturnGameOver()
    {
        // Arrange
        var totalCoins = 2;
        var enemyTeamCoins = 1;
        var coin1LighthouseLineMap = new TwoTileMapGenerator(
            TileParams.Coin(), 
            TileParams.Lighthouse(),
            totalCoins
        );
        var game = new TestGame(coin1LighthouseLineMap);
        game.AddEnemyTeamAndPirate(new TilePosition(2, 4), enemyTeamCoins);
        
        // Act - высадка с корабля на сундук с одной монетой
        game.Turn();
        
        // переносим монету на корабль
        game.SetMoveAndTurn(2, 0, true);
        
        // Assert - два игрока, карта не открыта, золота нет на карте, перенесли равные части золота = конец игры
        Assert.True(game.IsGameOver);
        Assert.Equal("Победа дружбы путём доминирования по золоту!", game.GameMessage);
        Assert.Equal(2, game.TurnNumber);
    }
    
    [Fact]
    public void TwoPlayersCoin2_OneCoinMoveToTheShipOneCoinLost_ReturnGameOverByGoldDomination()
    {
        // Arrange
        var totalCoins = 2;
        var coin1EmptyLineMap = new TwoTileMapGenerator(
            TileParams.Coin(2), 
            TileParams.FourArrowsDiagonal(),
            totalCoins
        );
        var game = new TestGame(coin1EmptyLineMap);
        game.AddEnemyTeamAndPirate(new TilePosition(2, 4));
        
        // Act - высадка с корабля на сундук с двумя монетами
        game.Turn();

        // открываем четыре стрелки по диагонали на все углы
        game.SetMoveAndTurn(2, 2);
        
        // прыгаем в воду по стрелке рядом со своим кораблем
        game.SetMoveAndTurn(1, 1);
        
        // залезаем из воды на свой корабль
        game.SetMoveAndTurn(2, 0);
        
        // высадка с корабля на сундук с двумя монетами
        game.Turn();
        
        // переносим монету на корабль
        game.SetMoveAndTurn(2, 0, true);
        
        // высадка с корабля на сундук с двумя монетами
        game.Turn();
        
        // топим вторую монету по стрелке рядом со своим кораблем
        game.SetMoveAndTurn(1, 1, true);
        
        // Assert - два игрока, карта не открыта, перенесли большую часть золота = конец игры
        Assert.True(game.IsGameOver);
        Assert.Equal("Победа HumanPlayer путём доминирования по золоту!", game.GameMessage);
        Assert.Equal(7, game.TurnNumber);
    }
}