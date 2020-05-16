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
        public Vector3 spawn { get; set; } = new Vector3(0, 0, 0);
        public float spawnRot { get; set; }
    }
}
