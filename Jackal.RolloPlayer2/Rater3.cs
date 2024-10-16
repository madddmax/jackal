using System;
using System.Collections.Generic;
using System.Linq;
using Jackal.Core;
using Jackal.Core.Domain;

namespace Jackal.RolloPlayer2;

public class Rater3: Rater2
{
	public Rater3(Board board, int teamId, Settings settings):base(board, teamId, settings)
	{
		Coef = new Settings()
		{ 
			AboardToShipWithGold = 1900
			,MoveToUnknown = 500
			,MoveToShipWithGold = 1800
			,MoveToGold = 1800
			,Atack = 2000
			,AtackManyEnemies = 900
			,MoveUnderAtack = 0
			,MoveFromAtackToAll = 1.2
			,MoveFromAtack = 1350
			,MoveToOccupiedGold = 300
			,MoveFromShip = -500
		};
	}

	public override MoveRate Rate(Move move)
	{
		if (RateActions == null)
			CreateRateActionsList();

		var rate = new MoveRate3 { Move = move };
		foreach (var rateAction in RateActions)
		{
			rateAction.Invoke(rate);
		}

		return rate;
	}

	protected override void CreateRateActionsList()
	{
		RateActions = new List<Action<MoveRate>>
		{
			MoveToUnknown,

			//Old
			AboardToShipWithGold,
			MoveToShipWithGold,
			MoveShipToMyPirateWithGold,
			MoveToGold,
			Atack,
			MoveUnderAtack,
			MoveFromAtack,
			StepOnSameTile,
			//MoveToOccupiedGold,
			//MoveFromShip
		};
	}


	#region MoveToUnknown
	private double[] _moveToUnknownPirateDistCoefs = null;

	private double MoveToUnknownPirateDistanceCoef(Position newPos, Position unknown)
	{
		if (_moveToUnknownPirateDistCoefs == null)
		{
			_moveToUnknownPirateDistCoefs = new Double[14];
			_moveToUnknownPirateDistCoefs[0] = 1;
			_moveToUnknownPirateDistCoefs[1] = 0.9;
			_moveToUnknownPirateDistCoefs[2] = 0.8;
			_moveToUnknownPirateDistCoefs[3] = 0.7;
			_moveToUnknownPirateDistCoefs[4] = 0.6;
			_moveToUnknownPirateDistCoefs[5] = 0.4;
			_moveToUnknownPirateDistCoefs[6] = 0.3;
			_moveToUnknownPirateDistCoefs[7] = 0.28;
			_moveToUnknownPirateDistCoefs[8] = 0.27;
			_moveToUnknownPirateDistCoefs[9] = 0.26;
			_moveToUnknownPirateDistCoefs[10] = 0.25;
			_moveToUnknownPirateDistCoefs[11] = 0.24;
			_moveToUnknownPirateDistCoefs[12] = 0.23;
			_moveToUnknownPirateDistCoefs[13] = 0.22;
		}

		return DistanceCoef(Distance(newPos, unknown), _moveToUnknownPirateDistCoefs);
	}

	private double[] _moveToUnknownMyShipDistCoefs = null;

	private double MoveToUnknownMyShipDistanceCoef(Position ship, Position unknown)
	{
		if (_moveToUnknownMyShipDistCoefs == null)
		{
			_moveToUnknownMyShipDistCoefs = new Double[14];
			_moveToUnknownMyShipDistCoefs[0] = 1;
			_moveToUnknownMyShipDistCoefs[1] = 1;
			_moveToUnknownMyShipDistCoefs[2] = 0.99;
			_moveToUnknownMyShipDistCoefs[3] = 0.95;
			_moveToUnknownMyShipDistCoefs[4] = 0.92;
			_moveToUnknownMyShipDistCoefs[5] = 0.90;
			_moveToUnknownMyShipDistCoefs[6] = 0.80;
			_moveToUnknownMyShipDistCoefs[7] = 0.70;
			_moveToUnknownMyShipDistCoefs[8] = 0.50;
			_moveToUnknownMyShipDistCoefs[9] = 0.40;
			_moveToUnknownMyShipDistCoefs[10] = 0.25;
			_moveToUnknownMyShipDistCoefs[11] = 0.24;
			_moveToUnknownMyShipDistCoefs[12] = 0.23;
			_moveToUnknownMyShipDistCoefs[13] = 0.22;
		}

		return DistanceCoef(Distance(unknown, ship), _moveToUnknownMyShipDistCoefs);
	}

