using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using RAGE;
using RAGE.Game;
using RAGE.Ui;

namespace DowntownRP_cs.login
{
    public class Login : Events.Script
    {
        private static HtmlWindow browserLogin;
        public Login()
        {
            // Eventos servidor - cliente
            Events.Add("ShowLoginWindow", ShowLoginWindow);
            Events.Add("ShowRegisterWindow", ShowRegisterWindow);
            Events.Add("DestroyWindow", DestroyWindow);
            Events.Add("ShowErrorAlert", ShowErrorAlert);
            Events.Add("GetPlayerReadyToPlay", GetPlayerReadyToPlay);

            // Eventos cliente - servidor
            Events.Add("LoginUser", LoginUser);
            Events.Add("RegisterUser", RegisterUser);

            // Eventos cliente
            Events.Add("SwitchLoginRegister", SwitchLoginRegister);

            // Callbacks clientside de RAGE
            Events.OnGuiReady += OnGuiReadyEvent; // Evento que se ejecuta cuando se inicia el servidor
        }


        private void GetPlayerReadyToPlay(object[] args)
        {
            Ui.DisplayRadar(true);
            Ui.DisplayHud(true);
            Chat.Show(true);
        }

        private void SwitchLoginRegister(object[] args)
        {
            switch ((int)args[0])
            {
                case 0:
                    browserLogin.Destroy();
                    Events.CallLocal("ShowRegisterWindow");
                    break;

                case 1:
                    browserLogin.Destroy();
                    Events.CallLocal("ShowLoginWindow");
                    break;
            }
        }

        private void RegisterUser(object[] args)
        {
            Events.CallRemote("RE_RegisterPlayerAccount", args[0].ToString(), args[1].ToString(), args[2].ToString(), args[3].ToString());
        }

        private void LoginUser(object[] args)
        {
            Events.CallRemote("RE_LoginPlayerAccount", args[0].ToString(), args[1].ToString());
        }

        private void ShowLoginWindow(object[] args)
        {
            browserLogin = new HtmlWindow("package://statics/login/index.html");
            RAGE.Ui.Cursor.Visible = true;
        }

        private void ShowRegisterWindow(object[] args)
        {
            browserLogin = new HtmlWindow("package://statics/login/register.html");
            RAGE.Ui.Cursor.Visible = true;
        }

        private void DestroyWindow(object[] args)
        {
            RAGE.Elements.Player.LocalPlayer.FreezePosition(false);
            Cam.RenderScriptCams(false, false, 0, true, false, 0);
            browserLogin.Destroy();
        }

        private void ShowErrorAlert(object[] args)
        {
            switch ((int)args[0])
            {
                case 1:
                    browserLogin.ExecuteJs("ShowError()");
                    break;

                case 2:
                    browserLogin.ExecuteJs("ShowError2()");
                    break;

                case 3:
                    browserLogin.ExecuteJs("ShowError3()");
                    break;

                case 4:
                    browserLogin.ExecuteJs("ShowError4()");
                    break;
            }
        }

        private void OnGuiReadyEvent()
        {
            // Initialize the player's position
            RAGE.Elements.Player.LocalPlayer.FreezePosition(true);

            // Create the camera
            var cam = Cam.CreateCameraWithParams(RAGE.Game.Misc.GetHashKey("DEFAULT_SCRIPTED_CAMERA"), 3400.0f, 5075.0f, 20.0f, 0.0f, 0.0f, 8.0f, 75.0f, true, 2);
            Cam.SetCamActive(cam, true);
            Cam.RenderScriptCams(true, false, 0, false, false, 0);

            // Hide the chat, HUD and radar
            Chat.Show(false);
            Chat.Activate(false);
            Ui.DisplayHud(false);
            Ui.DisplayRadar(false);
        }
    }
}
