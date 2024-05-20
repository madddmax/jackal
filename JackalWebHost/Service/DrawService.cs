using Jackal;
using Jackal.Players;
using JackalWebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JackalWebHost.Service
{
    public class DrawService
    {

        public const int Width = 100;
        public const int Height = 100;

        public List<TileChange> Draw(Board board, Board prevBoard)
        {
            var ships = board.Teams.Select(item => item.Ship).ToList();
            /*
            var diffPiratesList =
                from curr in board.AllPirates
                join prev in prevBoard.AllPirates on curr.Id equals prev.Id
                where curr.Position != prev.Position
                select new List<Pirate> { curr, prev };
            */

            var diffPositions = new HashSet<Position>();

            //нам нужны координаты клеток, где поменялось расположение пиратов
            var idList = board.AllPirates.Union(prevBoard.AllPirates).Select(x => x.Id).Distinct();
            foreach (var guid in idList)
            {
                var newPirate = board.AllPirates.FirstOrDefault(x => x.Id == guid);
                var oldPirate = prevBoard.AllPirates.FirstOrDefault(x => x.Id == guid);
                if (newPirate == null)
                {
                    diffPositions.Add(oldPirate.Position.Position);
                }
                else if (oldPirate == null)
                {
                    diffPositions.Add(newPirate.Position.Position);
                }
                else if (oldPirate.Position != newPirate.Position
                         || oldPirate.IsDrunk != newPirate.IsDrunk
                         || oldPirate.IsInTrap != newPirate.IsInTrap
                         || oldPirate.IsInLove != newPirate.IsInLove)
                {
                    diffPositions.Add(oldPirate.Position.Position);
                    diffPositions.Add(newPirate.Position.Position);
                }
            }

            /*
            var diffPositions = new List<Position>();
            foreach (var pirates in diffPiratesList)
            {
                diffPositions.AddRange(pirates.Select(item => item.Position.Position).ToList());
            }
            */


            List<TileChange> changes = new List<TileChange>();

            for (int y = 0; y < Board.Size; y++)
            {
                for (int x = 0; x < Board.Size; x++)
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
                        changes.Add(chg);
                    }
                }
            }

            return changes;
        }


        /// <summary>
        /// Формирование статистики после хода
        /// </summary>
        public GameStatistics GetStatistics(Game game)
        {
            List<DrawTeam> teams = new List<DrawTeam>();
            foreach (var team in game.Board.Teams)
            {
                int goldCount;
                game.Scores.TryGetValue(team.Id, out goldCount);
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


        public List<DrawMove> GetAvailableMoves(Game game)
        {
            List<DrawMove> result = new List<DrawMove>();
            List<LevelPosition> pirates = new List<LevelPosition>();

            int index = 0;
            int mindex = 1;
            foreach (var move in game.GetAvailableMoves())
            {
                var pirate = pirates.FirstOrDefault(p => (p.X == move.From.X) && (p.Y == move.From.Y) && (p.Level == move.From.Level));
                if (pirate == null)
                {
                    pirate = new LevelPosition
                        {
                            PirateNum = mindex++,
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
                        From = pirate,
                        To = new LevelPosition
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
            List<TileChange> changes = new List<TileChange>();

            var ships = board.Teams.Select(item => item.Ship).ToList();
            for (int y = 0; y < Board.Size; y++)
            {
                for (int x = 0; x < Board.Size; x++)
                {
                    var tile = board.Map[x, y];
                    var chg = Draw(tile, ships);
                    chg.X = x;
                    chg.Y = y;
                    changes.Add(chg);
                }
            }

            return new DrawMap{
                Width = Board.Size,
                Height = Board.Size,
                Changes = changes
            };
        }

        public TileChange Draw(Tile tile, List<Ship> ships)
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
                tileElement.BackgroundImageSrc = @"/Content/Fields/water.png";
                tileElement.BackgroundColor = "Gray";
            }

            tileElement.Levels = new LevelChange[tile.Levels.Count];

            for (int i = 0; i < tile.Levels.Count; i++)
            {
                var level = tile.Levels[i];
                tileElement.Levels[i] = DrawPiratesAndCoins(level, i, tile.Levels.Count, tileShip);
            }
            DrawTileBackground(tile, tileShip, ref tileElement);

            return tileElement;
        }



        private LevelChange DrawPiratesAndCoins(TileLevel level, int levelIndex, int levelCount, Ship tileShip)
        {
            LevelChange levelChange = new LevelChange();

            var pirates = level.Pirates;
            bool hasPirates = pirates != null && pirates.Count > 0;
            bool hasCoins = (tileShip != null && tileShip.Coins > 0) || level.Coins > 0;

            levelChange.hasPirates = hasPirates;
            levelChange.hasCoins = hasCoins;
            levelChange.Level = levelIndex;

            // draw pirates
            if (hasPirates)
            {
                DrawPirate pirate = new DrawPirate();

                pirate.ForeColor = "white";
                pirate.BackColor = GetTeamColor(pirates.First().TeamId);
                pirate.Text = pirates.Count().ToString();
                levelChange.Pirate = pirate;
            }

            // draw coins
            if (hasCoins)
            {
                int coins = tileShip != null ? tileShip.Coins : level.Coins;

                DrawCoin coin = new DrawCoin();

                coin.ForeColor = "black";
                coin.BackColor = "gold";
                coin.Text = coins.ToString();
                levelChange.Coin = coin;
            }

            return levelChange;
        }

        private void DrawTileBackground(Tile tile, Ship ship, ref TileChange tileChange)
        {
            TileType type = tile.Type;

            // не зарисовываем корабль водой
            if (ship != null)
            {
                return;
            }

            // после открытия поля - фон не меняется
            //if (type != TileType.Unknown)
            //{
              //  return;
            //}

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
                    filename = @"chest1";
                    break;
                case TileType.Chest2:
                    filename = @"chest2";
                    break;
                case TileType.Chest3:
                    filename = @"chest3";
                    break;
                case TileType.Chest4:
                    filename = @"chest4";
                    break;
                case TileType.Chest5:
                    filename = @"chest5";
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
                case TileType.Croc:
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
                case TileType.Canibal:
                    filename = @"canibal";
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

            string relativePath = string.Format(@"/Content/fields/{0}.png", filename);

            tileChange.BackgroundImageSrc = relativePath;
            tileChange.Rotate = rotateCount;
        }


        public static string GetTeamColor(int teamId)
        {
            switch (teamId)
            {
                case 0: return "DarkRed";
                case 1: return "DarkBlue";
                case 2: return "DarkViolet";
                case 3: return "DarkOrange";
                default: return null;
            }
        }



    }
}