using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;

namespace DowntownRP.Game.Commands
{
    public class Admin : Script
    {
        // Main commands
        [Command("crearveh")]
        public void CMD_crearveh(Client player, string vehicle_name)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");
            if (user.adminLv >= 5)
            {
                uint hash = NAPI.Util.GetHashKey(vehicle_name);
                Vehicle veh = NAPI.Vehicle.CreateVehicle(hash, player.Position.Around(5), 0, 0, 0);
                veh.EngineStatus = true;
                return;
            }
            else player.SendChatMessage("<font color='red'>[ERROR]</font> El comando no existe. (/ayuda para mas información)");
        }

        [Command("healthveh")]
        public void CMD_healthveh(Client player, float health)
        {
            if (player.IsInVehicle)
            {
                Vehicle veh = player.Vehicle;
                player.SendChatMessage($"{veh.Health}");
            }
        }

        [Command("borrarveh")]
        public void CMD_borrarveh(Client player)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");
            if (user.adminLv >= 5)
            {
                if (player.IsInVehicle)
                {
                    Vehicle veh = player.Vehicle;
                    veh.Delete();
                }
                else Utilities.Notifications.SendNotificationERROR(player, "No estás en un vehículo");
            }
            else player.SendChatMessage("<font color='red'>[ERROR]</font> El comando no existe. (/ayuda para mas información)");
        }

        [Command("fly")]
        public void CMD_fly(Client player)
        {
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (user.adminLv >= 5)
            {
                player.TriggerEvent("flyModeStart");
            }
            else player.SendChatMessage("<font color='red'>[ERROR]</font> El comando no existe. (/ayuda para mas información)");
        }

        [Command("dardinero")]
        public async Task CMD_dardinero(Client player, int id, double dinero)
        {
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (user.adminLv >= 5)
            {
                Client target = Utilities.PlayerId.FindPlayerById(id);
                if (target == null) Utilities.Notifications.SendNotificationERROR(player, "No hay ningún jugador conectado con esta id");
                else
                {
                    await Money.MoneyModel.AddMoney(target, dinero);
                    Utilities.Notifications.SendNotificationOK(player, $"Le has dado ${dinero} a {target.Name}");
                    Utilities.Notifications.SendNotificationOK(target, $"Un administrador te ha entregado ${dinero}");
                    Utilities.Webhooks.sendWebHook(1, $"💲 [{DateTime.Now.ToString()}] {player.Name} le ha dado ${dinero} a {target.Name}");
                }
            }
        }

        [Command("getpos")]
        public void CMD_obtenerpos(Client player)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");
            if (user.adminLv >= 1)
            {
                player.SendChatMessage($"{player.Position}");
                Console.WriteLine($"{player.Position}, {player.Heading}");
            }
        }

        [Command("irpos")]
        public void CMD_irpos(Client player, float x, float y, float z)
        {
            player.Position = new Vector3(x, y, z);
        }

        // Bank commands
        [Command("crearbanco")]
        public async Task CMD_crearnegocio(Client player, int type)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");
            if (user.adminLv == 5)
            {
                if (type == 1 || type == 2)
                {
                    int bankId = await World.Banks.DatabaseFunctions.CreateBank(player, type);

                    ColShape bank = NAPI.ColShape.CreateCylinderColShape(player.Position, 2, 6);
                    TextLabel label = NAPI.TextLabel.CreateTextLabel("Pulsa ~y~F5 ~w~para interactuar", player.Position.Subtract(new Vector3(0, 0, 0.1)), 15, 6, 2, new Color(255, 255, 255));
                    Marker marker = NAPI.Marker.CreateMarker(0, player.Position.Subtract(new Vector3(0, 0, 0.1)), new Vector3(), new Vector3(), 1, new Color(251, 244, 1));
                    Blip blip = NAPI.Blip.CreateBlip(player.Position);

                    if (type == 1)
                    {
                        blip.Sprite = 108;
                        blip.Scale = 1f;
                        blip.ShortRange = true;
                        blip.Name = "Banco";
                        blip.Color = 5;
                    }
                    else
                    {
                        blip.Sprite = 277;
                        blip.Scale = 1f;
                        blip.ShortRange = true;
                        blip.Name = "ATM";
                        blip.Color = 5;
                    }

                    Data.Entities.Bank banco = new Data.Entities.Bank
                    {
                        blip = blip,
                        marker = marker,
                        label = label,
                        id = bankId,
                        type = type
                    };

                    //bank.SetExternalData<Data.Entities.Bank>(0, banco);
                    bank.SetData("BANK_CLASS", banco);

                    Utilities.Notifications.SendNotificationOK(player, $"Has creado el banco con ID {bankId}");
                }
                else
                {
                    player.SendChatMessage("<font color='red'>ERROR</font> No existe ese tipo de banco");
                }
            }
            else player.SendChatMessage("<font color='red'>[ERROR]</font> El comando no existe. (/ayuda para mas información)");
        }

        [Command("borrarbanco")]
        public async Task CMD_borrarbanco(Client player)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");
            if (user.adminLv == 5)
            {
                if (user.isInBank)
                {
                    await World.Banks.DatabaseFunctions.DeleteBank(user.bankEntity.id);
                    user.bankEntity.label.Delete();
                    user.bankEntity.blip.Delete();
                    user.bankEntity.marker.Delete();

                    Utilities.Notifications.SendNotificationOK(player, $"Has borrado un banco correctamente");
                }
                else Utilities.Notifications.SendNotificationERROR(player, "No estás en un banco");
            }
            else player.SendChatMessage("<font color='red'>[ERROR]</font> El comando no existe. (/ayuda para mas información)");
        }

        // Company commands
        [Command("crearempresa")]
        public void CMD_crearempresa(Client player, int type, int price)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");

            if (user.adminLv == 5)
            {
                if (type == 1 || type == 2 || type == 3 || type == 4 || type == 5)
                {
                    //player.SetData<int>("CreateCompanyType", type);
                    //player.SetData<int>("CreateCompanyPrice", price);

                    player.SetData("CreateCompanyType", type);
                    player.SetData("CreateCompanyPrice", price);
                    player.TriggerEvent("CompanyAddStreetName");
                }
                else Utilities.Notifications.SendNotificationERROR(player, "No existe ese tipo de empresa");
            }
        }

        [Command("borrarempresa")]
        public async Task CMD_borrarempresa(Client player)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");
            if (user.adminLv == 5)
            {
                if (user.isInCompany)
                {
                    user.company.label.Delete();
                    user.company.blip.Delete();
                    user.company.marker.Delete();
                    user.company.shape.Delete();
                    user.company = null;

                    await World.Companies.DbFunctions.DeleteCompany(user.company.id);

                    Utilities.Notifications.SendNotificationOK(player, $"Has borrado una empresa correctamente");
                }
                else Utilities.Notifications.SendNotificationERROR(player, "No estás en una empresa");
            }
            else player.SendChatMessage("<font color='red'>[ERROR]</font> El comando no existe. (/ayuda para mas información)");
        }

        // Business commands
        [Command("crearnegocio")]
        public void CMD_crearnegocio(Client player, int type, int price)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");

            if (user.adminLv == 5)
            {
                if (type == 1 || type == 2 || type == 3 || type == 4 || type == 5 || type == 6 || type == 7 || type == 8 || type == 9)
                {
                    //player.SetData<int>("CreateCompanyType", type);
                    //player.SetData<int>("CreateCompanyPrice", price);

                    player.SetData("CreateBusinessType", type);
                    player.SetData("CreateBusinessPrice", price);
                    player.TriggerEvent("BusinessAddStreetName");
                }
                else Utilities.Notifications.SendNotificationERROR(player, "No existe ese tipo de negocio");
            }
        }

        [Command("borrarnegocio")]
        public async Task CMD_borrarnegocio(Client player)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");
            if (user.adminLv == 5)
            {
                if (user.isInBusiness)
                {
                    user.business.label.Delete();
                    user.business.blip.Delete();
                    user.business.marker.Delete();
                    user.business.shape.Delete();
                    user.business = null;

                    await World.Business.DbFunctions.DeleteBusiness(user.business.id);

                    Utilities.Notifications.SendNotificationOK(player, $"Has borrado un negocio correctamente");
                }
                else Utilities.Notifications.SendNotificationERROR(player, "No estás en un negocio");
            }
            else player.SendChatMessage("<font color='red'>[ERROR]</font> El comando no existe. (/ayuda para mas información)");
        }

    }
}
