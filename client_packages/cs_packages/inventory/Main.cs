using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RAGE;

namespace DowntownRP_cs.inventory
{
    public class Main : Events.Script
    {
        private static RAGE.Ui.HtmlWindow invBrowser;
        public Main()
        {
            Events.Add("OpenInventory", OpenInventory);
            Events.Add("CloseInventory", CloseInventory);
            Events.Add("CloseInventoryButton", CloseInventoryButton);
        }

        private void CloseInventoryButton(object[] args)
        {
            Events.CallRemote("ActionInventory");
        }

        private void OpenInventory(object[] args)
        {
            data.Inventory inventario = JsonConvert.DeserializeObject<data.Inventory>(args[0] as string);

            invBrowser = new RAGE.Ui.HtmlWindow("package://statics/inventory/index.html");
            RAGE.Ui.Cursor.Visible = true;

            invBrowser.ExecuteJs($"setInventoryData('{inventario.slot1.name}', {inventario.slot1.type}, {inventario.slot1.quantity}, '{inventario.slot2.name}', {inventario.slot2.type}, {inventario.slot2.quantity}, '{inventario.slot3.name}', {inventario.slot3.type}, {inventario.slot3.quantity}, '{inventario.slot4.name}', {inventario.slot4.type}, {inventario.slot4.quantity}, '{inventario.slot5.name}', {inventario.slot5.type}, {inventario.slot5.quantity}, '{inventario.slot6.name}', {inventario.slot6.type}, {inventario.slot6.quantity}, '{inventario.slot7.name}', {inventario.slot7.type}, {inventario.slot7.quantity}, '{inventario.slot8.name}', {inventario.slot8.type}, {inventario.slot8.quantity}, '{inventario.slot9.name}', {inventario.slot9.type}, {inventario.slot9.quantity}, '{inventario.slot10.name}', {inventario.slot10.type}, {inventario.slot10.quantity}, '{inventario.slot11.name}', {inventario.slot11.type}, {inventario.slot11.quantity}, '{inventario.slot12.name}', {inventario.slot12.type}, {inventario.slot12.quantity});");
            invBrowser.ExecuteJs($"setPjItems('{inventario.pjSlot1.name}', '{inventario.pjSlot2.name}', '{inventario.pjSlot3.name}', '{inventario.pjSlot4.name}', '{inventario.pjSlot5.name}', '{inventario.pjSlot6.name}');");
        }

        private void CloseInventory(object[] args)
        {
            invBrowser.Destroy();
            RAGE.Ui.Cursor.Visible = false;
        }
    }
}
