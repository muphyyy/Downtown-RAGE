using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace DowntownRP.Utilities
{
    public class Webhooks
    {
        public static byte[] Post(string url, NameValueCollection pairs)
        {
            using (WebClient webClient = new WebClient())
                return webClient.UploadValues(url, pairs);
        }

        // Función para enviar el webhook
        public static void sendWebHook(int type, string message)
        {
            string url = "";
            string username = "";
            switch (type)
            {
                case 1:
                    url = "https://discordapp.com/api/webhooks/640573064098349062/jGJn-hZqvZ6LTvQ94b6mlVBDZB9xU99G4ShAXg23SJKIvCtJdE70rx4vjT4awcBimFDB";
                    username = "Downtown Logs";
                    break;
            }

            Webhooks.Post(url, new NameValueCollection()
            {
                {
                    "username",
                    username
                },
                {
                    "content",
                    message
                }
            });
        }
    }
}
