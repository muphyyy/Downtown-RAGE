let browser;

var character_data = {
  name: "",
  gender: 0,
  faceFirst: 0,
  faceSecond: 0,
  faceMix: 0.0,
  skinFirst: 0,
  skinSecond: 0,
  skinMix: 0.0,
  hairType: 0,
  hairColor: 0,
  hairHighlight: 0,
  eyeColor: 0,
  eyebrows: 0,
  eyebrowsColor1: 0,
  eyebrowsColor2: 0,
  beard: null,
  beardColor: 0,
  makeup: null,
  makeupColor: 0,
  lipstick: null,
  lipstickColor: 0,
  torso: 0,
  legs: 1,
  feet: 1,
  undershirt: 57,
  topshirt: 1,
  topshirtTexture: 0,
  accessory: 0,
};

mp.events.add("ShowCharacterCreator", () => {
  if (!browser) {
    browser = mp.browsers.new("package://statics/character/creator.html");
  }

  resetCharacterCreation();
});

function resetCharacterCreation() {

character_data.torso = character_data.gender ? 5 : 0;
  character_data.undershirt = character_data.gender ? 95 : 57;
  character_data.hairType = character_data.gender ? 4 : 0;

  mp.events.callRemote('SetPlayerClothes', 2, character_data.hairType, 0);
  mp.events.callRemote('SetPlayerClothes', 3, character_data.torso, 0);
  mp.events.callRemote('SetPlayerClothes', 4, character_data.legs, 0);
  mp.events.callRemote('SetPlayerClothes', 6, character_data.feet, 0);
  mp.events.callRemote('SetPlayerClothes', 7, character_data.accessory, 0);
  mp.events.callRemote('SetPlayerClothes', 8, character_data.undershirt, 0);
  mp.events.callRemote('SetPlayerClothes', 11, character_data.topshirt, 0);
  mp.events.callRemote("SetPlayerSkin", character_data.gender ? 'FreemodeFemale01' : 'FreeModeMale01');

  mp.players.local.setHeadBlendData(character_data.faceFirst, character_data.faceSecond, 0, character_data.skinFirst, character_data.skinSecond, 0, character_data.faceMix, character_data.skinMix, 0, false);
  mp.players.local.setHairColor(character_data.hairColor, character_data.hairHighlight);
  mp.players.local.setEyeColor(0);

  mp.players.local.setHeadOverlayColor(2, 1, character_data.eyebrowsColor1, character_data.eyebrowsColor2);
  mp.players.local.setHeadOverlayColor(4, 0, character_data.makeupColor, character_data.makeupColor);
  mp.players.local.setHeadOverlayColor(8, 2, character_data.lipstickColor, character_data.lipstickColor);

}

mp.events.add("ChangeCharacterGender", (id) => {
  character_data.gender = id;
  character_data.faceFirst = 0;
  character_data.faceSecond = 0;
  character_data.faceMix = 0.0;
  character_data.skinFirst = 0;
  character_data.skinSecond = 0;
  character_data.skinMix = 0.0;
  character_data.hairType = 0;
  character_data.hairColor = 0;
  character_data.hairHighlight = 0;
  character_data.eyeColor = 0;
  character_data.eyebrows = 0;
  character_data.eyebrowsColor1 = 0;
  character_data.eyebrowsColor2 = 0;
  character_data.beard = null;
  character_data.beardColor = 0;
  character_data.makeup = null;
  character_data.makeupColor = 0;
  character_data.lipstick = null;
  character_data.lipstickColor = 0;
  character_data.torso = 0;
  character_data.legs = 1;
  character_data.feet = 1;
  character_data.undershirt = 57;
  character_data.topshirt = 1;
  character_data.topshirtTexture = 0;
  character_data.accessory = 0;

  resetCharacterCreation();
});

mp.events.add('MoveCameraPosition', (pos) => {
  switch (pos) {
    case 0:
      {
        // Head
        var camPos = new mp.Vector3(402.9378, -997.0, -98.35);
        var camRot = new mp.Vector3(0.0, 0.0, 176.891);

        var camera = mp.cameras.new('lookAtHead', camPos, camRot, 40);
        camera.pointAtCoord(402.9198, -996.5348, -98.35);
        camera.setActive(true);

        mp.game.cam.renderScriptCams(true, false, 2000, true, false);
        break;
      }
    case 1:
      {
        // Torso
        var camPos = new mp.Vector3(402.9378, -997.5, -98.60);
        var camRot = new mp.Vector3(0.0, 0.0, 176.891);

        var camera = mp.cameras.new('lookAtTorso', camPos, camRot, 40);
        camera.pointAtCoord(402.9198, -996.5348, -98.60);
        camera.setActive(true);

        mp.game.cam.renderScriptCams(true, false, 2000, true, false);
        break;
      }
    case 2:
      {
        // Legs
        var camPos = new mp.Vector3(402.9378, -997.5, -99.40);
        var camRot = new mp.Vector3(0.0, 0.0, 176.891);

        var camera = mp.cameras.new('lookAtLegs', camPos, camRot, 40);
        camera.pointAtCoord(402.9198, -996.5348, -99.40);
        camera.setActive(true);

        mp.game.cam.renderScriptCams(true, false, 2000, true, false);
        break;
      }
    case 3:
      {
        // Feet
        var camPos = new mp.Vector3(402.9378, -997.5, -99.85);
        var camRot = new mp.Vector3(0.0, 0.0, 176.891);

        var camera = mp.cameras.new('lookAtFeet', camPos, camRot, 40);
        camera.pointAtCoord(402.9198, -996.5348, -99.85);
        camera.setActive(true);

        mp.game.cam.renderScriptCams(true, false, 2000, true, false);
        break;
      }
    default:
      {
        // Default
        var camPos = new mp.Vector3(403.6378, -998.5422, -99.00404);
        var camRot = new mp.Vector3(0.0, 0.0, 176.891);

        var camera = mp.cameras.new('lookAtBody', camPos, camRot, 40);
        camera.pointAtCoord(402.9198, -996.5348, -99.00024);
        camera.setActive(true);

        mp.game.cam.renderScriptCams(true, false, 2000, true, false);
        break;
      }
  }
});

