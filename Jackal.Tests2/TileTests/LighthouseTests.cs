using System.Collections.Generic;
using System.Linq;
using Jackal.Core;
using Jackal.Core.Domain;
using Jackal.Core.MapGenerator;
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
        var moves = game.GetAvailableMoves();
        
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
        
        var moves = game.GetAvailableMoves();
        
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
    
    [Fact]
    public void LighthouseThenSearch2LighthouseThenSearch10Crocodile_GetAvailableMoves_ReturnNearestMoves()
    {
        // Arrange
        var lighthouseCrocodileLineMap = new TwoTileMapGenerator(
            new TileParams(TileType.Lighthouse),
            new TileParams(TileType.Crocodile)
        );
        var mapSize = 7; // карта большая - возможно движение корабля
        var game = new TestGame(lighthouseCrocodileLineMap, mapSize);
        
        // Act - высадка с корабля вперед на маяк
        game.SetMoveAndTurn(3, 1);
        
        // выбираем ход просветку - влево на маяк 1
        game.SetMoveAndTurn(2, 1);
        
        // выбираем ход просветку - вправо на маяк 2
        game.SetMoveAndTurn(4, 1);
        
        // по очереди смотрим неизвестные клетки с крокодилами, т.к. только они остались
        game.Turn(); // 1
        game.Turn(); // 2
        game.Turn(); // 3
        game.Turn(); // 4
        game.Turn(); // 5
        game.Turn(); // 6
        game.Turn(); // 7
        game.Turn(); // 8
        game.Turn(); // 9
        game.Turn(); // 10
        // в результате с 3-ех маяков посмотрели 12 клеток = 2 маяка + 10 крокодилов
        
        var moves = game.GetAvailableMoves();
        
        // Assert - доступно >= 3 хода с маяка в месте высадки, т.к. впереди крокодилы или неизвестные клетки
        Assert.True(moves.Count >= 3);
        Assert.Equal(new TilePosition(3, 1), moves.First().From);
        Assert.Contains(new TilePosition(2, 1), moves.Select(m => m.To)); // левый маяк
        Assert.Contains(new TilePosition(3, 0), moves.Select(m => m.To)); // свой корабль
        
        // тип хода - обычный
        Assert.True(moves.All(m => m.Type == MoveType.Usual));
        Assert.Equal(1, game.TurnNo);
    }
}