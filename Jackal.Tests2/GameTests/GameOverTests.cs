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
            TileFactory.Hole()
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
            new TileParams(TileType.Lighthouse), 
            TileFactory.SpinningForest()
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
    public void OnePlayerChest1_MoveAllCoinsToTheShip_ReturnNoGameOver()
    {
        // Arrange
        var totalCoins = 1;
        var chest1EmptyLineMap = new TwoTileMapGenerator(
            TileFactory.Coin(), 
            TileFactory.Empty(),
            totalCoins
        );
        var game = new TestGame(chest1EmptyLineMap);
        
        // Act - высадка с корабля на сундук с одной монетой
        game.Turn();
        
        // переносим монету на корабль
        game.SetMoveAndTurn(2, 0, true);
        
        // Assert - один игрок, карта не открыта, перенесли большую часть золота (всё золото) <> конец игры
        Assert.False(game.IsGameOver);
        Assert.Equal(2, game.TurnNumber);
    }
    
    [Fact]
    public void TwoPlayersChest1_MoveAllCoinsToTheShip_ReturnGameOverByGoldDomination()
    {
        // Arrange
        var totalCoins = 1;
        var chest1EmptyLineMap = new TwoTileMapGenerator(
            TileFactory.Coin(), 
            TileFactory.Empty(),
            totalCoins
        );
        var game = new TestGame(chest1EmptyLineMap);
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
    public void TwoPlayersChest1_EqualAmountOfCoinsThenSearchAllMap_ReturnGameOver()
    {
        // Arrange
        var totalCoins = 2;
        var enemyTeamCoins = 1;
        var chest1LighthouseLineMap = new TwoTileMapGenerator(
            TileFactory.Coin(), 
            new TileParams(TileType.Lighthouse),
            totalCoins
        );
        var game = new TestGame(chest1LighthouseLineMap);
        game.AddEnemyTeamAndPirate(new TilePosition(2, 4), enemyTeamCoins);
        
        // Act - высадка с корабля на сундук с одной монетой
        game.Turn();
        
        // переносим монету на корабль
        game.SetMoveAndTurn(2, 0, true);
        
        // высадка с корабля на сундук с одной монетой
        game.Turn();
        
        // идем вперед на маяк
        game.SetMoveAndTurn(2, 2);
        
        // открываем маяком оставшиеся 3 закрытых клетки 
        game.Turn();
        game.Turn();
        game.Turn();
        
        // Assert - два игрока, карта открыта, перенесли равные части золота = конец игры
        Assert.True(game.IsGameOver);
        Assert.Equal("Победа дружбы путём исследования карты!", game.GameMessage);
        Assert.Equal(4, game.TurnNumber);
    }
    
    [Fact]
    public void TwoPlayersChest2_OneCoinMoveToTheShipOneCoinLost_ReturnGameOverByGoldDomination()
    {
        // Arrange
        var totalCoins = 2;
        var chest1EmptyLineMap = new TwoTileMapGenerator(
            TileFactory.Coin(2), 
            TileFactory.FourArrowsDiagonal(),
            totalCoins
        );
        var game = new TestGame(chest1EmptyLineMap);
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