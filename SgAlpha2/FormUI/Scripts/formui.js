﻿var formui = {};

(function () {

    formui.init = init;

    // from:  http://stackoverflow.com/questions/484463/how-do-i-maintain-scroll-position-in-mvc/16209536#16209536
    $(document).scroll(function () {
        localStorage['page'] = document.URL;
        localStorage['scrollTop'] = $(document).scrollTop();
    });

    if (localStorage['page'] == document.URL) {
        $(document).scrollTop(localStorage['scrollTop']);
    }

    $(function () {
        init($(document));
    });

    function init(rootElement) {

        var niInput = $('[name=NationalInsuranceNumber');

        niInput.inputmask({
            mask: "aa 99 99 99 a",
            jitMasking: true,
            casing: 'upper'
        });

    }

}());
