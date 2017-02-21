var submitted = false;

var $clientError = $('.client-error');
var $formErrors = $('.form-errors');

var $registrationDate = $('#registration-date');
var $registrationTime = $('#registration-time');
var $name = $('#name');
var $birthdate = $('#birthdate');
var $email = $('#email');
var $telephoneNumber = $('#telephone-number');
var $address1 = $('#address-1');
var $postcode = $('#postcode');
var $smartphone = $('#smartphone-radio');
var $formDifficulty = $('#form-difficulty');
var $timeOnline = $('#time-online');
var $comments = $('#comments');
var $dateConfirmation = $('#session-date');
var $consent = $('#consent-checkbox');

/**
 * Checks if $field is completed. Appends or Removes an error message.
 *
 * @param {object} $field - the form field to be checked
 * @returns {boolean} whether the field value is valid
 */
var requiredField = function($field, optionalMessage) {
    var trimmedValue = $.trim($field.val());
    var label = $('label[for="' + $field.attr('id') + '"]');
    var message = optionalMessage || 'Please answer this question';

    var valid = trimmedValue !== '';

    addOrRemoveFormErrors($field, valid, 'required', label.text(), message);
    showOrHideCurrentErrors($field, valid, message);

    return valid;
};

//  Required dropdown

var requiredDateDropdown = function($field) {
	var dropdowns = $field.find('select');
    var label = $('label[for="' + $field.attr('id') + '"]');
    var message = 'Please enter a date';

	var valid = _.every(dropdowns, function(element){
		return element.value !== '';
	});

    addOrRemoveFormErrors($field, valid, 'required', label.text(), message);
    showOrHideCurrentErrors($field, valid, message);

    return valid;
}

// required checkbox

var requiredRadio = function($field){
    var radioButtons = $field.find('input:radio');
    var title = $field.find('legend').text();
    var message = 'Please select one of the options'

    var valid = false;

    for (var i = 0; i < radioButtons.length; i++) {
        if ($(radioButtons[i]).is(':checked')) {
            valid = true;
        } 
    }

    addOrRemoveFormErrors($field, valid, 'required', title, message);
    showOrHideCurrentErrors($field, valid, message);

    return valid;
}

// required checkbox

var requiredCheckbox = function($field){
    var valid = $field.prop('checked');

    addOrRemoveFormErrors($field, valid, 'consent-checkbox', 'Consent', 'You can only submit this form if you accept the consent statement.');
    showOrHideCurrentErrors($field, valid, 'You can only submit this form if you accept the consent statement.');

    return valid;
}


// required Time Spent boxes 

var requiredTimeSpent = function($field){
    var boxes = $field.find('input:text');
    var title = $field.find('legend').text();

    var valid = false;

    for (var i = 0; i < boxes.length; i++) {
        $(boxes[i]).next().next('.field-errors').remove();
        $(boxes[i]).css('border', '1px solid #727272')


        if($(boxes[i]).val() !== ''){
            valid = true;
        }
    }

    addOrRemoveFormErrors($field, valid, 'required', title, 'Please enter a number in at least one box');
    showOrHideCurrentErrors($field, valid, 'Please enter a number in at least one box.')

    return valid;
}


// validate Time Spent boxes - this is doing two things so could do with a refactor

var validateTimeSpent = function($field){
    var boxes = $field.find('input:text');
    var title = $field.find('legend').text();
    var message = 'Please give answers in under 24 hours and only enter numbers.'

    var valid = true;
    var total = 0;

    for (var i = 0; i < boxes.length; i++) {
        var inputAsInteger = parseFloat($(boxes[i]).val());

        if ( !inputAsInteger && $(boxes[i]).val() !== '' && $(boxes[i]).val() !== '0'){
            if($(boxes[i]).next().next('.field-errors').length === 0){
                $(boxes[i]).next().after('<p class="field-errors">Please only enter numbers.</p>');
                $(boxes[i]).css('border', '2px solid #d32205')
            } 
            valid = false;  
        } else if (inputAsInteger || $(boxes[i]).val() === '' || $(boxes[i]).val() === '0') {
            $(boxes[i]).next().next('.field-errors').remove();
            $(boxes[i]).css('border', '1px solid #727272')
        } 

        if (inputAsInteger) {
            total = total + inputAsInteger; 
        }
    }

    if (total > 24) {
        valid = false;
        showOrHideCurrentErrors($field, valid, 'Please give answers in under 24 hours.')
    }
    
    addOrRemoveFormErrors($field, valid, 'required', title, message);

    return valid;
}

