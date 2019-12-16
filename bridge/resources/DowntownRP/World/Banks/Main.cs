using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GTANetworkAPI;

namespace DowntownRP.World.Banks
{
    public class Main : Script
    {
        [ServerEvent(Event.PlayerEnterColshape)]
        public void Banks_EnterColShape(ColShape shape, Client player)
        {
            //var banco = shape.GetExternalData<Data.Entities.Bank>(0);
            if (!shape.HasData("BANK_CLASS")) return;
            Data.Entities.Bank banco = shape.GetData("BANK_CLASS");
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");

            if (banco != null)
            {
                user.isInBank = true;
                user.bankEntity = banco;
            }
        }

        [ServerEvent(Event.PlayerExitColshape)]
        public void Banks_ExitColShape(ColShape shape, Client player)
        {
            //var banco = shape.GetExternalData<Data.Entities.Bank>(0);
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!shape.HasData("BANK_CLASS")) return;
            Data.Entities.Bank banco = shape.GetData("BANK_CLASS");

            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");

            if (banco != null)
            {
                user.isInBank = false;
                user.bankEntity = null;
            }
        }

        [RemoteEvent("ActionBank")]
        public void RE_ActionBank(Client player)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");
            if (user.chatStatus == false)
            {
                if (user.isInBank)
                {
                    if (!user.isBankCefOpen)
                    {
                        switch (user.bankEntity.type)
                        {
                            case 1:
                                if (user.bankAccount == 0) player.TriggerEvent("OpenCreateBank");
                                else player.TriggerEvent("OpenBank", false);

                                user.isBankCefOpen = true;
                                break;

                            case 2:
                                if (user.bankAccount == 0) Utilities.Notifications.SendNotificationERROR(player, "No tienes cuenta de banco");
                                else
                                {
                                    player.TriggerEvent("OpenBank", true);
                                    user.isBankCefOpen = true;
                                }
                                break;
                        }
                    }
                    else
                    {
                        player.TriggerEvent("DestroyCreateBank");
                        user.isBankCefOpen = false;
                        if (user.bankDelay) user.bankDelay = false;
                    }
                }
            }
        }

        [RemoteEvent("ActionBankClose")]
        public void RE_ActionBankClose(Client player)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");
            user.isBankCefOpen = false;

            if (user.bankDelay) user.bankDelay = false;
        }

        [RemoteEvent("BankAccountCreate")]
        public async Task RE_BankAccountCreate(Client player, string pin, string repin)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");
            if (pin == repin)
            {
                if (pin.ToString().Length <= 4)
                {
                    if (await Game.Money.MoneyModel.SubMoney(player, 100))
                    {
                        string iban = Utilities.Generate.CreateIBANBank();
                        if (!await DatabaseFunctions.CheckIfIBANIsTaken(iban))
                        {
                            await DatabaseFunctions.UpdateIBANBank(user.idpj, iban);
                            await DatabaseFunctions.UpdatePINBank(user.idpj, Convert.ToInt32(pin));

                            player.TriggerEvent("DestroyCreateBank");
                            user.isBankCefOpen = false;
                            user.IBAN = iban;
                            user.bankAccount = Convert.ToInt32(pin);
                            player.TriggerEvent("chat_goal", "¡Felicidades!", "Has abierto tu cuenta de banco con éxito");
                        }
                        else await BankAccountCreateSecure(player, pin);
                    }
                    else Utilities.Notifications.SendNotificationERROR(player, "No tienes suficiente dinero para abrir una cuenta bancaria");
                }
                else Utilities.Notifications.SendNotificationERROR(player, "El número PIN no puede tener mas de 4 dígitos");
            }
            else Utilities.Notifications.SendNotificationERROR(player, "El número PIN no coincide");
        }

        public static async Task BankAccountCreateSecure(Client player, string pin)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");

            string iban = Utilities.Generate.CreateIBANBank();
            if (!await DatabaseFunctions.CheckIfIBANIsTaken(iban))
            {
                await DatabaseFunctions.UpdateIBANBank(user.idpj, iban);
                await DatabaseFunctions.UpdatePINBank(user.idpj, Convert.ToInt32(pin));

                player.TriggerEvent("DestroyCreateBank");
                user.isBankCefOpen = false;
                user.IBAN = iban;
                user.bankAccount = Convert.ToInt32(pin);
                player.TriggerEvent("chat_goal", "¡Felicidades!", "Has abierto tu cuenta de banco con éxito");
            }
            else await BankAccountCreateSecure(player, pin);
        }

        [RemoteEvent("BankAccountLogin")]
        public void RE_BankAccountLogin(Client player, string pin)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");

            if (user.bankAccount.ToString() == pin) player.TriggerEvent("LoggedOpenBank", user.bank.ToString(), user.IBAN);
            else Utilities.Notifications.SendNotificationERROR(player, "El pin no es correcto");
        }

        [RemoteEvent("BankDepositMoney")]
        public async Task RE_BankDepositMoney(Client player, string money)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");

            double quantity = Convert.ToDouble(money);
            if (user.bankEntity.type == 2 && quantity >= 1001) Utilities.Notifications.SendNotificationERROR(player, "Solo puedes ingresar $1000 en un cajero");
            else
            {
                if (!user.bankDelay)
                {
                    if (await Game.Money.MoneyModel.SubMoney(player, quantity))
                    {
                        await MoneyFunctions.AddMoneyBank(player, quantity);
                        Utilities.Notifications.SendNotificationOK(player, $"Has depositado ${money} en tu cuenta bancaria");
                        player.TriggerEvent("UpdateMoneyBankATM", user.bank.ToString());

                        if (user.bankEntity.type == 2) user.bankDelay = true;
                    }
                    else Utilities.Notifications.SendNotificationERROR(player, $"No tienes ${money} disponibles");
                }
                else Utilities.Notifications.SendNotificationERROR(player, "Debes de esperar un poco para hacer otro movimiento");
            }
        }

        [RemoteEvent("BankRetirarMoney")]
        public async Task RE_BankRetirarMoney(Client player, string money)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");

            double quantity = Convert.ToDouble(money);
            if (user.bankEntity.type == 2 && quantity >= 1001) Utilities.Notifications.SendNotificationERROR(player, "Solo puedes retirar $1000 en un cajero");
            else
            {
                if (!user.bankDelay)
                {
                    if (await MoneyFunctions.RemoveMoneyBank(player, quantity))
                    {
                        await Game.Money.MoneyModel.AddMoney(player, quantity);
                        Utilities.Notifications.SendNotificationOK(player, $"Has retirado ${money} de tu cuenta bancaria");
                        player.TriggerEvent("UpdateMoneyBankATM", user.bank.ToString());

                        if (user.bankEntity.type == 2) user.bankDelay = true;
                    }
                    else Utilities.Notifications.SendNotificationERROR(player, $"No tienes ${money} disponibles en tu cuenta");
                }
                else Utilities.Notifications.SendNotificationERROR(player, "Debes de esperar un poco para hacer otro movimiento");
            }
        }

        [RemoteEvent("BankTransferirMoney")]
        public async Task RE_BankTransferirMoney(Client player, string iban, string money)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");

            double quantity = Convert.ToDouble(money);
            if (user.bankEntity.type == 2 && quantity >= 1001) Utilities.Notifications.SendNotificationERROR(player, "Solo puedes retirar $1000 en un cajero");
            else
            {
                if (!user.bankDelay)
                {
                    Console.WriteLine(iban);
                    string username = await DatabaseFunctions.GetUsernameByIBAN(iban);
                    if (username != "no")
                    {
                        if (await MoneyFunctions.RemoveMoneyBank(player, quantity))
                        {
                            bool isOnline = false;
                            foreach (var target in NAPI.Pools.GetAllPlayers())
                            {
                                if (target.Name == username)
                                {
                                    isOnline = true;
                                    await MoneyFunctions.AddMoneyBank(target, quantity);
                                    Utilities.Notifications.SendNotificationOK(target, $"Has recibido una transferencia de ${money} desde la cuenta {user.IBAN}");
                                    Utilities.Notifications.SendNotificationOK(player, $"Has enviado una transferencia de ${money} a la cuenta {iban}");
                                    player.TriggerEvent("UpdateMoneyBankATM", user.bank.ToString());
                                }
                            }

                            if (!isOnline)
                            {
                                int offbank = await DatabaseFunctions.GetBankMoneyOfflineByName(username);
                                await DatabaseFunctions.UpdateOfflineUserBankByName(username, offbank + Convert.ToInt32(money));

                                Utilities.Notifications.SendNotificationOK(player, $"Has enviado una transferencia de ${money} a la cuenta {iban}");
                                player.TriggerEvent("UpdateMoneyBankATM", user.bank.ToString());
                            }

                            if (user.bankEntity.type == 2) user.bankDelay = true;
                        }
                        else Utilities.Notifications.SendNotificationERROR(player, $"No tienes ${money} disponibles en tu cuenta");
                    }
                    else Utilities.Notifications.SendNotificationERROR(player, "No existe ninguna cuenta con ese número IBAN");
                }
                else Utilities.Notifications.SendNotificationERROR(player, "Debes de esperar un poco para hacer otro movimiento");
            }
        }
    }
}
