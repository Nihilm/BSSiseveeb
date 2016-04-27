var $title = $("#title");
var $info = $("#info");
var $status = $("#status");


$(document).ready(function () {

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
        })
        .error(function() {
            $status.append("Midagi läks valesti");
        });
}