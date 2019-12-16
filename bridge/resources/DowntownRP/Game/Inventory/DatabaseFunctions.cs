using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;
using DowntownRP.Data.Entities;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace DowntownRP.Game.Inventory
{
    public class DatabaseFunctions : Script
    {
        public async static Task<Data.Entities.Item> SpawnCharacterItem(int itemid)
        {
            Data.Entities.Item item = new Data.Entities.Item(0, "NO", 0, 0);

            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM items WHERE id = @itemid";
                command.Parameters.AddWithValue("@itemid", itemid);

                DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        item.id = reader.GetInt32(reader.GetOrdinal("id"));
                        item.name = reader.GetString(reader.GetOrdinal("name"));
                        item.quantity = reader.GetInt32(reader.GetOrdinal("quantity"));
                        item.type = reader.GetInt32(reader.GetOrdinal("type"));
                    }
                }
            }

            return item;
        }

        public async static Task<Data.Entities.Inventory> SpawnInventoryItems(int idpj)
        {
            Data.Entities.Inventory inventory = new Data.Entities.Inventory();

            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM items WHERE userid = @idpj";
                command.Parameters.AddWithValue("@idpj", idpj);

                DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("id"));
                        string name = reader.GetString(reader.GetOrdinal("name"));
                        int type = reader.GetInt32(reader.GetOrdinal("type"));
                        int quantity = reader.GetInt32(reader.GetOrdinal("quantity"));
                        int slot = reader.GetInt32(reader.GetOrdinal("slot"));

                        Data.Entities.Item item = new Data.Entities.Item(id, name, type, quantity);

                        switch (slot)
                        {
                            case 1:
                                inventory.slot1 = item;
                                break;

                            case 2:
                                inventory.slot2 = item;
                                break;

                            case 3:
                                inventory.slot3 = item;
                                break;

                            case 4:
                                inventory.slot4 = item;
                                break;

                            case 5:
                                inventory.slot5 = item;
                                break;

                            case 6:
                                inventory.slot6 = item;
                                break;

                            case 7:
                                inventory.slot7 = item;
                                break;

                            case 8:
                                inventory.slot8 = item;
                                break;

                            case 9:
                                inventory.slot9 = item;
                                break;

                            case 10:
                                inventory.slot10 = item;
                                break;

                            case 11:
                                inventory.slot11 = item;
                                break;

                            case 12:
                                inventory.slot12 = item;
                                break;
                        }
                    }
                }
            }

            return inventory;
        }

        public async static Task<int> CreateItemDatabase(int userid, string name, int type, int quantity, int slot)
        {
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();

                command.CommandText = "INSERT INTO items (userid, name, type, quantity, slot) VALUES (@userid, @name, @type, @quantity, @slot)";
                command.Parameters.AddWithValue("@userid", userid);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@type", type);
                command.Parameters.AddWithValue("@quantity", quantity);
                command.Parameters.AddWithValue("@slot", slot);

                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                return (int)command.LastInsertedId;
            }
        }

        public async static Task UpdateItemDatabase(int id, int userid, string name, int type, int quantity, int slot)
        {
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE items SET userid = @userid, name = @name, type = @type, quantity = @quantity, slot = @slot WHERE id = @id";
                command.Parameters.AddWithValue("@userid", userid);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@type", type);
                command.Parameters.AddWithValue("@quantity", quantity);
                command.Parameters.AddWithValue("@slot", slot);
                command.Parameters.AddWithValue("@id", id);

                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }

        public async static Task<bool> SetNewItemInventory(Client player, Item item)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return false;
            Data.Entities.User user = player.GetData("USER_CLASS");

            if (user.inventory.slot1.name == "NO")
            {
                user.inventory.slot1 = item;
                int itemid = await CreateItemDatabase(user.idpj, item.name, item.type, item.quantity, 1);
                user.inventory.slot1.id = itemid;
                return true;
            }

            if (user.inventory.slot2.name == "NO")
            {
                user.inventory.slot2 = item;
                int itemid = await CreateItemDatabase(user.idpj, item.name, item.type, item.quantity, 2);
                user.inventory.slot2.id = itemid;
                return true;
            }

            if (user.inventory.slot3.name == "NO")
            {
                user.inventory.slot3 = item;
                int itemid = await CreateItemDatabase(user.idpj, item.name, item.type, item.quantity, 3);
                user.inventory.slot3.id = itemid;
                return true;
            }

            if (user.inventory.slot4.name == "NO")
            {
                user.inventory.slot4 = item;
                int itemid = await CreateItemDatabase(user.idpj, item.name, item.type, item.quantity, 4);
                user.inventory.slot4.id = itemid;
                return true;
            }

            if (user.inventory.slot5.name == "NO")
            {
                user.inventory.slot5 = item;
                int itemid = await CreateItemDatabase(user.idpj, item.name, item.type, item.quantity, 5);
                user.inventory.slot5.id = itemid;
                return true;
            }

            if (user.inventory.slot6.name == "NO")
            {
                user.inventory.slot6 = item;
                int itemid = await CreateItemDatabase(user.idpj, item.name, item.type, item.quantity, 6);
                user.inventory.slot6.id = itemid;
                return true;
            }

            if (user.inventory.slot7.name == "NO")
            {
                user.inventory.slot7 = item;
                int itemid = await CreateItemDatabase(user.idpj, item.name, item.type, item.quantity, 7);
                user.inventory.slot7.id = itemid;
                return true;
            }

            if (user.inventory.slot8.name == "NO")
            {
                user.inventory.slot8 = item;
                int itemid = await CreateItemDatabase(user.idpj, item.name, item.type, item.quantity, 8);
                user.inventory.slot8.id = itemid;
                return true;
            }

            if (user.inventory.slot9.name == "NO")
            {
                user.inventory.slot9 = item;
                int itemid = await CreateItemDatabase(user.idpj, item.name, item.type, item.quantity, 9);
                user.inventory.slot9.id = itemid;
                return true;
            }

            if (user.inventory.slot10.name == "NO")
            {
                user.inventory.slot10 = item;
                int itemid = await CreateItemDatabase(user.idpj, item.name, item.type, item.quantity, 10);
                user.inventory.slot10.id = itemid;
                return true;
            }

            if (user.inventory.slot11.name == "NO")
            {
                user.inventory.slot11 = item;
                int itemid = await CreateItemDatabase(user.idpj, item.name, item.type, item.quantity, 11);
                user.inventory.slot11.id = itemid;
                return true;
            }

            if (user.inventory.slot12.name == "NO")
            {
                user.inventory.slot12 = item;
                int itemid = await CreateItemDatabase(user.idpj, item.name, item.type, item.quantity, 12);
                user.inventory.slot12.id = itemid;
                return true;
            }

            return false; // Slots llenos
        }

        public async static Task<bool> SetItemInventory(Client player, Item item)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return false;
            Data.Entities.User user = player.GetData("USER_CLASS");

            if (user.inventory.slot1.name == "NO")
            {
                user.inventory.slot1 = item;
                await UpdateItemDatabase(item.id, user.idpj, item.name, item.type, item.quantity, 1);
                return true;
            }

            if (user.inventory.slot2.name == "NO")
            {
                user.inventory.slot2 = item;
                await UpdateItemDatabase(item.id, user.idpj, item.name, item.type, item.quantity, 2);
                return true;
            }

            if (user.inventory.slot3.name == "NO")
            {
                user.inventory.slot3 = item;
                await UpdateItemDatabase(item.id, user.idpj, item.name, item.type, item.quantity, 3);
                return true;
            }

            if (user.inventory.slot4.name == "NO")
            {
                user.inventory.slot4 = item;
                await UpdateItemDatabase(item.id, user.idpj, item.name, item.type, item.quantity, 4);
                return true;
            }

            if (user.inventory.slot5.name == "NO")
            {
                user.inventory.slot5 = item;
                await UpdateItemDatabase(item.id, user.idpj, item.name, item.type, item.quantity, 5);
                return true;
            }

            if (user.inventory.slot6.name == "NO")
            {
                user.inventory.slot6 = item;
                await UpdateItemDatabase(item.id, user.idpj, item.name, item.type, item.quantity, 6);
                return true;
            }

            if (user.inventory.slot7.name == "NO")
            {
                user.inventory.slot7 = item;
                await UpdateItemDatabase(item.id, user.idpj, item.name, item.type, item.quantity, 7);
                return true;
            }

            if (user.inventory.slot8.name == "NO")
            {
                user.inventory.slot8 = item;
                await UpdateItemDatabase(item.id, user.idpj, item.name, item.type, item.quantity, 8);
                return true;
            }

            if (user.inventory.slot9.name == "NO")
            {
                user.inventory.slot9 = item;
                await UpdateItemDatabase(item.id, user.idpj, item.name, item.type, item.quantity, 9);
                return true;
            }

            if (user.inventory.slot10.name == "NO")
            {
                user.inventory.slot10 = item;
                await UpdateItemDatabase(item.id, user.idpj, item.name, item.type, item.quantity, 10);
                return true;
            }

            if (user.inventory.slot11.name == "NO")
            {
                user.inventory.slot11 = item;
                await UpdateItemDatabase(item.id, user.idpj, item.name, item.type, item.quantity, 11);
                return true;
            }

            if (user.inventory.slot12.name == "NO")
            {
                user.inventory.slot12 = item;
                await UpdateItemDatabase(item.id, user.idpj, item.name, item.type, item.quantity, 12);
                return true;
            }

            return false;
        }

    }
}