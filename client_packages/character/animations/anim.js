const NativeUI = require("nativeui");
const Menu = NativeUI.Menu;
const UIMenuItem = NativeUI.UIMenuItem;
const Point = NativeUI.Point;
const Offset = require("character/animations/offsets.js")

var anim_menu = null;
var anim_playing = false;

mp.events.add('ReceiveAnims', (arg) => {
   if (anim_menu == null) {
        var data = JSON.parse(arg);
        anim_menu = new Menu("Anims", "Seleccione una anim.", new Point(mp.game.resolution.width - Offset.getWidthOffset(mp.game.resolution.width), 50));
        for (var i = 0; i < data.length; i++) anim_menu.AddItem(new UIMenuItem(data[i], ""));

        anim_menu.ItemSelect.on((item, index) => {
            mp.events.callRemote("PlayAnim", index);
        });
    }
    anim_menu.Visible = true;
});

mp.events.add('SetAnimPlaying', (data) => {
    anim_playing = data;
});


mp.events.add('CallAnimList', () => {
    if (anim_menu == null) mp.events.callRemote("RequestAnims");
    else anim_menu.Visible = !anim_menu.Visible;
});

setInterval(() => {
    if (mp.keys.isDown(0x57) === true || // W
        mp.keys.isDown(0x41) === true || // A
        mp.keys.isDown(0x53) === true || // S
        mp.keys.isDown(0x44) === true) { // D
        if (anim_playing) mp.events.callRemote("StopPlayerAnim");
    }
}, 500);
