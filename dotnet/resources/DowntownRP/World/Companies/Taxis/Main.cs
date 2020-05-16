using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace DowntownRP.World.Companies.Taxis
{
    public class Main : Script
    {
        [ServerEvent(Event.PlayerEnterVehicleAttempt)]
        public void Taxis_PlayerEnterVehicleAttempt(Player player, Vehicle vehicle, sbyte seatID)
        {

        }

    }
}
