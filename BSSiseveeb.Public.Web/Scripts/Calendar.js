var date = new Date();
var months = ["Jaanuar", "Veebruar", "Märts", "Aprill", "Mai", "Juuni", "Juuli", "August", "September", "Oktoober", "November", "Detsember"];
var $month = $("#month");
var $mainRow = $("#mainRow");
var $calendarBody = $("#calendarBody");
var employees;

$(document).ready(function () {
    $.get('/API/Calendar/GetEmployees').done(function(data) {
        employees = data;
        drawCalendar();
        getCalendar();
    });

    updateHeader();
});

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
    $.get('/API/Calendar/GetVacations', { date: date.toISOString() })
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
    for (var i = 1; i <= daysInMonth(); i++) {
        $mainRow.append("<th>" + i + "</th>");
    }
    for (var i = 0; i < employees.length; i++) {
        $calendarBody.append('<tr id="' + employees[i].Id + '"><td class="firstColumn">' + employees[i].Name + '</td></tr>');
        for (var k = 1; k <= daysInMonth() ; k++) {
            $("#" + (i+1)).append("<td></td>");
        }
    }
}
