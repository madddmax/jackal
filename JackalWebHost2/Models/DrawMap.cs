using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JackalWebHost.Models
{
    public class DrawMap
    {
        public int Width;
        public int Height;
        public List<TileChange> Changes;
    }
}