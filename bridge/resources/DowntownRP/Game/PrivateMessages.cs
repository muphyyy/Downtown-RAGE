using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace DowntownRP.Game
{
    public class PrivateMessages : Script
    {
        public async static Task UpdatePMStatus(int id, int mpStatus)
        {
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE characters SET mpStatus = @mpStatus WHERE id = @id";
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@mpStatus", mpStatus);

                command.ExecuteNonQuery();
            }
        }

        [RemoteEvent("ChangeStatusPM")]
        public async void RE_ChangeStatusPM(Client player, int status)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");

            if (status == 0)
            {
                await UpdatePMStatus(user.idpj, 0);
                Utilities.Notifications.SendNotificationOK(player, "Has desbloqueado tus mensajes privados");
                user.mpStatus = 0;
            }
            else
            {
                await UpdatePMStatus(user.idpj, 1);
                Utilities.Notifications.SendNotificationOK(player, "Has bloqueado tus mensajes privados");
                user.mpStatus = 1;
            }
        }
    }
}
