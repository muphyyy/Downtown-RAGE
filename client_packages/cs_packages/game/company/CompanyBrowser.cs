using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using RAGE.Elements;

namespace DowntownRP_cs.game.company
{
    public class CompanyBrowser : Events.Script
    {
        private static RAGE.Ui.HtmlWindow bCompany;
        public CompanyBrowser()
        {
            Events.Add("CompanyAddStreetName", CompanyAddStreetName);
            Events.Add("OpenCompanyBrowser", OpenCompanyBrowser);
            Events.Add("CloseCompanyBrowser", CloseCompanyBrowser);
            Events.Add("CloseCompanyBrowserSS", CloseCompanyBrowserSS);
            Events.Add("BuyCompanySS", BuyCompanySS);
        }

        private void BuyCompanySS(object[] args)
        {
            Events.CallRemote("BuyCompany");
        }

        private void CloseCompanyBrowserSS(object[] args)
        {
            Events.CallRemote("CompanyCloseBrowserSS");
        }

        private void CompanyAddStreetName(object[] args)
        {
            string name = RAGE.Game.Zone.GetNameOfZone(Player.LocalPlayer.Position.X, Player.LocalPlayer.Position.Y, Player.LocalPlayer.Position.Z);
            Events.CallRemote("CompanyFinishCreation", name);
        }

        private void OpenCompanyBrowser(object[] args)
        {
            bCompany = new RAGE.Ui.HtmlWindow("package://statics/company/venta.html");
            RAGE.Ui.Cursor.Visible = true;

            bCompany.ExecuteJs($"setCompaniesInfo('{args[0]}', {args[1]}, '{args[2]}');");

        }

        private void CloseCompanyBrowser(object[] args)
        {
            bCompany.Destroy();
            RAGE.Ui.Cursor.Visible = false;
        }
    }
}
