﻿var date = new Date();
var now = new Date(date.getFullYear(), date.getMonth(), date.getDate(), 0, 0, 0, 0);
var months = ["Jaanuar", "Veebruar", "Märts", "Aprill", "Mai", "Juuni", "Juuli", "August", "September", "Oktoober", "November", "Detsember"];
var datepickers = ["#dp1", "#dp2"];
var $month = $("#month");
var $mainRow = $("#mainRow");
var $calendarBody = $("#calendarBody");
var $comment = $("#comment");
var $myVacations = $("#myVacationsBody");
var $remainingDays = $("#remainingDays");
var employees;

$(document).ready(function () {

    $.get(app.root + 'API/Calendar/GetEmployees').done(function (data) {
        employees = data;
        drawCalendar();
        getCalendar();
    });
    updateHeader();
    drawMyVacations();
    initDatepicker(datepickers);
    
});

function showDatePicker(dp) {
    $(dp).datepicker('show');
}

function daysInMonth() {
    return new Date(date.getFullYear(), date.getMonth() + 1, 0).getDate();
}

function lowerMonth() {
    date.setMonth(date.getMonth() - 1);
    drawCalendar();
    getCalendar();
    updateHeader();
}

function raiseMonth() {
    date.setMonth(date.getMonth() + 1);
    drawCalendar();
    getCalendar();
    updateHeader();
}

function updateHeader() {
    $month.empty();
    $month.append(months[date.getMonth()] + " " + date.getFullYear());
}

function getCalendar() {
    var monthC = date.getMonth() + 1;

    $.get(app.root + 'API/Calendar/GetVacations', { date: date.toISOString() })
        .done(function (data) {
            $.each(data, function (key, item) {
                var startDate = new Date(item.StartDate);
                var endDate = new Date(item.EndDate);
                var employeeId = item.EmployeeId;
                if (startDate.getMonth() + 1 == monthC && endDate.getMonth() + 1 == monthC) {
                    for (i = startDate.getDate() + 1; i <= endDate.getDate() + 1; i++) {
                        $("#" + employeeId + " :nth-child(" + i + ")").addClass("vacation");
                    }
                } else if (startDate.getMonth() + 1 == monthC && endDate.getMonth() + 1 != monthC) {
                    for (i = startDate.getDate() + 1; i <= daysInMonth() + 1 ; i++) {
                        $("#" + employeeId + " :nth-child(" + i + ")").addClass("vacation");
                    }
                } else {
                    for (i = 2; i <= endDate.getDate() + 1; i++) {
                        $("#" + employeeId + " :nth-child(" + i + ")").addClass("vacation");
                    }
                }
            });
        });
}

function drawCalendar() {
    $mainRow.empty();
    $calendarBody.empty();
    $mainRow.append('<th class="firstColumn">Töötajad</th>');
    for (var i = 1; i <= daysInMonth() ; i++) {
        $mainRow.append("<th>" + i + "</th>");
    }
    for (var i = 0; i < employees.length; i++) {
        $calendarBody.append('<tr id="' + employees[i].Id + '"><td class="firstColumn">' + employees[i].Name + '</td></tr>');
        for (var k = 1; k <= daysInMonth() ; k++) {
            $("#" + employees[i].Id).append("<td></td>");
        }
    }
}

function sendVacationDate() {
    var start = new Date($("#dp1").data().date);
    var end = new Date($("#dp2").data().date);
    var comment = $comment.val();
    console.log(comment);
    $.post(app.root + 'API/Calendar/SetVacation', { Comment: comment, Start: start.toISOString(), End: end.toISOString() })
        .success(function () {
            $("#status").empty().append("Teie request on edastatud");
            $("#algus").val('');
            $("#lõpp").val('');
            $comment.val('');
            $("#dp1").removeData("date");
            $("#dp2").removeData("date");
            drawMyVacations();
        })
        .error(function (data) {
            $("#status").empty().append(data.responseJSON.Message);
        });
}

function drawMyVacations() {
    $myVacations.empty();
    getVacationDays();

    $.get(app.root + 'API/Calendar/GetMyVacation')
        .done(function (data) {
            $.each(data, function (key, item) {
                var start = dateFormat(new Date(item.StartDate));
                var end = dateFormat(new Date(item.EndDate));
                var status;
                switch (item.Status) {
                    case 0:
                        status = "Approved";
                        break;
                    case 1:
                        status = "Declined";
                        break;
                    case 2:
                        status = "Pending";
                        break;
                    case 3:
                        status = "Retired";
                        break;
                    default:
                        status = "Unknown";
                }
                $myVacations.append('<tr><td>' + start + '</td>' +
                    '<td>' + end + '</td>' +
                    '<td>' + status + '</td>' +
                    '<td><input value="Cancel" class="btn btn-submit" type="button" onclick="cancelVacation(' + item.Id + ')"></td></tr>');
            });
        });
}

function cancelVacation(id) {
    $.post(app.root + 'API/Calendar/CancelVacation', { Id: id })
        .success(function () {
            drawMyVacations();
        });
}

function getVacationDays() {
    $.get(app.root + 'API/Calendar/GetVacationDays')
        .done(function (data) {
            $remainingDays.empty().append("Alles jäänud puhkepäevad: " + data);
        });
}


function initDatepicker(datepickers) {
    $.each(datepickers, function (index, picker) {
        var checkin = $(picker).datepicker({
            onRender: function (date) {
                return date.valueOf() < now.valueOf() ? 'disabled' : '';
            }
        }).on('changeDate', function (ev) {
            if (ev.date.valueOf() > checkout.date.valueOf()) {
                var newDate = new Date(ev.date)
                newDate.setDate(newDate.getDate() + 1);
                checkout.setValue(newDate);
            }
            checkin.hide();
            $(picker)[0].focus();
        }).data('datepicker');
        var checkout = $(picker).datepicker({
            onRender: function (date) {
                return date.valueOf() <= checkin.date.valueOf() ? 'disabled' : '';
            }
        }).on('changeDate', function (ev) {
            checkout.hide();
        }).data('datepicker');

        $(picker).datepicker()
            .on('changeDate', function (ev) {
                $(picker).date = ev.date;
            });
    });
}