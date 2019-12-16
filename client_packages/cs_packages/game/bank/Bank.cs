using System;
using System.Collections.Generic;
using System.Text;
using RAGE;

namespace DowntownRP_cs.game.bank
{
    public class Bank : Events.Script
    {
        private static RAGE.Ui.HtmlWindow browserCreateBank;
        private static bool isATM;
        private static string sueldo;
        private static string IBAN;
        public Bank()
        {
            Events.Add("OpenChangePin", OpenChangePin);
            Events.Add("TransferirBank", TransferirBank);
            Events.Add("OpenTransfer", OpenTransfer);
            Events.Add("RetirarBank", RetirarBank);
            Events.Add("OpenRetirar", OpenRetirar);
            Events.Add("UpdateMoneyBankATM", UpdateMoneyBankATM);
            Events.Add("DepositBank", DepositBank);
            Events.Add("GoBackMainBanco", GoBackMainBanco);
            Events.Add("OpenDepositar", OpenDepositar);
            Events.Add("OpenBank", OpenBank);
            Events.Add("LoggedOpenBank", LoggedOpenBank);
            Events.Add("OpenCreateBank", OpenCreateBank);
            Events.Add("OpenCreateBankPin", OpenCreateBankPin);
            Events.Add("DestroyCreateBank", DestroyCreateBank);
            Events.Add("DestroyCreateBankClick", DestroyCreateBankClick);
            Events.Add("CreateBankAccount", CreateBankAccount);
            Events.Add("LoginBankAccount", LoginBankAccount);
        }

        private void OpenChangePin(object[] args)
        {
            Events.CallRemote("throwNotImplemented");
        }

        private void TransferirBank(object[] args)
        {
            Events.CallRemote("BankTransferirMoney", args[0].ToString(), args[1].ToString());
        }

        private void OpenTransfer(object[] args)
        {
            browserCreateBank.Destroy();

            browserCreateBank = new RAGE.Ui.HtmlWindow("package://statics/bank/transferir.html");
            browserCreateBank.ExecuteJs($"setSueldoActual('{sueldo}')");
            browserCreateBank.ExecuteJs($"setIBAN('{IBAN}')");

            if (isATM) browserCreateBank.ExecuteJs($"isATM()");
        }

        private void RetirarBank(object[] args)
        {
            Events.CallRemote("BankRetirarMoney", args[0].ToString());
        }

        private void OpenRetirar(object[] args)
        {
            browserCreateBank.Destroy();

            browserCreateBank = new RAGE.Ui.HtmlWindow("package://statics/bank/retirar.html");
            browserCreateBank.ExecuteJs($"setSueldoActual('{sueldo}')");
            browserCreateBank.ExecuteJs($"setIBAN('{IBAN}')");

            if (isATM) browserCreateBank.ExecuteJs($"isATM()");
        }

        private void UpdateMoneyBankATM(object[] args)
        {
            sueldo = args[0].ToString();
            browserCreateBank.ExecuteJs($"setSueldoActual('{sueldo}')");
        }

        private void DepositBank(object[] args)
        {
            Events.CallRemote("BankDepositMoney", args[0].ToString());
        }

        private void GoBackMainBanco(object[] args)
        {
            browserCreateBank.Destroy();
            browserCreateBank = new RAGE.Ui.HtmlWindow("package://statics/bank/main.html");
            browserCreateBank.ExecuteJs($"setSueldoActual('{sueldo}')");
            browserCreateBank.ExecuteJs($"setIBAN('{IBAN}')");
        }

        private void OpenDepositar(object[] args)
        {
            browserCreateBank.Destroy();

            browserCreateBank = new RAGE.Ui.HtmlWindow("package://statics/bank/ingresar.html");
            browserCreateBank.ExecuteJs($"setSueldoActual('{sueldo}')");
            browserCreateBank.ExecuteJs($"setIBAN('{IBAN}')");

            if(isATM) browserCreateBank.ExecuteJs($"isATM()");
        }

        private void LoggedOpenBank(object[] args)
        {
            sueldo = args[0].ToString();
            IBAN = args[1].ToString();

            browserCreateBank.Destroy();
            browserCreateBank = new RAGE.Ui.HtmlWindow("package://statics/bank/main.html");
            browserCreateBank.ExecuteJs($"setSueldoActual('{sueldo}')");
            browserCreateBank.ExecuteJs($"setIBAN('{IBAN}')");
        }

        private void LoginBankAccount(object[] args)
        {
            Events.CallRemote("BankAccountLogin", args[0].ToString());
        }

        private void OpenBank(object[] args)
        {
            browserCreateBank = new RAGE.Ui.HtmlWindow("package://statics/bank/pin.html");
            RAGE.Ui.Cursor.Visible = true;
            isATM = (bool)args[0];
            browserCreateBank.ExecuteJs($"setBankCreateUsername('{RAGE.Elements.Player.LocalPlayer.Name}')");
        }

        private void CreateBankAccount(object[] args)
        {
            Events.CallRemote("BankAccountCreate", args[0].ToString(), args[1].ToString());
        }

        private void DestroyCreateBankClick(object[] args)
        {
            Events.CallRemote("ActionBankClose");
            browserCreateBank.Destroy();
            RAGE.Ui.Cursor.Visible = false;
        }

        private void OpenCreateBank(object[] args)
        {
            browserCreateBank = new RAGE.Ui.HtmlWindow("package://statics/bank/index.html");
            browserCreateBank.ExecuteJs($"setBankCreateUsername('{RAGE.Elements.Player.LocalPlayer.Name}')");
            RAGE.Ui.Cursor.Visible = true;
        }

        private void OpenCreateBankPin(object[] args)
        {
            browserCreateBank.Destroy();
            browserCreateBank = new RAGE.Ui.HtmlWindow("package://statics/bank/crear.html");
        }

        private void DestroyCreateBank(object[] args)
        {
            browserCreateBank.Destroy();
            RAGE.Ui.Cursor.Visible = false;
        }
    }
}
