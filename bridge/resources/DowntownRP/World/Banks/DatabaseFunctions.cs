using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using GTANetworkAPI;
using System.Threading.Tasks;
using System.Data.Common;

namespace DowntownRP.World.Banks
{
    public class DatabaseFunctions : Script
    {
        public async static Task SpawnBank()
        {
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM banks";

                DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("id"));
                        int type = reader.GetInt32(reader.GetOrdinal("type"));
                        double x = reader.GetDouble(reader.GetOrdinal("x"));
                        double y = reader.GetDouble(reader.GetOrdinal("y"));
                        double z = reader.GetDouble(reader.GetOrdinal("z"));

                        string a_id = Convert.ToString(id);

                        NAPI.Task.Run(() =>
                        {
                            Vector3 position = new Vector3(x, y, z);
                            ColShape bank = NAPI.ColShape.CreateCylinderColShape(position.Subtract(new Vector3(0, 0, 1)), 2, 6);
                            TextLabel label = NAPI.TextLabel.CreateTextLabel("Pulsa ~y~F5 ~w~para interactuar", position.Subtract(new Vector3(0, 0, 0.1)), 15, 6, 2, new Color(255, 255, 255));
                            Marker marker = NAPI.Marker.CreateMarker(0, position.Subtract(new Vector3(0, 0, 0.1)), new Vector3(), new Vector3(), 1, new Color(251, 244, 1));
                            Blip blip;

                            if (type == 1)
                            {
                                blip = NAPI.Blip.CreateBlip(108, position, 1, 5, "Banco");
                                //blip.ShortRange = true;
                            }
                            else
                            {
                                blip = NAPI.Blip.CreateBlip(277, position, 1, 5, "ATM");
                                //blip.ShortRange = true;
                            }

                            Data.Entities.Bank banco = new Data.Entities.Bank
                            {
                                blip = blip,
                                marker = marker,
                                label = label,
                                id = id,
                                type = type
                            };

                            //bank.SetExternalData<Data.Entities.Bank>(0, banco);
                            bank.SetData("BANK_CLASS", banco);
                        });
                        Data.Info.banksSpawned++;
                    }
                }
            }
        }

        public static async Task<int> CreateBank(Client player, int type)
        {
            double x = player.Position.X;
            double y = player.Position.Y;
            double z = player.Position.Z;

            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO banks (type, x, y, z) VALUES (@type, @x, @y, @z)";
                command.Parameters.AddWithValue("@type", type);
                command.Parameters.AddWithValue("@x", x);
                command.Parameters.AddWithValue("@y", y);
                command.Parameters.AddWithValue("@z", z);

                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                return (int)command.LastInsertedId;
            }
        }

        public static async Task DeleteBank(int id)
        {

            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "DELETE FROM banks WHERE id = @id";
                command.Parameters.AddWithValue("@id", id);

                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        public async static Task UpdateMoneyBankDb(Client player, double bank)
        {
            //int id = player.GetExternalData<Data.Entities.User>(0).idpj;
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");

            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE characters SET bank = @bank WHERE id = @id";
                command.Parameters.AddWithValue("@id", user.idpj);
                command.Parameters.AddWithValue("@bank", bank);

                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        public async static Task UpdatePINBank(int id, int pin)
        {
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE characters SET bankAccount = @pin WHERE id = @id";
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@pin", pin);

                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        public async static Task UpdateIBANBank(int id, string iban)
        {
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE characters SET IBAN = @iban WHERE id = @id";
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@iban", iban);

                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        public static async Task<bool> CheckIfIBANIsTaken(string iban)
        {
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM characters WHERE IBAN = @iban";
                command.Parameters.AddWithValue("@iban", iban);
                DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
                return reader.HasRows;
            }
        }

        public static async Task<string> GetUsernameByIBAN(string iban)
        {
            string name = "no";
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM characters WHERE IBAN = @iban";
                command.Parameters.AddWithValue("@iban", iban);

                DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

                if (reader.HasRows)
                {
                    await reader.ReadAsync().ConfigureAwait(false);
                    {
                        name = reader.GetString(reader.GetOrdinal("name"));
                    }
                }
                return name;
            }
        }

        public static async Task<int> GetBankMoneyOfflineByName(string name)
        {
            int money = 0;
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM characters WHERE name = @name";
                command.Parameters.AddWithValue("@name", name);

                DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

                if (reader.HasRows)
                {
                    await reader.ReadAsync().ConfigureAwait(false);
                    {
                        money = reader.GetInt32(reader.GetOrdinal("bank"));
                    }
                }
                return money;
            }
        }

        public async static Task UpdateOfflineUserBankByName(string name, int quantity)
        {
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE characters SET bank = @quantity WHERE name = @name";
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@quantity", quantity);

                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

    }
}
