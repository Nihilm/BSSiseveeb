var $birthdayElement = $("#Birthday");
var datepickers = ["#dp1", "#dp2"];
var date = new Date();
var now = new Date(date.getFullYear(), date.getMonth(), date.getDate(), 0, 0, 0, 0);


$(document).ready(function () {
    $birthdayElement.birthdayPicker({
        "dateFormat": "littleEndian"
    });

    $.each(datepickers, function (index, picker) {
        var checkin = $(picker).datepicker().on('changeDate', function (ev) {
            if (ev.date.valueOf() > checkout.date.valueOf()) {
                var newDate = new Date(ev.date)
                newDate.setDate(newDate.getDate() + 1);
                checkout.setValue(newDate);
            }
            checkin.hide();
            $(picker)[0].focus();
        }).data('datepicker');
        var checkout = $(picker).datepicker().on('changeDate', function (ev) {
            checkout.hide();
        }).data('datepicker');

        $(picker).datepicker()
            .on('changeDate', function (ev) {
                $(picker).date = ev.date;
            });
    });
});

function showDatePicker(dp) {
    $(dp).datepicker('show');
}


