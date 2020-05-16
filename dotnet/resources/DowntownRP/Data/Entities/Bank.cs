using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace DowntownRP.Data.Entities
{
    public class Bank
    {
        public int id { get; set; }
        public int type { get; set; }
        public bool isBank { get; set; } = false;
        public TextLabel label { get; set; }
        public Blip blip { get; set; }
        public Marker marker { get; set; }
    }
}
