using System.Collections.Generic;
using System.Linq;
using Jackal.Core;
using Xunit;

namespace Jackal.Tests2.TileTests;

public class CarambaTests
{
    [Fact]
    public void OneCaramba_MoveOn_ReturnAllPiratesOnTheShips()
    {
        // Arrange
        var carambaOnlyMap = new OneTileMapGenerator(new TileParams(TileType.Caramba));
        var game = new TestGame(carambaOnlyMap);
        
        // добавляем пирата противника в воду, место выбрано случайно
        game.AddEnemyTeamAndPirate(new TilePosition(4, 1));
        
        // Act - высадка с корабля на карамбу
        game.Turn();
        
        // Assert - все пираты на своих кораблях
        Assert.Equal(2, game.Board.Teams.Length);
        Assert.Single(game.Board.Teams[0].Pirates);
        Assert.Single(game.Board.Teams[1].Pirates);

        var ownPirate = game.Board.Teams[0].Pirates[0];
        Assert.Equal(game.Board.Teams[0].Ship.Position, ownPirate.Position.Position);
        Assert.Equal(new Position(2, 0), ownPirate.Position.Position);
        
        var enemyPirate = game.Board.Teams[1].Pirates[0];
        Assert.Equal(game.Board.Teams[1].Ship.Position, enemyPirate.Position.Position);
        Assert.Equal(new Position(2, 4), enemyPirate.Position.Position);

        Assert.Equal(1, game.TurnNo);
    }
    
    [Fact]
    public void OneUsedCaramba_GetAvailableMoves_ReturnNearestMoves()
    {
        // Arrange
        var carambaOnlyMap = new OneTileMapGenerator(new TileParams(TileType.Caramba));
        var game = new TestGame(carambaOnlyMap);
        
        // Act - высадка с корабля на карамбу
        game.Turn();
        
        // высадка с корабля на использованную карамбу
        game.Turn();
        
        var moves = game.GetAvailableMoves();
        
        // Assert - доступно 4 хода на соседние клетки с Бен Ганна в месте высадки
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
        Assert.Equal(2, game.TurnNo);
    }
    
    [Fact]
    public void LighthouseThenSearchCaramba_Turn_ReturnAllPiratesOnTheShips()
    {
        // Arrange
        var lighthouseCarambaLineMap = new TwoTileMapGenerator(
            new TileParams(TileType.Lighthouse), new TileParams(TileType.Caramba)
        );
        var game = new TestGame(lighthouseCarambaLineMap);
        
        // добавляем пирата противника в воду, место выбрано случайно
        game.AddEnemyTeamAndPirate(new TilePosition(4, 1));
        
        // Act - высадка с корабля на маяк
        game.Turn();
        
        // открытие маяком карамбы
        game.Turn();
        
        // Assert - все пираты на своих кораблях
        Assert.Equal(2, game.Board.Teams.Length);
        Assert.Single(game.Board.Teams[0].Pirates);
        Assert.Single(game.Board.Teams[1].Pirates);

        var ownPirate = game.Board.Teams[0].Pirates[0];
        Assert.Equal(game.Board.Teams[0].Ship.Position, ownPirate.Position.Position);
        Assert.Equal(new Position(2, 0), ownPirate.Position.Position);
        
        var enemyPirate = game.Board.Teams[1].Pirates[0];
        Assert.Equal(game.Board.Teams[1].Ship.Position, enemyPirate.Position.Position);
        Assert.Equal(new Position(2, 4), enemyPirate.Position.Position);

        Assert.Equal(0, game.TurnNo);
    }
}