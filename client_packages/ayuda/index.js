const NativeUI = require("nativeui");
const Offset = require("ayuda/offsets")
const Menu = NativeUI.Menu;
const UIMenuItem = NativeUI.UIMenuItem;
const Point = NativeUI.Point;
var anim_menu = null;


mp.events.add('PedirAyuda', () => {
    mp.events.callRemote("PedirAyudaCommands");
});

mp.events.add('ReceiveHelp', (arg) => {
   if (anim_menu == null) {
        var data = JSON.parse(arg);
        anim_menu = new Menu("Ayuda", "Vea la ayuda sobre los diferentes Comandos.", new Point(mp.game.resolution.width - Offset.getWidthOffset(mp.game.resolution.width), 50));
        for (var i = 0; i < data.length; i++) anim_menu.AddItem(new UIMenuItem(data[i].Item, data[i].Descrpt));
		anim_menu.ItemSelect.on((item, index) => {
            anim_menu.Visible = false;
        });

    }
    anim_menu.Visible = true;
});

