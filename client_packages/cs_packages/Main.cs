using RAGE;
using System;
using System.Collections.Generic;
using System.Text;
using RAGE.Elements;

namespace DowntownRP_cs
{
    public class Main : Events.Script
    {
        public Main()
        {
            Events.OnGuiReady += OnGuiReadyEvent;
            Events.OnEntityStreamIn += OnEntityStreamIn;
        }

        private void OnEntityStreamIn(Entity entity)
        {
            object objeto = entity.GetSharedData("BUSINESS_VEHICLE_SHARED");

            if(objeto != null)
            {
                Vehicle veh = (Vehicle)objeto;
                veh.FreezePosition(true);
            }
        }

        private void OnGuiReadyEvent()
        {
            RAGE.Game.Streaming.RequestIpl("ex_dt1_02_office_02b");

            Player.LocalPlayer.SetConfigFlag(429, true);
        }
    }
}
