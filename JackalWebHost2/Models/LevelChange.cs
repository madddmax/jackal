using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JackalWebHost.Models
{
    public class LevelChange
    {
        public int Level;
        public bool hasPirates;
        public DrawPirate Pirate;
        public bool hasCoins;
        public DrawCoin Coin;
    }
}