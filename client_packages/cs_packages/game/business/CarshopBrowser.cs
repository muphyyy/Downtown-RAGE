using RAGE;
using System;
using System.Collections.Generic;
using System.Text;
using RAGE.Ui;

namespace DowntownRP_cs.game.business
{
    public class CarshopBrowser : Events.Script
    {
        private static HtmlWindow carsBrowser;
        public CarshopBrowser()
        {
            Events.Add("OpenCarshopBrowser", OpenCarshopBrowser);
            Events.Add("DestroyCarshopBrowser", DestroyCarshopBrowser);
            Events.Add("BuyCarshopSS", BuyCarshopSS);
        }

        private void OpenCarshopBrowser(object[] args)
        {
            carsBrowser = new HtmlWindow("package://statics/business/ventaveh.html");
            carsBrowser.ExecuteJs($"setCarShopInfo('{args[0].ToString()}', '{args[1].ToString()}', {(int)args[2]})");
            Cursor.Visible = true;
        }

        private void DestroyCarshopBrowser(object[] args)
        {
            carsBrowser.Destroy();
            Cursor.Visible = false;
        }

        private void BuyCarshopSS(object[] args)
        {
            Events.CallRemote("BuyCarCompany", args[0].ToString(), args[1].ToString());
        }
    }
}
