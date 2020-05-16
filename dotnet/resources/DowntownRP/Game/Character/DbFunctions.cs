using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using GTANetworkAPI;
using System.Threading.Tasks;
using System.Data.Common;

namespace DowntownRP.Game.Character
{
    public class DbFunctions : Script
    {
        public async static Task ShowCharacterList(Player player)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData<Data.Entities.User>("USER_CLASS");
            int pjOrder = 0;

            player.Position = new Vector3(-900.4282, -432.3842, 89.2646); // Seteamos posición del selector de personajes
            player.Rotation = new Vector3(0, 0, 89.2646); // Seteamos rotación del selector de personajes

            player.TriggerEvent("OpenCharacterSelector"); // Llamamos al selector de personaje

            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM characters WHERE user_id = @id LIMIT 3";
                command.Parameters.AddWithValue("@id", user.id);

                DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

                if (reader.HasRows)
                {
                    await reader.ReadAsync().ConfigureAwait(false);
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("id"));
                        string name = reader.GetString(reader.GetOrdinal("name"));

                        player.TriggerEvent("ShowCharactersOnList", pjOrder, id, name); // Agregamos los personajes creados al selector
                        pjOrder++;
                    }
                }
            }
        }
    }
}
