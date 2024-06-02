using System;
using System.Collections.Generic;
using System.Linq;
using Jackal.Core;

namespace Jackal.RolloPlayer2
{
	public class Rater
	{
		#region base
		public Settings Coef;
		public Board Board;
		public int TeamId;

		public Rater(Board board, int teamId, Settings settings)
		{
			Board = board;
			TeamId = teamId;
			Coef = settings;
		}

		protected List<Action<MoveRate>> RateActions = null;
		
		protected virtual void CreateRateActionsList()
		{
			RateActions = new List<Action<MoveRate>>
			{
				AboardToShipWithGold,
				MoveToShipWithGold,
				MoveToGold,
				MoveToUnknown,
				Atack,
				MoveUnderAtack,
				MoveFromAtack,
				//MoveToOccupiedGold,
				//MoveFromShip
			};
		}

		public virtual MoveRate Rate(Move move)
		{
			if (RateActions == null) 
				CreateRateActionsList();

			var rate = new MoveRate { Move = move};
			if (RateActions == null)
				return rate;
			
			foreach (var rateAction in RateActions)
			{
				rateAction.Invoke(rate);
			}

			return rate;
		}

		

		#endregion base

		#region rate actions
		protected void AboardToShipWithGold(MoveRate moveRate)
		{
			if (moveRate.Move.WithCoins && TargetIsShip(moveRate.Move))
				moveRate.AddRate("AboardToShipWithGold", Coef.AboardToShipWithGold);
		}

		/// <summary>
		/// Двигаемся к закрытому
		/// </summary>
		/// <param name="moveRate"></param>
		protected virtual void MoveToUnknown(MoveRate moveRate)
		{
			var tilesWithUnknown = Board.AllTiles(x => x.Type == TileType.Unknown).ToList();
			if (!tilesWithUnknown.Any())
				return;

			//Ищем минимальное растояние до закрытого до хода и после хода
			var distBefore = tilesWithUnknown.Select(t => new Tuple<int, Tile>(Distance(moveRate.Move.From.Position, t.Position), t)).ToList();
			var distAfter = tilesWithUnknown.Select(t => new Tuple<int, Tile>(Distance(moveRate.Move.To.Position, t.Position), t)).ToList();

			var minMovesBefore = distBefore.Select(t => t.Item1).Min();
			var minMovesAfter = distAfter.Select(t => t.Item1).Min();

			//TODO: Ищем кол-во закрытых в радиусе. Приоритет отдать тем, что близко к своему караблю

			if (minMovesAfter >= minMovesBefore)
				return;

			moveRate.AddRate("MoveToUnknown", Coef.MoveToUnknown * DistanceCoef(minMovesAfter));
		}

		/// <summary>
		/// перемещаем золото ближе к кораблю
		/// </summary>
		/// <param name="moveRate"></param>
		protected virtual void MoveToShipWithGold(MoveRate moveRate)
		{
			if (!moveRate.Move.WithCoins)
				return;

			var currentDistance = Distance(moveRate.Move.From.Position, MyShip.Position);
			var newDistance = Distance(moveRate.Move.To.Position, MyShip.Position);
			if (currentDistance <= newDistance)
				return;

			moveRate.AddRate("MoveToShipWithGold", Coef.MoveToShipWithGold);
		}

		/// <summary>
		/// Атака
		/// </summary>
		/// <param name="moveRate"></param>
		protected virtual void Atack(MoveRate moveRate)
		{
			var enemiesOnPosition = EnemiesOnPosition(moveRate.Move.To.Position);
			var coinsTo = GoldOnPosition(moveRate.Move.To.Position);
			var coinsFrom = GoldOnPosition(moveRate.Move.From.Position);
			if (enemiesOnPosition == 0) // || (coinsTo == 0 && coinsFrom == 0))
				return;

			//Если стоим с золотом прямо у корабля - не атакуем если у врага нет золота
			//if (coinsFrom > 0 && coinsTo == 0 && Distance(MyShip.Position, moveRate.Move.Pirate.Position) == 1)
			//	return;

			//Если кто-то другой без золота может атаковать этого же чудика, а я с золотом - не бьем
			var myEmptyFriendAtackers = MyPirates.Count(p => p.Position.Position != moveRate.Move.From.Position 
								&& GoldOnPosition(p.Position.Position) == 0
								&& Distance(p.Position.Position, moveRate.Move.To.Position) <= 1);

			if (myEmptyFriendAtackers > 0 && coinsFrom > 0)
 				return;

			//Не атаковать если враг в 2-х ходах от его корабля
			var occupationTeamId = Board.Map[moveRate.Move.To.X, moveRate.Move.To.Y].OccupationTeamId;
			if (occupationTeamId.HasValue && Distance(Board.Teams[occupationTeamId.Value].Ship.Position, moveRate.Move.To.Position) < 3)
				return;

			moveRate.AddRate("Atack", (Coef.Atack + ((enemiesOnPosition - 1) * Coef.AtackManyEnemies)) * (1 + coinsFrom * 0.1) * (1 + coinsTo * 0.1));
		}

