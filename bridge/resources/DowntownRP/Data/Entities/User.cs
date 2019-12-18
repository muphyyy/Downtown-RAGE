using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace DowntownRP.Data.Entities
{
    public class User
    {
        // General variables
        public int id { get; set; }
        public int idpj { get; set; }
        public int enableMicrophone { get; set; }
        public int mpStatus { get; set; }
        public int bankAccount { get; set; }
        public string IBAN { get; set; }
        public double money { get; set; }
        public double bank { get; set; }
        public int level { get; set; }
        public int exp { get; set; }
        public int adminLv { get; set; }
        public bool chatStatus { get; set; } = false;
        public bool isFlying { get; set; } = false;
        public Vector3 lastPositionInterior { get; set; }

        // Inventory
        public Inventory inventory { get; set; }
        public Item slot1 { get; set; }
        public Item slot2 { get; set; }
        public Item slot3 { get; set; }
        public Item slot4 { get; set; }
        public Item slot5 { get; set; }
        public Item slot6 { get; set; }
        public bool isInventoryOpen { get; set; } = false;

        // Bank variables
        public bool isInBank { get; set; } = false;
        public Bank bankEntity { get; set; }
        public bool isBankCefOpen { get; set; } = false;
        public bool bankDelay { get; set; } = false;

        // Company variables
        public bool isInCompany { get; set; } = false;
        public bool isCompanyCefOpen { get; set; } = false;
        public bool isInCompanyInterior { get; set; } = false;
        public bool isInCompanyExitInterior { get; set; } = false;
        public Company company { get; set; }
        public Company companyInterior { get; set; }
        public Company companyProperty { get; set; }

        // Business variables
        public bool isInBusiness { get; set; } = false;
        public bool isBusinessCefOpen { get; set; } = false;
        public Business business { get; set; }


    }
}
