using System;
using System.Collections.Generic;
using Jackal.Core;
using Jackal.Core.MapGenerator;
using Jackal.Core.Players;

namespace Jackal.Tests2;

/// <summary>
/// Класс обертка для тестирования игры,
/// чтобы не плодить зависимости на Game
/// </summary>
public class TestGame
{
    /// <summary>
    /// Тестовая игра
    /// </summary>
    private readonly Game _testGame;
    
    /// <summary>
    /// Игровое поле
    /// </summary>
    public Board Board => _testGame.Board;
    
    /// <summary>
    /// Текущий ход - определяет какая команда ходит
    /// </summary>
    public int TurnNo => _testGame.TurnNo;

    /// <summary>
    /// Конец игры
    /// </summary>
    public bool IsGameOver => _testGame.IsGameOver;
    
    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="generator">Генератор карты</param>
    /// <param name="mapSize">Размер карты вместе с морем, по умолчанию минимальный 5x5 (поле из 5 клеток)</param>
    /// <param name="piratesPerPlayer">Пиратов в команде, по умолчанию 1</param>
    public TestGame (IMapGenerator generator, int mapSize = 5, int piratesPerPlayer = 1)
    {
        IPlayer[] players = [new WebHumanPlayer()];
        var board = new Board(players, generator, mapSize, piratesPerPlayer);
        _testGame = new Game(players, board);
    }

    /// <summary>
    /// Добавить монету в игру
    /// </summary>
    /// <param name="coinPosition">Позиция монеты</param>
    public void AddCoin(TilePosition coinPosition)
    {
        if (Board.Map[coinPosition.Position].Type == TileType.Unknown)
        {
            throw new Exception("Tile must not be Unknown");
        }
        
        Board.Map[coinPosition].Coins++;
    }
    
    /// <summary>
    /// Добавить вражескую команду и пирата в игру
    /// </summary>
    /// <param name="piratePosition">Позиция пирата противника</param>
    public void AddEnemyTeamAndPirate(TilePosition piratePosition)
    {
        const int enemyTeamId = 1;
        
        // помещаем корабль противника сверху на противоположный берег
        var enemyShip = new Ship(enemyTeamId, new Position((Board.MapSize - 1) / 2, Board.MapSize - 1));
        Board.Teams = [Board.Teams[0], new Team(enemyTeamId, "Test enemy team", enemyShip, [])];
        _testGame.AddPirate(enemyTeamId, piratePosition, PirateType.Usual);
    }

    /// <summary>
    /// Добавить своего пирата в игру
    /// </summary>
    /// <param name="piratePosition">Позиция своего пирата</param>
    /// <param name="type">Тип пирата</param>
    public void AddOwnTeamPirate(TilePosition piratePosition, PirateType type)
    {
        const int ownTeamId = 0;
        
        _testGame.AddPirate(ownTeamId, piratePosition, type);
    }
    
    /// <summary>
    /// Получить возможные ходы
    /// </summary>
    public List<Move> GetAvailableMoves()
    {
        return _testGame.GetAvailableMoves();
    }
    
    /// <summary>
    /// Сделать ход по умолчанию,
    /// выбирает первый доступный ход,
    /// если ход всего один то сделает его
    /// </summary>
    public void Turn()
    {
        _testGame.CurrentPlayer.SetHumanMove(0, null);
        _testGame.Turn();
    }
    
    /// <summary>
    /// Сделать ход по координатам целевой клетки
    /// </summary>
    /// <param name="x">X координата куда делаем ход</param>
    /// <param name="y">Y координата куда делаем ход</param>
    public void SetMoveAndTurn(int x, int y)
    {
        var position = new TilePosition(x, y);
        var moves = _testGame.GetAvailableMoves();
        var moveNum = moves.FindIndex(a => a.To == position);
        
        _testGame.CurrentPlayer.SetHumanMove(moveNum, null);
        _testGame.Turn();
    }
    
    /// <summary>
    /// Сделать ход по позициям клеток,
    /// используется если несколько пиратов
    /// и надо выбрать с какой клетки делать ход
    /// </summary>
    /// <param name="from">Позиция откуда делаем ход</param>
    /// <param name="to">Позиция куда делаем ход</param>
    public void SetMoveAndTurn(TilePosition from, TilePosition to)
    {
        var moves = _testGame.GetAvailableMoves();
        var moveNum = moves.FindIndex(a => a.From == from && a.To == to);
        
        _testGame.CurrentPlayer.SetHumanMove(moveNum, null);
        _testGame.Turn();
    }
}