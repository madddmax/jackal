using Jackal.Core;
using Jackal.Core.Domain;
using Jackal.Core.MapGenerator;
using Jackal.Core.Players;
using Newtonsoft.Json;
using Xunit;

namespace Jackal.Tests2;

public class JsonHelperTests
{
    [Fact]
    public void Board_SerializeAndDeserialize_Correct()
    {
        // Arrange
        const int mapSize = 5;
        IPlayer[] players = [new HumanPlayer(1)];
        var randomMap = new RandomMapGenerator(1, mapSize);
        var gameRequest = new GameRequest(mapSize, randomMap, players, GameModeType.TwoPlayersInTeam, 1);
        var board = new Board(gameRequest);
        
        // Act
        var json = JsonHelper.SerializeWithType(board, Formatting.Indented);
        var board2 = JsonHelper.DeserializeWithType<Board>(json);
        var json2 = JsonHelper.SerializeWithType(board2, Formatting.Indented);
        
        // Assert
        Assert.True(json == json2);
    }
    
    [Fact]
    public void TilePosition_SerializeAndDeserialize_Correct()
    {
        // Arrange
        const int level = 3;
        var position = new Position(1, 2);
        var tilePosition = new TilePosition(position, level);
        
        // Act
        var json = JsonHelper.SerializeWithType(tilePosition, Formatting.Indented);
        var tilePosition2 = JsonHelper.DeserializeWithType<TilePosition>(json);
        var json2 = JsonHelper.SerializeWithType(tilePosition2, Formatting.Indented);
        
        // Assert
        Assert.True(json == json2);
    }
    
    [Fact]
    public void Position_SerializeAndDeserialize_Correct()
    {
        // Arrange
        var position = new Position(1, 2);
        
        // Act
        var json = JsonHelper.SerializeWithType(position, Formatting.Indented);
        var position2 = JsonHelper.DeserializeWithType<Position>(json);
        var json2 = JsonHelper.SerializeWithType(position2, Formatting.Indented);
        
        // Assert
        Assert.True(json == json2);
    }       
}