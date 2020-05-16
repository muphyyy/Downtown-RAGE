using System;
using System.Threading.Tasks;
using GTANetworkAPI;

namespace DowntownRP
{
    public class Main : Script
    {

        [ServerEvent(Event.ResourceStart)]
        public async Task ResourceStartMain()
        {
            Utilities.Webhooks.sendWebHook(1, $"✅ [{DateTime.Now.ToString()}] {Data.Info.serverName} se ha iniciado con éxito ({Data.Info.serverVersion})");
            await Task.Delay(100);
            await SpawnDatabaseEntities();

            NAPI.Server.SetGlobalServerChat(false);
            NAPI.Server.SetGamemodeName(Data.Info.serverName + " v" + Data.Info.serverVersion);
            NAPI.Server.SetCommandErrorMessage("<font color='red'>[ERROR]</font> El comando no existe. (/ayuda para mas información)");
        }

        public async Task SpawnDatabaseEntities()
        {
            await World.Banks.DatabaseFunctions.SpawnBank();
            await World.Companies.DbFunctions.SpawnCompanies();
            await World.Business.DbFunctions.SpawnBusiness();

            await Task.Delay(1000);
            Console.WriteLine("- - - - - - - - - - - - - - - - - - - - - - - -");
            Console.WriteLine($"{Data.Info.serverName} by Muphy || Versión {Data.Info.serverVersion}");
            Console.WriteLine($"=> {Data.Info.banksSpawned} bancos/atms spawneados en el mapa");
            Console.WriteLine($"=> {Data.Info.companiesSpawned} empresas spawneadas en el mapa");
            Console.WriteLine($"=> {Data.Info.businessSpanwed} negocios spawneados en el mapa");
            Console.WriteLine("- - - - - - - - - - - - - - - - - - - - - - - -");
        }
    }
}
