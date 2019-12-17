using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace DowntownRP.World.Companies
{
    public class DbFunctions : Script
    {
        public async static Task SpawnCompanies()
        {
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM companies";

                DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("id"));
                        int userid = reader.GetInt32(reader.GetOrdinal("owner"));
                        int type = reader.GetInt32(reader.GetOrdinal("type"));
                        string name = reader.GetString(reader.GetOrdinal("name"));
                        int price = reader.GetInt32(reader.GetOrdinal("price"));
                        string area = reader.GetString(reader.GetOrdinal("area"));
                        int number = reader.GetInt32(reader.GetOrdinal("number"));
                        double safeBox = reader.GetDouble(reader.GetOrdinal("safeBox"));
                        int percentage = reader.GetInt32(reader.GetOrdinal("percentage"));
                        int subsidy = reader.GetInt32(reader.GetOrdinal("subsidy"));

                        double x = reader.GetDouble(reader.GetOrdinal("x"));
                        double y = reader.GetDouble(reader.GetOrdinal("y"));
                        double z = reader.GetDouble(reader.GetOrdinal("z"));

                        Vector3 position = new Vector3(x, y, z);
                        string nombre;

                        if (name == "NO") nombre = "Compañía en venta";
                        else nombre = name;

                        int workers = GetWorkersCompany(id);

                        NAPI.Task.Run(() =>
                        {
                            // Exterior entities

                            ColShape company = NAPI.ColShape.CreateCylinderColShape(position.Subtract(new Vector3(0, 0, 1)), 2, 2);
                            TextLabel label = NAPI.TextLabel.CreateTextLabel($"{nombre}~n~Pulsa ~y~F5 ~w~para interactuar~n~~p~{area}, {number}", position, 5, 1, 0, new Color(255, 255, 255));
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
                            }

                            // Interior entities
                            TextLabel label_interior = NAPI.TextLabel.CreateTextLabel("~w~Pulsa ~y~F5 ~w~para salir", Interior.interior, 3, 1, 0, new Color(255, 255, 255));
                            label_interior.Dimension = (uint)id;
                            ColShape shape_interior = NAPI.ColShape.CreateCylinderColShape(Interior.interior, 2, 2, (uint)id);


                            Data.Entities.Company dcompany = new Data.Entities.Company
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
                                workers = workers,
                                percentage = percentage,
                                subsidy = subsidy,
                                interior = shape_interior,
                                shape = company
                            };

                            //company.SetExternalData<Data.Entities.Company>(0, dcompany);
                            //shape_interior.SetExternalData<Data.Entities.Company>(0, dcompany);

                            company.SetData("COMPANY_CLASS", dcompany);
                            shape_interior.SetData("COMPANY_CLASS", dcompany);
                        });
                        

                        Data.Info.companiesSpawned++;
                    }
                }
            }
        }

        public async static Task<int> CreateCompany(Client player, int type, int price, string area, int number)
        {
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "INSERT INTO companies (name, type, price, x, y, z, area, number) VALUES (@name, @type, @price, @x, @y, @z, @area, @number)";
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

        public async static Task DeleteCompany(int id)
        {

            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "DELETE FROM companies WHERE id = @id";
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
                command.CommandText = "SELECT * FROM companies WHERE area = @streetname ORDER BY number DESC LIMIT 1";
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

        public async static Task UpdateCompanyOwner(int id, int owner)
        {
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE companies SET owner = @owner WHERE id = @id";
                command.Parameters.AddWithValue("@owner", owner);
                command.Parameters.AddWithValue("@id", id);

                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        public static int GetWorkersCompany(int idcompany)
        {
            int workers = 0;
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM characters WHERE job = @idcompany";
                command.Parameters.AddWithValue("@idcompany", idcompany);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            workers++;
                        }

                    }
                }
            }
            return workers;
        }

        public async static Task UpdateCompanyName(int id, string name)
        {
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE companies SET name = @name WHERE id = @id";
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@id", id);

                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        public async static Task UpdateCompanyPercentage(int id, int percentage)
        {
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE companies SET percentage = @percentage WHERE id = @id";
                command.Parameters.AddWithValue("@percentage", percentage);
                command.Parameters.AddWithValue("@id", id);

                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        public async static Task UpdateCompanySubsidy(int id, int subsidy)
        {
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE companies SET subsidy = @subsidy WHERE id = @id";
                command.Parameters.AddWithValue("@subsidy", subsidy);
                command.Parameters.AddWithValue("@id", id);

                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        public async static Task UpdateCompanySafebox(int id, int safeBox)
        {
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE companies SET safeBox = @safeBox WHERE id = @id";
                command.Parameters.AddWithValue("@safeBox", safeBox);
                command.Parameters.AddWithValue("@id", id);

                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

    }
}
