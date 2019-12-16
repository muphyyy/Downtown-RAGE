let browser;
let characters;

mp.events.add("UpdateCharactersList", (data) => {
  if (!browser) {
    browser = mp.browsers.new("package://statics/character/selector.html");
  }

  characters = data;
  setTimeout(() => {
    mp.gui.cursor.visible = true;
    browser.execute(`updateList(${data})`);
  }, 500);

  mp.gui.cursor.visible = true;

  mp.players.local.position = new mp.Vector3(402.9198, -996.5348, -100.00024);
  mp.events.callRemote("SetPlayerRot", 176.8912);

  var start_camera = mp.cameras.new("start", new mp.Vector3(400.9627, -1005.109, -99.00404), new mp.Vector3(0, 0, -30.0), 60.0);
  start_camera.pointAtCoord(400.6378, -1005.109, -99.00404);
  start_camera.setActive(true);
  mp.game.cam.renderScriptCams(true, false, 0, true, false);

  var end_camera = mp.cameras.new("end", new mp.Vector3(403.6378, -998.5422, -99.00404), new mp.Vector3(0, 0, -30), 60.0);
  end_camera.pointAtCoord(402.9198, -996.5348, -99.00024);
  end_camera.setActiveWithInterp(start_camera.handle, 5000, 0, 0);

  if (characters != null) {
    if (JSON.parse(characters).length > 0) {
      mp.events.call('ApplyCharacterFeatures', 0);
    }
  }
});

mp.events.add("SelectCharacterToPlay", (id) => {
  if (browser) {
    browser.destroy();
    browser = undefined;
  }

  mp.gui.cursor.visible = false;
  mp.game.cam.renderScriptCams(false, false, 0, true, false);

  const character = JSON.parse(characters);
  mp.events.callRemote("SelectCharacter", character[id].id);
});

mp.events.add('ApplyCharacterFeatures', (i) => {
  var character = JSON.parse(characters);

  mp.players.local.model = character[i].gender ? -1667301416 : 1885233650;
  mp.events.callRemote("SetPlayerClothes", 2, character[i].hairType, 0);

  mp.players.local.setHeadBlendData(character[i].faceFirst, character[i].faceSecond, 0, character[i].skinFirst, character[i].skinSecond, 0, character[i].faceMix, character[i].skinMix, 0, false);
  mp.players.local.setHairColor(character[i].hairColor, character[i].hairHighlight);
  mp.players.local.setEyeColor(character[i].eyeColor);
  mp.players.local.setHeadOverlay(2, character[i].eyebrows, 1, character[i].eyebrowsColor1, character[i].eyebrowsColor2);

  mp.events.callRemote('DebugCharacterSelector1', character[i].beard);
  mp.events.callRemote('DebugCharacterSelector', character[i].beardColor);

  if (character[i].beard != null) mp.players.local.setHeadOverlay(1, character[i].beard == null ? 255 : character[i].beard, 1, character[i].beardColor, character[i].beardColor);
  if (character[i].makeup != null) mp.players.local.setHeadOverlay(4, character[i].makeup == null ? 255 : character[i].makeup, 1, character[i].makeupColor, character[i].makeupColor);
  if (character[i].lipstick != null) mp.players.local.setHeadOverlay(8, character[i].lipstick == null ? 255 : character[i].lipstick, 1, character[i].lipstickColor, character[i].lipstickColor);

  mp.events.callRemote("SetPlayerClothes", 3, character[i].torso, 0);
  mp.events.callRemote("SetPlayerClothes", 11, character[i].topshirt, character[i].topshirtTexture);
  mp.events.callRemote("SetPlayerClothes", 8, character[i].undershirt, 0);
  mp.events.callRemote("SetPlayerClothes", 4, character[i].legs, 0);
  mp.events.callRemote("SetPlayerClothes", 6, character[i].feet, 0);
  mp.events.callRemote("SetPlayerClothes", 7, character[i].accessory, 0);
});

mp.events.add('DeleteCharacter', (id) => {
  var character = JSON.parse(characters);
  mp.events.callRemote("DeleteCharacter", character[id].id);
});

mp.events.add("SendToCharacterCreator", () => {
  if (browser) {
    browser.destroy();
    browser = undefined;
  }

  mp.events.call("ShowCharacterCreator");
});