mp.events.add('SelectCharacterClothes', (data) => {
  data = JSON.parse(data);
  if (data.torso != undefined) {
    character_data.torso = data.torso;
    mp.events.callRemote('SetPlayerClothes', 3, data.torso, 0);
  }
  if (data.undershirt != undefined) {
    character_data.undershirt = data.undershirt;
    mp.events.callRemote('SetPlayerClothes', 8, data.undershirt, 0);
  }
  if (data.slot == 4)
    character_data.legs = data.variation;
  else if (data.slot == 6)
    character_data.feet = data.variation;
  else if (data.slot == 7)
    character_data.accessory = data.variation;
  else if (data.slot == 8)
    character_data.undershirt = data.variation;
  else if (data.slot == 11) {
    character_data.topshirt = data.variation;
    character_data.topshirtTexture = data.texture;
  }
  mp.events.callRemote('SetPlayerClothes', data.slot, data.variation, data.texture);
});


mp.events.add("GoBackToSelection", () => {
  if (browser) {
    browser.destroy();
    browser = undefined;
  }

  mp.events.callRemote("RetrieveCharactersList");
});

mp.events.add('SelectCharacterComponent', (data) => {
  var mix_data = [
    0.0,
    0.1,
    0.2,
    0.3,
    0.4,
    0.5,
    0.6,
    0.7,
    0.8,
    0.9
  ];

  data = JSON.parse(data);
  switch (data.type) {
    // Face
    case 13:
      {
        character_data.faceFirst = data.config1;
        character_data.faceSecond = data.config2;
        character_data.faceMix = mix_data[data.config3];
        mp.players.local.setHeadBlendData(character_data.faceFirst, character_data.faceSecond, 0, character_data.skinFirst, character_data.skinSecond, 0, character_data.faceMix, character_data.skinMix, 0, false);
        break;
      }
    // Eyes
    case 14:
      {
        character_data.eyeColor = data.config1;
        mp.players.local.setEyeColor(character_data.eyeColor);
        break;
      }
    // Hair
    case 15:
      {
        character_data.hairType = data.config1;
        character_data.hairColor = data.config2;
        character_data.hairHighlight = data.config3;
        mp.players.local.setHairColor(character_data.hairColor, character_data.hairHighlight);
        mp.events.callRemote('SetPlayerClothes', 2, character_data.hairType, 0);
        break;
      }
    case 16:
      {
        if (!character_data.gender) {
          if (data.config1 < 1) {
            character_data.beard = null;
          }
          else {
            character_data.beard = data.config1 - 1;
          }
          character_data.beardColor = data.config2;
          mp.players.local.setHeadOverlay(1, character_data.beard == null ? 255 : character_data.beard, 1, character_data.beardColor, character_data.beardColor);
        }
        else {
          if (data.config1 < 1) {
            character_data.lipstick = null;
          }
          else {
            character_data.lipstick = data.config1 - 1;
          }
          character_data.lipstickColor = data.config2;
          mp.players.local.setHeadOverlay(8, character_data.lipstick == null ? 255 : character_data.lipstick, 1, character_data.lipstickColor, character_data.lipstickColor);
        }
        break;
      }
    // Skin color
    case 17:
      {
        character_data.skinFirst = data.config1;
        character_data.skinSecond = data.config2;
        character_data.skinMix = mix_data[data.config3];
        mp.players.local.setHeadBlendData(character_data.faceFirst, character_data.faceSecond, 0, character_data.skinFirst, character_data.skinSecond, 0, character_data.faceMix, character_data.skinMix, 0, false);
        break;
      }
    // Eyebrow
    case 18:
      {
        character_data.eyebrows = data.config1;
            character_data.eyebrowsColor1 = data.config2;
            character_data.eyebrowsColor2 = data.config2;
            mp.players.local.setHeadOverlay(2, character_data.eyebrows == null ? 255 : character_data.eyebrows, 1, character_data.eyebrowsColor1, character_data.eyebrowsColor2);
            break;
      }
  }
});

mp.events.add('finishCharacterCreation', (character_name) => {
  if (browser) {
    browser.destroy();
    browser = undefined;
  }

  mp.gui.cursor.visible = false;
  mp.game.cam.renderScriptCams(false, false, 0, true, false);

  character_data.name = character_name;
  mp.events.callRemote('FinishCharacterCreation', JSON.stringify(character_data));
});

