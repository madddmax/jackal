using JackalWebHost.Models;
using Jackal.Core;
using Jackal.Core.Players;

namespace JackalWebHost.Service
{
    public class DrawService
    {
        public (List<PirateChange> pirateChanges, List<TileChange> tileChanges) Draw(Board board, Board prevBoard)
        {
            var ships = board.Teams.Select(item => item.Ship).ToList();
            
            var pirateChanges = new List<PirateChange>();
            var diffPositions = new HashSet<Position>();

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
                    
                    diffPositions.Add(oldPirate.Position.Position);
                }
                else if (oldPirate == null)
                {
                    pirateChange = new PirateChange(newPirate) { IsAlive = true };
                    pirateChanges.Add(pirateChange);
                    
                    diffPositions.Add(newPirate.Position.Position);
                }
                else if (oldPirate.Position != newPirate.Position
                         || oldPirate.IsDrunk != newPirate.IsDrunk
                         || oldPirate.IsInTrap != newPirate.IsInTrap)
                {
                    pirateChange = new PirateChange(newPirate)
                    {
                        IsDrunk = oldPirate.IsDrunk != newPirate.IsDrunk ? newPirate.IsDrunk : null,
                        IsInTrap = oldPirate.IsInTrap != newPirate.IsInTrap ? newPirate.IsInTrap : null
                    };
                    pirateChanges.Add(pirateChange);
                    
                    diffPositions.Add(oldPirate.Position.Position);
                    diffPositions.Add(newPirate.Position.Position);
                }
            }

            //координаты клеток, где поменялось расположение пиратов
            var tileChanges = new List<TileChange>();
            for (int y = 0; y < board.MapSize; y++)
            {
                for (int x = 0; x < board.MapSize; x++)
                {
                    var tile = board.Map[x, y];
                    var prevTile = prevBoard.Map[x, y];
                    bool isDiffPosition = diffPositions.Any(item => item == tile.Position || item == prevTile.Position);

                    if (isDiffPosition
                        || tile.Type != prevTile.Type
                        || tile.Coins != prevTile.Coins)
                    {
                        var chg = Draw(tile, ships);
                        chg.X = x;
                        chg.Y = y;
                        tileChanges.Add(chg);
                    }
                }
            }

