function getWidthOffset (width) {
    if (width <= 800) return -160
    else if (width <= 1024) return 40
    else if (width <= 1152) return 160
    else if (width <= 1280) return -160
    else if (width <= 1360) return -80
    else if (width <= 1366) return -70
    else if (width <= 1400) return 450
    else if (width <= 1440) return 180
    else if (width <= 1600) return 150
    else if (width <= 1680) return 460
    else if (width <= 1914) return 400
    else if (width <= 1920) return 475
    else return 200
}

exports.getWidthOffset = getWidthOffset
