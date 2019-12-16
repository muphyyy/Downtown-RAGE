using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DowntownRP.Game.Voice
{
    public class DatabaseFunctions
    {
        public async static Task UpdateVoiceMode(int id, int voiceMode)
        {
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE characters SET voiceMode = @voiceMode WHERE id = @id";
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@voiceMode", voiceMode);

                command.ExecuteNonQuery();
            }
        }
    }
}
