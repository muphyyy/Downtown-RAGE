using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace DowntownRP.World.Companies
{
    public class Interior
    {
        public static Vector3 interior = new Vector3(-139.9714, -617.5212, 168.8205);
        public static Vector3 contract = new Vector3(-139.2477, -631.9765, 168.8604);
        public static Vector3 duty = new Vector3(-142.4859, -637.9554, 168.8205);

        public static void EnterCompany(Client player)
        {
            //var user = player.GetExternalData<Data.Entities.User>(0);
            if (!player.HasData("USER_CLASS")) return;
            Data.Entities.User user = player.GetData("USER_CLASS");

            if (!user.isInCompanyInterior)
            {
                Data.Entities.Company company = user.company;
                user.companyInterior = company;
                user.lastPositionInterior = player.Position;

                player.Dimension = (uint)company.id;
                player.Position = interior;
                user.isInCompanyInterior = true;
                player.TriggerEvent("GenerateSecretaryPedCompany", company.id);
                return;
            }
            else
            {
                player.Dimension = 0;
                player.Position = user.lastPositionInterior;
                user.isInCompanyInterior = false;
            }
        }
    }
}
