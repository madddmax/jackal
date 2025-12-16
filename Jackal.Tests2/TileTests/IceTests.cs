using System;
using System.Collections.Generic;
using System.Linq;
using Jackal.Core.Domain;
using Jackal.Core.MapGenerator;
using Xunit;

namespace Jackal.Tests2.TileTests;

public class IceTests
{
    [Fact]
    public void OneIce_MovingToWater()
    {
        // Arrange
        var map = new TwoTileMapGenerator(TileParams.Ice(), TileParams.Empty());
        var game = new TestGame(map, 7);

        Assert.Single(game.Board.AllPirates);

        /*
        Схема движения пирата:
          P22     <    P32
           v            ^  
        P21(лёд)     P31(лёд)
           v            ^   
        P20(вода) > P30(корабль)
        */

        var p30 = new Position(3, 0);
        var p31 = new Position(3, 1);
        var p32 = new Position(3, 2);
        var p20 = new Position(2, 0);
        var p21 = new Position(2, 1);
        var p22 = new Position(2, 2);

        List<int> GetMovesIndexesToPosition(Position position)
        {
            var moves = game.GetAvailableMoves();
            var movesIndexes = moves.Select((move, index) => new { move, index })
                .Where(x => x.move.To.Position == position)
                .Select(x => x.index)
                .ToList();
            return movesIndexes;
        }

        int GetMoveIndexToPosition(Position position)
        {
            var indexes = GetMovesIndexesToPosition(position);
            Assert.NotEmpty(indexes);
            return indexes.First();
        }

        void AssertPiratePosition(Position position)
        {
            Assert.Equal(new TilePosition(position), game.Board.AllPirates[0].Position);
        }

        // Высадка с корабля на лёд
        game.Turn(GetMoveIndexToPosition(p31));
        AssertPiratePosition(p31);

        // Скольжение на льду вверх
        game.Turn();
        AssertPiratePosition( p32);

        // Ход влево
        game.Turn(GetMoveIndexToPosition(p22));
        AssertPiratePosition(p22);

        // Ход вниз, на лед
        game.Turn(GetMoveIndexToPosition(p21));
        AssertPiratePosition(p21);

        // Скольжение на льду вниз, в воду
        game.Turn();
        AssertPiratePosition(p20);

        // Ход вправо, на корабль
        game.Turn(GetMoveIndexToPosition(p30));
        AssertPiratePosition(p30);

        // Высадка с корабля, через лёд
        game.Turn(GetMoveIndexToPosition(p32));
        AssertPiratePosition(p32);

        // Ход влево
        game.Turn(GetMoveIndexToPosition(p22));
        AssertPiratePosition(p22);

        // Теперь хода в воду не должно быть в спике доступных
        var movesIndexes = GetMovesIndexesToPosition(p20);
        Assert.Empty(movesIndexes);
    }
}