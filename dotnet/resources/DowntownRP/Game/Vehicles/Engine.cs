using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DowntownRP.Game.Vehicles
{
    public class Engine : Script
    {
        [ServerEvent(Event.PlayerEnterVehicleAttempt)]
        public void Engine_PlayerEnterVehicleAttempt(Player player, Vehicle vehicle, sbyte seatID)
        {
            if (vehicle.EngineStatus == false) player.TriggerEvent("tipEngineVehicle");
        }

        [ServerEvent(Event.PlayerExitVehicle)]
        public void Engine_PlayerExitVehicle(Player player, Vehicle vehicle)
        {
            if (vehicle.HasSharedData("ENGINE_STATUS"))
            {
                if (vehicle.GetSharedData<bool>("ENGINE_STATUS") == true) vehicle.EngineStatus = true;
            }
        }

        [RemoteEvent("ActionEngineVehicle")]
        public async Task RE_ActionEngineVehicle(Player player)
        {
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData<Data.Entities.User>("USER_CLASS");

            if (!user.chatStatus)
            {
                if (player.IsInVehicle)
                {
                    if (player.Vehicle.EngineStatus)
                    {
                        player.Vehicle.EngineStatus = false;
                        Utilities.Chat.EntornoMe(player, "ha apagado el motor");
                        player.Vehicle.SetSharedData("ENGINE_STATUS", false);
                        return;
                    }
                    else
                    {
                        Utilities.Chat.EntornoMe(player, "está encendiendo el motor del vehículo");
                        await Task.Delay(1000);
                        player.Vehicle.EngineStatus = true;
                        Utilities.Chat.EntornoDo(player, "Motor encendido");
                        player.Vehicle.SetSharedData("ENGINE_STATUS", true);
                    }
                }
            }
        }
    }
}
