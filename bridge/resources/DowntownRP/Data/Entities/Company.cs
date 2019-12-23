using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace DowntownRP.Data.Entities
{
    public class Company
    {
        public int id { get; set; }
        public int owner { get; set; }
        public int type { get; set; } // 1 taxi, 2 camioneros, 3 mecanicos, 4 mineros, 5 cnn
        public string name { get; set; }
        public int price { get; set; }
        public string area { get; set; }
        public int number { get; set; }
        public bool isOpen { get; set; } = true;
        public ColShape shape { get; set; }
        public TextLabel label { get; set; }
        public Marker marker { get; set; }
        public Blip blip { get; set; }
        public double safeBox { get; set; }
        public int workers { get; set; }
        public int percentage { get; set; }
        public int subsidy { get; set; }
        public bool ManualRecruitment { get; set; } = false;

        // Interior entities
        public ColShape interior { get; set; }
        public ColShape duty { get; set; }
        public ColShape contract { get; set; }
    }
}
