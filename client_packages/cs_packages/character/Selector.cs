using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using RAGE.Game;

namespace DowntownRP_cs.character
{
    public class Selector : Events.Script
    {
        private static RAGE.Ui.HtmlWindow selectorBrowser;
        public Selector()
        {
            Events.Add("OpenCharacterSelector", OpenCharacterSelector);
            Events.Add("ShowCharactersOnList", ShowCharactersOnList);
            Events.Add("ShowCharacterIS", ShowCharacterIS);
        }

        private void ShowCharacterIS(object[] args)
        {
            Events.CallRemote("ShowCharacterInSelector", (int)args[0]);
        }

        private void ShowCharactersOnList(object[] args)
        {
            string name = args[2].ToString();
            int id = (int)args[1];

            switch ((int)args[0])
            {
                case 0:
                    selectorBrowser.ExecuteJs($"setPj1({id}, {name});");
                    break;

                case 1:
                    selectorBrowser.ExecuteJs($"setPj2({id}, {name});");
                    break;

                case 2:
                    selectorBrowser.ExecuteJs($"setPj3({id}, {name});");
                    break;
            }
        }

        private void OpenCharacterSelector(object[] args)
        {
            // Initialize the player's position
            RAGE.Elements.Player.LocalPlayer.FreezePosition(true);

            // Create the camera
            var cam = Cam.CreateCameraWithParams(RAGE.Game.Misc.GetHashKey("DEFAULT_SCRIPTED_CAMERA"), -898.5869f, -435.9167f, 90.2943f, 0.0f, 24.9442f, 8.0f, 75.0f, true, 2);
            Cam.SetCamActive(cam, true);
            Cam.RenderScriptCams(true, false, 0, false, false, 0);

            selectorBrowser = new RAGE.Ui.HtmlWindow("package://statics/pj/selector.html");
            RAGE.Ui.Cursor.Visible = true;
        }

    }
}
