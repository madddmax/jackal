using System.Collections.Generic;
using System.Linq;
using Jackal.Core.Domain;
using Jackal.Core.MapGenerator;
using Xunit;

namespace Jackal.Tests2.TileTests;

public class SpinningTests
{
    [Fact]
    public void OneSpinning_GetAvailableMoves_ReturnSingleSpinningMove()
    {
        // Arrange
        var spinningOnlyMap = new OneTileMapGenerator(
            new TileParams(TileType.Spinning) { SpinningCount = 2 }
        );
        var game = new TestGame(spinningOnlyMap);
        
        // Act - высадка с корабля на джунгли-вертушку
        game.Turn(); // 1 ход
        
        var moves = game.GetAvailableMoves();
        
        // Assert - доступен единственный ход на следующую клетку джунглей-вертушки
        Assert.Single(moves);
        Assert.Equal(new TilePosition(2, 1, 1), moves.First().From);
        Assert.Equal(new TilePosition(2, 1, 0), moves.First().To);
        Assert.Equal(1, game.TurnNo);
    }
    
    [Fact]
    public void OneSpinningFinish_GetAvailableMoves_ReturnNearestMoves()
    {
        // Arrange
        var spinningOnlyMap = new OneTileMapGenerator(
            new TileParams(TileType.Spinning) { SpinningCount = 2 }
        );
        var game = new TestGame(spinningOnlyMap);
        
        // Act - высадка с корабля на джунгли-вертушку
        game.Turn(); // 1 ход
        
        // доходим до конца клетки джунгли-вертушка
        game.Turn(); // 2 ход
        
        var moves = game.GetAvailableMoves();
        
        // Assert - доступно 4 хода на соседние клетки с клетки джунгли-вертушка в месте высадки
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
    public void OneSpinningMaxFinish_GetAvailableMoves_ReturnNearestMoves()
    {
        // Arrange
        var spinningOnlyMap = new OneTileMapGenerator(
            new TileParams(TileType.Spinning) { SpinningCount = 5 }
        );
        var game = new TestGame(spinningOnlyMap);
        
        // Act - высадка с корабля на горы-вертушку
        game.Turn(); // 1 ход
        
        // доходим до конца клетки горы-вертушка
        game.Turn(); // 2 ход
        game.Turn(); // 3 ход
        game.Turn(); // 4 ход
        game.Turn(); // 5 ход
        
        var moves = game.GetAvailableMoves();
        
        // Assert - доступно 4 хода на соседние клетки с клетки горы-вертушка в месте высадки
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
        Assert.Equal(5, game.TurnNo);
    }
    
    [Fact]
    public void SpinningFinishThenCrocodile_GetAvailableMoves_ReturnSpinningStart()
    {
        // Arrange
        var spinningCrocodileLineMap = new TwoTileMapGenerator(
            new TileParams(TileType.Spinning) { SpinningCount = 2 },
            new TileParams(TileType.Crocodile)
        );
        var game = new TestGame(spinningCrocodileLineMap);
        
        // Act - высадка с корабля на джунгли-вертушку
        game.Turn(); // 1 ход
        
        // доходим до конца клетки джунгли-вертушка
        game.Turn(); // 2 ход
        
        // выбираем ход - вперед на крокодила
        game.SetMoveAndTurn(2, 2);

        var moves = game.GetAvailableMoves();
        
        // Assert - возврат на начало клетки джунгли-вертушка
        Assert.Single(moves);
        Assert.Equal(new TilePosition(2, 2), moves.Single().From);
        Assert.Equal(new TilePosition(2, 1, 1), moves.Single().To);
        Assert.Equal(2, game.TurnNo);
    }
    
    [Fact]
    public void LighthouseThenSearch4Spinning_Turn_ReturnGameOver()
    {
        // Arrange
        var lighthouseSpinningLineMap = new TwoTileMapGenerator(
            new TileParams(TileType.Lighthouse), 
            new TileParams(TileType.Spinning) { SpinningCount = 2 }
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
    public void OneSpinning_BeatTheEnemyTurn_ReturnEnemyOnHisShip()
    {
        // Arrange
        var spinningOnlyMap = new OneTileMapGenerator(
            new TileParams(TileType.Spinning) { SpinningCount = 2 }
        );
        var game = new TestGame(spinningOnlyMap);
        
        // Act - высадка с корабля на джунгли-вертушку
        game.Turn(); // 1 ход
        
        // добавляем пирата противника - в месте высадки нашего пирата на последнюю позицию джунгли-вертушки
        game.AddEnemyTeamAndPirate(new TilePosition(2, 1, 0));
        
        // доходим до конца клетки джунгли-вертушка - сбиваем вражеского пирата
        game.Turn(); // 2 ход
        
        // Assert
        Assert.Equal(2, game.Board.Teams.Length);
        Assert.Single(game.Board.Teams[0].Pirates);
        Assert.Single(game.Board.Teams[1].Pirates);

        // наш пират на последней позиции джунгли-вертушки
        var ownPirate = game.Board.Teams[0].Pirates[0];
        Assert.Equal(new TilePosition(2, 1, 0), ownPirate.Position);
        
        // пират противника на своем корабле
        var enemyPirate = game.Board.Teams[1].Pirates[0];
        Assert.Equal(game.Board.Teams[1].Ship.Position, enemyPirate.Position.Position);
        Assert.Equal(new Position(2, 4), enemyPirate.Position.Position);

        Assert.Equal(2, game.TurnNo);
    }
    
    [Fact]
    public void OneSpinningWithCoin_GetAvailableMoves_ReturnTwoMovesWithAndWithoutCoin()
    {
        // Arrange
        var spinningOnlyMap = new OneTileMapGenerator(
            new TileParams(TileType.Spinning) { SpinningCount = 2 }
        );
        var game = new TestGame(spinningOnlyMap);
        
        // Act - высадка с корабля на джунгли-вертушку
        game.Turn(); // 1 ход
        
        // добавляем монету - в месте высадки нашего пирата на первую позицию джунгли-вертушки
        game.AddCoin(new TilePosition(2, 1, 1));
        
        var moves = game.GetAvailableMoves();
        
        // Assert - доступно 2 хода на следующую клетку джунглей-вертушки с монетой и без монеты
        Assert.Equal(2, moves.Count);
        
        var withCoin = moves.Single(m => m.WithCoin);
        Assert.Equal(new TilePosition(2, 1, 1), withCoin.From);
        Assert.Equal(new TilePosition(2, 1, 0), withCoin.To);
        
        var withoutCoin = moves.Single(m => !m.WithCoin);
        Assert.Equal(new TilePosition(2, 1, 1), withoutCoin.From);
        Assert.Equal(new TilePosition(2, 1, 0), withoutCoin.To);
        
        Assert.Equal(1, game.TurnNo);
    }

    [Fact]
    public void OneSpinningWithCoinAndEnemyOnNextPosition_GetAvailableMoves_ReturnSingleMoveWithoutCoin()
    {
        // Arrange
        var spinningOnlyMap = new OneTileMapGenerator(
            new TileParams(TileType.Spinning) { SpinningCount = 2 }
        );
        var game = new TestGame(spinningOnlyMap);
        
        // Act - высадка с корабля на джунгли-вертушку
        game.Turn(); // 1 ход
        
        // добавляем монету - в месте высадки нашего пирата на первую позицию джунгли-вертушки
        game.AddCoin(new TilePosition(2, 1, 1));
        
        // добавляем пирата противника - в месте высадки нашего пирата на последнюю позицию джунгли-вертушки
        game.AddEnemyTeamAndPirate(new TilePosition(2, 1, 0));
        
        var moves = game.GetAvailableMoves();
        
        // Assert - доступен единственный ход на следующую клетку джунглей-вертушки без монеты
        Assert.Single(moves);
        
        var withoutCoin = moves.Single(m => !m.WithCoin);
        Assert.Equal(new TilePosition(2, 1, 1), withoutCoin.From);
        Assert.Equal(new TilePosition(2, 1, 0), withoutCoin.To);
        
        Assert.Equal(1, game.TurnNo);
    }
    
    [Fact]
    public void SpinningThenGrassWithCoinThenSpinningAgainWithEnemyOnFirstPosition_GetAvailableMoves_ReturnAllMoveWithoutCoin()
    {
        // Arrange
        var spinningGrassLineMap = new TwoTileMapGenerator(
            new TileParams(TileType.Spinning) { SpinningCount = 2 },
            new TileParams(TileType.Grass)
        );
        var game = new TestGame(spinningGrassLineMap);
        
        // Act - высадка с корабля на джунгли-вертушку
        game.Turn(); // 1 ход
        game.Turn(); // 2 ход
        
        // выбираем ход - вперед на пустую клетку
        game.SetMoveAndTurn(2,2);
        
        // добавляем монету - на текущую позицию нашего пирата
        game.AddCoin(new TilePosition(2, 2));
        
        // добавляем пирата противника - в месте высадки нашего пирата на первую позицию джунгли-вертушки
        game.AddEnemyTeamAndPirate(new TilePosition(2, 1, 1));
        
        var moves = game.GetAvailableMoves();
        
        // Assert - доступно 4 хода без монеты на соседние клетки из цента карты
        Assert.Equal(4, moves.Count);
        Assert.True(moves.All(m => !m.WithCoin));
        Assert.Equal(3, game.TurnNo);
    }
    
    [Fact]
    public void SpinningThenGrassWithCoinThenSpinningAgainWithEnemyOnLastPosition_GetAvailableMoves_ReturnSingleMoveWithCoin()
    {
        // Arrange
        var spinningGrassLineMap = new TwoTileMapGenerator(
            new TileParams(TileType.Spinning) { SpinningCount = 2 },
            new TileParams(TileType.Grass)
        );
        var game = new TestGame(spinningGrassLineMap);
        
        // Act - высадка с корабля на джунгли-вертушку
        game.Turn(); // 1 ход
        game.Turn(); // 2 ход
        
        // выбираем ход - вперед на пустую клетку
        game.SetMoveAndTurn(2,2);
        
        // добавляем монету - на текущую позицию нашего пирата
        game.AddCoin(new TilePosition(2, 2));
        
        // добавляем пирата противника - в месте высадки нашего пирата на последнюю позицию джунгли-вертушки
        game.AddEnemyTeamAndPirate(new TilePosition(2, 1, 0));
        
        var moves = game.GetAvailableMoves();
        
        // Assert - доступно 4 хода без монеты на соседние клетки из цента карты и 1 ход с монетой на джунгли-вертушку
        Assert.Equal(5, moves.Count);
        Assert.Single(moves, m => m.WithCoin);
        Assert.Equal(3, game.TurnNo);
    }
}