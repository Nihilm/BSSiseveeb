var $title = $("#title");
var $info = $("#info");
var $status = $("#status");
var $myRequests = $("#myRequestsBody");


$(document).ready(function () {
    drawMyRequests();
});

function sendRequest() {
    var title = $title.val();
    var info = $info.val();
    $.post('/API/Requests/SetRequest', { Title: title, Info: info })
        .success(function() {
            $status.empty();
            $status.append("Request on vastuvõetud");
            $title.val('');
            $info.val('');
            drawMyRequests();
        })
        .error(function(data) {
            $status.append(data.responseJSON.Message);
        });
}

function drawMyRequests() {
    $myRequests.empty();

    $.get('/API/Requests/GetMyRequest')
        .done(function (data) {
            $.each(data, function (key, item) {
                var timestamp = dateFormat(new Date(item.TimeStamp));
                var status;
                switch (item.Status) {
                    case 0:
                        status = "Confirmed";
                        break;
                    case 1:
                        status = "Declined";
                        break;
                    case 2:
                        status = "Pending";
                        break;
                    case 3:
                        status = "Cancelled";
                        break;
                    default:
                        status = "Unknown";
                }
                $myRequests.append('<tr><td>' + item.Req + '</td>' +
                    '<td>' + item.Description + '</td>' +
                    '<td>' + timestamp + '</td>' +
                    '<td>' + status + '</td>' +
                    '<td><input value="Cancel" class="btn btn-submit" type="button" onclick="cancelRequest(' + item.Id + ')"></td></tr>');
            });
        });
}

function cancelRequest(id) {
    $.post('/API/Requests/RemoveVacation', { Id: id })
        .success(function () {
            drawMyRequests();
        });
}
