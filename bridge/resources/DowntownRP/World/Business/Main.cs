using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DowntownRP.World.Business
{
    public class Main : Script
    {
        [ServerEvent(Event.PlayerEnterColshape)]
        public void Business_EnterColShape(ColShape shape, Client player)
        {
            //var business = shape.GetExternalData<Data.Entities.Business>(0);
            if (!shape.HasData("BUSINESS_CLASS")) return;
            Data.Entities.Business business = shape.GetData("BUSINESS_CLASS");

            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");

            if (business != null)
            {
                if (business.owner == 0) player.TriggerEvent("displayBusinessVenta");
                else
                {
                    if (business.type == 6 || business.type == 7 || business.type == 8) player.TriggerEvent("adviceBuyVehicle");
                }

                user.isInBusiness = true;
                user.business = business;
            }
        }

        [ServerEvent(Event.PlayerExitColshape)]
        public void Business_ExitColShape(ColShape shape, Client player)
        {
            //var business = shape.GetExternalData<Data.Entities.Business>(0);
            if (!shape.HasData("BUSINESS_CLASS")) return;
            Data.Entities.Business business = shape.GetData("BUSINESS_CLASS");
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");

            if (business != null)
            {
                user.isInBusiness = false;
                user.business = null;
            }
        }

        [RemoteEvent("BusinessFinishCreation")]
        public async Task RE_BusinessFinishCreation(Client player, string name)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");

            if (user.adminLv == 5)
            {
                if (player.HasData("CreateBusinessType"))
                {
                    if (player.HasData("CreateBusinessPrice"))
                    {
                        int streetid = await DbFunctions.GetLastStreetNumber(name) + 1;

                        //int type = player.GetData<int>("CreateCompanyType");
                        //int price = player.GetData<int>("CreateCompanyPrice
                        int type = player.GetData("CreateBusinessType");
                        int price = player.GetData("CreateBusinessPrice");

                        int idempresa = await World.Business.DbFunctions.CreateBusiness(player, type, price, name, streetid);
                        Data.Entities.Business business = new Data.Entities.Business();

                        ColShape buss = NAPI.ColShape.CreateCylinderColShape(player.Position, 2, 2);
                        Marker marker = NAPI.Marker.CreateMarker(0, player.Position, new Vector3(), new Vector3(), 1, new Color(248, 218, 79));
                        TextLabel label = NAPI.TextLabel.CreateTextLabel($"Negocio en venta~n~Pulsa ~y~F5 ~w~para interactuar~n~~p~{name}, {streetid}", player.Position, 5, 1, 0, new Color(255, 255, 255));
                        Blip blip = NAPI.Blip.CreateBlip(player.Position);
                        blip.Color = 3;
                        blip.Name = "Negocio en venta";

                        switch (type)
                        {
                            case 1:
                                blip.Sprite = 198;
                                break;

                            case 2:
                                blip.Sprite = 477;
                                break;

                            case 3:
                                blip.Sprite = 72;
                                break;

                            case 4:
                                blip.Sprite = 528;
                                break;

                            case 5:
                                blip.Sprite = 135;
                                break;

                            case 6:
                                blip.Sprite = 135;
                                label.Text = $"Negocio en venta~n~~p~{name}, {streetid}";
                                break;

                            case 7:
                                blip.Sprite = 135;
                                label.Text = $"Negocio en venta~n~~p~{name}, {streetid}";
                                break;

                            case 8:
                                blip.Sprite = 135;
                                label.Text = $"Negocio en venta~n~~p~{name}, {streetid}";
                                break;

                            case 9:
                                blip.Sprite = 135;
                                break;
                        }

                        business.id = idempresa;
                        business.type = type;
                        business.name = "Compañía en venta";
                        business.owner = 0;
                        business.price = price;
                        business.blip = blip;
                        business.marker = marker;
                        business.label = label;
                        business.area = name;
                        business.number = streetid;
                        business.safeBox = 0;
                        business.shape = buss;

                        //empresa.SetExternalData<Data.Entities.Company>(0, company);
                        buss.SetData("BUSINESS_CLASS", business);
                    }
                }
            }
        }

        [RemoteEvent("ActionOpenBuyBusiness")]
        public void RE_ActionOpenBuyBusiness(Client player)
        {
            if (!player.HasData("USER_CLASS")) return;

            Data.Entities.User user = player.GetData("USER_CLASS");
            if (user.isInBusiness)
            {
                if(user.business.owner == 0)
                {
                    if (!user.isBusinessCefOpen)
                    {
                        player.TriggerEvent("OpenBusinessBuyBrowser", GetNameTypeBusiness(user.business.type), user.business.price, $"{user.business.area}, {user.business.number}");
                        user.isBusinessCefOpen = true;
                        return;
                    }
                    else
                    {
                        player.TriggerEvent("DestroyBusinessBuyBrowser");
                        user.isBusinessCefOpen = false;
                    }
                }
            }
        }

        [RemoteEvent("CloseBrowserBuyBusiness")]
        public void RE_CloseBrowserBuyBusiness(Client player)
        {
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");

            player.TriggerEvent("DestroyBusinessBuyBrowser");
            user.isBusinessCefOpen = false;
        }

        [RemoteEvent("BuyBusiness")]
        public async Task RE_BuyBusiness(Client player)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");

            if (user.business != null)
            {
                if (await Game.Money.MoneyModel.SubMoney(player, (double)user.business.price))
                {
                    await DbFunctions.UpdateBusinessOwner(user.business.id, user.idpj);
                    player.TriggerEvent("DestroyBusinessBuyBrowser");
                    user.isBusinessCefOpen = false;

                    user.business.owner = user.idpj;
                    user.business.label.Text = $"Negocio~n~Pulsa ~y~F5 ~w~para interactuar~n~{user.business.area}, {user.business.number}";

                    player.TriggerEvent("chat_goal", "¡Felicidades!", "Ahora eres propietario de un negocio");
                    await Task.Delay(2000);
                    Utilities.Notifications.SendNotificationINFO(player, "Para configurar tu negocio presiona ~g~F6");
                }
                else Utilities.Notifications.SendNotificationERROR(player, "No tienes suficiente dinero para comprar este negocio");
            }
        }

        public static string GetNameTypeBusiness(int type)
        {
            switch (type)
            {
                case 1:
                    return "Supermarket";

                case 2:
                    return "Tienda de ropa";

                case 3:
                    return "Gasolinera";

                case 4:
                    return "Tatuador";

                case 5:
                    return "Peluquero";

                case 6:
                    return "Renta";

                case 7:
                    return "Concesionario";

                case 8:
                    return "Concesionario empresas";

                case 9:
                    return "Bar";

                default:
                    return "N/A";
            }
        }
    }
}
