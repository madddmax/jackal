﻿using System;
using Newtonsoft.Json;

namespace Jackal.Core
{
    public class Pirate(int teamId, TilePosition position)
    {
        [JsonProperty]
        public readonly Guid Id = Guid.NewGuid();

        [JsonProperty]
        public readonly int TeamId = teamId;

        [JsonProperty]
        public TilePosition Position = position;

        [JsonProperty]
        public bool IsDrunk;
        
        internal int? DrunkSinceTurnNo;

        [JsonProperty]
        public bool IsInTrap;
    }
}