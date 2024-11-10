using System.Collections.Generic;
using Newtonsoft.Json;

namespace Jackal.Core.Domain;

public record Tile
{
	/// <summary>
	/// Позиция
	/// </summary>
	[JsonProperty]
	public readonly Position Position;

	/// <summary>
	/// Тип клетки
	/// </summary>
	[JsonProperty]
	public readonly TileType Type;
        
	/// <summary>
	/// Код клетки, используется для стрелок и пустых клеток.
	/// По коду вычисляем номер картинки для клетки.
	/// </summary>
	[JsonProperty]
	public readonly int ArrowsCode;

	/// <summary>
	/// Направление клетки (или количество поворотов)
	/// </summary>
	[JsonProperty]
	public readonly DirectionType Direction;
		
	/// <summary>
	/// Количество ходов на клетке-вертушке
	/// </summary>
	[JsonProperty]
	public readonly int SpinningCount;

	/// <summary>
	/// Уровни клетки (0 - обычный уровень/уровень выхода с клетки)
	/// </summary>
	[JsonProperty]
	public readonly List<TileLevel> Levels = [];

	/// <summary>
	/// Использована (например самолет уже взлетал)
	/// </summary>
	[JsonProperty]
	public bool Used;
        
	[JsonIgnore]
	public int Coins => Levels[0].Coins;

	[JsonIgnore]
	public int? OccupationTeamId => Levels[0].OccupationTeamId;

	/// <summary>
	/// Предлагаю выкинуть пиратов из тайлов,
	/// для отрисовки на задерживающих клетках ввести в
	/// Team->Pirates->Position зачение z
	/// </summary>
	[JsonIgnore]
	public HashSet<Pirate> Pirates => Levels[0].Pirates;

	public Tile()
	{
	}

	public Tile(Position position, Tile tile) : this(
		new TileParams(tile.Type)
		{
			Position = position,
			ArrowsCode = tile.ArrowsCode,
			Direction = tile.Direction,
			SpinningCount = tile.SpinningCount
		})
	{
	}

	public Tile(TileParams tileParams)
	{
		Position = tileParams.Position;
		Type = tileParams.Type;
		int levelsCount = (tileParams.Type == TileType.Spinning) ? tileParams.SpinningCount : 1;
		for (int level = 0; level < levelsCount; level++)
		{
			var tileLevel = new TileLevel(new TilePosition(tileParams.Position, level));
			Levels.Add(tileLevel);
		}
		ArrowsCode = tileParams.ArrowsCode;
		SpinningCount = tileParams.SpinningCount;
		Direction = tileParams.Direction;
	}

	public bool HasNoEnemy(int ownTeamId) => OccupationTeamId.HasValue == false || OccupationTeamId == ownTeamId;
}