var formui = {};

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

    // http://stackoverflow.com/a/21742107/357728
    function isMobileDevice() {
        var userAgent = navigator.userAgent || navigator.vendor || window.opera;

        // Windows Phone must come first because its UA also contains "Android"
        if (/windows phone/i.test(userAgent))
            return true;

        if (/android/i.test(userAgent))
            return true;

        // iOS detection from: http://stackoverflow.com/a/9039885/177710
        if (/iPad|iPhone|iPod/.test(userAgent) && !window.MSStream)
            return true;

        return false;
    }

    function init(rootElement) {

        if (!isMobileDevice()) { // the input mask doesn't work with a mobile's virtual keyboard
            rootElement.find('[data-input-mask]').each(function (index, e) {
                var input = $(e)
                var maskName = input.attr('data-input-mask');
                var mask = namedMasks[maskName];

                if (mask === null)
                    alert('Cannot find named mask: ' + maskName);

                input.inputmask(mask);
            });
        }

        rootElement.on('change', '[type=checkbox]', function (e) {
            var cb = $(this);

            if (cb.is(':checked')) {
                rootElement.find('[data-checkbox-checked-show]').show();
                rootElement.find('[data-checkbox-checked-hide]').hide();
            } else {
                rootElement.find('[data-checkbox-checked-show]').hide();
                rootElement.find('[data-checkbox-checked-hide]').show();
            }
        });

        rootElement.find('[data-ajax-change]').each(function (index, e) {

            var element = $(e);
            var inputs = element.find('input');

            inputs.change(function (evt) {
                var actionUrl = element.attr('data-ajax-change');
                var form = element.closest('form');
                var formData = form.serialize();
                
                $.ajax({
                    type: 'POST',
                    url: actionUrl,
                    data: formData,
                    success: function (data, textStatus, jqXHR) {

                        var actions = data;

                        for (var i = 0; i < actions.length; i++) {
                            var action = actions[i];

                            switch (action.Action) {

                                case 'ShowHide':
                                    ShowHide(action.TargetId, action.Show);
                                    break;

                                default: alert('Unhandled action: ' + action.Action);
                            }
                        }

                    }
                });
            });

            if (inputs.length > 0)
                $(inputs[0]).trigger('change');

        });

        function ShowHide(targetId, show) {
            var target = $('#' + targetId);

            if (show)
                target.show('fast');
            else
                target.hide('fast');
        }

    }

}());
