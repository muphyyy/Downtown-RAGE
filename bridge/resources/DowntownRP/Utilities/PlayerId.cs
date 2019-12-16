using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace DowntownRP.Utilities
{
    public class PlayerId : Script
    {
        public static Client FindPlayerById(int id)
        {
            foreach (var player in NAPI.Pools.GetAllPlayers())
            {
                if (player.Value == id) return player;
                else return null;
            }
            return null;
        }
    }
}
