using RAGE;
using System;
using System.Collections.Generic;
using System.Text;
using RAGE.Ui;

namespace DowntownRP_cs.game.business
{
    public class BusinessBrowser : Events.Script
    {
        private static HtmlWindow busBrowser;
        public BusinessBrowser()
        {
            Events.Add("OpenBusinessBuyBrowser", OpenBusinessBuyBrowser);
            Events.Add("DestroyBusinessBuyBrowser", DestroyBusinessBuyBrowser);
            Events.Add("BusinessCloseBrowserSS", BusinessCloseBrowserSS);
            Events.Add("BuyBusinessSS", BuyBusinessSS);
        }

        private void OpenBusinessBuyBrowser(object[] args)
        {
            busBrowser = new HtmlWindow("package://statics/business/venta.html");
            busBrowser.ExecuteJs($"setBusinessInfo('{args[0].ToString()}', {(int)args[1]}, '{args[2].ToString()}')");
            Cursor.Visible = true;
        }

        private void DestroyBusinessBuyBrowser(object[] args)
        {
            busBrowser.Destroy();
            Cursor.Visible = false;
        }

        private void BusinessCloseBrowserSS(object[] args)
        {
            Events.CallRemote("CloseBrowserBuyBusiness");
        }

        private void BuyBusinessSS(object[] args)
        {
            Events.CallRemote("BuyBusiness");
        }
    }
}
