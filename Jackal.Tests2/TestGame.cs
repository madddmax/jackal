using System;
using System.Collections.Generic;
using Jackal.Core;
using Jackal.Core.Domain;
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
    public int TurnNumber => _testGame.TurnNumber;

    /// <summary>
    /// Конец игры
    /// </summary>
    public bool IsGameOver => _testGame.IsGameOver;

    /// <summary>
    /// Игровое сообщение
    /// </summary>
    public string GameMessage => _testGame.GameMessage;

    /// <summary>
    /// Конструктор, всегда тестируем - производим ход только одной командой.
    /// Пиратов команды противника добавляем отдельно, за них не ходим.
    /// </summary>
    /// <param name="generator">Генератор карты</param>
    /// <param name="mapSize">Размер карты вместе с морем, по умолчанию минимальный 5x5 (поле из 5 клеток)</param>
    /// <param name="piratesPerPlayer">Пиратов в команде, по умолчанию 1</param>
    public TestGame(IMapGenerator generator, int mapSize = 5, int piratesPerPlayer = 1)
    {
        var gameRequest = new GameRequest(
            mapSize, generator, [new HumanPlayer(1, "HumanPlayer")], GameModeType.FreeForAll, piratesPerPlayer
        );
        _testGame = new Game(gameRequest);
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

        _testGame.CoinsOnMap++;
        Board.Map[coinPosition].Coins++;
    }
    
    /// <summary>
    /// Добавить вражескую команду и пирата в игру,
    /// при этом не добавляем сущность игрока противника
    /// следовательно ход не будет передаваться вражеской команде
    /// </summary>
    /// <param name="piratePosition">Позиция пирата противника</param>
    /// <param name="coins">Количество затащенных монет</param>
    public void AddEnemyTeamAndPirate(TilePosition piratePosition, int coins = 0)
    {
        const int enemyTeamId = 1;
        
        // помещаем корабль противника сверху на противоположный берег
        var shipPosition = new Position((Board.MapSize - 1) / 2, Board.MapSize - 1);
        var enemyTeam = new Team(enemyTeamId, "Test enemy team", 0, shipPosition, [])
        {
            Coins = coins,
            EnemyTeamIds = [0]
        };

        Board.Teams[0].EnemyTeamIds = [1];
        Board.Teams = [Board.Teams[0], enemyTeam];
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
    /// Убрать пирата из игры
    /// </summary>
    /// <param name="pirate">Пират</param>
    public void KillPirate(Pirate pirate) => _testGame.KillPirate(pirate);
    
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
    public void Turn(int moveNum = 0)
    {
        if (_testGame.CurrentPlayer is IHumanPlayer humanPlayer)
        {
            humanPlayer.SetMove(moveNum, null);
        }
        
        _testGame.Turn();
    }
    
    /// <summary>
    /// Сделать ход по координатам целевой клетки
    /// </summary>
    /// <param name="x">X координата куда делаем ход</param>
    /// <param name="y">Y координата куда делаем ход</param>
    /// <param name="withCoin">С монетой</param>
    /// <param name="withBigCoin">С большой монетой</param>
    public void SetMoveAndTurn(int x, int y, bool withCoin = false, bool withBigCoin = false)
    {
        var position = new TilePosition(x, y);
        var moves = _testGame.GetAvailableMoves();
        var moveNum = moves.FindIndex(a =>
            a.To == position && a.WithCoin == withCoin && a.WithBigCoin == withBigCoin
        );
        Turn(moveNum);
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
        Turn(moveNum);
    }
}