using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace DowntownRP.Data.Entities
{
    public class VehicleCompany
    {
        public int id { get; set; }
        public string model { get; set; }
        public Vehicle vehicle { get; set; }
        public Company company { get; set; }
    }
}