// validate date dropdown to make sure it's a real date

var validDateFromDropdown = function($field){
    var dropdowns = $field.find('select');
    var fieldName = $('label[for="' + $field.attr('id') + '"]').text();
    var message = 'Please check you\'ve entered the correct date';

    var day = dropdowns[0].value;
    var month = dropdowns[1].value;
    var year = dropdowns[2].value;

    if (!day || !month || !year) {
        return true;
    }

    var valid = isValidDate(day, month, year);

    if (!valid) {
        $(dropdowns[0]).addClass('input-error');
    } else {
        $(dropdowns[0]).removeClass('input-error');
    }

    addOrRemoveFormErrors($field, valid, 'invalid-date', fieldName, message);
    showOrHideCurrentErrors($field, valid, message);

    return valid;
}

// valid date finder

var isValidDate = function(day, month, year){
    // solution by Matti Virkkunen: http://stackoverflow.com/a/4881951
    var isLeapYear = year % 4 === 0 && year % 100 !== 0 || year % 400 === 0;
    var daysInMonth = [31, isLeapYear ? 29 : 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][month - 1];

    var valid = day <= daysInMonth;

    return valid;
}

// participant old enough


var minimumAge = function($field, minimumAge){
    var dropdowns = $field.find('select');
    var fieldName = $('label[for="' + $field.attr('id') + '"]').text();
    var day = dropdowns[0].value;
    var month = dropdowns[1].value;
    var year = dropdowns[2].value;

    if (!day || !month || !year) {
        return true;
    }

    var birthdate = new Date('' + year + '-' +  month + '-' + day);
    var today = new Date();
    var age = today.getFullYear() - birthdate.getFullYear();
    var m = today.getMonth() - birthdate.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birthdate.getDate())) {
        age--;
    }
    
    var valid = age >= minimumAge;

    if (!valid) {
        $(dropdowns[2]).addClass('input-error');
    } else {
        $(dropdowns[2]).removeClass('input-error');
    }

    addOrRemoveFormErrors($field, valid, 'minimum-age', fieldName, 'Participants must be aged ' + minimumAge + ' or older.');
    showOrHideCurrentErrors($field, valid, 'Sorry, you must be aged ' + minimumAge + ' or over to use this service - please check you\'ve entered the correct date of birth.');

    return valid;
}


// Time in 24 hours

var time24Hours = function($field){
    var trimmedValue = $.trim($field.val());
    var fieldName = $('label[for="' + $field.attr('id') + '"]').text();
    var message = 'Please enter the time using the 24 hour format, for example 14.30. Times should be after 09.00 and before 17.00';

    // A regular expression only allowing times in 24 hours from 09.00 (or 9.00) until 17.00
    var regex = new RegExp("^(9|09|1[0-6]).\\d\\d$");

    var valid = trimmedValue.match(regex) !== null;

    addOrRemoveFormErrors($field, valid, 'invalid-time-24', fieldName, message);
    showOrHideCurrentErrors($field, valid, message);

    return valid;
}

// regex for emails 

var validateEmail = function ($field) {
    var trimmedValue = $.trim($field.val());
    var message = 'Please enter a valid email address, for example joe@gov.scot';

    var regex = /^[^@ ]+@[^@ ]+\.[^@ ]+$/

    var valid = trimmedValue.match(regex) !== null;

    addOrRemoveFormErrors($field, valid, 'invalid-email', 'Email address', message);
    showOrHideCurrentErrors($field, valid, message);

    return valid;
};

var isGovScotEmail = function($field) {
    var trimmedValue = $.trim($field.val());
    var message = 'Please enter a valid \'@gov.scot\' email address, for example joe@gov.scot';

    var regex = new RegExp('^[a-zA-Z0-9_.+-]+@gov.scot$');

    var valid = trimmedValue.match(regex) !== null;

    addOrRemoveFormErrors($field, valid, 'invalid-email', 'Email address', message);
    handleServerSideErrors($field, valid, message);

    return valid;
}

