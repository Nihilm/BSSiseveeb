var $requests = $("#requestsBody");
var $requestStatus = $("#requestStatus");
var $status = $("#rStatus");
var datepickers = ["#dp1", "#dp2"];
var date = new Date();
var now = new Date(date.getFullYear(), date.getMonth(), date.getDate(), 0, 0, 0, 0);


$(document).ready(function () {

    drawRequests();

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
});

function showDatePicker(dp) {
    $(dp).datepicker('show');
}

function drawRequests() {
    $.get(app.root + 'API/AdminApi/GetPendingRequests').done(function (data) {
        if (data.Message) {
            $requestStatus.empty().append("Teil puuduvad õigused näha pending taotlusi.");
        } else {
            $.each(data, function (key, item) {
                var timestamp = dateFormat(new Date(item.TimeStamp));
                $requests.append('<tr><td class="name">' + item.EmployeeId + '</td>' +
                    '<td>' + item.Req + '</td>' +
                    '<td>' + item.Description + '</td>' +
                    '<td>' + timestamp + '</td>' +
                    '<td><input type="submit" class="btn btn-submit" value="Approve" onClick="acceptRequest(' + item.Id + ')">' +
                    '<input type="submit" class="btn btn-submit" value="Decline" onClick="declineRequest(' + item.Id + ')"></td>' +
                    '</tr>');
            });
            setNames();
        }
    });
}

function acceptRequest(id) {
    $.post(app.root + 'API/AdminApi/ApproveRequest', { Id: id })
        .success(function () {
            $requests.empty();
            drawRequests();
            $requestStatus.empty().append("Tehing on lõpuni viidud");
        })
        .error(function () {
            $requestStatus.empty().append("Midagi läks Valesti");
        });
}

function declineRequest(id) {
    $.post(app.root + 'API/AdminApi/DeclineRequest', { Id: id })
        .success(function () {
            $requests.empty();
            drawRequests();
            $requestStatus.empty().append("Tehing on lõpuni viidud");
        })
        .error(function () {
            $requestStatus.empty().append("Midagi läks Valesti");
        });
}