            return (pirateChanges, tileChanges);
        }
        
        /// <summary>
        /// Формирование статистики после хода
        /// </summary>
        public static GameStatistics GetStatistics(Game game)
        {
            var teams = new List<DrawTeam>();
            foreach (var team in game.Board.Teams)
            {
                game.Scores.TryGetValue(team.Id, out var goldCount);
                teams.Add(new DrawTeam
                {
                    backcolor = GetTeamColor(team.Id),
                    id = team.Id,
                    name = team.Name,
                    gold = goldCount
                });
            }

            return new GameStatistics
            {
                Teams = teams,
                TurnNo = game.TurnNo,
                IsGameOver = game.IsGameOver,
                CurrentTeamId = game.CurrentTeamId,
                IsHumanPlayer = game.CurrentPlayer is WebHumanPlayer
            };
        }

        public static List<DrawMove> GetAvailableMoves(Game game)
        {
            var result = new List<DrawMove>();
            var pirates = new List<PiratePosition>();

            int index = 0;
            foreach (var move in game.GetAvailableMoves())
            {
                var pirate = pirates.FirstOrDefault(p =>
                    p.X == move.From.X && p.Y == move.From.Y && p.Level == move.From.Level
                );
                
                if (pirate == null)
                {
                    var pirateIds = game.Board.AllPirates
                        .Where(p => p.Position.Equals(move.From))
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
                    WithCoin = move.WithCoins,
                    WithRespawn = move.WithRespawn,
                    WithLighthouse = move.WithLighthouse,
                    From = pirate,
                    To = new PiratePosition
                    {
                        X = move.To.X,
                        Y = move.To.Y,
                        Level = move.To.Level
                    }
                });
            }
            return result;
        }

        public DrawMap Map(Board board)
        {
            var changes = new List<TileChange>();

            var ships = board.Teams.Select(item => item.Ship).ToList();
            for (int y = 0; y < board.MapSize; y++)
            {
                for (int x = 0; x < board.MapSize; x++)
                {
                    var tile = board.Map[x, y];
                    var chg = Draw(tile, ships);
                    chg.X = x;
                    chg.Y = y;
                    changes.Add(chg);
                }
            }

            return new DrawMap{
                Width = board.MapSize,
                Height = board.MapSize,
                Changes = changes
            };
        }

        private TileChange Draw(Tile tile, List<Ship> ships)
        {
            var tileElement = new TileChange();

            var tileShip = ships.FirstOrDefault(item => item.Position == tile.Position);
            if (tileShip != null)
            {
                tileElement.BackgroundImageSrc = null;
                tileElement.BackgroundColor = GetTeamColor(tileShip.TeamId);
            }
            else if (tile.Type == TileType.Water && tileElement.BackgroundImageSrc == null)
            {
                tileElement.BackgroundImageSrc = @"/fields/water.png";
                tileElement.BackgroundColor = "Gray";
            }

            tileElement.Levels = new LevelChange[tile.Levels.Count];

            for (int i = 0; i < tile.Levels.Count; i++)
            {
                var level = tile.Levels[i];
                tileElement.Levels[i] = DrawCoins(level, i, tileShip);
            }
            DrawTileBackground(tile, tileShip, ref tileElement);

            return tileElement;
        }

        private static LevelChange DrawCoins(TileLevel level, int levelIndex, Ship? tileShip)
        {
            LevelChange levelChange = new LevelChange();
            
            bool hasCoins = tileShip is { Coins: > 0 } || level.Coins > 0;
            
            levelChange.hasCoins = hasCoins;
            levelChange.Level = levelIndex;

            // draw coins
            if (hasCoins)
            {
                int coins = tileShip?.Coins ?? level.Coins;

                var coin = new DrawCoin
                {
                    ForeColor = "black",
                    BackColor = "gold",
                    Text = coins.ToString()
                };

                levelChange.Coin = coin;
            }

            return levelChange;
        }

        private static void DrawTileBackground(Tile tile, Ship? ship, ref TileChange tileChange)
        {
            TileType type = tile.Type;

            // не зарисовываем корабль водой
            if (ship != null)
            {
                return;
            }

            int rotateCount = 0;

            string filename;
            switch (type)
            {
                case TileType.Unknown:
                    tileChange.IsUnknown = true;
                    filename = @"back";
                    break;
                case TileType.Water:
                    filename = @"water";
                    break;
                case TileType.Grass:
                    filename = @"empty1";
                    break;
                case TileType.Chest1:
                    filename = @"empty1";
                    break;
                case TileType.Chest2:
                    filename = @"empty2";
                    break;
                case TileType.Chest3:
                    filename = @"empty3";
                    break;
                case TileType.Chest4:
                    filename = @"empty4";
                    break;
                case TileType.Chest5:
                    filename = @"empty4";
                    break;
                case TileType.Fort:
                    filename = @"fort";
                    break;
                case TileType.RespawnFort:
                    filename = @"respawn";
                    break;
                case TileType.RumBarrel:
                    filename = @"rumbar";
                    break;
                case TileType.Horse:
                    filename = @"horse";
                    break;
                case TileType.Cannon:
                    filename = @"cannon";
                    rotateCount = tile.CannonDirection;
                    break;
                case TileType.Crocodile:
                    filename = @"croc";
                    break;
                case TileType.Airplane:
                    filename = @"airplane";
                    break;
                case TileType.Balloon:
                    filename = @"balloon";
                    break;
                case TileType.Ice:
                    filename = @"ice";
                    break;
                case TileType.Trap:
                    filename = @"trap";
                    break;
                case TileType.Cannibal:
                    filename = @"canibal";
                    break;
                case TileType.Lighthouse:
                    filename = @"pharos";
                    break;
                case TileType.BenGunn:
                    filename = @"bengunn";
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
                    var search = ArrowsCodesHelper.Search(tile.ArrowsCode);
                    rotateCount = search.RotateCount;
                    int fileNumber = (search.ArrowType + 1);
                    filename = @"arrow" + fileNumber;
                    break;
                default:
                    throw new NotSupportedException();
            }

            string relativePath = $@"/fields/{filename}.png";

            tileChange.BackgroundImageSrc = relativePath;
            tileChange.Rotate = rotateCount;
        }

        private static string GetTeamColor(int teamId) =>
            teamId switch
            {
                0 => "DarkRed",
                1 => "DarkBlue",
                2 => "DarkViolet",
                3 => "DarkOrange",
                _ => ""
            };
    }
}