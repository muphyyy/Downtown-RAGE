var speedo = null;
var hud = null;
var sshowed = false;
var hshowed = false;
let player = mp.players.local;

//mp.game.graphics.pushScaleformMovieFunction(1, "SET_HEALTH_ARMOUR_BAR_VISIBLE");
//mp.game.graphics.pushScaleformMovieFunctionParameterBool(false);
//mp.game.graphics.popScaleformMovieFunctionVoid();

var hud = mp.browsers.new("package://statics/hud/hud.html");

mp.events.add('UpdateMoneyHUD', (money, modo) => {
		
		hud.execute(`actHud("cash", "set", ${money});`);
		
});

mp.events.add('UpdateSupervivencia', (tipo, modo, valor) => {
	let str = "actHud('"+tipo+"','"+modo+"','"+valor+"');";
	hud.execute(str);
	//mp.gui.chat.push(str);
		
});

mp.events.add('infolog', (texto) => {
	let str = "infolog('"+texto+"');";
	hud.execute(str);
});


mp.events.add('iniciaSupervivencia', () => {
		
		setInterval(function(){
			let player = mp.players.local;
			hud.execute(`actHud("hambre", "resto", "2");`);
			hud.execute(`actHud("sed", "resto", "3");`);
			//let heal = player.getHealth();
			//mp.gui.chat.push("Aumenta la sed y hambre y salud es: " + heal);
		}, 120000);

});

mp.events.add('muertodehambre', () => {
	let player = mp.players.local;
	mp.gui.chat.push("Estas muriendo de hambre ...");
	let heal = player.getHealth();
	let sangre = heal - 1;
	player.setHealth(sangre);

});


mp.events.add('showHUD', () => {
	hshowed = true;
	hud.execute(`showHud();`);		
});

mp.events.add('hideHUD', () => {
	hshowed = false;
	hud.execute(`hideHud();`);		
});

mp.events.add('update_hud_players', (players) => {
	if(hshowed){
		hud.execute(`updatePlayersOnline(` + players + `);`);
	}	
});

mp.events.add('update_hud_player', (id) => {
	if(hshowed){
		hud.execute(`updatePlayerId(` + id + `);`);
	}	
});

mp.events.add('update_hud_bank', (money) => {
	if(hshowed){
		hud.execute(`updatePlayerBank(` + money + `);`);
	}	
});

mp.events.add('update_hud_microphone', (microphone) => {
	if(hshowed){
		hud.execute(`updateMicrophoneStatus(` + microphone + `);`);
	}	
});

mp.events.add('render', () =>
{
	if (player.vehicle && player.vehicle.getPedInSeat(-1) === player.handle) // Check if player is in vehicle and is driver
	{
		if(player.vehicle.getVariable("IS_BUSINESS_VEHICLE")) return;
		if(sshowed === false) // Check if speedo is already showed
		{
			speedo = mp.browsers.new("package://statics/hud/index.html");
			speedo.execute(`$('.huds').fadeIn()`);
			sshowed = true;
		}
		/*Get vehicle infos*/
		let vel = player.vehicle.getSpeed() * 3.6;  	//Doc: https://wiki.rage.mp/index.php?title=Entity::getSpeed 2.8
		let rpm = player.vehicle.rpm * 7; 			//Doc: https://wiki.rage.mp/index.php?title=Vehicle::rpm
		let health = player.vehicle.getHealth();
		let maxHealth = player.vehicle.getMaxHealth();
		let healthPercent = Math.floor((health / maxHealth) * 100);
		let gas = player.vehicle.getPetrolTankHealth();
		gas = gas < 0 ? 0: gas / 10;

		var velo = parseInt(vel, 10);
		
		speedo.execute(`setProgressSpeed(`+velo+`, '.progress-speed');`); // Send data do CEF
		speedo.execute(`setProgressFuel(`+gas+`, '.progress-fuel');`); // Send data do CEF
	}
	else
	{
		if(sshowed)
		{
			speedo.execute("hideSpeedo();");
			sshowed = false;
			setTimeout(function() {
				speedo.destroy();
				speedo = null;
				//hud.execute(`speedo_off();`);
			}, 2000);
		}
	}
	
	/*mp.game.ui.hideHudComponentThisFrame(7);
	mp.game.ui.hideHudComponentThisFrame(9);*/
});


