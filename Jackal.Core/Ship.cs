using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Jackal
{
    public class Ship
    {
        [JsonProperty]
        public int TeamId;

        [JsonProperty]
        public Position Position;

        [JsonProperty]
        public int Coins;

        public Ship(int teamId, Position position)
        {
            TeamId = teamId;
            Position = position;
            Coins = 0;
        }
    }
}