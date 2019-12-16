using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DowntownRP.Events
{
    public class Disconnect : Script
    {
        [ServerEvent(Event.PlayerDisconnected)]
        public void Event_PlayerDisconnected(Client player, DisconnectionType type, string reason)
        {
            Data.Info.playersConnected = Data.Info.playersConnected - 1;
            NAPI.ClientEvent.TriggerClientEventForAll("update_hud_players", Data.Info.playersConnected);
        }
    }
}
