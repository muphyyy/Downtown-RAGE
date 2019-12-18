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

                        NAPI.Task.Run(async () =>
                        {
                            ColShape shape = NAPI.ColShape.CreateCylinderColShape(position.Subtract(new Vector3(0, 0, 1)), 2, 2);
                            TextLabel label = NAPI.TextLabel.CreateTextLabel($"{nombre}~n~Pulsa ~y~F5 ~w~para interactuar~n~~p~{area}, {number}", position, 3, 1, 0, new Color(255, 255, 255));
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
                                    label.Text = $"{nombre}~n~~p~{area}, {number}";
                                    break;

                                case 7:
                                    blip.Sprite = 135;
                                    label.Text = $"{nombre}~n~~p~{area}, {number}";
                                    break;

                                case 8:
                                    blip.Sprite = 135;
                                    label.Text = $"{nombre}~n~~p~{area}, {number}";
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

                            await SpawnVehicleBusiness(business);
                            await GetBusinessVehicleSpawn(business);
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

        public static async Task SpawnVehicleBusiness(Data.Entities.Business business)
        {
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM vehicles_business WHERE business = @business";
                command.Parameters.AddWithValue("@business", business.id);

                DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("id"));
                        string type = reader.GetString(reader.GetOrdinal("type"));
                        int price = reader.GetInt32(reader.GetOrdinal("price"));
                        int color1 = reader.GetInt32(reader.GetOrdinal("color1"));
                        int color2 = reader.GetInt32(reader.GetOrdinal("color2"));
                        string numberplate = reader.GetString(reader.GetOrdinal("numberplate"));

                        double x = reader.GetDouble(reader.GetOrdinal("x"));
                        double y = reader.GetDouble(reader.GetOrdinal("y"));
                        double z = reader.GetDouble(reader.GetOrdinal("z"));
                        double rot = reader.GetDouble(reader.GetOrdinal("rot"));

                        bool isCompanySelling = false;
                        bool isRentSelling = false;
                        bool isNormalSelling = false;

                        switch (business.type)
                        {
                            case 6:
                                isRentSelling = true;
                                break;

                            case 7:
                                isNormalSelling = true;
                                break;

                            case 8:
                                isCompanySelling = true;
                                break;
                        }

                        Vector3 position = new Vector3(x, y, z);

                        NAPI.Task.Run(() =>
                        {
                            uint hash = NAPI.Util.GetHashKey(type);
                            Vehicle vehicle = NAPI.Vehicle.CreateVehicle(hash, position, (float)rot, color1, color2, numberplate, 255, false, false);
                            TextLabel label = NAPI.TextLabel.CreateTextLabel($"~y~{type}~n~~w~Precio: ~g~${price}", position, 8, 1, 0, new Color(255, 255, 255));
                            vehicle.NumberPlate = numberplate;

                            Data.Entities.VehicleBusiness veh = new Data.Entities.VehicleBusiness()
                            {
                                id = id,
                                model = type,
                                vehicle = vehicle,
                                business = business,
                                price = price,
                                isRentSelling = isRentSelling,
                                isNormalSelling = isNormalSelling,
                                isCompanySelling = isCompanySelling,
                                label = label

                            };

                            vehicle.SetData("VEHICLE_BUSINESS_DATA", veh);

                            vehicle.SetSharedData("BUSINESS_VEHICLE_SHARED", vehicle);
                        });
                    }
                }


            }
        }

        public async static Task<int> CreateBusinessVehicle(int id_business, string model, int price, int color1, int color2, string numberplate, double x, double y, double z, double rot)
        {
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO vehicles_business (business, type, price, color1, color2, numberplate, x, y, z, rot) VALUES (@id_business, @model, @price, @color1, @color2, @numberplate, @x, @y, @z, @rot)";
                command.Parameters.AddWithValue("@id_business", id_business);
                command.Parameters.AddWithValue("@model", model);
                command.Parameters.AddWithValue("@price", price);
                command.Parameters.AddWithValue("@color1", color1);
                command.Parameters.AddWithValue("@color2", color2);
                command.Parameters.AddWithValue("@numberplate", numberplate);
                command.Parameters.AddWithValue("@x", x);
                command.Parameters.AddWithValue("@y", y);
                command.Parameters.AddWithValue("@z", z);
                command.Parameters.AddWithValue("@rot", rot);


                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                return (int)command.LastInsertedId;
            }
        }

        public async static Task<int> GetBusinessTypeById(int businessid)
        {
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM business WHERE id = @id";
                command.Parameters.AddWithValue("@id", businessid);

                DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("id"));
                        connection.Close();
                        return id;
                    }

                }
            }
            return 0;
        }

        public async static Task DeleteBusinessVehicle(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "DELETE FROM vehicles_business WHERE id = @id";
                command.Parameters.AddWithValue("@id", id);

                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        public async static Task GetBusinessVehicleSpawn(Data.Entities.Business business)
        {
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM business_vehicle_spawn WHERE business = @id";
                command.Parameters.AddWithValue("@id", business.id);

                DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        business.spawn = new Vector3(reader.GetDouble(reader.GetOrdinal("x")), reader.GetDouble(reader.GetOrdinal("y")), reader.GetDouble(reader.GetOrdinal("z")));
                        business.spawnRot = (float)reader.GetDouble(reader.GetOrdinal("rot"));
                    }
                }
            }
        }

        public async static Task CreateBusinessVehicleSpawn(int business, double x, double y, double z, double rot)
        {
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO business_vehicle_spawn (business, x, y, z, rot) VALUES (@business, @x, @y, @z, @rot)";
                command.Parameters.AddWithValue("@business", business);
                command.Parameters.AddWithValue("@x", x);
                command.Parameters.AddWithValue("@y", y);
                command.Parameters.AddWithValue("@z", z);
                command.Parameters.AddWithValue("@rot", rot);

                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }
    }

}
