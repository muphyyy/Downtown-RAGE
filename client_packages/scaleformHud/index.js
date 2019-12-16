const Scaleform = require('./scaleformHud/Scaleform')
let banner1, banner2, banner3

mp.banners = {
  createIntro: function (style, header, subHeader) {
    return new Promise(resolve => {
      initBanners()
      mp.gui.chat.push('Started...')
      createBanner('intro', style, header, subHeader).then(_ => {
        resolve();
      });
    })
  },
  createObjective: function (style, header, obj) {
    return new Promise(resolve => {
      initBanners()
      createBanner('obj', style, header, obj).then(resolve)
    })
  },
  missionResult: function (style, missionName, reason, passed) {
    return new Promise(resolve => {
      initBanners()
      createBanner('missionResult', style, missionName, reason, passed).then(resolve)
    })
  },
  missionEnd: function (style, cashWon, level) {
    return new Promise(resolve => {
      initBanners()
      createBanner('endMsg', style, cashWon, level).then(resolve)
    })
  },
}

mp.events.add('render', () => {
  if (banner1 && banner2 && banner3) renderScaleforms()
})

function createBanner(...args) {
  return new Promise(resolve => {
    let type = args[0]
    let style = args[1]
    let hud = 'HUD_COLOUR_BLACK'
    
    cleanWall(type, hud)

    switch (type) {
      case 'intro': {
        let header = args[2]
        let subHeader = args[3]
        banner1.callFunction('SET_PAUSE_DURATION', 5)
        banner1.callFunction('ADD_INTRO_TO_WALL', type, header, subHeader, '', '', '', 0, 0, 0, true, 'HUD_COLOUR_WHITE')
        banner1.callFunction('ADD_BACKGROUND_TO_WALL', type, 75, style)
        banner1.callFunction('SHOW_STAT_WALL', type)

        banner2.callFunction('SET_PAUSE_DURATION', 5)
        banner2.callFunction('ADD_INTRO_TO_WALL', type, header, subHeader, '', '', '', 0, 0, 0, true, 'HUD_COLOUR_WHITE')
        banner2.callFunction('ADD_BACKGROUND_TO_WALL', type, 75, style)
        banner2.callFunction('SHOW_STAT_WALL', type)

        banner3.callFunction('SET_PAUSE_DURATION', 5)
        banner3.callFunction('ADD_INTRO_TO_WALL', type, header, subHeader, '', '', '', 0, 0, 0, true, 'HUD_COLOUR_WHITE')
        banner3.callFunction('ADD_BACKGROUND_TO_WALL', type, 75, style)
        banner3.callFunction('SHOW_STAT_WALL', type)

        setTimeout(_ => {
          removeBanners()
          resolve(true)
        }, 7500)
      }
      break
    case 'obj': {
      let header = args[2]
      let subHeader = args[3]
      banner1.callFunction('SET_PAUSE_DURATION', 5)
      banner1.callFunction('ADD_OBJECTIVE_TO_WALL', type, header, subHeader, true)
      banner1.callFunction('ADD_BACKGROUND_TO_WALL', type, 75, style)
      banner1.callFunction('SHOW_STAT_WALL', type)

      banner2.callFunction('SET_PAUSE_DURATION', 5)
      banner2.callFunction('ADD_OBJECTIVE_TO_WALL', type, header, subHeader, true)
      banner2.callFunction('ADD_BACKGROUND_TO_WALL', type, 75, style)
      banner2.callFunction('SHOW_STAT_WALL', type)

      banner3.callFunction('SET_PAUSE_DURATION', 5)
      banner3.callFunction('ADD_OBJECTIVE_TO_WALL', type, header, subHeader, true)
      banner3.callFunction('ADD_BACKGROUND_TO_WALL', type, 75, style)
      banner3.callFunction('SHOW_STAT_WALL', type)

      setTimeout(_ => {
        removeBanners()
        resolve(true)
      }, 7500)
    }
    break
    case 'missionResult': {
      let missionName = args[2]
      let missionReason = args[3]
      let passed = args[4] ? 'Mission Passed' : 'Mission Failed'
      let color = args[4] ? 'HUD_COLOR_GREEN' : 'HUD_COLOR_RED'
  
      banner1.callFunction('SET_PAUSE_DURATION', 5)
      banner1.callFunction('ADD_MISSION_RESULT_TO_WALL', type, missionName, passed, missionReason, true, true, true, 0, color)
      banner1.callFunction('ADD_BACKGROUND_TO_WALL', type, 75, style)
      banner1.callFunction('SHOW_STAT_WALL', type)

      banner2.callFunction('SET_PAUSE_DURATION', 5)
      banner2.callFunction('ADD_MISSION_RESULT_TO_WALL', type, missionName, passed, missionReason, true, true, true, 0, color)
      banner2.callFunction('ADD_BACKGROUND_TO_WALL', type, 75, style)
      banner2.callFunction('SHOW_STAT_WALL', type)

      banner3.callFunction('SET_PAUSE_DURATION', 5)
      banner3.callFunction('ADD_MISSION_RESULT_TO_WALL', type, missionName, passed, missionReason, true, true, true, 0, color)
      banner3.callFunction('ADD_BACKGROUND_TO_WALL', type, 75, style)
      banner3.callFunction('SHOW_STAT_WALL', type)

      setTimeout(_ => {
        removeBanners()
        resolve(true)
      }, 7500)
    }
    break
    case 'endMsg': {
      let cashWon = args[2]
      let level = args[3]
      let allowLevel = typeof level === 'object';

      banner1.callFunction('ADD_BACKGROUND_TO_WALL', type, 255, style)
      banner1.callFunction('SET_PAUSE_DURATION', 2) // level.rpGain, level.rpStart, level.rpMin, level.rpMax, level.currentRank, level.nextRank, level.currentRank ? level.currentRank.toString() : '', level.nextRank ? level.nextRank.toString() : ''
      banner1.callFunction('ADD_CASH_TO_WALL', type, cashWon, 0)
      if (allowLevel) banner1.callFunction("ADD_REP_POINTS_AND_RANK_BAR_TO_WALL", type, level.rpGain, level.rpStart, level.rpMin, level.rpMax, level.currentRank, level.nextRank, level.rankUpText, level.rankUpExtraText)
      banner1.callFunction('SHOW_STAT_WALL', type)

      banner2.callFunction('ADD_BACKGROUND_TO_WALL', type, 255, style)
      banner2.callFunction('SET_PAUSE_DURATION', 2)
      banner2.callFunction('ADD_CASH_TO_WALL', type, cashWon, 0)
      if (allowLevel) banner2.callFunction("ADD_REP_POINTS_AND_RANK_BAR_TO_WALL", type, level.rpGain, level.rpStart, level.rpMin, level.rpMax, level.currentRank, level.nextRank, level.rankUpText, level.rankUpExtraText)
      banner2.callFunction('SHOW_STAT_WALL', type)

      banner3.callFunction('ADD_BACKGROUND_TO_WALL', type, 255, style)
      banner3.callFunction('SET_PAUSE_DURATION', 2)
      banner3.callFunction('ADD_CASH_TO_WALL', type, cashWon, 0)
      if (allowLevel) banner3.callFunction("ADD_REP_POINTS_AND_RANK_BAR_TO_WALL", type, level.rpGain, level.rpStart, level.rpMin, level.rpMax, level.currentRank, level.nextRank, level.rankUpText, level.rankUpExtraText)
      banner3.callFunction('SHOW_STAT_WALL', type)

      setTimeout(_ => {
        removeBanners()
        resolve(true)
      }, 5500)
    }
    }
  })
}

function initBanners() {
  banner1 = new Scaleform('mp_celebration_bg')
  banner2 = new Scaleform('mp_celebration_fg')
  banner3 = new Scaleform('mp_celebration')
}

function removeBanners() {
  banner1.dispose()
  banner2.dispose()
  banner3.dispose()
}

function cleanWall(type, hud) {
  banner1.callFunction('CLEANUP', type)
  banner1.callFunction('CREATE_STAT_WALL', type, hud)
  banner2.callFunction('CLEANUP', type)
  banner2.callFunction('CREATE_STAT_WALL', type, hud)
  banner3.callFunction('CLEANUP', type)
  banner3.callFunction('CREATE_STAT_WALL', type, hud)
}

function renderScaleforms() {
  banner1.renderFullscreen()
  banner2.renderFullscreen()
  banner3.renderFullscreen()
}