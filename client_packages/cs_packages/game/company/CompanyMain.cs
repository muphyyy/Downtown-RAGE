using System;
using System.Collections.Generic;
using System.Text;
using RAGE;
using RAGE.Elements;

namespace DowntownRP_cs.game.company
{
    public class CompanyMain : Events.Script
    {
        private static Ped Secretary = null;
        public CompanyMain()
        {
            Events.Add("GenerateSecretaryPedCompany", GenerateSecretaryPedCompany);
        }

        private void GenerateSecretaryPedCompany(object[] args)
        {
            int dimension = (int)args[0];

            Vector3 SecretaryPos = new Vector3(-139.0331f, -633.9948f, 168.8205f);
            if (Secretary != null) Secretary.Destroy();
            Secretary = new Ped(0x50610C43, SecretaryPos, 0, (uint)dimension);
        }
    }
}
