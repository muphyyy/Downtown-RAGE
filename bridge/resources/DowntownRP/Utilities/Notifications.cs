using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace DowntownRP.Utilities
{
    public class Notifications : Script
    {
        public static void SendNotificationOK(Client player, string message)
        {
            player.SendNotification("~g~INFO: ~w~" + message);
            player.TriggerEvent("NotificationSound");
        }

        public static void SendNotificationERROR(Client player, string message)
        {
            player.SendNotification("~r~ERROR: ~w~" + message);
            player.TriggerEvent("NotificationSound");
        }

        public static void SendNotificationINFO(Client player, string message)
        {
            player.SendNotification("~b~INFO: ~w~" + message);
            player.TriggerEvent("NotificationSound");
        }

        [RemoteEvent("SendNotificationUser")]
        public void RE_SendNotificationUser(Client player, string text)
        {
            player.SendNotification(text);
            player.TriggerEvent("NotificationSound");
        }

        [RemoteEvent("throwNotImplemented")]
        public void RE_throwNotImplemented(Client player)
        {
            player.SendNotification("~r~INFO: ~w~ Función no implementada");
            player.TriggerEvent("NotificationSound");
        }

        [RemoteEvent("debug")]
        public void debug(Client player)
        {
            Console.WriteLine("it callss");
        }
    }
}
