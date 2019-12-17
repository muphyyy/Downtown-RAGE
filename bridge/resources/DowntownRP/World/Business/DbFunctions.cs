using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace DowntownRP.World.Business
{
    public class DbFunctions : Script
    {
        public static async Task SpawnBusiness()
        {
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM business";

                DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("id"));
                        int userid = reader.GetInt32(reader.GetOrdinal("owner"));
                        int type = reader.GetInt32(reader.GetOrdinal("type"));
                        string name = reader.GetString(reader.GetOrdinal("name"));
                        int price = reader.GetInt32(reader.GetOrdinal("price"));
                        double safeBox = reader.GetDouble(reader.GetOrdinal("safeBox"));
                        string area = reader.GetString(reader.GetOrdinal("area"));
                        int number = reader.GetInt32(reader.GetOrdinal("number"));

                        double x = reader.GetDouble(reader.GetOrdinal("x"));
                        double y = reader.GetDouble(reader.GetOrdinal("y"));
                        double z = reader.GetDouble(reader.GetOrdinal("z"));

                        Vector3 position = new Vector3(x, y, z);
                        string nombre;

                        if (name == "NO") nombre = "Negocio en venta";
                        else nombre = name;

                        NAPI.Task.Run(() =>
                        {
                            ColShape shape = NAPI.ColShape.CreateCylinderColShape(position.Subtract(new Vector3(0, 0, 1)), 2, 2);
                            TextLabel label = NAPI.TextLabel.CreateTextLabel($"{nombre}~n~Pulsa ~y~F5 ~w~para interactuar~n~{area}, {number}", position, 1, 1, 0, new Color(255, 255, 255));
                            Marker marker = NAPI.Marker.CreateMarker(0, position.Subtract(new Vector3(0, 0, 0.1)), new Vector3(), new Vector3(), 1, new Color(251, 244, 1));
                            Blip blip = NAPI.Blip.CreateBlip(position);
                            blip.Color = 3;
                            blip.Name = nombre;

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
                                    break;

                                case 7:
                                    blip.Sprite = 135;
                                    break;

                                case 8:
                                    blip.Sprite = 135;
                                    break;

                                case 9:
                                    blip.Sprite = 135;
                                    break;
                            }

                            Data.Entities.Business business = new Data.Entities.Business
                            {
                                id = id,
                                owner = userid,
                                type = type,
                                name = name,
                                price = price,
                                blip = blip,
                                label = label,
                                marker = marker,
                                area = area,
                                number = number,
                                safeBox = safeBox,
                                shape = shape
                            };

                            //shape.SetExternalData<Data.Entities.Business>(0, business);
                            shape.SetData("BUSINESS_CLASS", business);

                            Data.Info.businessSpanwed++;
                        });
                    }
                }


            }
        }

        public async static Task<int> CreateBusiness(Client player, int type, int price, string area, int number)
        {
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO business (name, type, price, x, y, z, area, number) VALUES (@name, @type, @price, @x, @y, @z, @area, @number)";
                command.Parameters.AddWithValue("@name", "NO");
                command.Parameters.AddWithValue("@type", type);
                command.Parameters.AddWithValue("@price", price);
                command.Parameters.AddWithValue("@x", player.Position.X);
                command.Parameters.AddWithValue("@y", player.Position.Y);
                command.Parameters.AddWithValue("@z", player.Position.Z);
                command.Parameters.AddWithValue("@area", area);
                command.Parameters.AddWithValue("@number", number);

                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                return (int)command.LastInsertedId;
            }
        }

        public async static Task DeleteBusiness(int id)
        {

            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "DELETE FROM business WHERE id = @id";
                command.Parameters.AddWithValue("@id", id);

                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        public async static Task<int> GetLastStreetNumber(string streetname)
        {
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM business WHERE area = @streetname ORDER BY number DESC LIMIT 1";
                command.Parameters.AddWithValue("@streetname", streetname);

                DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("number"));
                        connection.Close();
                        return id;
                    }

                }
            }
            return 0;
        }

        public async static Task UpdateBusinessOwner(int id, int owner)
        {
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE business SET owner = @owner WHERE id = @id";
                command.Parameters.AddWithValue("@owner", owner);
                command.Parameters.AddWithValue("@id", id);

                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }
    }

}
