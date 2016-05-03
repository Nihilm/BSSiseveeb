
function dateFormat(d) {

    var date = d.getDate();
    date = date < 10 ? "0" + date : date;
    var mon = d.getMonth() + 1;
    mon = mon < 10 ? "0" + mon : mon;
    var year = d.getFullYear();

    return (date + "/" + mon + "/" + year);
}

function setNames() {
    var $names = $(".name");
    $.get('/API/Calendar/GetEmployees').done(function (deta) {
        $.each($names, function (i, name) {
            $.each(deta, function (index, employee) {
                if (name.innerText == employee.Id) {
                    name.innerText = employee.Name;
                }
            });
        });
    });
}