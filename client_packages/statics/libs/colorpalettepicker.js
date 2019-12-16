(function ($) {
    "use strict";

    var paletteObj = {
        yellow: 'rgb(229, 207, 100)',
        errieblack: 'rgb(12, 24, 33)',
        yankeeblue: 'rgb(27, 42, 65)',
        black: 'rgb(0, 0, 0)',
        blue: 'rgb(0, 0, 255)',
        green: 'rgb(121, 180, 115)',
        shinyshamrock: 'rgb(112, 163, 127)',
        palelavander: 'rgb(216, 220, 255)',
        grey: 'rgb(89, 95, 114)',
        lightseagreen: 'rgb(40, 175, 176)',
        orange: 'rgb(238, 150, 75)',
        red: 'rgb(211, 101, 130)',
        whiteguy: 'rgb(255, 238, 207)',
        marron: 'rgb(201, 166, 144)',
        celeste: 'rgb(129, 195, 215)',
        naranjofuerte: 'rgb(247, 127, 0)',
        moonstoneblue: 'rgb(119, 166, 182)',
        grannysmithapple: 'rgb(179, 216, 156)',
        verdeoscuro: 'rgb(44, 81, 76)',
        desertsand: 'rgb(236, 184, 165)',
        gold: 'rgb(255, 215, 0)',
        pastelblue: 'rgb(177, 181, 200)',
        indigoweb: 'rgb(95, 10, 135)',
        deepviolet: 'rgb(47, 0, 79)',
        lightblue: 'rgb(173, 216, 230)',
        lightcyan: 'rgb(224, 255, 255)',
        lightgreen: 'rgb(144, 238, 144)',
        lightgrey: 'rgb(211, 211, 211)',
        lightpink: 'rgb(255, 182, 193)',
        lightyellow: 'rgb(255, 255, 224)',
        lime: 'rgb(0, 255, 0)',
        magenta: 'rgb(255, 0, 255)',
        maroon: 'rgb(128, 0, 0)',
        navy: 'rgb(0, 0, 128)',
        olive: 'rgb(128, 128, 0)',
        orange: 'rgb(255, 165, 0)',
        pink: 'rgb(255, 192, 203)',
        purple: 'rgb(128, 0, 128)',
        violet: 'rgb(128, 0, 128)',
        red: 'rgb(255, 0, 0)',
        silver: 'rgb(192, 192, 192)',
        white: 'rgb(255, 255, 255)',
        yellow: 'rgb(255, 255, 0)',
        transparent: 'rgb(255, 255, 255)'
    }

    var methods = {
        init: function (params) {
            const defaults = $.fn.colorPalettePicker.defaults;
            if (params.bootstrap == 3) {
                $(this).addClass('dropdown');
                defaults.buttonClass = 'btn btn-default dropdown-toggle';
                defaults.button = '<button id="colorpaletterbuttonid" name="colorpalettebutton" class="{buttonClass} btn-primary btn-block" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true"><span name="{buttonPreviewName}" style="display:none">■ </span>{buttonText} <span class="caret"></span></button>';
                defaults.dropdown = '<ul class="dropdown-menu" aria-labelledby="colorpaletterbuttonid"><h5 class="dropdown-header text-center">{dropdownTitle}</h5>';
                defaults.menu = '<ul class="list-inline" style="padding-left:10px;padding-right:10px">';
                defaults.item = '<li><div name="picker_{name}" style="background-color:{color};width:32px;height:32px;border-radius:5px;border: 1px solid #666;margin: 0px;cursor:pointer" data-toggle="tooltip" title="{name}" data-color="{color}"></div></li>';
            }
            const options = $.extend({}, defaults, params);

            // button configuration
            const btn = $(options.button
                .replace('{buttonText}', options.buttonText)
                .replace('{buttonPreviewName}', options.buttonPreviewName)
                .replace('{buttonClass}', options.buttonClass));
            $(this).html(btn);
            // dropdown configuration
            const dropdown = $(options.dropdown.replace('{dropdownTitle}', options.dropdownTitle));
            // check if colors passed throught data-colors
            const dataColors = $(this).attr('data-colors');
            if (dataColors != undefined) {
                options.palette = dataColors.split(',');
            }
            // check if lines passed throught data-lines
            const dataLines = $(this).attr('data-lines');
            if (dataLines != undefined)
                options.lines = dataLines;
            // calculating items per line
            const paletteLength = options.palette.length;
            const itemsPerLine = Math.round(paletteLength / options.lines);
            let paletteIndex = 0;
            for (let i = 0; i < options.lines; i++) {
                const menu = $(options.menu);

                for (let j = 0; j < itemsPerLine; j++) {
                    const paletteObjItem = paletteObj[options.palette[paletteIndex]];
                    if (paletteObjItem != undefined) {
                        menu.append(options.item.replace(/{name}/gi, options.palette[paletteIndex]).replace(/{color}/gi, paletteObjItem));
                    }
                    paletteIndex++;
                }
                dropdown.append(menu);
            }
            $(this).append(dropdown);
            // item click bindings
            $(this).find('div[name^=picker_]').on('click',
                function () {
                    const selectedColor = $(this).attr('data-color');
                    const colorSquare = $('span[name=' + options.buttonPreviewName + ']');
                    colorSquare.css('color', selectedColor);
                    if (!colorSquare.is(':visible'))
                        colorSquare.show();
                    if (typeof options.onSelected === 'function') {
                        options.onSelected(selectedColor);
                    }
                });
        }
    }

    $.fn.colorPalettePicker = function (options) {
        if (methods[options]) {
            return methods[options].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof options === 'object' || !options) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Option ' + options + ' not found in colorPalettePicker');
        }
    };

    $.fn.colorPalettePicker.defaults = {
        button: '<button name="colorpalettebutton" class="{buttonClass} btn-primary btn-block" data-toggle="dropdown"><span name="{buttonPreviewName}" style="display:none">■ </span>{buttonText}</button>',
        buttonClass: 'btn btn-secondary dropdown-toggle',
        buttonPreviewName: 'colorpaletteselected',
        buttonText: 'Selecciona un color',
        dropdown: '<div class="dropdown-menu"><h5 class="dropdown-header text-center">{dropdownTitle}</h5>',
        dropdownTitle: 'Colores disponibles',
        menu: '<ul class="list-inline" style="padding-left:10px;padding-right:10px">',
        item: '<li class="list-inline-item"><div name="picker_{name}" style="background-color:{color};width:32px;height:32px;border-radius:5px;border: 1px solid #666;margin: 0px;cursor:pointer" data-toggle="tooltip" title="{name}" data-color="{color}"></div></li>',
        palette: ['yellow', 'errieblack', 'yankeeblue', 'black', 'blue', 'green', 'shinyshamrock', 'palelavander', 'grey', 'lightseagreen', 'orange', 'red', 'whiteguy', 'marron', 'celeste', 'naranjofuerte', 'moonstoneblue', 'grannysmithapple', 'verdeoscuro', 'desertsand', 'gold', 'pastelblue', 'indigoweb', 'deepviolet', 'orange', 'pink', 'silver', 'white'],
        lines: 1,
        bootstrap: 4,
        onSelected: null
    };
})(jQuery);