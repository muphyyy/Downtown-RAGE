using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DowntownRP.World.Companies
{
    public class Main : Script
    {
        [ServerEvent(Event.PlayerEnterColshape)]
        public void Banks_EnterColShape(ColShape shape, Client player)
        {
            //var company = shape.GetExternalData<Data.Entities.Company>(0);
            if (!shape.HasData("COMPANY_CLASS")) return;
            Data.Entities.Company company = shape.GetData("COMPANY_CLASS");

            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");

            if (company != null)
            {
                if(company.interior == shape)
                {
                    user.isInCompanyExitInterior = true;
                    return;
                }

                if(company.contract == shape)
                {
                    user.isInCompanyContract = true;
                    return;
                }

                user.isInCompany = true;
                user.company = company;
            }
        }

        [ServerEvent(Event.PlayerExitColshape)]
        public void Banks_ExitColShape(ColShape shape, Client player)
        {
            //var company = shape.GetExternalData<Data.Entities.Company>(0);
            if (!shape.HasData("COMPANY_CLASS")) return;
            Data.Entities.Company company = shape.GetData("COMPANY_CLASS");
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");

            if (company != null)
            {
                if (company.interior == shape)
                {
                    user.isInCompanyExitInterior = false;
                    return;
                }

                if (company.contract == shape)
                {
                    user.isInCompanyContract = false;
                    return;
                }

                user.isInCompany = false;
                user.company = null;
            }
        }

        [RemoteEvent("ActionEnterCompany")]
        public void RE_ActionEnterCompany(Client player)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");

            // Exit from interior
            if (user.isInCompanyExitInterior) Interior.EnterCompany(player);

            // Enter to interior
            if (user.isInCompany)
            {
                if (user.company.owner == 0)
                {
                    if (!user.isCompanyCefOpen)
                    {
                        string street = $"{user.company.area}, {user.company.number}";
                        player.TriggerEvent("OpenCompanyBrowser", GetNameTypeCompany(user.company.type), user.company.price, street);
                        user.isCompanyCefOpen = true;
                        return;
                    }
                    else
                    {
                        player.TriggerEvent("CloseCompanyBrowser");
                        user.isCompanyCefOpen = false;
                    }
                }
                else
                {
                    if (user.company.isOpen)
                    {
                        if (user.company.owner == user.idpj) player.TriggerEvent("tipMenuEmpresa");
                        Interior.EnterCompany(player);
                    }
                    else
                    {
                        if (user.company.owner == user.idpj)
                        {
                            Interior.EnterCompany(player);
                            player.TriggerEvent("tipMenuEmpresa");
                        }
                        else Utilities.Notifications.SendNotificationERROR(player, "Esta empresa se encuentra cerrada");
                    }
                }
            }
        }

        [RemoteEvent("CompanyCloseBrowserSS")]
        public void RE_CompanyCloseBrowserSS(Client player)
        {
            player.TriggerEvent("CloseCompanyBrowser");
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");
            user.isCompanyCefOpen = false;
            //player.GetExternalData<Data.Entities.User>(0).isCompanyCefOpen = false;
        }

        [RemoteEvent("BuyCompany")]
        public async Task RE_BuyCompany(Client player)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");
            if (user.company != null)
            {
                if (await Game.Money.MoneyModel.SubMoney(player, (double)user.company.price))
                {
                    await DbFunctions.UpdateCompanyOwner(user.company.id, user.idpj);

                    player.TriggerEvent("CloseCompanyBrowser");
                    user.isCompanyCefOpen = false;

                    user.company.owner = user.idpj;
                    user.company.label.Text = $"Empresa~n~~r~Cerrado~n~~p~{user.company.area}, {user.company.number}";

                    user.companyProperty = user.company;

                    player.TriggerEvent("chat_goal", "¡Felicidades!", "Ahora eres propietario de una empresa");
                    await Task.Delay(2000);
                    Utilities.Notifications.SendNotificationINFO(player, "Entra en tu empresa (pulsa F5) para poder configurarla");
                }
                else Utilities.Notifications.SendNotificationERROR(player, "No tienes suficiente dinero para comprar esta empresa");
            }
        }

        [RemoteEvent("CompanyFinishCreation")]
        public async Task RE_CompanyFinishCreation(Client player, string name)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");

            if (user.adminLv == 5)
            {
                if (player.HasData("CreateCompanyType"))
                {
                    if (player.HasData("CreateCompanyPrice"))
                    {
                        int streetid = await DbFunctions.GetLastStreetNumber(name) + 1;

                        //int type = player.GetData<int>("CreateCompanyType");
                        //int price = player.GetData<int>("CreateCompanyPrice
                        int type = player.GetData("CreateCompanyType");
                        int price = player.GetData("CreateCompanyPrice");

                        int idempresa = await World.Companies.DbFunctions.CreateCompany(player, type, price, name, streetid);
                        Data.Entities.Company company = new Data.Entities.Company();

                        ColShape empresa = NAPI.ColShape.CreateCylinderColShape(player.Position, 2, 2);
                        Marker marker = NAPI.Marker.CreateMarker(0, player.Position, new Vector3(), new Vector3(), 1, new Color(248, 218, 79));
                        TextLabel label = NAPI.TextLabel.CreateTextLabel($"Compañía en venta~n~Pulsa ~y~F5 ~w~para interactuar~n~~p~{name}, {streetid}", player.Position, 5, 1, 0, new Color(255, 255, 255));
                        Blip blip = NAPI.Blip.CreateBlip(player.Position);
                        blip.Color = 3;
                        blip.Name = "Compañía en venta";

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
                        }

                        company.id = idempresa;
                        company.type = type;
                        company.name = "Compañía en venta";
                        company.owner = 0;
                        company.price = price;
                        company.blip = blip;
                        company.marker = marker;
                        company.label = label;
                        company.area = name;
                        company.number = streetid;
                        company.safeBox = 0;
                        company.workers = 0;
                        company.percentage = 0;
                        company.subsidy = 0;
                        company.shape = empresa;

                        //empresa.SetExternalData<Data.Entities.Company>(0, company);
                        empresa.SetData("COMPANY_CLASS", company);

                        Data.Lists.Companies.Add(company);
                    }
                }
            }
        }

        [RemoteEvent("ActionSignContractCompany")]
        public void RE_ActionSignContractCompany(Client player)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");

            if (user.isInCompanyContract)
            {
                if (user.companyInterior.owner != user.idpj)
                {
                    if(user.companyMember != user.companyInterior)
                    {
                        if (!user.companyInterior.ManualRecruitment)
                        {
                            if (!user.isCompanyCefOpen)
                            {
                                player.TriggerEvent("CreateContractCompanyBrowser");
                                user.isCompanyCefOpen = true;
                            }
                            else
                            {
                                player.TriggerEvent("DestroyContractCompanyBrowser");
                                user.isCompanyCefOpen = false;
                            }
                        }
                        else Utilities.Notifications.SendNotificationERROR(player, "Esta empresa tiene restringidos los contratos automáticos");
                    }
                    else Utilities.Notifications.SendNotificationERROR(player, "Ya trabajas para esta empresa");
                }
                else Utilities.Notifications.SendNotificationERROR(player, "Eres el dueño de esta empresa");
            }
        }

        [RemoteEvent("SignContractCompany")]
        public async Task RE_SignContractCompany(Client player)
        {
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");

            if (user.isInCompanyContract)
            {
                player.TriggerEvent("DestroyContractCompanyBrowser");
                user.isCompanyCefOpen = false;

                await Game.CharacterSelector.CharacterSelector.UpdateUserCompany(user.idpj, user.companyInterior.id);
                user.companyMember = user.companyInterior;

                player.TriggerEvent("chat_goal", "¡Felicidades!", $"Has firmado un contrato de empleo con {user.companyInterior.name}");
            }
        }

        [RemoteEvent("CloseContractCompany")]
        public void RE_CloseContractCompany(Client player)
        {
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");

            player.TriggerEvent("DestroyContractCompanyBrowser");
            user.isCompanyCefOpen = false;
        }

        public static string GetNameTypeCompany(int type)
        {
            switch (type)
            {
                case 1:
                    return "Taxista";

                case 2:
                    return "Camioneros";

                case 3:
                    return "Mecánicos";

                case 4:
                    return "Mineros";

                case 5:
                    return "CNN";

                default:
                    return "N/A";
            }
        }
    }
}
