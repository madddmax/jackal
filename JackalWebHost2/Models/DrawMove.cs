using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JackalWebHost.Models
{
    public class DrawMove
    {
        public int MoveNum;
        //public int PirateNum;
        public LevelPosition From;
        public LevelPosition To;
        public bool WithCoin;
        public bool WithRespawn; 
    }
}