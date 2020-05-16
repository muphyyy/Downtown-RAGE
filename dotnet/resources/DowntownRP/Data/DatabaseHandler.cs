using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace DowntownRP.Data
{
    public class DatabaseHandler : Script
    {
        public static string connectionHandle;

        [ServerEvent(Event.ResourceStart)]
        public void ResourceStartEvent()
        {
            string host = NAPI.Resource.GetSetting<string>(this, "host");
            string user = NAPI.Resource.GetSetting<string>(this, "username");
            string pass = NAPI.Resource.GetSetting<string>(this, "password");
            string db = NAPI.Resource.GetSetting<string>(this, "database");
            string ssl = NAPI.Resource.GetSetting<string>(this, "ssl");

            connectionHandle = "SERVER=" + host + "; DATABASE=" + db + "; UID=" + user + "; PASSWORD=" + pass + "; SSLMODE=" + ssl + ";";
        }
    }
}
