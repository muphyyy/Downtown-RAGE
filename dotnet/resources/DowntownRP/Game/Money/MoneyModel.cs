using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DowntownRP.Game.Money
{
    public class MoneyModel : Script
    {
        public async static Task<bool> AddMoney(Player player, double money_to_add)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return false;
            Data.Entities.User user = player.GetData<Data.Entities.User>("USER_CLASS");

            if (money_to_add <= 0)
                return false;

            double actual_money = user.money;
            double money = actual_money += money_to_add;
            user.money = money;

            NAPI.ClientEvent.TriggerClientEvent(player, "UpdateMoneyHUD", Convert.ToString(money), "sumo");
            await DatabaseFunctions.UpdateMoney(user.idpj, money);
            return true;
        }

        public async static Task<bool> SubMoney(Player player, double money_to_sub)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return false;
            Data.Entities.User user = player.GetData<Data.Entities.User>("USER_CLASS");

            if (money_to_sub <= 0)
                return false;

            double actual_money = user.money;

            if (money_to_sub > actual_money)
                return false;

            if (user.money < 0)
            {
                user.money = 0;
                await DatabaseFunctions.UpdateMoney(user.idpj, 0);
            }

            double money = actual_money -= money_to_sub;
            user.money = money;
            NAPI.ClientEvent.TriggerClientEvent(player, "UpdateMoneyHUD", Convert.ToString(money), "resto");
            await DatabaseFunctions.UpdateMoney(user.idpj, money);
            return true;
        }

        public static bool HasEnoughMoney(Player player, double amount)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return false;
            Data.Entities.User user = player.GetData<Data.Entities.User>("USER_CLASS");

            if (user.money >= amount)
                return true;

            return false;
        }
    }
}
