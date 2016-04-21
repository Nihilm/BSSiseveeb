var $vacations = $("#vacationsBody");
var $vacationStatus = $("#vacationStatus");

$(document).ready(function () {
    drawVacations();
});

function approveVacation(id) {
    $.get('/API/Admin/ApproveVacation', {id : id})
        .done(function (data) {
            $vacations.empty();
            drawVacations();
            if (data == 0) {
                $vacationStatus.empty().append("Tehing on lõpuni viidud");
            } else {
                $vacationStatus.empty().append("Midagi läks Valesti");
            }
        });
}

function declineVacation(id) {
    $.get('/API/Admin/DeclineVacation', {id : id})
        .done(function (data) {
            $vacations.empty();
             drawVacations();
             if (data == 0) {
                 $vacationStatus.empty().append("Tehing on lõpuni viidud");
             } else {
                 $vacationStatus.empty().append("Midagi läks Valesti");
             }
         });
}

function drawVacations() {
    $.get('/API/Admin/GetPendingVacations').done(function (data) {
        $.each(data, function (key, item) {
            var start = dateFormat(new Date(item.StartDate));
            var end = dateFormat(new Date(item.EndDate));
            $vacations.append('<tr><td class="name">' + item.EmployeeId + '</td>' +
                '<td>' + start + '</td>' +
                '<td>' + end + '</td>' +
                '<td><input type="submit" class="btn btn-submit" value="Approve" onClick="approveVacation(' + item.Id + ')">' +
                '<input type="submit" class="btn btn-submit" value="Decline" onClick="declineVacation(' + item.Id + ')"></td>' +
                '</tr>');
        });
        var $names = $(".name");
        $.get('/API/Admin/GetEmployees').done(function (deta) {
            $.each($names, function (i, name) {
                $.each(deta, function (index, employee) {
                    if (name.innerText == employee.Id) {
                        name.innerText = employee.Name;
                    }
                });
            });
        });
    });
}

function dateFormat(d) {

    var date = d.getDate();
    date = date < 10 ? "0" + date : date;
    var mon = d.getMonth() + 1;
    mon = mon < 10 ? "0" + mon : mon;
    var year = d.getFullYear();

    return (date + "/" + mon + "/" + year);
}
