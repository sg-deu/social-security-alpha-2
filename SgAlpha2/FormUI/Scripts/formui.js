var formui = {};

(function () {

    formui.init = init;

    init($(document));

    function init(rootElement) {

        var niInput = $('[name=NationalInsuranceNumber');

        niInput.inputmask({
            mask: "aa 99 99 99 a",
            jitMasking: true,
            casing: 'upper'
        });

    }

}());