		/// <summary>
		/// Двигаемся к золоту
		/// </summary>
		/// <param name="moveRate"></param>
		protected virtual void MoveToGold(MoveRate moveRate)
		{
			//TODO: исключить те, что супер далеко или те у которых уже полно пиратов других и своих
			var tilesWithGold = Board.AllTiles(x => x.Type != TileType.Water && x.Coins > 0).ToList();

			//Исключаем, те, что уже у моих пиратов
			tilesWithGold.RemoveAll(t => MyPirates.Count(e => e.Position.Position == t.Position) >= t.Coins);

			if (!tilesWithGold.Any())
				return;

			//Исключаем золото уже у чужого пирата
			tilesWithGold.RemoveAll(t => AllEnemies.Count(e => e.Position.Position == t.Position) >= t.Coins);

			if (!tilesWithGold.Any())
				return;

			//Ищем минимальное растояние до золота до хода и после хода
			var distBefore = tilesWithGold.Select(t => new Tuple<int, Tile>(Distance(moveRate.Move.From.Position, t.Position), t)).ToList();
			var distAfter = tilesWithGold.Select(t => new Tuple<int, Tile>(Distance(moveRate.Move.To.Position, t.Position), t)).ToList();
			var minMovesBefore = distBefore.Select(t => t.Item1).Min();
			var minMovesAfter = distAfter.Select(t => t.Item1).Min();

			if (minMovesAfter >= minMovesBefore)
				return;

			moveRate.AddRate("MoveToGold", (Coef.MoveToGold * DistanceCoef(minMovesAfter)));
		}


		/// <summary>
		/// Ход под удар
		/// </summary>
		/// <param name="moveRate"></param>
		protected virtual void MoveUnderAtack(MoveRate moveRate)
		{
			//Не боимся слазить с корабля стоя на нем
			if (moveRate.Move.From.Position == MyShip.Position)
				return;

			if (AllEnemies.Any(e => Distance(e.Position.Position, moveRate.Move.To.Position) == 1))
				moveRate.AddApplyToAllRate("MoveUnderAtack", Coef.MoveUnderAtack);
		}

		/// <summary>
		/// Уход от удара
		/// </summary>
		/// <param name="moveRate"></param>
		protected virtual void MoveFromAtack(MoveRate moveRate)
		{
			//Не боимся слазить с корабля стоя на нем
			if (moveRate.Move.From.Position == MyShip.Position)
				return;

			if (AllEnemies.Any(e => Distance(e.Position.Position, moveRate.Move.From.Position) == 1) &&
			    AllEnemies.All(e => Distance(e.Position.Position, moveRate.Move.To.Position) != 1))
			{
				moveRate.AddRate("MoveFromAtack", Coef.MoveFromAtack 
					* (moveRate.Move.WithCoins ? 1.3 : 1) 
					* (GoldOnPosition(moveRate.Move.To.Position) > 0 ? 1.2 : 1)
					* (Distance(moveRate.Move.From.Position, MyShip.Position) > Distance(moveRate.Move.To.Position, MyShip.Position) ? 1.1 : 1)
					* (Distance(moveRate.Move.From.Position, MyShip.Position) < Distance(moveRate.Move.To.Position, MyShip.Position) ? 0.9 : 1)
					);
				moveRate.AddApplyToAllRate("MoveFromAtackToAll", Coef.MoveFromAtackToAll);
			}
				
		}

		

		/// <summary>
		/// Отнимаем золото
		/// </summary>
		/// <param name="moveRate"></param>
		protected void MoveToOccupiedGold(MoveRate moveRate)
		{
			var tilesWithGold = Board.AllTiles(x => x.Type != TileType.Water && x.Coins > 0).ToList();

			if (!tilesWithGold.Any())
				return;

			//Исключаем свободное золото
			tilesWithGold.RemoveAll(t => AllEnemies.Count(e => e.Position.Position == t.Position) < t.Coins);

			if (!tilesWithGold.Any())
				return;

			//Ищем минимальное растояние до золота до хода и после хода
			var distBefore = tilesWithGold.Select(t => new Tuple<int, Tile>(Distance(moveRate.Move.From.Position, t.Position), t)).ToList();
			var distAfter = tilesWithGold.Select(t => new Tuple<int, Tile>(Distance(moveRate.Move.To.Position, t.Position), t)).ToList();
			var minMovesBefore = distBefore.Select(t => t.Item1).Min();
			var minMovesAfter = distAfter.Select(t => t.Item1).Min();

			if (minMovesAfter >= minMovesBefore)
				return;

			moveRate.AddRate("MoveToOccupiedGold", (Coef.MoveToOccupiedGold * DistanceCoef(minMovesAfter)));
		}

