using Jackal.Core;
using Jackal.Core.Players;
using Xunit;

namespace Jackal.Tests2.TileTests;

public class AirplaneTests
{
    [Fact]
    public void AvailableMovesTest()
    {
        IPlayer[] players = [new WebHumanPlayer(), new WebHumanPlayer()];
        var tileParams = new TileParams { Type = TileType.Airplane };
        var airplaneMap = new OneTileMapGenerator(tileParams);
        var board = new Board(players, airplaneMap, 5, 1);
        var game = new Game(players, board);

        var moves = game.GetAvailableMoves();
        Assert.Single(moves); // высадка с корабля
        
        game.Turn();
        moves = game.GetAvailableMoves();
        Assert.Equal(6, moves.Count); // все поле + свой корабль
    }
}