// time in dd/mm/yyyy

var dateRegex = function($field){
    var trimmedValue = $.trim($field.val());
    var fieldName = $('label[for="' + $field.attr('id') + '"]').text();
    var message = 'Please check you\'ve entered a correct date';

    // A regular expression only allowing dd/mm/yyyy format
    var regex = new RegExp('^\\d\\d\\/\\d\\d\\/\\d\\d\\d\\d$');

    var valid = trimmedValue.match(regex) !== null;

    if (!valid) {
        addOrRemoveFormErrors($field, valid, 'invalid-date-format', fieldName, 'Please enter the date as DD/MM/YYYY');
        showOrHideCurrentErrors($field, valid, 'Please enter the date as DD/MM/YYYY');
        return valid;
    }

    var day = parseInt(trimmedValue.slice(0, 2));
    var month = parseInt(trimmedValue.slice(3, 5));
    var year = parseInt(trimmedValue.slice(6, 10));

    var valid = isValidDate(day, month, year);

    addOrRemoveFormErrors($field, valid, 'invalid-date-format', fieldName, message);
    showOrHideCurrentErrors($field, valid, message);

    return valid;
}

// check date is today's date

var todaysDate = function($field){
    var trimmedValue = $.trim($field.val());
    var fieldName = $('label[for="' + $field.attr('id') + '"]').text();
    var message = 'Your session date must be today\'s date';

    var day = parseInt(trimmedValue.slice(0, 2));
    var month = parseInt(trimmedValue.slice(3, 5));
    var year = parseInt(trimmedValue.slice(6, 10));

    var today = new Date();

    var valid = (today.getFullYear() === year) && (today.getMonth() === month - 1) && (today.getDate() === day);

    addOrRemoveFormErrors($field, valid, 'not-todays-date', fieldName, message);
    showOrHideCurrentErrors($field, valid, message);

    return valid;
}

// check two date inputs are matching

var matchesField = function($field, $compareField){
    var trimmedValue = $.trim($field.val());
    var compareValue = $.trim($compareField.val());
    var fieldName = $('label[for="' + $field.attr('id') + '"]').text();
    var compareFieldName = $('label[for="' + $compareField.attr('id') + '"]').text();

    var valid = trimmedValue === compareValue;

    addOrRemoveFormErrors($field, valid, 'must-match-field', fieldName, 'This must match the value of \'' + compareFieldName + '\'.')
    showOrHideCurrentErrors($field, valid, 'This must match the value of \'' + compareFieldName + '\'.');

    return valid;
}

// valid postcode 

var validatePostcode = function($field){
    var message = 'Please enter a valid postcode, for example EH6 6QQ'

    var postcodeRegExp = new RegExp('^([A-PR-UWYZ0-9][A-HK-Y0-9][AEHMNPRTVXY0-9]?[ABEHMNPRVWXY0-9]? {0,2}[0-9][ABD-HJLN-UW-Z]{2}|GIR ?0AA)$');
    // var scottishPostcodeRegExp = new RegExp('^(AB|DD|DG|EH|FK|G|HS|IV|KA|KW|KY|ML|PA|PH|TD|ZE)[0-9]{1,2} {0,2}[0-9][ABD-HJLN-UW-Z]{2}$');

    var postcodeValue = $field.val().toUpperCase().replace(/\s+/g, '');

    var valid = postcodeValue.match(postcodeRegExp) !== null;

    addOrRemoveFormErrors($field, valid, 'invalid-postcode', 'Postcode', message);
    showOrHideCurrentErrors($field, valid, message);

    return valid;
}

// valid phone number 

var validatePhone = function($field){
    var trimmedValue = $.trim($field.val()).replace(/\s+/g, '');

    // phone number is not required
    if (trimmedValue === ''){
        return true;
    }

    // A regular expression matching only up to 20 numbers and possibly a '+' character at the beginning
    var regex = new RegExp('^\\+?[0-9]{0,20}$');

    var valid = trimmedValue.match(regex) !== null;

    addOrRemoveFormErrors($field, valid, 'invalid-phone', 'Phone number', 'Phone numbers should only contain numbers and the "+" character and be under 20 characters.');
    showOrHideCurrentErrors($field, valid, 'Use only numbers 0-9 and the "+" character. Phone numbers should be less than 20 characters long.');

    return valid;
}

