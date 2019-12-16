using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;

namespace DowntownRP.Game.Voice
{
    public class Voice : Script
    {
        [RemoteEvent("add_voice_listener")]
        public void AddVoiceListener(Client player, Client target)
        {
            if (target != null)
            {
                player.EnableVoiceTo(target);
            }
        }

        [RemoteEvent("remove_voice_listener")]
        public void RemoveVoiceListener(Client player, Client target)
        {
            if (target != null)
            {
                player.DisableVoiceTo(target);
            }

        }

        [RemoteEvent("sendVoiceChangedNotification")]
        public async Task RE_sendVoiceChangedNotification(Client player, int type)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");

            if (type == 0)
            {
                await DatabaseFunctions.UpdateVoiceMode(user.idpj, 0);
                Utilities.Notifications.SendNotificationOK(player, "Has cambiado el modo de voz a transmisión continua");
                user.enableMicrophone = 0;
            }
            else
            {
                await DatabaseFunctions.UpdateVoiceMode(user.idpj, 1);
                Utilities.Notifications.SendNotificationOK(player, "Has cambiado el modo de voz a push to talk");
                user.enableMicrophone = 1;
            }
        }
    }
}
