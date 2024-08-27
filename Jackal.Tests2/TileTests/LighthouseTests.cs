using System.Collections.Generic;
using System.Linq;
using Jackal.Core;
using Xunit;

namespace Jackal.Tests2.TileTests;

public class LighthouseTests
{
    [Fact]
    public void OneLighthouse_GetAvailableMoves_ReturnAllUnknownTiles()
    {
        // Arrange
        var lighthouseOnlyMap = new OneTileMapGenerator(new TileParams(TileType.Lighthouse));
        var game = new TestGame(lighthouseOnlyMap);
        
        // Act - высадка с корабля на маяк
        game.Turn();
        List<Move> moves = game.GetAvailableMoves();
        
        // Assert - все оставшиеся неизвестные клетки: поле 5 клеток минус открытый маяк
        Assert.Equal(4, moves.Count);
        Assert.Equal(new TilePosition(2, 1), moves.First().From);
        Assert.Equivalent(new List<TilePosition>
            {
                new(1, 2), 
                new(2, 2),
                new(2, 3),
                new(3, 2)
            },
            moves.Select(m => m.To)
        );
        
        // тип хода - открытие клетки с маяка
        Assert.True(moves.All(m => m.WithLighthouse));
        Assert.Equal(0, game.TurnNo);
    }
    
    [Fact]
    public void LighthouseThenSearch4Grass_Turn_ReturnGameOver()
    {
        // Arrange
        var lighthouseGrassLineMap = new TwoTileMapGenerator(
            new TileParams(TileType.Lighthouse),
            new TileParams(TileType.Grass)
        );
        var game = new TestGame(lighthouseGrassLineMap);
        
        // Act - высадка с корабля на маяк
        game.Turn();
        
        // по очереди смотрим неизвестные клетки: все пустые поля
        game.Turn();
        game.Turn();
        game.Turn();
        game.Turn();
        
        // Assert - все поле открыто, золота нет = конец игры
        Assert.True(game.IsGameOver);
        Assert.Equal(1, game.TurnNo);
    }
    
    [Fact]
    public void LighthouseThenSearch3LighthouseAndChest_GetAvailableMoves_ReturnNearestMoves()
    {
        // Arrange
        const int coinsOnMap = 1;
        var lighthouseChestLineMap = new ThreeTileMapGenerator(
            new TileParams(TileType.Lighthouse),
            new TileParams(TileType.Lighthouse),
            new TileParams(TileType.Chest1),
            coinsOnMap
        );
        var game = new TestGame(lighthouseChestLineMap);
        
        // Act - высадка с корабля на маяк
        game.Turn();
        
        // по очереди смотрим неизвестные клетки: 3 маяка и 1 сундук
        game.Turn();
        game.Turn();
        game.Turn();
        game.Turn();
        
        List<Move> moves = game.GetAvailableMoves();
        
        // Assert - доступно 4 хода на соседние клетки с маяка в месте высадки
        Assert.Equal(4, moves.Count);
        Assert.Equal(new TilePosition(2, 1), moves.First().From);
        Assert.Equivalent(new List<TilePosition>
            {
                new(1, 2), // соседний маяк
                new(2, 0), // свой корабль
                new(2, 2), // соседний маяк
                new(3, 2) // соседний маяк
            },
            moves.Select(m => m.To)
        );
        
        // тип хода - обычный
        Assert.True(moves.All(m => m.Type == MoveType.Usual));
        
        // Все поле открыто, золото есть = игра продолжается
        Assert.False(game.IsGameOver);
        Assert.Equal(1, game.TurnNo);
    }
}