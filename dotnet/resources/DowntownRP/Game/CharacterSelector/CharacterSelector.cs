using GTANetworkAPI;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace DowntownRP.Game.CharacterSelector
{
    public class CharacterSelector : Script
    {
        [RemoteEvent("RetrieveCharactersList")]
        public static void RetrieveCharactersList(Player player)
        {
            //int? player_id = player.GetExternalData<Data.Entities.User>(0).id;
            //if (player_id == null) return;
            if (!player.HasData("USER_CLASS")) return;

            Data.Entities.User user = player.GetData<Data.Entities.User>("USER_CLASS");
            int player_id = user.id;

            uint player__id = Convert.ToUInt32(player_id);

            player.Dimension = player__id + 1;

            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM characters WHERE user_id = @id";
                command.Parameters.AddWithValue("@id", player_id);

                var items = new List<dynamic>();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var item = new Dictionary<string, object>(reader.FieldCount - 1);
                            for (var i = 0; i < reader.FieldCount; i++)
                            {
                                item[reader.GetName(i)] = reader.GetValue(i);
                            }
                            items.Add(item);
                        }
                    }
                }

                player.TriggerEvent("UpdateCharactersList", JsonConvert.SerializeObject(items));
                connection.Close();
            }
        }

        [RemoteEvent("SelectCharacter")]
        public async Task SelectCharacter(Player player, int characterId) //iniciador principal del personaje.
        {
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM characters WHERE id = @id LIMIT 1";
                command.Parameters.AddWithValue("@id", characterId);

                DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

                if (reader.HasRows)
                {
                    await reader.ReadAsync().ConfigureAwait(false);
                    double x = reader.GetDouble(reader.GetOrdinal("x"));
                    double y = reader.GetDouble(reader.GetOrdinal("y"));
                    double z = reader.GetDouble(reader.GetOrdinal("z"));
                    string name = reader.GetString(reader.GetOrdinal("name"));
                    string register_date = reader.GetString(reader.GetOrdinal("register_date"));
                    string last_login_date = reader.GetString(reader.GetOrdinal("last_login_date"));
                    double money = reader.GetDouble(reader.GetOrdinal("money"));
                    double bank = reader.GetDouble(reader.GetOrdinal("bank"));
                    int faction = reader.GetInt32(reader.GetOrdinal("faction"));
                    int rank = reader.GetInt32(reader.GetOrdinal("rank"));
                    int job = reader.GetInt32(reader.GetOrdinal("job"));
                    int level = reader.GetInt32(reader.GetOrdinal("level"));
                    int exp = reader.GetInt32(reader.GetOrdinal("exp"));
                    int health = reader.GetInt32(reader.GetOrdinal("health"));
                    int armour = reader.GetInt32(reader.GetOrdinal("armour"));
                    double rotation = reader.GetDouble(reader.GetOrdinal("rotation"));
                    string dni = reader.GetString(reader.GetOrdinal("dni"));
                    int age = reader.GetInt32(reader.GetOrdinal("age"));
                    string height = reader.GetString(reader.GetOrdinal("height"));
                    int gender = reader.GetInt32(reader.GetOrdinal("gender"));
                    int voiceMode = reader.GetInt32(reader.GetOrdinal("voiceMode"));
                    int mpStatus = reader.GetInt32(reader.GetOrdinal("mpStatus"));
                    int bankAccount = reader.GetInt32(reader.GetOrdinal("bankAccount"));
                    string IBAN = reader.GetString(reader.GetOrdinal("IBAN"));

                    // Character
                    int faceFirst = reader.GetInt32(reader.GetOrdinal("faceFirst"));
                    int faceSecond = reader.GetInt32(reader.GetOrdinal("faceSecond"));
                    int faceMix = reader.GetInt32(reader.GetOrdinal("faceMix"));
                    int skinFirst = reader.GetInt32(reader.GetOrdinal("skinFirst"));
                    int skinSecond = reader.GetInt32(reader.GetOrdinal("skinSecond"));
                    int skinMix = reader.GetInt32(reader.GetOrdinal("skinSecond"));
                    int eyeColor = reader.GetInt32(reader.GetOrdinal("eyeColor"));
                    int hairColor = reader.GetInt32(reader.GetOrdinal("hairColor"));
                    int hairHighlight = reader.GetInt32(reader.GetOrdinal("hairHighLight"));

                    // Character slots
                    int slot1 = reader.GetInt32(reader.GetOrdinal("slot1"));
                    int slot2 = reader.GetInt32(reader.GetOrdinal("slot2"));
                    int slot3 = reader.GetInt32(reader.GetOrdinal("slot3"));
                    int slot4 = reader.GetInt32(reader.GetOrdinal("slot4"));
                    int slot5 = reader.GetInt32(reader.GetOrdinal("slot5"));
                    int slot6 = reader.GetInt32(reader.GetOrdinal("slot6"));


                    player.Dimension = 0;

                    NAPI.Entity.SetEntityPosition(player, new Vector3(x, y, z));
                    NAPI.Entity.SetEntityRotation(player, new Vector3(0, 0, rotation));
                    NAPI.Player.SetPlayerHealth(player, health);
                    NAPI.Player.SetPlayerArmor(player, armour);
                    NAPI.Player.SetPlayerName(player, name);

                    //var user = player.GetExternalData<Data.Entities.User>(0);

                    if (!player.HasData("USER_CLASS")) return;
                    Data.Entities.User user = player.GetData<Data.Entities.User>("USER_CLASS");

                    user.idpj = characterId;
                    user.enableMicrophone = voiceMode;
                    user.mpStatus = mpStatus;
                    user.bankAccount = bankAccount;
                    user.IBAN = IBAN;
                    user.money = money;
                    user.bank = bank;
                    user.level = level;
                    user.exp = exp;
                    user.adminLv = 5;
                    user.job = job;
                    user.inventory = await Inventory.DatabaseFunctions.SpawnInventoryItems(characterId);

                    CheckIfUserHasCompany(user);
                    CheckIfUserWorksInCompany(user);

                    user.slot1 = await Inventory.DatabaseFunctions.SpawnCharacterItem(slot1);
                    user.slot2 = await Inventory.DatabaseFunctions.SpawnCharacterItem(slot2);
                    user.slot3 = await Inventory.DatabaseFunctions.SpawnCharacterItem(slot3);
                    user.slot4 = await Inventory.DatabaseFunctions.SpawnCharacterItem(slot4);
                    user.slot5 = await Inventory.DatabaseFunctions.SpawnCharacterItem(slot5);
                    user.slot6 = await Inventory.DatabaseFunctions.SpawnCharacterItem(slot6);

                    player.TriggerEvent("GetPlayerReadyToPlay");
                    player.TriggerEvent("showHUD");
                    player.TriggerEvent("UpdateMoneyHUD", money.ToString(), "set");
                    player.TriggerEvent("enableMicrophone", voiceMode);
                    player.TriggerEvent("update_hud_player", player.Value);
                    player.TriggerEvent("update_hud_players", Data.Info.playersConnected);
                    player.TriggerEvent("update_hud_microphone", 0);
                    player.TriggerEvent("update_hud_bank", bank.ToString());

                    player.SendChatMessage($"{Data.Info.serverName} || {Data.Info.serverVersion}");
                }

                connection.Close();
            }
        }

        [RemoteEvent("FinishCharacterCreation")]
        public async Task FinishCharacterCreation(Player player, string arguments)
        {
            //int? player_id = player.GetExternalData<Data.Entities.User>(0).id;
            //if (player_id == null) return;

            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData<Data.Entities.User>("USER_CLASS");
            int player_id = user.id;

            int character_id;

            var characterData = NAPI.Util.FromJson<CharacterData>(arguments);

            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = @"INSERT INTO characters(user_id, level, name, register_date, last_login_date, x, y, z, money,
                bank, gender, faceFirst, faceSecond, faceMix, skinFirst, skinSecond, skinMix, hairType, hairColor,
                hairHighlight, eyeColor, eyebrows, eyebrowsColor1, eyebrowsColor2, beard, beardColor, makeup, makeupColor,
                lipstick, lipstickColor, torso, topshirt, topshirtTexture, undershirt, legs, feet, accessory) 
                VALUES(@user_id, @level, @name, @register_date, @last_login_date, @x, @y, @z, @money, @bank, @gender, @faceFirst, @faceSecond,
                @faceMix, @skinFirst, @skinSecond, @skinMix, @hairType, @hairColor, @hairHighlight, @eyeColor, @eyebrows, @eyebrowsColor1,
                @eyebrowsColor2, @beard, @beardColor, @makeup, @makeupColor, @lipstick, @lipstickColor, @torso, @topshirt,
                @topshirtTexture, @undershirt, @legs, @feet, @accessory)";

                command.Parameters.AddWithValue("@user_id", player_id);
                command.Parameters.AddWithValue("@level", 1);
                command.Parameters.AddWithValue("@name", characterData.name);
                command.Parameters.AddWithValue("@register_date", DateTime.Now.ToString("dd/MM/yyyy"));
                command.Parameters.AddWithValue("@last_login_date", DateTime.Now.ToString("dd/MM/yyyy"));
                command.Parameters.AddWithValue("@x", 169.3792f);
                command.Parameters.AddWithValue("@y", -967.8402f);
                command.Parameters.AddWithValue("@z", 29.98808f);
                command.Parameters.AddWithValue("@money", 0);
                command.Parameters.AddWithValue("@bank", 0);
                command.Parameters.AddWithValue("@gender", characterData.gender);
                command.Parameters.AddWithValue("@faceFirst", characterData.faceFirst);
                command.Parameters.AddWithValue("@faceSecond", characterData.faceSecond);
                command.Parameters.AddWithValue("@faceMix", characterData.faceMix);
                command.Parameters.AddWithValue("@skinFirst", characterData.skinFirst);
                command.Parameters.AddWithValue("@skinSecond", characterData.skinSecond);
                command.Parameters.AddWithValue("@skinMix", characterData.skinMix);
                command.Parameters.AddWithValue("@hairType", characterData.hairType);
                command.Parameters.AddWithValue("@hairColor", characterData.hairColor);
                command.Parameters.AddWithValue("@hairHighlight", characterData.hairHighlight);
                command.Parameters.AddWithValue("@eyeColor", characterData.eyeColor);
                command.Parameters.AddWithValue("@eyebrows", characterData.eyebrows);
                command.Parameters.AddWithValue("@eyebrowsColor1", characterData.eyebrowsColor1);
                command.Parameters.AddWithValue("@eyebrowsColor2", characterData.eyebrowsColor2);
                command.Parameters.AddWithValue("@beard", characterData.beard);
                command.Parameters.AddWithValue("@beardColor", characterData.beardColor);
                command.Parameters.AddWithValue("@makeup", characterData.makeup);
                command.Parameters.AddWithValue("@makeupColor", characterData.makeupColor);
                command.Parameters.AddWithValue("@lipstick", characterData.lipstick);
                command.Parameters.AddWithValue("@lipstickColor", characterData.lipstickColor);
                command.Parameters.AddWithValue("@torso", characterData.torso);
                command.Parameters.AddWithValue("@topshirt", characterData.topshirt);
                command.Parameters.AddWithValue("@topshirtTexture", characterData.topshirtTexture);
                command.Parameters.AddWithValue("@undershirt", characterData.undershirt);
                command.Parameters.AddWithValue("@legs", characterData.legs);
                command.Parameters.AddWithValue("@feet", characterData.feet);
                command.Parameters.AddWithValue("@accessory", characterData.accessory);

                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                character_id = (int)command.LastInsertedId;
                connection.Close();
            }

            player.SendChatMessage($"{Data.Info.serverName} || {Data.Info.serverVersion}");
            player.Dimension = 0;
            player.Position = new Vector3(169.3792, -967.8402, 29.98808);

            //var user = player.GetExternalData<Data.Entities.User>(0);
            user.idpj = character_id;
            user.enableMicrophone = 0;
            user.mpStatus = 0;
            user.bankAccount = 0;
            user.IBAN = "0";
            user.money = 0;
            user.bank = 0;
            user.level = 1;
            user.exp = 0;
            user.inventory = new Data.Entities.Inventory();

            player.TriggerEvent("GetPlayerReadyToPlay");
            player.TriggerEvent("showHUD");
            player.TriggerEvent("UpdateMoneyHUD", "0", "set");
            player.TriggerEvent("enableMicrophone", 0);
            player.TriggerEvent("update_hud_player", player.Value);
            player.TriggerEvent("update_hud_players", Data.Info.playersConnected);
            player.TriggerEvent("update_hud_bank", "0");
            player.TriggerEvent("update_hud_microphone", 0);
        }

        [RemoteEvent("SetPlayerPos")]
        public void SetPlayerPos(Player player, object[] arguments)
        {
            if (arguments.Length < 3) return;
            NAPI.Entity.SetEntityPosition(player.Handle, new Vector3((float)arguments[0], (float)arguments[1], (float)arguments[2]));

            bool freeze = arguments.Length == 4 ? (bool)arguments[3] : false;
        }

        [RemoteEvent("SetPlayerRot")]
        public void SetPlayerRot(Player player, object[] arguments)
        {
            if (arguments.Length < 1) return;
            NAPI.Entity.SetEntityRotation(player.Handle, new Vector3(0f, 0f, (float)arguments[0]));
        }

        [RemoteEvent("SetPlayerSkin")]
        public void SetPlayerSkin(Player player, string pedName)
        {
            NAPI.Player.SetPlayerSkin(player, NAPI.Util.PedNameToModel(pedName));
        }

        [RemoteEvent("SetPlayerClothes")]
        public void SetPlayerClothes(Player player, int slot, int drawable, int texture)
        {
            NAPI.Player.SetPlayerClothes(player, slot, drawable, texture);
        }

        [RemoteEvent("SetPlayerAccessory")]
        public void SetPlayerAccessory(Player player, int slot, int drawable, int texture)
        {
            NAPI.Player.SetPlayerAccessory(player, slot, drawable, texture);
        }
        public static void CheckIfUserHasCompany(Data.Entities.User user)
        {
            Data.Entities.Company company = Data.Lists.Companies.Find(x => x.owner == user.idpj);
            if (company != null) user.companyProperty = company;
        }

        public static void CheckIfUserWorksInCompany(Data.Entities.User user)
        {
            Data.Entities.Company company = Data.Lists.Companies.Find(x => x.id == user.job);
            if (company != null) user.companyMember = company;
        }

        public async static Task UpdateUserCompany(int idpj, int company)
        {
            using (MySqlConnection connection = new MySqlConnection(Data.DatabaseHandler.connectionHandle))
            {
                await connection.OpenAsync().ConfigureAwait(false);

                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE characters SET job = @company WHERE id = @idpj";
                command.Parameters.AddWithValue("@idpj", idpj);
                command.Parameters.AddWithValue("@company", company);

                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }
    }
}

