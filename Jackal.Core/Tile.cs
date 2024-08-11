using System.Collections.Generic;
using Newtonsoft.Json;

namespace Jackal.Core
{
    public class Tile
    {
        [JsonProperty]
        public readonly Position Position;

        [JsonProperty]
        public readonly TileType Type;
        
        [JsonProperty]
        public readonly int ArrowsCode;

        [JsonProperty]
		public readonly int CannonDirection;
		
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
	        CannonDirection = tileParams.CanonDirection;
		}
	}
}