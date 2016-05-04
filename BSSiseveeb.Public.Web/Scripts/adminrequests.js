var $requests = $("#requestsBody");
var $requestStatus = $("#requestStatus");

function drawRequests() {
    $.get('/API/AdminApi/GetPendingRequests').done(function (data) {
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
    $.post('/API/AdminApi/ApproveRequest', { Id: id })
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
    $.post('/API/AdminApi/DeclineRequest', { Id: id })
        .success(function () {
            $requests.empty();
            drawRequests();
            $requestStatus.empty().append("Tehing on lõpuni viidud");
        })
        .error(function () {
            $requestStatus.empty().append("Midagi läks Valesti");
        });
}