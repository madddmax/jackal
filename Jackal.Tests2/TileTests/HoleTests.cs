using System.Collections.Generic;
using System.Linq;
using Jackal.Core;
using Jackal.Core.Domain;
using Jackal.Core.MapGenerator;
using Xunit;

namespace Jackal.Tests2.TileTests;

public class HoleTests
{
    [Fact]
    public void OneHole_GetAvailableMoves_ReturnNoAvailableMoves()
    {
        // Arrange
        var holeOnlyMap = new OneTileMapGenerator(
            new TileParams(TileType.Hole)
        );
        var game = new TestGame(holeOnlyMap);
        
        // Act - высадка с корабля на дыру
        game.Turn();
        
        var moves = game.GetAvailableMoves();
        
        // Assert - не доступно ни одного хода, т.к. застряли в дыре
        Assert.Empty(moves);
        Assert.Equal(1, game.TurnNo);
    }
    
    [Fact]
    public void GrassThenHoleThenNextPirateGrassAndSameHole_GetAvailableMoves_ReturnNoAvailableMoves()
    {
        // Arrange
        var grassHoleLineMap = new TwoTileMapGenerator(
            new TileParams(TileType.Grass),
            new TileParams(TileType.Hole)
        );
        const int mapSize = 5;
        const int piratesPerPlayer = 2;
        var game = new TestGame(grassHoleLineMap, mapSize, piratesPerPlayer);
        
        // Act - высадка с корабля на пустое поле
        game.Turn();
        
        // выбираем ход - вперед на дыру
        game.SetMoveAndTurn(2, 2);
        
        // высадка с корабля вторым пиратом на пустое поле
        game.Turn();
        
        // выбираем ход - вперед на дыру
        game.SetMoveAndTurn(2, 2);
        
        var moves = game.GetAvailableMoves();
        
        // Assert - не доступно ни одного хода, т.к. оба пирата застряли в дыре
        Assert.Empty(moves);
        Assert.Equal(4, game.TurnNo);
    }
    
    [Fact]
    public void GrassThenHoleThenNextPirateGrassAndOtherHole_GetAvailableMoves_ReturnAvailableMoves()
    {
        // Arrange
        var grassHoleLineMap = new TwoTileMapGenerator(
            new TileParams(TileType.Grass),
            new TileParams(TileType.Hole)
        );
        const int mapSize = 5;
        const int piratesPerPlayer = 2;
        var game = new TestGame(grassHoleLineMap, mapSize, piratesPerPlayer);
        
        // Act - высадка с корабля на пустое поле
        game.Turn();
        
        // выбираем ход - вперед на дыру
        game.SetMoveAndTurn(2, 2);
        
        // высадка с корабля вторым пиратом на пустое поле
        game.Turn();
        
        // выбираем ход - вперед и вправо на другую дыру
        game.SetMoveAndTurn(3, 2);
        
        var moves = game.GetAvailableMoves();
        
        // Assert - доступны новые ходы
        Assert.True(moves.Count > 0);
        Assert.Equal(4, game.TurnNo);
    }
    
    [Fact]
    public void MoveOnHoleWhenEnemyPirateInOtherHole_GetAvailableMoves_ReturnNearestMoves()
    {
        // Arrange
        var grassHoleLineMap = new TwoTileMapGenerator(
            new TileParams(TileType.Grass),
            new TileParams(TileType.Hole)
        );
        const int mapSize = 5;
        const int piratesPerPlayer = 2;
        var game = new TestGame(grassHoleLineMap, mapSize, piratesPerPlayer);
        
        // Act - высадка с корабля на пустое поле
        game.Turn();
        
        // выбираем ход - вперед на дыру
        game.SetMoveAndTurn(2, 2);
        
        // высадка с корабля вторым пиратом на пустое поле
        game.Turn();
        
        // выбираем ход - вперед и вправо на другую дыру
        game.SetMoveAndTurn(3, 2);
        
        // убираем одного своего пирата
        var ownPirate = game.Board.Teams[0].Pirates[0];
        game.KillPirate(ownPirate);
        
        // выбираем ход - назад на пустое поле
        game.SetMoveAndTurn(2, 1);
        
        // добавляем пирата противника вперед и вправо на дыру
        game.AddEnemyTeamAndPirate(new TilePosition(3, 2));
        
        // выбираем ход - вперед на дыру
        game.SetMoveAndTurn(2, 2);
        
        var moves = game.GetAvailableMoves();
        
        // Assert - в дыру не провалились, доступны ходы на ближайшие клетки
        Assert.Equal(4, moves.Count);
        Assert.Equal(new TilePosition(2, 2), moves.First().From);
        Assert.Equivalent(new List<TilePosition>
            {
                new(1, 2), // влево
                new(2, 1), // назад - пустая клетка
                new(2, 3), // вперед
                new(3, 2) // вправо
            },
            moves.Select(m => m.To)
        );
        Assert.Equal(6, game.TurnNo);
    }
    
    [Fact]
    public void WaitMoveOnHole_Turn_ReturnAvailableMovesFromOtherHole()
    {
        // Arrange
        var arrowHoleLineMap = new TwoTileMapGenerator(
            new TileParams(TileType.Arrow) { ArrowsCode = ArrowsCodesHelper.FourArrowsDiagonal },
            new TileParams(TileType.Hole)
        );
        const int mapSize = 5;
        const int piratesPerPlayer = 2;
        var game = new TestGame(arrowHoleLineMap, mapSize, piratesPerPlayer);
        
        // Act - высадка с корабля на пустое стрелку
        game.Turn();
        
        // выбираем ход - вперед и влево на дыру
        game.SetMoveAndTurn(1, 2);
        
        // выбираем ход - вперед и вправо на другую дыру
        game.SetMoveAndTurn(3, 2);
        
        // убираем одного своего пирата
        var ownPirate = game.Board.Teams[0].Pirates[0];
        game.KillPirate(ownPirate);
        
        // ходим на ту же дыру где стоим
        var moves = game.GetAvailableMoves();
        var waitMove = moves.Single(x => x.From == x.To);
        game.SetMoveAndTurn(waitMove.From, waitMove.To);
        
        // выбираем единственный выход на другой дыре
        game.Turn();
        
        moves = game.GetAvailableMoves();
        
        // Assert - оказываемся на другой дыре
        Assert.NotEqual(waitMove.From, moves.First().From);
        Assert.Equal(3, game.TurnNo);
    }
}