

// Calendar code
    function PickerOptions(field, container, settings, theme) {
        this.field = field;
        this.container = container;
        this.format = 'DD/MM/YYYY';
        this.maxDate = settings.maxDate;
        this.minDate = settings.minDate;
        this.bound = false;
        // this.yearRange = [1980, settings.maxDate.getFullYear()];
        this.theme = theme;
        this.onDraw = function() {
            $(this.el).find('select > option:disabled').prop('disabled', false);
            $(this.el).find('button, a, select').attr('tabindex', '-1');
        };

        this.onSelect = function () {

            // dataLayer.push({
            //     'field': this.name,
            //     'interaction': 'select',
            //     'event': 'date-picker'
            // });

            $(this._o.container).removeClass('date-entry__calendar--open');
            $(this._o.field).trigger('blur');
            }
        };
    

var testSettings = { maxDate: '2019-01-20', minDate: '2008-01-01'}
var toPickerOptions = new PickerOptions(document.getElementById('registration-date'), $('#registration-date').closest('.date-entry').find('.date-entry__calendar')[0], testSettings, 'tst-date-to'); //NOSONAR

var dateToPicker = new Pikaday(toPickerOptions);

$('.js-show-calendar').on('click', function () {
    var calendar = $($(this).closest('.date-entry').find('.date-entry__calendar').get(0));

            // dataLayer.push({
            //     'field': calendar.parent().find('label').text().toLowerCase().replace(' ', '-'),
            //     'interaction': calendar.hasClass('date-entry__calendar--open') ? 'close': 'open',
            //     'event': 'date-picker'
            // });

            // close any other open calendars
    $(this).closest('.date-entry').siblings().find('.date-entry__calendar').removeClass('date-entry__calendar--open');
    calendar.toggleClass('date-entry__calendar--open');
});

$('#registration-time').on('focus', function () {
    var calendar = $('.date-entry').find('.date-entry__calendar').get(0);
    $(calendar).removeClass('date-entry__calendar--open');
});

$('#registration-date').on('blur', function () {
    var calendar = $($(this).closest('.date-entry').find('.date-entry__calendar').get(0));
    calendar.removeClass('date-entry__calendar--open');
});

