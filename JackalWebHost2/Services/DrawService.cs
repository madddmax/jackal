using Jackal.Core;
using Jackal.Core.Domain;
using JackalWebHost2.Models;

namespace JackalWebHost2.Services;

public class DrawService : IDrawService
{
    public List<PirateChange> GetPirateChanges(Board board, Board prevBoard)
    {
        var pirateChanges = new List<PirateChange>();

        var idList = board.AllPirates.Union(prevBoard.AllPirates).Select(x => x.Id).Distinct();
        foreach (var guid in idList)
        {
            var newPirate = board.AllPirates.FirstOrDefault(x => x.Id == guid);
            var oldPirate = prevBoard.AllPirates.FirstOrDefault(x => x.Id == guid);

            PirateChange pirateChange;
            if (newPirate == null)
            {
                var deadPirate = board.DeadPirates.First(x => x.Id == oldPirate.Id);
                    
                pirateChange = new PirateChange(deadPirate) { IsAlive = false };
                pirateChanges.Add(pirateChange);
            }
            else if (oldPirate == null)
            {
                pirateChange = new PirateChange(newPirate) { IsAlive = true };
                pirateChanges.Add(pirateChange);
            }
            else if (oldPirate.Position != newPirate.Position
                     || oldPirate.IsDrunk != newPirate.IsDrunk
                     || oldPirate.IsInTrap != newPirate.IsInTrap
                     || oldPirate.IsInHole != newPirate.IsInHole)
            {
                pirateChange = new PirateChange(newPirate)
                {
                    IsDrunk = oldPirate.IsDrunk != newPirate.IsDrunk ? newPirate.IsDrunk : null,
                    IsInTrap = newPirate.IsInTrap ? true : null,
                    IsInHole = oldPirate.IsInHole != newPirate.IsInHole ? newPirate.IsInHole : null
                };
                pirateChanges.Add(pirateChange);
            }
        }
        
        return pirateChanges;
    }
    
    public List<TileChange> GetTileChanges(Board board, Board prevBoard)
    {
        var tileChanges = new List<TileChange>();
        
        for (int y = 0; y < board.MapSize; y++)
        {
            for (int x = 0; x < board.MapSize; x++)
            {
                var tile = board.Map[x, y];
                var prevTile = prevBoard.Map[x, y];
                if (tile == prevTile) 
                    continue;
                
                var tileChange = Draw(tile, board.Teams);
                tileChange.X = x;
                tileChange.Y = y;
                tileChanges.Add(tileChange);
            }
        }

        return tileChanges;
    }
    
    public GameStatistics GetStatistics(Game game) =>
        new()
        {
            TurnNumber = game.TurnNumber,
            IsGameOver = game.IsGameOver,
            GameMessage = game.GameMessage,
            CurrentTeamId = game.CurrentTeamId,
            CurrentUserId = game.Board.Teams[game.CurrentPlayerIndex].UserId
        };

    public List<DrawMove> GetAvailableMoves(Game game)
    {
        var result = new List<DrawMove>();
        var pirates = new List<PiratePosition>();

        int index = 0;
        foreach (var move in game.GetAvailableMoves())
        {
            var pirate = pirates.FirstOrDefault(p =>
                p.X == move.From.X && p.Y == move.From.Y && (move.WithRumBottle || p.Level == move.From.Level)
            );
                
            if (pirate == null)
            {
                var pirateIds = game.Board.AllPirates
                    .Where(x => !x.IsDrunk && !x.IsInHole && x.Position == move.From)
                    .Where(x => (!x.IsInTrap && !move.WithRumBottle) || move.WithRumBottle)
                    .Select(p => p.Id)
                    .ToList();
                    
                pirate = new PiratePosition
                {
                    PirateIds = pirateIds,
                    X = move.From.X,
                    Y = move.From.Y,
                    Level = move.From.Level
                };
                pirates.Add(pirate);
            }

            result.Add(new DrawMove
            {
                MoveNum = index++,
                WithRumBottle = move.WithRumBottle,
                WithCoin = move.WithCoin,
                WithBigCoin = move.WithBigCoin,
                WithRespawn = move.WithRespawn,
                WithLighthouse = move.WithLighthouse,
                WithQuake = move.WithQuake,
                From = pirate,
                To = new PiratePosition
                {
                    X = move.To.X,
                    Y = move.To.Y,
                    Level = move.To.Level
                },
                Prev = move.Prev != null ? new DrawPosition(move.Prev) : null
            });
        }
        return result;
    }
    