	//MoveToUnknownEnemyShipDistanceCoef
	private double[] _moveToUnknownEnemyShipDistCoefs = null;

	private double MoveToUnknownEnemyShipDistanceCoef(Position unknown)
	{
		var nearestShipDist = AllEnemyShips.Min(s => Distance(s.Position, unknown));

		if (_moveToUnknownEnemyShipDistCoefs == null)
		{
			_moveToUnknownEnemyShipDistCoefs = new Double[4];
			_moveToUnknownEnemyShipDistCoefs[0] = 0; 
			_moveToUnknownEnemyShipDistCoefs[1] = 0.3;
			_moveToUnknownEnemyShipDistCoefs[2] = 0.8;
			_moveToUnknownEnemyShipDistCoefs[3] = 1;
		}

		return DistanceCoef(nearestShipDist, _moveToUnknownEnemyShipDistCoefs);
	}

	private double MoveToUnknownCornersDistanceCoef(Position unknown)
	{
		//var firstPartOfGame = Board.AllTiles(t => t.Type == TileType.Unknown).Count() < ((121 - 37)/3);
		var goodCorners = MapCorners.Where(c => AllEnemyShips.Min(s => Distance(s.Position, c) > 4)).ToList();
		var minSumDist = goodCorners.Min(c => Math.Abs(unknown.X - c.X) + Math.Abs(unknown.Y - c.Y));
		if (minSumDist <= 4)
			return 1.2;
			
		return 1;
	}

	/// <summary>
	/// Двигаемся к закрытому
	/// </summary>
	/// <param name="moveRate"></param>
	protected override void MoveToUnknown(MoveRate moveRate)
	{
		//Собираем все закрытые к которым приближаемся
		var tilesWithUnknown = Board.AllTiles(x => x.Type == TileType.Unknown && Distance(moveRate.Move.From.Position, x.Position) > Distance(moveRate.Move.To.Position, x.Position)).Select(t=> new PosRate(t.Position)).ToList();
		if (!tilesWithUnknown.Any())
			return;

		//Cчитаем коэф закрытых к оставшемуся золоту - шансы на золото
		var hiddenGold = 37 - Board.AllTiles(t => t.Coins > 0).Sum(t => t.Coins) - Board.Teams.Sum(t => t.Ship.Coins);
		if (hiddenGold == 0)
		{
			return;
		}

		var unknownCount = Board.AllTiles(t => t.Type == TileType.Unknown).Count();
		var goldPossibility = (double)unknownCount/hiddenGold;

		if (goldPossibility > 30)
			goldPossibility = 0.3;
		else if (goldPossibility > 8)
			goldPossibility = 0.6;
		else if (goldPossibility > 2)
			goldPossibility = 0.8;
		else
			goldPossibility = 1;

		//Для каждого:
		//расстояние до пирата
		tilesWithUnknown.ForEach(t => t.Rate = t.Rate * MoveToUnknownPirateDistanceCoef(moveRate.Move.To.Position, t.Pos));

		//расстояние до своего корабля
		tilesWithUnknown.ForEach(t => t.Rate = t.Rate * MoveToUnknownMyShipDistanceCoef(MyShip.Position, t.Pos));

		//расстояние до ближайшего чужего корабля
		tilesWithUnknown.ForEach(t => t.Rate = t.Rate * MoveToUnknownEnemyShipDistanceCoef(t.Pos));

		//даем приемущество шариться по углам (треугольнико от углов)
		tilesWithUnknown.ForEach(t => t.Rate = t.Rate * MoveToUnknownCornersDistanceCoef(t.Pos));

		//Если двигаем корабль - чуток получаем приемущество
		if (IsShipMove(moveRate.Move))
			tilesWithUnknown.ForEach(t => t.Rate = t.Rate * 1.05);

		//TODO: коэф присутствия врагов
			
		//берем наилучший вариант * goldPossibility
		var res = tilesWithUnknown.Max(t => t.Rate) * goldPossibility;

		moveRate.AddRate("MoveToUnknown", Coef.MoveToUnknown * res);
	}

