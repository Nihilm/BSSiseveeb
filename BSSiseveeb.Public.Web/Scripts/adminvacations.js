var $vacations = $("#vacationsBody");
var $vacationStatus = $("#vacationStatus");
var $confirmedVacations = $("#confirmedVacationsBody");
var $confirmedVacationStatus = $("#confirmedVacationStatus");
var isVisible = false;
var date = new Date();
var now = new Date(date.getFullYear(), date.getMonth(), date.getDate(), 0, 0, 0, 0);

$(document).ready(function () {
    drawVacations();
    drawConfirmedVacations();
});

function drawVacations() {
    $.get('/API/AdminApi/GetPendingVacations').done(function (data) {
        if (data.Message) {
            $vacationStatus.empty().append("Teil puuduvad õigused näha pending puhkusi.");
        } else {
            $.each(data, function (key, item) {
                var start = dateFormat(new Date(item.StartDate));
                var end = dateFormat(new Date(item.EndDate));
                var comment = item.Comments;
                if (comment == null) {
                    comment = "";
                }
                $vacations.append('<tr><td class="name">' + item.EmployeeId + '</td>' +
                    '<td>' + start + '</td>' +
                    '<td>' + end + '</td>' +
                    '<td><p>' + comment + '</p></td>' +
                    '<td><input type="submit" class="btn btn-submit" value="Approve" onClick="approveVacation(' + item.Id + ')">' +
                    '<input type="submit" class="btn btn-submit" value="Decline" onClick="declineVacation(' + item.Id + ')"></td>' +
                    '</tr>');
            });
        }
        setNames();
    });
}

function approveVacation(id) {
    $.post('/API/AdminApi/ApproveVacation', { Id: id })
        .success(function () {
            $vacations.empty();
            drawVacations();
            $confirmedVacations.empty();
            drawConfirmedVacations();
            $vacationStatus.empty().append("Tehing on lõpuni viidud");
        })
        .error(function () {
            $vacationStatus.empty().append("Midagi läks Valesti");
        });
}

function declineVacation(id) {
    $.post('/API/AdminApi/DeleteVacation', { Id: id })
        .success(function () {
            $vacations.empty();
            drawVacations();
            $confirmedVacations.empty();
            drawConfirmedVacations();
            $vacationStatus.empty().append("Tehing on lõpuni viidud");
        })
        .error(function () {
            $vacationStatus.empty().append("Midagi läks Valesti");
        });
}

function drawConfirmedVacations() {
    $.get('/API/AdminApi/GetConfirmedVacations').done(function (data) {
        if (data.Message) {
            $confirmedVacationStatus.empty().append("Teil puuduvad õigused näha confirmed puhkusi.");
        } else {
            $.each(data, function (key, item) {
                var start = dateFormat(new Date(item.StartDate));
                var end = dateFormat(new Date(item.EndDate));
                var comment = item.Comments;
                if (comment == null) {
                    comment = "";
                }
                $confirmedVacations.append('<tr><td class="name">' + item.EmployeeId + '</td>' +
                    '<td>' + start + '</td>' +
                    '<td>' + end + '</td>' +
                    '<td><p>' + comment + '</p></td>' +
                    '<td><input id="Modify' + item.Id + '" value="Modify" data-placement="bottom" data-toggle="popover" data-title="Modify" data-container="body" type="button" data-html="true" class="btn btn-submit modify" onclick="">' +
                    '<input value="Delete" type="button" class="btn btn-submit" onclick="deleteVacation(' + item.Id + ')"</td>' +
                    '</tr>');
                popover(item.Id);
            });
            setNames();
        }
    });
}

function modifyVacation(id) {
    var start = new Date($("#dpa" + id).data().date);
    var end = new Date($("#dpl" + id).data().date);

    $.post('/API/AdminApi/ModifyVacation', { Id: id, Start: start.toISOString(), End: end.toISOString() })
        .success(function () {
            $("#dpl" + id).removeData();
            $("#dpa" + id).removeData();
            hideAllPopovers();
            isVisible = false;
            $confirmedVacations.empty();
            drawConfirmedVacations();
            $confirmedVacationStatus.empty().append("Puhkus edukalt muudetud.");
        })
        .error(function () {
            $confirmedVacationStatus.empty().append("Midagi läks valesti.");
        });
}

function deleteVacation(id) {
    $.post('/API/AdminApi/DeleteVacation', { Id: id })
        .success(function () {
            $confirmedVacations.empty();
            drawConfirmedVacations();
            $confirmedVacationStatus.empty().append("Puhkus edukalt kustutatud.");
        })
        .error(function () {
            $confirmedVacationStatus.empty().append("Midagi läks valesti.");
        });
}

function popover(id) {
    $("#Modify" + id).popover({
        html: true,
        trigger: 'manual',
        content: formatPopover(id)
    }).on('click', function (e) {
        if (isVisible) {
            hideAllPopovers();
            isVisible = false;
            e.stopPropagation();
        } else {
            $(this).popover('show');
            isVisible = true;
            e.stopPropagation();
            initDatepicker(["#dpa" + id, "#dpl" + id]);
        }
    });
}

function formatPopover(id) {
    return '<form class="form-inline" role="form" class="popover">' +
                '<div class="form-group">' +
                     '<div class="input-append date" id="dpa' + id + '" data-date="" data-date-format="yyyy-mm-dd">' +
                         '<label for="algus' + id + '">Algus: </label>' +
                         '<input id="algus' + id + '" class="span2 algus" size="16" type="text" value="" onclick="showDatePicker(&apos;#dpa' + id + '&apos;)">' +
                         '<span class="add-on"><i class="icon-th"></i></span>' +
                     '</div>' +
                     '<div class="input-append date" id="dpl' + id + '" data-date="" data-date-format="yyyy-mm-dd">' +
                         '<label for="lõpp' + id + '">Lõpp: </label>' +
                         '<input id="lõpp' + id + '" class="span2 lõpp" size="16" type="text" value="" onclick="showDatePicker(&apos;#dpl' + id + '&apos;)">' +
                         '<span class="add-on"><i class="icon-th"></i></span>' +
                     '</div>' +
                     '<input type="button" class="btn btn-submit" value="Save" onClick="modifyVacation(' + id + ')">' +
                '</div>' +
            '</form>';
}

function hideAllPopovers() {
    $('.modify').each(function () {
        $(this).popover('hide');
    });
    $('.datepicker').each(function () {
        $(this).remove();
    });
}


function showDatePicker(dp) {
    $(dp).datepicker('show');

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