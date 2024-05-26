using System.Collections.Generic;
using System.Linq;

namespace Jackal
{
	/// <summary>
	/// Считальщик расстояний
	/// </summary>
	public class DistanceCalc
	{
		private readonly Board _board;
		private readonly double _unknownSteps;
		private readonly Dictionary<string, Wave> _waves = new Dictionary<string, Wave>();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="board"></param>
		/// <param name="unknownSteps">Оценка хождения по закрытой клетке</param>
		public DistanceCalc(Board board, double unknownSteps = 1)
		{
			_board = board;
			_unknownSteps = unknownSteps;
		}

		/// <summary>
		/// Расстояние от from до to
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="teamId"></param>
		/// <param name="withGold"></param>
		/// <returns></returns>
		public double? Distance(Position from, Position to, int teamId, bool withGold = false)
		{
			if (from == to) return 0;

			var wave = GetWave(from, teamId, withGold);
			return  wave.DistanceTo(to);
		}

		private Wave GetWave(Position from, int teamId, bool withGold = false)
		{
			var wKey = Wave.WaveKey(from, teamId, withGold);
			Wave w;
			if (_waves.TryGetValue(wKey, out w))
				return w;

			w = new Wave(_board, teamId, from, withGold, _unknownSteps);
			_waves[w.ToString()] = w;

			return w;
		}

		/// <summary>
		/// Позиция с расстоянием до нее от старта
		/// </summary>
		public class CalcPosition
		{
			public CalcPosition(double distance, Position pos, int stepNumber)
			{
				Distance = distance;
				Position = pos;
				StepNumber = stepNumber;
			}

			public double Distance;
			public Position Position;
			public int StepNumber;

			/// <summary>
			/// Самый короткий путь из возмножных?
			/// </summary>
			/// <param name="unknownSteps">Оценка закрытого тайла</param>
			/// <returns></returns>
			public bool IsShortestDistance(double unknownSteps)
			{
				return (Distance < StepNumber + 1) && (Distance < (StepNumber + 1) * unknownSteps);
			}

			public double SelfDistance(Board board, double unknownSteps)
			{
				var tile = board.Map[Position];
				switch (tile.Type)
				{
					case TileType.Canibal:
					case TileType.Croc:
					case TileType.Water:
					case TileType.Trap:
						return 10000;
					case TileType.RumBarrel:
						return 2;
					case TileType.Unknown:
						return unknownSteps;
					case TileType.Spinning:
						return tile.SpinningCount;
					default:
						return 1;
				}
			}
		}

		public class Wave
		{
			private readonly Position _from;
			private readonly int _teamId;
			private int _lastCalculatedStepNumber;
			private readonly Board _board;
			private readonly bool _withGold;
			private bool _isComplete;
			private readonly double _unknownSteps;

			private readonly Dictionary<Position, CalcPosition> _calculated = new Dictionary<Position, CalcPosition>();
			private List<CalcPosition> _lastCalculatedStep;

			public Wave(Board board, int teamId, Position from, bool withGold, double unknownSteps)
			{
				_board = board;
				_teamId = teamId;
				_from = from;
				_withGold = withGold;
				_unknownSteps = unknownSteps;
				_lastCalculatedStepNumber = 0;
				_lastCalculatedStep = new List<CalcPosition> { new CalcPosition(0, _from, _lastCalculatedStepNumber) };
			}


			public static string WaveKey( Position from, int teamId, bool withGold)
			{
				return string.Format("{0}:{1}:{2}", from, teamId, withGold);
			}

			public override string ToString()
			{
				return WaveKey(_from, _teamId, _withGold);
			}

			public double? DistanceTo(Position to)
			{
				//Если уже подсчитали
				CalcPosition res;
				if (_isComplete || (_calculated.TryGetValue(to, out res) && res.IsShortestDistance(_unknownSteps)))
				{
					return Distance(to);
				}

				do
				{
					var newPos = new List<CalcPosition>();
				    foreach (var pos in _lastCalculatedStep)
				    {
				        _lastCalculatedStepNumber++;
				        var position = pos;
				        //TODO: Вызывать правильную функцию GetAllAvaliableMoves с результатами которые отдаются плеерам для хода
				        //TODO: если нужны только ходы с золотом - из результатов убирать ходы без золота
				        //Не проверяем ходы до клеток, до которых уже просчитали самые короткие маршруты
				        var checkedPositions = _calculated.Where(c => c.Value.IsShortestDistance(_unknownSteps)).Select(c => new CheckedPosition(new TilePosition(c.Value.Position))).ToList();

				        var task = new GetAllAvaliableMovesTask();
				        task.TeamId = _teamId;
                        task.alreadyCheckedList=new List<CheckedPosition>(checkedPositions);
				        task.FirstSource = new TilePosition( position.Position);
				        task.NoJumpToWater = true;
				        task.NoCanibal = true;
				        var avaliableMoves = _board.GetAllAvaliableMoves(task);
				        newPos.AddRange(avaliableMoves
				            .Select(p => new CalcPosition(position.Distance + position.SelfDistance(_board, _unknownSteps), p.Target.Position, _lastCalculatedStepNumber))
				            .ToList());
				    }

				    _lastCalculatedStep = AddStepPositions(newPos);

					if (!_lastCalculatedStep.Any())
					{
						_isComplete = true;
						break;
					}

					//Если уже нашли дистанцию - останавливаемся
					if (_calculated.TryGetValue(to, out res) && res.IsShortestDistance(_unknownSteps))
					{
						return Distance(to);
					}

				} while (true);

				return Distance(to);
			}

			/// <summary>
			/// Добавить рассчитанные значения шага волны
			/// </summary>
			/// <param name="step"></param>
			/// <returns>Возвращает только те от которых нужно считать волну дальше</returns>
			private List<CalcPosition> AddStepPositions(IEnumerable<CalcPosition> step)
			{
				var res = new List<CalcPosition>();
				foreach (var pos in step)
				{
					if (pos.Distance > 1000)
						continue;

					if (_calculated.ContainsKey(pos.Position))
					{
						if (pos.Distance < _calculated[pos.Position].Distance)
						{
							_calculated[pos.Position] = pos;
							res.Add(pos);
						}
					}
					else
					{
						_calculated.Add(pos.Position, pos);
						res.Add(pos);
					}
				}

				return res;
			}

			/// <summary>
			/// Расстояние до
			/// </summary>
			/// <param name="to"></param>
			/// <returns></returns>
			private double? Distance(Position to)
			{
				CalcPosition res;
				if (_calculated.TryGetValue(to, out res))
				{
					return res.Distance > 1000 ? (double?)null : res.Distance;
				}

				return null;
			}
		}
	}

	
}