	#endregion MoveToUnknown


	#region MoveToGold
	private double[] _moveToGoldPirateDistCoefs = null;

	private double MoveToGoldNewPosDistanceCoef(Position pirate, Position gold)
	{
		if (_moveToGoldPirateDistCoefs == null)
		{
			_moveToGoldPirateDistCoefs = new Double[14];
			_moveToGoldPirateDistCoefs[0] = 1;
			_moveToGoldPirateDistCoefs[1] = 0.99;
			_moveToGoldPirateDistCoefs[2] = 0.9;
			_moveToGoldPirateDistCoefs[3] = 0.8;
			_moveToGoldPirateDistCoefs[4] = 0.7;
			_moveToGoldPirateDistCoefs[5] = 0.3;
			_moveToGoldPirateDistCoefs[6] = 0.29;
			_moveToGoldPirateDistCoefs[7] = 0.28;
			_moveToGoldPirateDistCoefs[8] = 0.27;
			_moveToGoldPirateDistCoefs[9] = 0.26;
			_moveToGoldPirateDistCoefs[10] = 0.25;
			_moveToGoldPirateDistCoefs[11] = 0.24;
			_moveToGoldPirateDistCoefs[12] = 0.23;
			_moveToGoldPirateDistCoefs[13] = 0.22;
		}

		return DistanceCoef(Distance(pirate, gold), _moveToGoldPirateDistCoefs);
	}

	private double[] _moveToGoldMyShipDistCoefs = null;

	private double MoveToGoldMyShipDistanceCoef(Position ship, Position gold)
	{
		if (_moveToGoldMyShipDistCoefs == null)
		{
			_moveToGoldMyShipDistCoefs = new Double[14];
			_moveToGoldMyShipDistCoefs[0] = 1;
			_moveToGoldMyShipDistCoefs[1] = 1;
			_moveToGoldMyShipDistCoefs[2] = 0.99;
			_moveToGoldMyShipDistCoefs[3] = 0.9;
			_moveToGoldMyShipDistCoefs[4] = 0.8;
			_moveToGoldMyShipDistCoefs[5] = 0.5;
			_moveToGoldMyShipDistCoefs[6] = 0.3;
			_moveToGoldMyShipDistCoefs[7] = 0.28;
			_moveToGoldMyShipDistCoefs[8] = 0.27;
			_moveToGoldMyShipDistCoefs[9] = 0.26;
			_moveToGoldMyShipDistCoefs[10] = 0.25;
			_moveToGoldMyShipDistCoefs[11] = 0.24;
			_moveToGoldMyShipDistCoefs[12] = 0.23;
			_moveToGoldMyShipDistCoefs[13] = 0.22;
		}

		return DistanceCoef(Distance(gold, ship), _moveToGoldMyShipDistCoefs);
	}

	private double[] _moveToGoldEnemyShipDistCoefs = null;

	private double MoveToGoldEnemyShipDistanceCoef(Position gold)
	{
		var nearestShipDist = AllEnemyShips.Min(s => Distance(s.Position, gold));

		if (_moveToGoldEnemyShipDistCoefs == null)
		{
			_moveToGoldEnemyShipDistCoefs = new Double[4];
			_moveToGoldEnemyShipDistCoefs[0] = 0;
			_moveToGoldEnemyShipDistCoefs[1] = 0;
			_moveToGoldEnemyShipDistCoefs[2] = 0.8;
			_moveToGoldEnemyShipDistCoefs[3] = 1;
		}

		return DistanceCoef(nearestShipDist, _moveToGoldEnemyShipDistCoefs);
	}

	private double MoveToGoldCornersDistanceCoef(Position gold)
	{
		var minSumDist = MapCorners.Min(c => Math.Abs(gold.X - c.X) + Math.Abs(gold.Y - c.Y));
		if (minSumDist <= 4)
			return 1.1;
			
		return 1;
	}

