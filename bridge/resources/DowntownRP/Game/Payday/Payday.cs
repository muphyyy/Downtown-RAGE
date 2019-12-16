using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;

namespace DowntownRP.Game.Payday
{
    public class Payday : Script
    {
        [ServerEvent(Event.ResourceStart)]
        public async Task PaydayFunction()
        {
            await Task.Delay(1000);

            while (true)
            {
                await Task.Delay(3600000);
                foreach (var player in NAPI.Pools.GetAllPlayers())
                {
                    //var info = player.GetExternalData<Data.Entities.User>(0);
                    if (!player.HasData("USER_CLASS")) return;
                    Data.Entities.User user = player.GetData("USER_CLASS");
                    if (user != null)
                    {
                        player.TriggerEvent("hideHUD");
                        player.TriggerEvent("HideChat");
                        await Task.Delay(500);

                        await PaydayPlayerFunction(player, user);

                        await Task.Delay(6000);
                        player.TriggerEvent("ShowChat");
                        player.TriggerEvent("showHUD");
                    }
                }
            }
        }

        [Command("teste")]
        public async Task CMD_test(Client player)
        {
            //var info = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");
            if (user != null)
            {
                player.TriggerEvent("hideHUD");
                player.TriggerEvent("HideChat");
                await Task.Delay(500);

                await PaydayPlayerFunction(player, user);

                await Task.Delay(6000);
                player.TriggerEvent("ShowChat");
                player.TriggerEvent("showHUD");
            }
        }

        public async Task PaydayPlayerFunction(Client player, Data.Entities.User user)
        {
            int level = user.level;
            int exp = user.exp;

            user.exp = exp + 1;

            if (user.exp == user.level * 4)
            {
                user.level = level + 1;
                player.TriggerEvent("testPaydayLevel", "¡Has subido de nivel!");
                await DatabaseFunctions.UpdateUserXp(user.idpj, user.exp);
                await DatabaseFunctions.UpdateUserLevel(user.idpj, user.level);
            }
            else
            {
                player.TriggerEvent("testPaydayLevel", "");
                await DatabaseFunctions.UpdateUserXp(user.idpj, user.exp);
            }

            player.SendChatMessage("<font color='purple'>---------<i class='fas fa-info-circle'></i> PAGOS E IMPUESTOS ---------</font>");
            player.SendChatMessage($"<font color='blue'><i class='fas fa-star'></i></font> Nivel {user.level} | Experiencia {user.exp}");

            if (user.bankAccount != 0)
            {
                await World.Banks.MoneyFunctions.RemoveMoneyBank(player, 10);
                player.SendChatMessage($"<font color='green'><i class='fas fa-money-check'></i></font> Impuestos por cuenta bancaria: <font color='green'>$10</font>");
                await World.Banks.MoneyFunctions.AddMoneyBank(player, 50); // Ganancia gubernamental
            }
            else await Money.MoneyModel.AddMoney(player, 50); // Ganancia gubernamental

            /*player.SendChatMessage($"<font color='green'><i class='fas fa-money-check'></i></font> Impuestos por vehículos: <font color='green'>$10</font>");
            player.SendChatMessage($"<font color='green'><i class='fas fa-money-check'></i></font> Impuestos por propiedades: <font color='green'>$10</font>");
            player.SendChatMessage($"<font color='green'><i class='fas fa-money-bill-alt'></i></font> Ganancias por empleo: <font color='green'>$10</font>");*/
            player.SendChatMessage($"<font color='green'><i class='fas fa-money-bill-alt'></i></font> Ayuda gubernamental: <font color='green'>$50</font>");
            player.SendChatMessage("<font color='purple'>---------<i class='fas fa-info-circle'></i> PAGOS E IMPUESTOS ---------</font>");
        }
    }
}
