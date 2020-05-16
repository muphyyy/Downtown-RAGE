using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DowntownRP.World.Companies
{
    public class Menu : Script
    {
        [RemoteEvent("ActionMenuCompany")]
        public void RE_ActionMenuCompany(Player player)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData<Data.Entities.User>("USER_CLASS");
            if (user.isInCompanyInterior)
            {
                if (user.companyInterior.owner == user.idpj)
                {
                    if (!user.isCompanyCefOpen)
                    {
                        player.TriggerEvent("OpenMenuCompany", (int)user.companyInterior.safeBox, user.companyInterior.workers, user.companyInterior.percentage, user.companyInterior.subsidy, user.companyInterior.name);
                        user.isCompanyCefOpen = true;
                        return;
                    }
                    else
                    {
                        player.TriggerEvent("DestroyMenuCompany");
                        user.isCompanyCefOpen = false;
                    }
                }
            }
        }

        [RemoteEvent("MenuCompanyClose")]
        public void RE_MenuCompanyClose(Player player)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData<Data.Entities.User>("USER_CLASS");
            player.TriggerEvent("DestroyMenuCompany");
            user.isCompanyCefOpen = false;
        }

        [RemoteEvent("MenuCompanyChangeName")]
        public async Task RE_MenuCompanyChangeName(Player player, string name)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData<Data.Entities.User>("USER_CLASS");
            if (user.companyInterior.owner == user.idpj)
            {
                await DbFunctions.UpdateCompanyName(user.companyInterior.id, name);
                user.companyInterior.name = name;
                user.companyInterior.blip.Name = name;
                user.companyInterior.label.Text = $"{name}~n~Pulsa ~y~F5 ~w~para interactuar~n~~p~{user.companyInterior.area}, {user.companyInterior.number}";

                Utilities.Notifications.SendNotificationOK(player, $"Has cambiado el nombre de la empresa a {name}");
            }
        }

        [RemoteEvent("MenuCompanyOfficeStatus")]
        public void RE_MenuCompanyOfficeStatus(Player player, int status)
        {
            // var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData<Data.Entities.User>("USER_CLASS");
            if (user.companyInterior.owner == user.idpj)
            {
                switch (status)
                {
                    case 1:
                        user.companyInterior.isOpen = true;
                        Utilities.Notifications.SendNotificationOK(player, $"Has abierto la oficina");
                        break;

                    case 2:
                        user.companyInterior.isOpen = false;
                        Utilities.Notifications.SendNotificationOK(player, $"Has cerrado la oficina");
                        break;
                }
            }
        }

        [RemoteEvent("MenuCompanyRecruitmentMode")]
        public void RE_MenuCompanyRecruitmentMode(Player player, int status)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData<Data.Entities.User>("USER_CLASS");
            if (user.companyInterior.owner == user.idpj)
            {
                switch (status)
                {
                    case 1:
                        user.companyInterior.ManualRecruitment = false;
                        Utilities.Notifications.SendNotificationOK(player, "Has cambiado el modo de contrato a automático");
                        break;

                    case 2:
                        user.companyInterior.ManualRecruitment = true;
                        Utilities.Notifications.SendNotificationOK(player, "Has cambiado el modo de contrato a manual");
                        break;
                }
            }
        }

        [RemoteEvent("MenuCompanyGanancias")]
        public async Task RE_MenuCompanyGanancias(Player player, string cant)
        {
            int cantidad = Convert.ToInt32(cant);

            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData<Data.Entities.User>("USER_CLASS");
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (user.companyInterior.owner == user.idpj)
            {
                await DbFunctions.UpdateCompanyPercentage(user.companyInterior.id, cantidad);
                user.companyInterior.percentage = cantidad;
                Utilities.Notifications.SendNotificationOK(player, $"Has cambiado el porcentaje de ganancias a {cantidad}%");
            }
        }

        [RemoteEvent("MenuCompanySubsidio")]
        public async Task RE_MenuCompanySubsidio(Player player, string cant)
        {
            int cantidad = Convert.ToInt32(cant);

            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData<Data.Entities.User>("USER_CLASS");
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (user.companyInterior.owner == user.idpj)
            {
                await DbFunctions.UpdateCompanySubsidy(user.companyInterior.id, cantidad);
                user.companyInterior.subsidy = cantidad;
                Utilities.Notifications.SendNotificationOK(player, $"Has cambiado el subsidio a ~g~${cantidad}");
            }
        }

        [RemoteEvent("MenuCompanyIngresar")]
        public async Task RE_MenuCompanyIngresar(Player player, string cant)
        {
            int cantidad = Convert.ToInt32(cant);

            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData<Data.Entities.User>("USER_CLASS");
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (user.companyInterior.owner == user.idpj)
            {
                if (await Game.Money.MoneyModel.SubMoney(player, (double)cantidad))
                {
                    user.companyInterior.safeBox = user.companyInterior.safeBox + cantidad;
                    await DbFunctions.UpdateCompanySafebox(user.companyInterior.id, (int)user.companyInterior.safeBox);
                    Utilities.Notifications.SendNotificationOK(player, $"Has ingresado ${cantidad} a la caja fuerte de tu empresa");
                }
                else Utilities.Notifications.SendNotificationERROR(player, "No tienes suficiente dinero");
            }
        }

        [RemoteEvent("MenuCompanyRetirar")]
        public async Task RE_MenuCompanyRetirar(Player player, string cant)
        {
            int cantidad = Convert.ToInt32(cant);

            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData<Data.Entities.User>("USER_CLASS");
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (user.companyInterior.owner == user.idpj)
            {
                if (user.companyInterior.safeBox <= cantidad)
                {
                    user.companyInterior.safeBox = user.companyInterior.safeBox - cantidad;
                    await DbFunctions.UpdateCompanySafebox(user.companyInterior.id, (int)user.companyInterior.safeBox);
                    Utilities.Notifications.SendNotificationOK(player, $"Has retirado ${cantidad} de la caja fuerte de tu empresa");

                }
                else Utilities.Notifications.SendNotificationERROR(player, "No tienes suficiente dinero en la caja fuerte");
            }
        }

    }
}