/**
 * Checks whether if $field is less than or equal to maxLength.
 * Appends or Removes an error message.
 * Updates character count
 *
 * @param {object} $field - the form field to be checked
 * @param {int} maxLength - the permitted character length of $field
 * @returns {boolean} whether the field value is valid
 */
var maxCharacters = function($field, maxLength) {
    var valid = $field.val().length <= maxLength;
    var className = '.' + $field.attr('id') +'-errors';
    var label = $('label[for="' + $field.attr('id') + '"]');

    var message = 'Please answer in under ' + maxLength + ' characters. A character is a letter, number or symbol and includes spaces.';

    $field.next().find('span').html((maxLength - $field.val().length));

    if (!valid) {
        $field.next().find('span').css('color', '#d32205');
    } else {
        $field.next().find('span').css('color', 'grey');
    }

    addOrRemoveFormErrors($field, valid, 'too-many', label.text(), message);
    showOrHideCurrentErrors($field, valid, message);

    return valid;
};

/**
 * Appends an error container for $field to the DOM
 *
 * @param {object} $field - the form field to log errors against
 */
var appendErrorContainer = function($field) {
    var className = $field.attr('id') + '-errors';
    if($('.' + className).length === 0) {
        $formErrors.append('<ul class="' + className + '"></ul>');
    }
};

/**
 * Removes the error container for $field from the DOM
 *
 * @param {object} $field - the form field errors were logged against
 */
var removeErrorContainer = function($field) {
    var className = $field.attr('id') + '-errors';
    if($('.' + className).length !== 0) {
        $('.' + className).remove();
    }
};

/**
 * Validates $field
 *
 * @param {object} $field - the form field to validate
 * @param {boolean} highlightField - should a visual indicator be applied to the UI
 * @param {arguments} - [2 ..] list of validation functions to test $field against
 * @returns {boolean} whether the field value is valid
 */
var validateField = function($field, highlightField) {

    appendErrorContainer($field);

    var validationChecks = [];

    for(var i = 2; i < arguments.length; i++) {
        validationChecks.push(arguments[i]);
    }

    var valid = true;

    for (var i = 0; i < validationChecks.length; i++) {
        if (validationChecks[i]($field) === false) {
            valid = false;
            break;
        }
    }

    var $label = $('label[for="' + $field.attr('id') + '"]');

    if (highlightField) {
        if (!valid) {
            $field.addClass('input-error');
        } else {
            $field.removeClass('input-error');
        }
    }

    // if (valid) {
    //     removeErrorContainer($field);
    // }

    return valid;
};


/**
 * Adds or removes error messages to main error container (at top of page)
 */

 var addOrRemoveFormErrors = function (field, valid, errorClass, fieldName, message) {
    var className = '.' + field.attr('id') +'-errors';

    if (!valid) {
        if($(className + ' .' + errorClass).length === 0) {
            $(className).append('<li class="error ' + errorClass + '">'
                 + '<span>' + fieldName + ': </span><a class="form-nav" href="#' + field.attr('id') + '-link">' +  message + '</a></li>');
           
            // Set on click property of the link to focus on the input it's related to if possible
            $(className).find('.form-nav').on('click', function(e){
                if (field.prop('tagName') === 'INPUT') {
                    e.preventDefault()
                    field.focus();
                }
            })
        }
    } else {
        if ($('.form-message--error').hasClass('hidden')) {
            $(className + ' .' + errorClass).remove();            
        } else {
            $(className + ' .' + errorClass).addClass('error-grey');
        }

    }
 }

/**
 *  Shows or hides individual field's current errors box (box below each field)
 */

var showOrHideCurrentErrors = function (field, valid, message) {
    var errorContainer;

    if (field.find('.current-errors').length !== 0){
        errorContainer = field.find('.current-errors');
    } else {
        errorContainer = field.siblings('.current-errors');
    }

    if (!valid) {
        errorContainer.html('<span class="fa-circle"></span>' + message);
    } else {
        errorContainer.html('');
    }
}

