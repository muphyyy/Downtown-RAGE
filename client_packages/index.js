require('character/creator.js');
require('character/selector.js');
require('character/animations/anim.js');
require('hud/index.js');
require('scaleform_messages/index.js');
require('./scaleformHud');

require('binds.js');
require('./ayuda');
require('notifications.js');
require('voice.js');
require('fly.js');

mp.gui.chat.show(false);
chatbox = mp.browsers.new('package://chat_old/index.html');
chatbox.markAsChat();
chatbox.execute('show()');

mp.game.vehicle.defaultEngineBehaviour = false;
mp.game.player.restoreStamina(1.0);