	private double MoveToGoldOccupaneCoef(Position myPos, Position gold)
	{
		//ищем кол-во чужих
		var myDist = Distance(myPos, gold);
		var othersCloserThenMy = Board.AllTiles(t => t.Pirates.Count > 0 && Distance(t.Position, gold) < myDist).Sum(t => t.Pirates.Count);
		var coins = GoldOnPosition(gold);
		var coinsCoef = coins - othersCloserThenMy;
		if (coinsCoef < -3)
			return 0.2;
		if (coinsCoef < -2)
			return 0.4;
		if (coinsCoef < -1)
			return 0.6;
		if (coinsCoef < 0)
			return 0.7;
		if (coinsCoef < 1)
			return 1;
		if (coinsCoef < 2)
			return 1.1;
		if (coinsCoef < 3)
			return 1.2;
		if (coinsCoef < 4)
			return 1.3;
		if (coinsCoef < 5)
			return 1.4;

		return 1;
	}

	/// <summary>
	/// Двигаемся к золоту
	/// </summary>
	/// <param name="moveRate"></param>
	protected override void MoveToGold(MoveRate moveRate)
	{
		//Если я уже с золотом - нечего бегать за другим
		if (GoldOnPosition(moveRate.Move.From.Position) > 0)
			return;
			
		//Ищем все золото к кому приближаемся
		var tilesWithGold = Board.AllTiles(x => x.Type != TileType.Water && x.Coins > 0 && Distance(moveRate.Move.From.Position, x.Position) > Distance(moveRate.Move.To.Position, x.Position)).Select(t => new PosRate(t.Position)).ToList();

		if (!tilesWithGold.Any())
			return;

		//Для каждого золота считаем
		tilesWithGold.ForEach(t => t.Rate = t.Rate * MoveToGoldNewPosDistanceCoef(moveRate.Move.To.Position, t.Pos));

		//расстояние до своего корабля
		tilesWithGold.ForEach(t => t.Rate = t.Rate * MoveToGoldMyShipDistanceCoef(MyShip.Position, t.Pos));

		//расстояние до ближайшего чужего корабля
		tilesWithGold.ForEach(t => t.Rate = t.Rate * MoveToGoldEnemyShipDistanceCoef(t.Pos));

		//даем приемущество шариться по углам (треугольнико от углов)
		//tilesWithGold.ForEach(t => t.Rate = t.Rate * MoveToGoldCornersDistanceCoef(t.Pos));

		//Если двигаем корабль к золоту - чуток получаем приемущество
		//Если двигаем корабль - чуток получаем приемущество
		if (IsShipMove(moveRate.Move))
			tilesWithGold.ForEach(t => t.Rate = t.Rate * 1.05);

		//TODO: кол-во чужих и своих пиратов которым ближе чем мне. Исключам то золото которое не отнять или оно уже у нас
		tilesWithGold.ForEach(t => t.Rate = t.Rate * MoveToGoldOccupaneCoef(moveRate.Move.From.Position, t.Pos));


		//берем наилучший вариант
		var res = tilesWithGold.Max(t => t.Rate);

		moveRate.AddRate("MoveToGold", Coef.MoveToGold * res);
	}
	#endregion MoveToGold

	#region MoveShipToMyPirateWithGold
	/// <summary>
	/// перемещаем золото ближе к кораблю
	/// </summary>
	/// <param name="moveRate"></param>
	protected override void MoveShipToMyPirateWithGold(MoveRate moveRate)
	{
		if (!IsShipMove(moveRate.Move))
			return;

		var myPiratesWithGold = MyPirates.Where(p => GoldOnPosition(p.Position.Position) > 0 && Distance(moveRate.Move.From.Position, p.Position.Position) > Distance(moveRate.Move.To.Position, p.Position.Position)).ToList();
		if (!myPiratesWithGold.Any())
			return;

		var coins = myPiratesWithGold.Sum(p => GoldOnPosition(p.Position.Position));

		moveRate.AddRate("MoveShipToPirateWithGold", Coef.MoveToShipWithGold 
		                                             * (myPiratesWithGold.Count > 1 ? (1 + myPiratesWithGold.Count * 0.1) : 1)
		                                             * (coins > 1 ? (1 + coins * 0.1) : 1)
		);
	}

	#endregion MoveShipToMyPirateWithGold

	#region MoveWithGoldToShip