// Server side error handling

var handleServerSideErrors = function (field, valid, message) {
    var errorContainer;

    if (field.find('.server-side-errors').length !== 0){
        errorContainer = field.find('.server-side-errors');
    } else {
        errorContainer = field.siblings('.server-side-errors');
    }

    if (!valid) {
        errorContainer.html('<span class="fa-circle"></span>' + message);
        errorContainer.css('color', '#d32205');
    } else {
        errorContainer.html('');
    }

    field.on('keydown', function(){
        errorContainer.css('color', '#727272');
        field.css('border', '1px solid #727272');
    });
}

var registrationForm = {};

/**
 * Checks all form field to determine if form is valid
 * Hightlights UI if highlightField
 *
 * @returns {boolean} is the form valid
 */
var formIsValid = function (highlightField) {
    $formErrors.html('');

    var registrationDate = validateField($registrationDate, highlightField, 
        _.partialRight(requiredField, 'Please enter a date, for example 06/02/2017'), dateRegex, todaysDate);
    var registrationTime = validateField($registrationTime, highlightField,
        _.partialRight(requiredField, 'Please enter a time, for example 12.43'), time24Hours);
    var name = validateField($name, highlightField, _.partialRight(requiredField, 'Please enter your name'),
        _.partialRight(maxCharacters, 300));
    var birthdate = validateField($birthdate, highlightField, requiredDateDropdown, 
        validDateFromDropdown, _.partialRight(minimumAge, 18));
    var email = validateField($email, highlightField, _.partialRight(requiredField, 'Please enter an email address, for example joe@gov.scot'), 
        validateEmail, _.partialRight(maxCharacters, 300), isGovScotEmail);
    var telephoneNumber = validateField($telephoneNumber, highlightField, validatePhone);
    var address1 = validateField($address1, highlightField, _.partialRight(requiredField, 'Please enter your address'), 
        _.partialRight(maxCharacters, 300));
    var postcode = validateField($postcode, highlightField, 
        _.partialRight(requiredField, 'Please enter your postcode, for example EH6 6QQ'), validatePostcode);
    var smartphone = validateField($smartphone, highlightField, requiredRadio);
    var timeOnline = validateField($timeOnline, highlightField, requiredTimeSpent, validateTimeSpent);
    var formDifficulty = validateField($formDifficulty, highlightField, requiredRadio);
    var comments = validateField($comments, highlightField, _.partialRight(maxCharacters, 700));
    var dateConfirmation = validateField($dateConfirmation, highlightField, _.partialRight(requiredField, 'Please enter a date'),
        dateRegex, todaysDate, _.partialRight(matchesField, $registrationDate));
    var consent = validateField($consent, highlightField, requiredCheckbox);
    // var recaptcha = validateField($recaptcha, highlightField, recaptchaCompleted);

    return registrationTime && name && birthdate && email && telephoneNumber && address1 && postcode && smartphone && formDifficulty && timeOnline && comments && dateConfirmation && consent;//NOSONAR
};
/**
 * Module entry point
 */
