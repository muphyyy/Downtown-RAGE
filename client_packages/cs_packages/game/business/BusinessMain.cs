using RAGE;
using RAGE.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace DowntownRP_cs.game.business
{
    public class BusinessMain : Events.Script
    {
        public BusinessMain()
        {
            Events.Add("BusinessAddStreetName", BusinessAddStreetName);
        }

        private void BusinessAddStreetName(object[] args)
        {
            string name = RAGE.Game.Zone.GetNameOfZone(Player.LocalPlayer.Position.X, Player.LocalPlayer.Position.Y, Player.LocalPlayer.Position.Z);
            Events.CallRemote("BusinessFinishCreation", name);
        }
    }
}