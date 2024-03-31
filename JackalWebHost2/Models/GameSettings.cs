using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JackalWebHost.Models
{
    public class GameSettings
    {
        public string[] players { get; set; }
        public int? mapId { get; set; }
    }
}