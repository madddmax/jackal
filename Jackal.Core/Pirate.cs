using System;
using Newtonsoft.Json;

namespace Jackal.Core
{
    public class Pirate(int teamId, TilePosition position, PirateType type)
    {
        [JsonProperty]
        public readonly Guid Id = Guid.NewGuid();

        [JsonProperty]
        public PirateType Type = type;

        [JsonProperty]
        public readonly int TeamId = teamId;

        [JsonProperty]
        public TilePosition Position = position;

        [JsonProperty]
        public bool IsDrunk;
        
        internal int? DrunkSinceTurnNo;

        [JsonProperty]
        public bool IsInTrap;
        
        // TODO-MAD Оптимизация DrawService - заполнять при изменении позиции и сбрасывать каждый ход
        public bool Changed;
    }
}