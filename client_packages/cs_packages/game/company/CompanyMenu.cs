using System;
using System.Collections.Generic;
using System.Text;
using RAGE;

namespace DowntownRP_cs.game.company
{
    public class CompanyMenu : Events.Script
    {
        private static RAGE.Ui.HtmlWindow companyMenu;
        private static int cajaFuerte;
        private static int trabajadores;
        private static int porcentaje;
        private static int subsidio;
        private static string nombre;

        public CompanyMenu()
        {
            Events.Add("OpenMenuCompany", OpenMenuCompany);
            Events.Add("DestroyMenuCompany", DestroyMenuCompany);
            Events.Add("CloseMenuCompanyBrowser", CloseMenuCompanyBrowser);
            Events.Add("ReturnMainMenuCompany", ReturnMainMenuCompany);
            Events.Add("OpenConfigMenuCompany", OpenConfigMenuCompany);
            Events.Add("OpenGestionMenuCompany", OpenGestionMenuCompany);
            Events.Add("OpenMembersMenuCompany", OpenMembersMenuCompany);
            Events.Add("OpenSafeMenuCompany", OpenSafeMenuCompany);
            Events.Add("ChangeNameMenuCompany", ChangeNameMenuCompany);
            Events.Add("StatusOfficeMenuCompany", StatusOfficeMenuCompany);
            Events.Add("RecruitmentModeMenuCompany", RecruitmentModeMenuCompany);
            Events.Add("GananciasMenuCompany", GananciasMenuCompany);
            Events.Add("SubsidioMenuCompany", SubsidioMenuCompany);
            Events.Add("IngresarMenuCompanyBrowser", IngresarMenuCompanyBrowser);
            Events.Add("RetirarMenuCompanyBrowser", RetirarMenuCompanyBrowser);
        }

        private void RetirarMenuCompanyBrowser(object[] args)
        {
            Events.CallRemote("MenuCompanyRetirar", args[0].ToString());
        }

        private void IngresarMenuCompanyBrowser(object[] args)
        {
            Events.CallRemote("MenuCompanyIngresar", args[0].ToString());
        }

        private void SubsidioMenuCompany(object[] args)
        {
            Events.CallRemote("MenuCompanySubsidio", args[0].ToString());
        }

        private void GananciasMenuCompany(object[] args)
        {
            Events.CallRemote("MenuCompanyGanancias", args[0].ToString());
        }

        private void RecruitmentModeMenuCompany(object[] args)
        {
            Events.CallRemote("MenuCompanyRecruitmentMode", (int)args[0]);
        }

        private void StatusOfficeMenuCompany(object[] args)
        {
            Events.CallRemote("MenuCompanyOfficeStatus", (int)args[0]);
        }

        private void ChangeNameMenuCompany(object[] args)
        {
            Events.CallRemote("MenuCompanyChangeName", args[0].ToString());
        }

        private void OpenSafeMenuCompany(object[] args)
        {
            companyMenu.Destroy();
            companyMenu = new RAGE.Ui.HtmlWindow("package://statics/company/cajafuerte.html");
        }

        private void OpenMembersMenuCompany(object[] args)
        {
            //companyMenu.Destroy();
            //companyMenu = new RAGE.Ui.HtmlWindow("package://statics/company/miembros.html");
            Events.CallRemote("throwNotImplemented");
        }

        private void OpenGestionMenuCompany(object[] args)
        {
            companyMenu.Destroy();
            companyMenu = new RAGE.Ui.HtmlWindow("package://statics/company/gestion.html");
        }

        private void OpenConfigMenuCompany(object[] args)
        {
            companyMenu.Destroy();
            companyMenu = new RAGE.Ui.HtmlWindow("package://statics/company/configuracion.html");
        }

        private void ReturnMainMenuCompany(object[] args)
        {
            companyMenu.Destroy();
            companyMenu = new RAGE.Ui.HtmlWindow("package://statics/company/menu.html");
            companyMenu.ExecuteJs($"setStats({cajaFuerte}, {trabajadores}, {porcentaje}, {subsidio}, '{nombre}');");
        }

        private void CloseMenuCompanyBrowser(object[] args)
        {
            Events.CallRemote("MenuCompanyClose");
        }

        private void OpenMenuCompany(object[] args)
        {
            cajaFuerte = (int)args[0];
            trabajadores = (int)args[1];
            porcentaje = (int)args[2];
            subsidio = (int)args[3];
            nombre = args[4].ToString();

            companyMenu = new RAGE.Ui.HtmlWindow("package://statics/company/menu.html");
            RAGE.Ui.Cursor.Visible = true;
            companyMenu.ExecuteJs($"setStats({cajaFuerte}, {trabajadores}, {porcentaje}, {subsidio}, '{nombre}');");
        }

        private void DestroyMenuCompany(object[] args)
        {
            companyMenu.Destroy();
            RAGE.Ui.Cursor.Visible = false;
        }
    }
}
