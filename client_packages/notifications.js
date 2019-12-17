mp.game.audio.setAudioFlag("LoadMPData", true);

mp.events.add('NotificationSound', () => {
  mp.game.audio.playSoundFrontend(-1, "Pin_Good", "DLC_HEIST_BIOLAB_PREP_HACKING_SOUNDS", true);
});

mp.events.add('chat_goal', (texto, texto2) => {
  mp.game.ui.messages.showShard(texto, texto2, 0, 11);
});

mp.events.add('testPaydayLevel', (texto) => {
  mp.banners.createObjective(2, "Payday", texto).then(done => { });
});

mp.events.add('displayBusinessVenta', () => {
  mp.events.callRemote('debuginv', 'lol');
  const instructions = require('betterInstructions/better_instructions');

  const horizontalInstructionList = new instructions(-1);

  horizontalInstructionList.addButton('Negocio en venta, para mas información presiona ', 'F6');

  if(!horizontalInstructionList.isActive()) {
    horizontalInstructionList.toggleHud(true);
  }

  setTimeout( () => {
    if(horizontalInstructionList.isActive()) {
      horizontalInstructionList.toggleHud(false);
    }
  }, 5000);
});