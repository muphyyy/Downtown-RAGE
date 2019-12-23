using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DowntownRP.World.Business.CompCarShop
{
    public class Main : Script
    {
        [ServerEvent(Event.PlayerEnterVehicle)]
        public void OnPlayerEnterVehicle(Client player, Vehicle vehicle, sbyte seatID)
        {
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");

            if (vehicle.HasData("VEHICLE_BUSINESS_DATA"))
            {
                Data.Entities.VehicleBusiness veh = vehicle.GetData("VEHICLE_BUSINESS_DATA");

                if (veh.isCompanySelling)
                {
                    if (user.companyProperty != null)
                    {
                        if(veh.business.spawn != null) player.TriggerEvent("OpenCarshopBrowser", veh.business.name, veh.model, veh.price);
                        else Utilities.Notifications.SendNotificationERROR(player, "Esta empresa no tiene punto de spawn para vehículos. Contacta con un administrador.");
                    }
                    else Utilities.Notifications.SendNotificationERROR(player, "No eres dueño de ninguna empresa");
                }
            }
        }

        [RemoteEvent("BuyCarCompany")]
        public async Task RE_BuyCarCompany(Client player, string color1, string color2)
        {
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");

            Vehicle vehicle;
            if (player.IsInVehicle) vehicle = player.Vehicle;
            else return;

            if (vehicle.HasData("VEHICLE_BUSINESS_DATA"))
            {
                Data.Entities.VehicleBusiness veh = vehicle.GetData("VEHICLE_BUSINESS_DATA");

                if (veh.isCompanySelling)
                {
                    if (user.companyProperty != null)
                    {
                        if(await Game.Money.MoneyModel.SubMoney(player, (double)veh.price))
                        {
                            player.WarpOutOfVehicle();
                            await Task.Delay(1000);
                            NAPI.Task.Run(async () => 
                            {
                                Vehicle new_veh = NAPI.Vehicle.CreateVehicle(NAPI.Util.GetHashKey(veh.model), veh.business.spawn, veh.business.spawnRot, 1, 1);
                                player.SetIntoVehicle(new_veh, -1);

                                player.TriggerEvent("DestroyCarshopBrowser");

                                switch (color1)
                                {
                                    case "black":
                                        new_veh.PrimaryColor = 0;
                                        break;

                                    case "white":
                                        new_veh.PrimaryColor = 5;
                                        break;

                                    case "yellow":
                                        new_veh.PrimaryColor = 42;
                                        break;

                                    case "red":
                                        new_veh.PrimaryColor = 27;
                                        break;

                                    case "blue":
                                        new_veh.PrimaryColor = 80;
                                        break;

                                    case "green":
                                        new_veh.PrimaryColor = 55;
                                        break;

                                }

                                switch (color2)
                                {
                                    case "black":
                                        new_veh.SecondaryColor = 0;
                                        break;

                                    case "white":
                                        new_veh.SecondaryColor = 5;
                                        break;

                                    case "yellow":
                                        new_veh.SecondaryColor = 42;
                                        break;

                                    case "red":
                                        new_veh.SecondaryColor = 27;
                                        break;

                                    case "blue":
                                        new_veh.SecondaryColor = 80;
                                        break;

                                    case "green":
                                        new_veh.SecondaryColor = 55;
                                        break;

                                }

                                player.TriggerEvent("chat_goal", "¡Felicidades!", "Has comprado un nuevo vehículo para tu empresa");

                                new_veh.NumberPlate = "EMPRESA";
                                int vehid = await Companies.DbFunctions.CreateCompanyVehicle(user.companyProperty.id, veh.model, new_veh.PrimaryColor, new_veh.SecondaryColor, "EMPRESA", veh.business.spawn.X, veh.business.spawn.Y, veh.business.spawn.Z, veh.business.spawnRot);

                                Data.Entities.VehicleCompany veh_company = new Data.Entities.VehicleCompany()
                                {
                                    id = vehid,
                                    vehicle = new_veh,
                                    company = user.companyProperty,
                                    model = veh.model
                                };

                                new_veh.SetData("VEHICLE_COMPANY_DATA", veh_company);
                                await Task.Delay(2000);
                                Utilities.Notifications.SendNotificationINFO(player, "Para estacionar el vehículo en el lugar deseado usa /estacionarveh");
                            });
                        }
                        else Utilities.Notifications.SendNotificationERROR(player, "No tienes suficiente dinero");
                    }
                }
            }
        }

    }
}
