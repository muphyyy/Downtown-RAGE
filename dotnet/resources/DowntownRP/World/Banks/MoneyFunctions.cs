using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;

namespace DowntownRP.World.Banks
{
    public class MoneyFunctions : Script
    {
        public async static Task AddMoneyBank(Player player, double quantity)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData<Data.Entities.User>("USER_CLASS");

            double bank = user.bank;
            user.bank = bank + quantity;
            await DatabaseFunctions.UpdateMoneyBankDb(player, bank + quantity);
            player.TriggerEvent("update_hud_bank", (int)user.bank);
        }

        public async static Task<bool> RemoveMoneyBank(Player player, double quantity)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return false;
            Data.Entities.User user = player.GetData<Data.Entities.User>("USER_CLASS");

            if (quantity <= 0)
                return false;

            double actual_money = user.bank;

            if (quantity > actual_money)
                return false;

            if (user.bank < 0)
            {
                user.bank = 0;
                await DatabaseFunctions.UpdateMoneyBankDb(player, 0);
            }

            double money = actual_money -= quantity;
            user.bank = money;
            await DatabaseFunctions.UpdateMoneyBankDb(player, money);
            player.TriggerEvent("update_hud_bank", (int)user.bank);
            return true;
        }
    }
}
