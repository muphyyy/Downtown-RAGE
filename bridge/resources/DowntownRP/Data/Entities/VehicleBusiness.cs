using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace DowntownRP.Data.Entities
{
    public class VehicleBusiness
    {
        public int id { get; set; }
        public string model { get; set; }
        public Vehicle vehicle { get; set; }
        public Business business { get; set; }
        public TextLabel label { get; set; }
        public int price { get; set; }
        public bool isCompanySelling { get; set; }
        public bool isRentSelling { get; set; }
        public bool isNormalSelling { get; set; }
    }
}