		/// <summary>
		/// Удаление от корабля
		/// </summary>
		/// <param name="moveRate"></param>
		protected void MoveFromShip(MoveRate moveRate)
		{
			//Не считаем удаленность если двигаемся с корабля или с кораблем
			if(moveRate.Move.From.Position == MyShip.Position)

			if (Distance(moveRate.Move.To.Position, MyShip.Position) < Distance(moveRate.Move.From.Position, MyShip.Position))
				return;

			moveRate.AddApplyToAllRate("MoveFromShip", DistanceCoefRevert(Distance(MyShip.Position, moveRate.Move.To.Position)));
		}

		

		#endregion rate actions


		#region private
		protected int MinDistance(List<Position> positions, Position to)
		{
			return positions.ConvertAll(x => Distance(x, to)).Min();
		}

		protected int EnemiesOnPosition(Position to)
		{
			var occupationTeamId = Board.Map[to.X, to.Y].OccupationTeamId;
			if (!occupationTeamId.HasValue || occupationTeamId.Value == TeamId)
				return 0;

			return Board.Map[to.X, to.Y].Pirates.Count;
		}

		protected int GoldOnPosition(Position to)
		{
			return Board.Map[to.X, to.Y].Coins;
		}

		protected bool IsEnemyPosition(Position to)
		{
			return EnemiesOnPosition(to) > 0;
		}

		protected List<Pirate> AllEnemies
		{
			get
			{
				var allEnemies = Board.Teams.Where(t => t.Id != TeamId).SelectMany(t => t.Pirates).ToList();
				return allEnemies;
			}
		}

		protected List<Ship> AllEnemyShips
		{
			get
			{
				var allEnemyShips = Board.Teams.Where(t => t.Id != TeamId).Select(t => t.Ship).ToList();
				return allEnemyShips;
			}
		}

		/// <summary>
		/// Движение корабля
		/// </summary>
		/// <param name="move"></param>
		/// <returns></returns>
		protected bool IsShipMove(Move move)
		{
			var pos = move.To;
			return MyShip.Position == move.From.Position && (pos.X == 0 || pos.X == 12 || pos.Y == 0 || pos.Y == 12);
		}

		protected List<Pirate> MyPirates
		{
			get
			{
				var allEnemies = Board.Teams.First(t => t.Id == TeamId).Pirates.ToList();
				return allEnemies;
			}
		}

		protected static List<Position> MapCorners = new List<Position> { new Position(1, 1), new Position(11, 11), new Position(1, 11), new Position(11, 1) };

		protected int Distance(Position pos1, Position pos2)
		{
			int deltaX = Math.Abs(pos1.X - pos2.X);
			int deltaY = Math.Abs(pos1.Y - pos2.Y);
			if (deltaY > 0 && (pos1.X == 0 || pos1.X == 12))
				deltaY++;
			if (deltaX > 0 && (pos1.Y == 0 || pos1.Y == 12))
				deltaX++;
			int totalDelta = Math.Max(deltaX, deltaY);
			return totalDelta;
		}

		protected bool TargetIsShip(Move move)
		{
			return (MyShip.Position == move.To.Position);
		}

		protected Ship MyShip
		{
			get
			{
				return Board.Teams[TeamId].Ship;
			}
		}

		private readonly double[] _distanceCoef = { 1, 0.9, 0.8, 0.8, 0.8, 0.6, 0.4, 0.2, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1, 0.1 };

		/// <summary>
		/// Коэф удаленности (0 = 1, 11 = 0)
		/// </summary>
		/// <param name="distance"></param>
		/// <returns></returns>
		protected double DistanceCoef(int distance)
		{
			return _distanceCoef[distance]; 
		}

		/// <summary>
		/// Коэф удаленности ()
		/// </summary>
		/// <param name="distance"></param>
		/// <param name="coefs"></param>
		/// <returns></returns>
		protected double DistanceCoef(int distance, double[] coefs)
		{
			if (distance > coefs.Length - 1)
				distance = coefs.Length - 1;

			if (distance < 0)
				distance = 0;

			return coefs[distance];
		}

		/// <summary>
		/// Коэф удаленности (0 = 0, 11 = 1)
		/// </summary>
		/// <param name="distance"></param>
		/// <returns></returns>
		protected double DistanceCoefRevert(int distance)
		{
			return 1 -_distanceCoef[distance];
		}

		#endregion private
	}
}
