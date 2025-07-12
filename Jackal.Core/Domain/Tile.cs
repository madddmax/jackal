using System;
using System.Collections.Generic;
using System.Linq;
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
	/// Код клетки:
	/// задаёт подтип клетки для стрелок и пустых клеток,
	/// задаёт количество для клеток с монетами и бутылками
	/// </summary>
	[JsonProperty]
	public readonly int Code;

	/// <summary>
	/// Направление клетки (или количество поворотов)
	/// </summary>
	[JsonProperty]
	public readonly DirectionType Direction;
		
	/// <summary>
	/// Количество ходов на задерживающей клетке
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
	public int BigCoins => Levels[0].BigCoins;

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
			Code = tile.Code,
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
		Code = tileParams.Code;
		SpinningCount = tileParams.SpinningCount;
		Direction = tileParams.Direction;
	}

	public bool HasNoEnemy(int[] enemyTeamIds) => 
		OccupationTeamId.HasValue == false || !enemyTeamIds.Contains(OccupationTeamId.Value);
	
	public virtual bool Equals(Tile? other)
	{
		if (ReferenceEquals(null, other)) return false;
		if (ReferenceEquals(this, other)) return true;
		return Position.Equals(other.Position) && 
		       Type == other.Type && 
		       Code == other.Code && 
		       Direction == other.Direction && 
		       SpinningCount == other.SpinningCount && 
		       Levels.SequenceEqual(other.Levels) && 
		       Used == other.Used;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Position, (int)Type, Code, (int)Direction, SpinningCount, Levels);
	}
}