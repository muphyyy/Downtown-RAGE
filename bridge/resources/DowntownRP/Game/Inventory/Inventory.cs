using DowntownRP.Data.Entities;
using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DowntownRP.Game.Inventory
{
    public class Inventory : Script
    {

        [RemoteEvent("ActionInventory")]
        public void RE_ActionInventory(Client player)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");

            if (!user.isInventoryOpen)
            {
                player.TriggerEvent("OpenInventory", JsonConvert.SerializeObject(user.inventory));
                user.isInventoryOpen = true;
                return;
            }
            else
            {
                player.TriggerEvent("CloseInventory");
                user.isInventoryOpen = false;
                return;
            }
        }

        [RemoteEvent("debuginv")]
        public void RE_debuginv(Client player, string lol)
        {
            Console.WriteLine($"eso es {lol}");
        }

    }
}
