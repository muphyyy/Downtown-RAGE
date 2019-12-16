using System;
using System.Collections.Generic;
using System.Text;

namespace DowntownRP_cs.data
{
    public class Item
    {
        public int id { get; set; }
        public string name { get; set; }
        public int type { get; set; } // 1 use, drop, trade | 2 use | 3 drop, trade | 4 show | 5 nothing
        public int quantity { get; set; }

        public Item(int itemid, string itemname, int itemtype, int itemquantity)
        {
            id = itemid;
            name = itemname;
            type = itemtype;
            quantity = itemquantity;
        }
    }
}
