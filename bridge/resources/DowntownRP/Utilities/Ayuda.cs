using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;

namespace DowntownRP.Utilities
{
    public class Ayuda:Script
    {

        List<AyudaEntry> EntriesAyuda = new List<AyudaEntry>
        {
            new AyudaEntry ("/b <Mensaje> o /ooc <mensaje", "Mensaje fuera de rol, que leen todos los usuarios conectados y se manda al discord firmado.")
        };

        [RemoteEvent ("PedirAyudaCommands")]
        public void AYUDA_REQ (Client player)
        {
            player.TriggerEvent("ReceiveHelp", NAPI.Util.ToJson(EntriesAyuda));
        }

    }
}
