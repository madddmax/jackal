using Jackal.Core.Domain;
using Jackal.Core.MapGenerator;
using Xunit;

namespace Jackal.Tests2.TileTests;

public class BalloonTests
{
    [Fact]
    public void OneBalloon_Turn_ReturnToOurShip()
    {
        // Arrange
        var balloonOnlyMap = new OneTileMapGenerator(new TileParams(TileType.Balloon));
        var game = new TestGame(balloonOnlyMap);
        
        // Act - высадка с корабля на закрытый воздушный шар
        game.Turn();
        
        // Assert - пират находится на нашем корабле
        Assert.Single(game.Board.AllPirates);
        Assert.Equal(new TilePosition(2, 0), game.Board.AllPirates[0].Position);
        Assert.Equal(1, game.TurnNo);
    }
    
    [Fact]
    public void OneBalloon_2Turn_ReturnToOurShip()
    {
        // Arrange
        var balloonOnlyMap = new OneTileMapGenerator(new TileParams(TileType.Balloon));
        var game = new TestGame(balloonOnlyMap);
        
        // Act - высадка с корабля на закрытый воздушный шар
        game.Turn();
        
        // высадка с корабля на открытый воздушный шар
        game.Turn();
        
        // Assert - пират находится на нашем корабле
        Assert.Single(game.Board.AllPirates);
        Assert.Equal(new TilePosition(2, 0), game.Board.AllPirates[0].Position);
        Assert.Equal(2, game.TurnNo);
    }
}