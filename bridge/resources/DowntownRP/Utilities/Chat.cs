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
    }
}
