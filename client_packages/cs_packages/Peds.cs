using RAGE;
using System;
using System.Collections.Generic;
using System.Text;
using RAGE.Elements;

namespace DowntownRP_cs
{
    public class Peds : Events.Script
    {
        //private static Ped SecretaryCompanies;
        public Peds()
        {
            Events.OnGuiReady += OnGuiReadyEvent;
            Events.Add("lolasote", lolasote);
        }

        private void lolasote(object[] args)
        {
            RAGE.Elements.Player.LocalPlayer.Vehicle.SetLivery(3);
        }

        private void OnGuiReadyEvent()
        {
            RAGE.Game.Streaming.RequestIpl("ex_dt1_02_office_02b");
            //Vector3 SecretaryPos = new Vector3(-139.0331f, -633.9948f ,168.8205f);
            //SecretaryCompanies = new Ped(0x50610C43, SecretaryPos, 0);
        }
    }
}
