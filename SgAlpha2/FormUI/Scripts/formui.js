var formui = {};

(function () {

    formui.init = init;

    // from:  http://stackoverflow.com/questions/484463/how-do-i-maintain-scroll-position-in-mvc/16209536#16209536
    $(document).scroll(function () {
        localStorage['page'] = document.URL;
        localStorage['scrollTop'] = $(document).scrollTop();
    });

    if (localStorage['page'] == document.URL && $('.validation-summary-errors').length == 0) {
        $(document).scrollTop(localStorage['scrollTop']);
    }

    $(function () {
        init($(document));
    });

    function init(rootElement) {

        var niInput = $('[name=NationalInsuranceNumber]');

        niInput.inputmask({
            mask: 'aa 99 99 99 a',
            jitMasking: true,
            casing: 'upper'
        });

        var baInput = $('[name=AccountNumber]');

        baInput.inputmask({
            mask: '9999999999',
            jitMasking: true,
        });

        var scInput = $('[name=SortCode]');

        scInput.inputmask({
            mask: '99-99-99',
            jitMasking: true,
        });

    }

}());
