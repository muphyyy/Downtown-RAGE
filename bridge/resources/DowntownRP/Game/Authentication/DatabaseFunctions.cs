using System;
using System.Data.Common;
using System.Threading.Tasks;
using BCrypt;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace DowntownRP.Game.Authentication
{
    public class DatabaseFunctions
    {
        public static async Task<string> GetSaltPlayer(string username)
        {
            string salt = "";
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM users WHERE name = @username";
                command.Parameters.AddWithValue("@username", username);

                DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

                if (reader.HasRows)
                {
                    await reader.ReadAsync().ConfigureAwait(false);
                    {
                        salt = reader.GetString(reader.GetOrdinal("salt"));
                    }
                }
                return salt;
            }
        }

        public async static Task<int> LoginPlayer(string username, string password)
        {
            int lastid = 0;
            string saltuser = await GetSaltPlayer(username);
            string penc = BCryptHelper.HashPassword(password, saltuser);

            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM users WHERE name = @username AND password = @password";
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", penc);

                DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

                if (reader.HasRows)
                {
                    await reader.ReadAsync().ConfigureAwait(false);
                    {
                        lastid = reader.GetInt32(reader.GetOrdinal("id"));
                    }
                }
                return lastid;
            }
        }

        public async static Task<bool> CheckIfPlayerRegistered(string username)
        {
            bool checkName = false;
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM users WHERE name = @playerName";
                command.Parameters.AddWithValue("@playerName", username);

                DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
                checkName = reader.HasRows;
                return checkName;
            }
        }

        public async static Task<bool> CheckIfEmailRegistered(string email)
        {
            bool checkName = false;
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM users WHERE email = @email";
                command.Parameters.AddWithValue("@email", email);

                DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
                checkName = reader.HasRows;
                return checkName;
            }
        }

        public async static Task<int> RegisterPlayer(string username, string password, string email, string socialName, string ip)
        {
            if (await CheckIfPlayerRegistered(username)) return 0;
            else
            {
                using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
                {
                    await connection.OpenAsync().ConfigureAwait(false);
                    MySqlCommand command = connection.CreateCommand();

                    string pSalt = BCryptHelper.GenerateSalt();
                    string pEncrypt = BCryptHelper.HashPassword(password, pSalt);

                    command.CommandText = "INSERT INTO users (name, password, salt, email, socialName, ip) VALUES (@username, @password, @salt, @email, @socialName, @ip)";
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", pEncrypt);
                    command.Parameters.AddWithValue("@salt", pSalt);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@socialName", socialName);
                    command.Parameters.AddWithValue("@ip", ip);

                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                    return (int)command.LastInsertedId;
                }
            }
        }

    }
}
