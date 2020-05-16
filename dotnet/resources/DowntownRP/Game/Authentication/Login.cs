using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;
using MySql.Data.MySqlClient;

namespace DowntownRP.Game.Authentication
{
    public class Login : Script
    {
        [ServerEvent(Event.PlayerConnected)]
        public void OnPlayerConnected(Player player)
        {
            Data.Info.playersConnected += 1;
            NAPI.ClientEvent.TriggerClientEventForAll("update_hud_players", Data.Info.playersConnected);
            player.TriggerEvent("ShowLoginWindow");
        }

        [RemoteEvent("RE_LoginPlayerAccount")]
        public async Task RE_LoginPlayerAccount(Player player, string username, string password)
        {
            if (await DatabaseFunctions.CheckIfPlayerRegistered(username) == true)
            {
                int playerid = await DatabaseFunctions.LoginPlayer(username, password);
                if (playerid != 0)
                {
                    player.TriggerEvent("DestroyWindow");

                    Data.Entities.User user = new Data.Entities.User
                    {
                        id = playerid
                    };

                    player.SetData("USER_CLASS", user);

                    //player.SetExternalData<Data.Entities.User>(0, user);
                    CharacterSelector.CharacterSelector.RetrieveCharactersList(player);
                    // await Character.DbFunctions.ShowCharacterList(player); <- NUEVO CHARACTER SELECTOR
                }
                else player.TriggerEvent("ShowErrorAlert", 1);
            }
            else player.TriggerEvent("ShowErrorAlert", 1);
        }

        [RemoteEvent("RE_RegisterPlayerAccount")]
        public async void RE_RegisterPlayerAccount(Player player, string username, string email, string password, string repassword)
        {
            if (await DatabaseFunctions.CheckIfPlayerRegistered(username) == false)
            {
                if (await DatabaseFunctions.CheckIfEmailRegistered(email) == false)
                {
                    if (password == repassword)
                    {
                        int playerId = await DatabaseFunctions.RegisterPlayer(username, password, email, player.SocialClubName, player.Address);

                        Data.Entities.User user = new Data.Entities.User
                        {
                            id = playerId
                        };

                        //player.SetExternalData<Data.Entities.User>(0, user);
                        player.SetData("USER_CLASS", user);

                        Console.WriteLine($"Registrado {playerId}");
                    }
                    else player.TriggerEvent("ShowErrorAlert", 4);
                }
                else player.TriggerEvent("ShowErrorAlert", 3);
            }
            else player.TriggerEvent("ShowErrorAlert", 2);
        }
    }
}
