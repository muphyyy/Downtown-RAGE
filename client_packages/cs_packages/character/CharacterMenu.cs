using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using RAGE.Game;
using RAGE.Ui;

namespace DowntownRP_cs.character
{
    public class CharacterMenu : Events.Script
    {
        private static HtmlWindow browserMenu;
        private static bool isBrowserMenuOpened = false;
        public CharacterMenu()
        {
            Events.Add("OpenCharacterMenu", OpenCharacterMenu);
            Events.Add("ActionMenu", ActionMenu);
            Events.Add("VoiceChangeType", VoiceChangeType);
            Events.Add("PMChangeStatus", PMChangeStatus);
            Events.Add("RadarChangeStatus", RadarChangeStatus);
            Events.Add("HudChangeStatus", HudChangeStatus);
        }

        private void HudChangeStatus(object[] args)
        {
            switch ((int)args[0])
            {
                case 0:
                    Events.CallLocal("showHUD");
                    Events.CallRemote("SendNotificationUser", "~g~INFO: ~w~Has activado el HUD");
                    break;

                case 1:
                    Events.CallLocal("hideHUD");
                    Events.CallRemote("SendNotificationUser", "~g~INFO: ~w~Has desactivado el HUD");
                    break;
            }
        }

        private void RadarChangeStatus(object[] args)
        {
            switch ((int)args[0])
            {
                case 0:
                    Ui.DisplayRadar(true);
                    Events.CallRemote("SendNotificationUser", "~g~INFO: ~w~Has activado el radar");
                    break;

                case 1:
                    Ui.DisplayRadar(false);
                    Events.CallRemote("SendNotificationUser", "~g~INFO: ~w~Has desactivado el radar");
                    break;
            }
        }

        private void PMChangeStatus(object[] args)
        {
            Events.CallRemote("ChangeStatusPM", (int)args[0]);
        }

        private void VoiceChangeType(object[] args)
        {
            Events.CallLocal("enableMicrophone", (int)args[0]);
            Events.CallRemote("sendVoiceChangedNotification", (int)args[0]);
        }

        private void ActionMenu(object[] args)
        {
            switch ((int)args[0])
            {
                case 1:
                    browserMenu.Destroy();
                    Events.CallLocal("CallAnimList");
                    Cursor.Visible = false;
                    break;

                case 2:
                    browserMenu.Destroy();
                    browserMenu = new HtmlWindow("package://statics/settings/info.html");
                    break;

                case 3:
                    browserMenu.Destroy();
                    browserMenu = new HtmlWindow("package://statics/settings/settings.html");
                    break;

                case 4:
                    Events.CallRemote("throwNotImplemented");
                    break;
            }
        }

        private void OpenCharacterMenu(object[] args)
        {
            if (!isBrowserMenuOpened)
            {
                browserMenu = new HtmlWindow("package://statics/settings/menu.html");
                Cursor.Visible = true;
                isBrowserMenuOpened = true;
            }
            else
            {
                browserMenu.Destroy();
                Cursor.Visible = false;
                isBrowserMenuOpened = false;
            }
        }
    }
}
