using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace DowntownRP.Game.Commands
{
    public class Main : Script
    {
        [ServerEvent(Event.ChatMessage)]
        public void OnChatMessage(Player player, string message)
        {
            var msg = $"{player.Name} dice: {message}";
            var playersInRadius = NAPI.Player.GetPlayersInRadiusOfPlayer(30, player);

            foreach (var players in playersInRadius)
            {
                NAPI.Chat.SendChatMessageToPlayer(players, msg);
            }
        }


        [Command("me", GreedyArg = true)]
        public void CMD_me(Player player, string message)
        {
            var msg = "<font color='B950C3'>" + player.Name + " " + message + "</font>";
            var playersInRadius = NAPI.Player.GetPlayersInRadiusOfPlayer(30, player);

            foreach (var players in playersInRadius)
            {
                NAPI.Chat.SendChatMessageToPlayer(players, msg);
            }
        }

        [Command("do", GreedyArg = true)]
        public void CMD_do(Player player, string message)
        {
            var msg = "<font color='65C350'>" + message + " (" + player.Name + ")</font>";
            var playersInRadius = NAPI.Player.GetPlayersInRadiusOfPlayer(30, player);

            foreach (var players in playersInRadius)
            {
                NAPI.Chat.SendChatMessageToPlayer(players, msg);
            }
        }
        
        [Command("b", GreedyArg = true, Alias = "ooc")]
        public void CMD_oc(Player player, string message)
        {
            var msg = "<font color='158D06'> <b>OOC-" + player.SocialClubName + ":</b> </font> <font color='808080'>" + message + "</font>";

            NAPI.Chat.SendChatMessageToAll(msg);
            Utilities.Webhooks.sendWebHook(1, "**"+player.SocialClubName+"**: "+message);
        }

       

        [Command("estacionarveh")]
        public async Task CMD_estacionarveh(Player player)
        {
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData<Data.Entities.User>("USER_CLASS");

            if (player.IsInVehicle)
            {
                if (user.companyProperty != null)
                {
                    Vehicle vehicle = player.Vehicle;

                    if (vehicle.HasData("VEHICLE_COMPANY_DATA"))
                    {
                        Data.Entities.VehicleCompany veh = vehicle.GetData<Data.Entities.VehicleCompany>("VEHICLE_COMPANY_DATA");
                        if(veh.company == user.companyProperty)
                        {
                            veh.spawn = player.Position;
                            veh.spawnRot = player.Heading;

                            await World.Companies.DbFunctions.UpdateCompanyVehicleSpawn(veh.id, player.Position.X, player.Position.Y, player.Position.Z, player.Heading);

                            Utilities.Notifications.SendNotificationOK(player, "Has actualizado las coordenadas de spawn de tu vehículo");
                        }
                        else Utilities.Notifications.SendNotificationERROR(player, "No estás en un vehículo de tu empresa");
                    }
                    else Utilities.Notifications.SendNotificationERROR(player, "No estás en un vehículo de empresa");
                }
                else Utilities.Notifications.SendNotificationERROR(player, "No eres dueño de una empresa");
            }
            else Utilities.Notifications.SendNotificationERROR(player, "No estás en un vehículo");
        }

        [Command ("panim")]
        public async Task CMD_panim(Player player, int id = -1)
        {
            if (id == -1)
            {
                player.StopAnimation();
            }
            else
            {
                Player target = Utilities.PlayerId.FindPlayerById(id);
                Data.Entities.User user = player.GetData<Data.Entities.User>("USER_CLASS");
                if (!(target == player))
                {

                    if (!(user.adminLv == 0))
                    {
                        target.StopAnimation();
                    }
                }else
                {
                    player.StopAnimation();
                }
            }

        }

        [Command ("anims")]
        public async Task CMD_anims (Player player)
        {
            player.TriggerEvent("CallAnimList");
        }
        
        [Command("ayuda")]
        public async Task CMD_ayuda(Player player)
        {
            player.TriggerEvent("PedirAyuda");
        }
    }
}