registrationForm.init = function () {

    // textchange is a shim to allow us to use the input event in IE8 & IE9

    $registrationDate.on('blur', function() {
        validateField($registrationDate, true, 
        _.partialRight(requiredField, 'Please enter a date, for example 06/02/2017'), dateRegex, todaysDate);

        // Remove error from date confirmation if user has gone back to change first date entry
        if ($dateConfirmation.val() !== '' ) {
            validateField($dateConfirmation, true, todaysDate, _.partialRight(matchesField, $registrationDate));
        }
    });

    $registrationTime.on('blur', function() {
        validateField($registrationTime, true,
        _.partialRight(requiredField, 'Please enter a time, for example 12.43'), time24Hours);
    });

    $name.on('blur', function() {
        validateField($name, true, _.partialRight(requiredField, 'Please enter your name'),
        _.partialRight(maxCharacters, 300));
    });

    $('#birthdate-day, #birthdate-month, #birthdate-year').on('change', function() {
        validateField($birthdate, true, validDateFromDropdown, _.partialRight(minimumAge, 18));
    });

    $email.on('focus', function(){
        validateField($birthdate, true, requiredDateDropdown, validDateFromDropdown, _.partialRight(minimumAge, 18));
    });

    $email.on('blur', function(){
        validateField($email, true, _.partialRight(requiredField, 'Please enter an email address, for example joe@gov.scot'), 
        validateEmail, _.partialRight(maxCharacters, 300));
    });

    $telephoneNumber.on('blur', function() {
    	validateField($telephoneNumber, true, validatePhone);
    });

    $address1.on('blur', function() {
        validateField($address1, true, _.partialRight(requiredField, 'Please enter your address'), 
        _.partialRight(maxCharacters, 300));
    });

    $postcode.on('blur', function(){
        validateField($postcode, true, 
        _.partialRight(requiredField, 'Please enter your postcode, for example EH6 6QQ'), validatePostcode);

        $postcode.val($postcode.val().toUpperCase());
    });

    $('#smartphone-radio input:radio').on('blur change', function(){
        validateField($smartphone, true, requiredRadio);
    });

    $('#time-online input:text').on('keyup', function() {
        validateField($timeOnline, true, requiredTimeSpent, validateTimeSpent);
    });

    $('#form-difficulty input:radio').on('blur change', function(){
        validateField($formDifficulty, true, requiredRadio);
    });

   	$comments.on('textchange keyup blur', function(){
   		validateField($comments, true, _.partialRight(maxCharacters, 700));
   	});

    $dateConfirmation.on('blur', function(){
        validateField($dateConfirmation, true, _.partialRight(requiredField, 'Please enter a date'),
        dateRegex, todaysDate, _.partialRight(matchesField, $registrationDate));
    })

    $consent.on('change', function(){
        validateField($consent, true, requiredCheckbox);
    });

    $('form#registration-form').on('submit', function(e) {
        submitted = true;
        e.preventDefault();

        if(!formIsValid(true)) {
            $clientError.removeClass('hidden');

            //Click event to scroll to top
            var feedbackTop = $('#feedback-box').position().top;
            $('html, body').animate({scrollTop : feedbackTop},1000);

            return;
        } else {
            window.location.href = ('complete.html');
        }

        // do not get the data and post it anywhere - this is a prototype.
    });
};

window.format = registrationForm;


// Prevent form from being submitted if 'return' key is pressed, unless focus is on submit button. 
$('html').bind('keypress', function(e){
    var submitButton = document.getElementById('form-submit');

    if(e.keyCode == 13 && document.activeElement !== submitButton && document.activeElement.tagName !== 'TEXTAREA'){
      return false;
    }
});


// This would normally be included in main site's JS so could be removed for real version.
var expandable = {
    init: function () {

        // init all data-gtm to being closed
        $('.panel a.panel-heading').each(function () {
            $(this).attr('data-gtm', 'panel-closed');
        });

        //Handle changing colours on panel headers when open
        $('.expandable-item').on('click', '.js-toggle-expand', function(event) {
            event.preventDefault();

            var expandableItem = $(this).closest('.expandable-item'),
                expandableItemBody = expandableItem.find('.expandable-item__body');

            if (expandableItem.hasClass('expandable-item--open')) {
                expandableItemBody.attr('aria-expanded', "false");
                expandableItem.removeClass('expandable-item--open');
                expandableItemBody.slideUp(200, function () {
                    // hacky IE8 fix to force redraw of changed inline-block element
                    expandableItem.closest('.grid__item').toggleClass('foo');
                    expandableItem.closest('.grid__item').toggleClass('foo');
                });
                $(this).attr('data-gtm', 'panel-closed');
            } else {
                expandableItemBody.attr('aria-expanded', "true");
                expandableItem.addClass('expandable-item--open');
                expandableItemBody.slideDown(200, function () {
                    // hacky IE8 fix to force redraw of changed inline-block element
                    expandableItem.closest('.grid__item').toggleClass('foo');
                    expandableItem.closest('.grid__item').toggleClass('foo');
                });
                $(this).attr('data-gtm', 'panel-opened');
            }
        });
    }
};

// self-initialize
expandable.init();