	/// <summary>
	/// Защищаем золото вместо похода с ним домой
	/// </summary>
	/// <param name="moveRate"></param>
	/// <returns></returns>
	protected override double DefenceGoldCoef(MoveRate moveRate)
	{
		var pos = moveRate.Move.From.Position;
		var goldOnPos = GoldOnPosition(pos);
		//Если золота > 1 и я от корабля не дальше чем 5 ходов
		if (goldOnPos < 2 || Distance(MyShip.Position, pos) > 5)
			return 1;

		//Нет других пиратов
		//TODO: Проверка по Id пирата который ходит
		var myOtherPirtates = MyPirates.Where(p => p.Position.Position != moveRate.Move.From.Position).ToList();
		if (!myOtherPirtates.Any())
			return 1;

		//Возле корабля не защищаем
		if (Distance(MyShip.Position, pos) == 1)
			return 1;

		//и до ближайшего моего пирата не менее 4-х ходов
		if (myOtherPirtates.Min(p => Distance(p.Position.Position, pos)) > 5)
			return 1;

		//Если уже людей сколько и золота - мотаем на корабль
		if (MyPirates.Count(p => Distance(p.Position.Position, pos) <= 1) >= Math.Min(2, goldOnPos))
			return 1;

		//Если до ближайшего корабля противника меньше 3-х ходов
		if (AllEnemyShips.Min(s => Distance(s.Position, pos)) < 4)
			return 1;

		//Если я под атакой 2-х врагов - бежим
		if (Board.Teams.Count(t => t.Id != TeamId && t.Pirates.Any(p => Distance(p.Position.Position, pos) == 1)) > 1)
			return 1;

		return 0.2;
	}

	/// <summary>
	/// перемещаем золото ближе к кораблю
	/// </summary>
	/// <param name="moveRate"></param>
	protected override void MoveToShipWithGold(MoveRate moveRate)
	{
		if (!moveRate.Move.WithCoins)
			return;

		var currentDistance = Distance(moveRate.Move.From.Position, MyShip.Position);
		var newDistance = Distance(moveRate.Move.To.Position, MyShip.Position);
		if (currentDistance <= newDistance)
			return;

		moveRate.AddRate("MoveToShipWithGold", Coef.MoveToShipWithGold * DefenceGoldCoef(moveRate));
	}

	#endregion MoveWithGoldToShip

	#region StepOnSameTile
	protected override void StepOnSameTile(MoveRate moveRate)
	{
		if (!TargetIsShip(moveRate.Move) && MyPirates.Any(p=>p.Position == moveRate.Move.To))
			moveRate.AddApplyToAllRate("StepOnSameTile", 0.9);
	}
	#endregion StepOnSameTile

	/// <summary>
	/// Ход под удар
	/// </summary>
	/// <param name="moveRate"></param>
	protected override void MoveUnderAtack(MoveRate moveRate)
	{
		//Не боимся слазить с корабля стоя на нем
		if (moveRate.Move.From.Position == MyShip.Position)
			return;

		var myAtakers = AllEnemies.Count(p => Distance(p.Position.Position, moveRate.Move.To.Position) == 1);
		if (myAtakers == 0)
			return;

		var myDefence = MyPirates.Count(p => Distance(p.Position.Position, moveRate.Move.To.Position) == 1) - 1;

		if (myDefence < myAtakers)
			moveRate.AddApplyToAllRate("MoveUnderAtack", Coef.MoveUnderAtack);
	}

	/// <summary>
	/// Уход от удара
	/// </summary>
	/// <param name="moveRate"></param>
	protected override void MoveFromAtack(MoveRate moveRate)
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
	/// Атака
	/// </summary>
	/// <param name="moveRate"></param>
	protected override void Atack(MoveRate moveRate)
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

		double atackBackCoef = 1;
		//Не атаковать назад если можно идти к кораблю с золотом. Вот так не работает оборона
		//if (coinsFrom > 0 && Distance(moveRate.Move.From, MyShip.Position) < Distance(moveRate.Move.To, MyShip.Position))
		//	atackBackCoef = 0.4;

		moveRate.AddRate("Atack", (Coef.Atack + ((enemiesOnPosition - 1) * Coef.AtackManyEnemies)) * (1 + coinsFrom * 0.1) * (1 + coinsTo * 0.1) * atackBackCoef);
	}
}