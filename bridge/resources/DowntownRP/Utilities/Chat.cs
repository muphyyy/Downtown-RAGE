using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace DowntownRP.Utilities
{
    public class Chat : Script
    {
        [RemoteEvent("ActionPressT")]
        public void ActionPressT(Client player)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");
            user.chatStatus = true;
        }

        [RemoteEvent("ActionPressEnterOrEsc")]
        public void ActionPressEnterOrEsc(Client player)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");
            user.chatStatus = false;
        }

        public static void EntornoMe(Client player, string message)
        {
            var msg = "<font color='B950C3'>" + player.Name + " " + message + "</font>";
            var playersInRadius = NAPI.Player.GetPlayersInRadiusOfPlayer(30, player);

            foreach (var players in playersInRadius)
            {
                NAPI.Chat.SendChatMessageToPlayer(players, msg);
            }
        }

        public static void EntornoDo(Client player, string message)
        {
            var msg = "<font color='65C350'>" + message + " (" + player.Name + ")</font>";
            var playersInRadius = NAPI.Player.GetPlayersInRadiusOfPlayer(30, player);

            foreach (var players in playersInRadius)
            {
                NAPI.Chat.SendChatMessageToPlayer(players, msg);
            }
        }
    }
}
