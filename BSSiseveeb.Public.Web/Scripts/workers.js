$workers = $('#workersBody');

$(document).ready(function () {
    drawWorkers();
});

function drawWorkers() {
    $.get('/API/Workers/GetEmployees').done(function (data) {
        $.each(data, function (index, employee) {
            var birthday = dateFormat(new Date(employee.Birthdate));
            $workers.append('<tr><td class="name">' + employee.Name + '</td>' +
                '<td>' + birthday + '</td>' +
                '<td>' + employee.PhoneNumber + '</td>' +
                '<td>' + employee.Email + '</td>' +
                '</tr>');
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