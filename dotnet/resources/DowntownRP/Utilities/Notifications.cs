using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace DowntownRP.Utilities
{
    public class Notifications : Script
    {
        public static void SendNotificationOK(Player player, string message)
        {
            player.SendNotification("~g~INFO: ~w~" + message);
            player.TriggerEvent("NotificationSound");
        }

        public static void SendNotificationERROR(Player player, string message)
        {
            player.SendNotification("~r~ERROR: ~w~" + message);
            player.TriggerEvent("NotificationSound");
        }

        public static void SendNotificationINFO(Player player, string message)
        {
            player.SendNotification("~b~INFO: ~w~" + message);
            player.TriggerEvent("NotificationSound");
        }

        [RemoteEvent("SendNotificationUser")]
        public void RE_SendNotificationUser(Player player, string text)
        {
            player.SendNotification(text);
            player.TriggerEvent("NotificationSound");
        }

        [RemoteEvent("throwNotImplemented")]
        public void RE_throwNotImplemented(Player player)
        {
            player.SendNotification("~r~INFO: ~w~ Función no implementada");
            player.TriggerEvent("NotificationSound");
        }

        [RemoteEvent("debug")]
        public void debug(Player player)
        {
            Console.WriteLine("it callss");
        }
    }
}
