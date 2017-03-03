﻿var formui = {};

(function () {

    formui.init = init;

    // from:  http://stackoverflow.com/questions/484463/how-do-i-maintain-scroll-position-in-mvc/16209536#16209536
    $(document).scroll(function () {
        localStorage['page'] = document.URL;
        localStorage['scrollTop'] = $(document).scrollTop();
    });

    if (localStorage['page'] === document.URL && $('.validation-summary-errors').length === 0) {
        $(document).scrollTop(localStorage['scrollTop']);
    }

    $(function () {
        init($(document));
    });

    var namedMasks = {

        NationalInsuranceNumber: {
            mask: 'aa 99 99 99 a',
            jitMasking: true,
            casing: 'upper'
        },

        SortCode: {
            mask: '99-99-99',
            jitMasking: true,
        },

        AccountNumber: {
            mask: '9999999999',
            jitMasking: true,
        },

    };

    function init(rootElement) {

        var isMobile = true;

        // this simple check is enough for Alpha2
        if (window.orientation === null)
            isMobile = false;

        if (!isMobile) { // the input mask doesn't work with a mobile's virtual keyboard
            rootElement.find('[data-input-mask]').each(function (index, e) {
                var input = $(e)
                var maskName = input.attr('data-input-mask');
                var mask = namedMasks[maskName];

                if (mask === null)
                    alert('Cannot find named mask: ' + maskName);

                input.inputmask(mask);
            });
        }

        rootElement.find('[data-ajax-change]').each(function (index, e) {
            var element = $(e);
            element.find('input').change(function (evt) {
                var action = element.attr('data-ajax-change');
                alert(action);
            });
        });

    }

}());