    public DrawMap Map(Board board)
    {
        var changes = new List<TileChange>();
        
        for (int y = 0; y < board.MapSize; y++)
        {
            for (int x = 0; x < board.MapSize; x++)
            {
                var tile = board.Map[x, y];
                var tileChange = Draw(tile, board.Teams);
                tileChange.X = x;
                tileChange.Y = y;
                changes.Add(tileChange);
            }
        }

        return new DrawMap{
            Width = board.MapSize,
            Height = board.MapSize,
            Changes = changes
        };
    }

    private static TileChange Draw(Tile tile, Team[] teams)
    {
        var tileElement = new TileChange
        {
            Levels = new LevelChange[tile.Levels.Count]
        };
        var teamShip = teams.FirstOrDefault(item => item.ShipPosition == tile.Position);

        for (int i = 0; i < tile.Levels.Count; i++)
        {
            var level = tile.Levels[i];
            tileElement.Levels[i] = DrawCoins(level, i, teamShip);
        }
        DrawTileBackground(tile, teamShip, ref tileElement);

        return tileElement;
    }

    private static LevelChange DrawCoins(TileLevel level, int levelIndex, Team? teamShip) =>
        new()
        {
            Level = levelIndex,
            Coins = teamShip?.Coins ?? level.Coins,
            BigCoins = level.BigCoins
        };

    private static void DrawTileBackground(Tile tile, Team? teamShip, ref TileChange tileChange)
    {
        string filename;
        switch (tile.Type)
        {
            case TileType.Unknown:
                tileChange.IsUnknown = true;
                filename = "back";
                break;
            case TileType.Water:
                filename = teamShip != null ? $"ship_{teamShip.Id + 1}" : "water";
                break;
            case TileType.Empty:
                filename = $"empty_{tile.Code}";
                break;
            case TileType.Coin:
            case TileType.BigCoin:
                filename = "chest";
                break;
            case TileType.Fort:
                filename = "fort";
                break;
            case TileType.RespawnFort:
                filename = "respawn";
                break;
            case TileType.RumBarrel:
                filename = "rumbar";
                break;
            case TileType.RumBottle:
                filename = tile.Used ? $"used_rum_{tile.Code}" : $"rum_{tile.Code}";
                break;
            case TileType.Horse:
                filename = "horse";
                break;
            case TileType.Cannon:
                filename = "cannon";
                break;
            case TileType.Crocodile:
                filename = "croc";
                break;
            case TileType.Airplane:
                filename = tile.Used ? "used_airplane" : "airplane";
                break;
            case TileType.Balloon:
                filename = "balloon";
                break;
            case TileType.Ice:
                filename = "ice";
                break;
            case TileType.Trap:
                filename = "trap";
                break;
            case TileType.Cannibal:
                filename = "cannibal";
                break;
            case TileType.Lighthouse:
                filename = "lighthouse";
                break;
            case TileType.BenGunn:
                filename = tile.Used ? "used_bengunn" : "bengunn";
                break;
            case TileType.Caramba:
                filename = "caramba";
                break;
            case TileType.Jungle:
                filename = "jungle";
                break;
            case TileType.Hole:
                filename = "hole";
                break;
            case TileType.Quake:
                filename = "quake";
                break;
            case TileType.Cannabis:
                filename = tile.Used ? "used_cannabis" : "cannabis";
                break;
            case TileType.Spinning:
                switch (tile.SpinningCount)
                {
                    case 2:
                        filename = "forest";
                        break;
                    case 3:
                        filename = "desert";
                        break;
                    case 4:
                        filename = "swamp";
                        break;
                    case 5:
                        filename = "mount";
                        break;
                    default:
                        throw new NotSupportedException();
                }
                break;
            case TileType.Arrow:
                filename = ArrowsCodesHelper.GetArrowName(tile.Code);
                break;
            
            default:
                throw new NotSupportedException();
        }
        
        tileChange.TileType = filename;
        tileChange.Rotate = (int)tile.Direction;
    }
}