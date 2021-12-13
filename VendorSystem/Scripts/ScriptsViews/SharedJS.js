function AlertNotify(Type, Message) {
    $.notify({

        message: "<h4>" + Message + "</h4>"
    }, {
        icon_type: 'image',
        placement: {
            from: "top",
            align: "center"
        },

        type: Type,
        delay: 5000,
        element: 'body',
        left: 10,
        allow_dismiss: true,
        newest_on_top: false,
        showProgressbar: false,
        offset: 20,
        spacing: 10,
        z_index: 999999999999999999,
        timer: 1000,
        url_target: '_blank',
        mouse_over: null,

        onShow: null,
        onShown: null,
        onClose: null,
        onClosed: null,
        icon_type: 'class',
        animate: {
            enter: 'animated bounceInDown',
            exit: 'animated bounceOutUp'
        }
    });
}

function StopSave(id) {
    setTimeout(function () {
        $('#' + id).attr('disabled', 'disabled');
        $('#' + id).addClass("slideInUp animated");
    }, 0);
}

function EnableSave(id) {
    setTimeout(function () {
        $('#' + id).removeAttr('disabled');
        // $('#' + id).addClass("slideInUp animated");
    }, 0);
}

function ToJavaScriptDateTime(datetime) {
    if (datetime == null)
        return false;
    var value = new Date
               (
                    parseInt(datetime.replace(/(^.*\()|([+-].*$)/g, ''))
               );
    var month = value.getMonth() + 1;
    if (month < 10)
        month = "0" + month;
    var day = value.getDate();
    if (day < 10)
        day = "0" + day;
    var Hourse = value.getHours();
    var Minuts = value.getMinutes();
    var Seconds = value.getSeconds();
    var datTime = day + "/" + month + "/" + value.getFullYear() + "-" + Hourse + ":" + Minuts + ":" + Seconds;
    return datTime;
}

function showloader() {
    $('.loader').show()
}

function hideloader() {
    $('.loader').hide()
}

function AlertMe(type, MessageEng, MessageArabic) {

    if (MessageEng.indexOf('<head>') > -1) {
        location.reload();
    }
    if ($("#language").html() == "اللغة") {
        AlertNotify(type, MessageArabic);
    }
    else {
        AlertNotify(type, MessageEng);
    }
}

function GetChildrenDataByParentID(_ParentID, _IsFromCentralizedLookUp) {
    let _Data;

    $.ajax({
        url: '/LockUp/GetChildrenDataByParentID',
        data: { ParentID: _ParentID, IsFromCentralizedLookUp: _IsFromCentralizedLookUp },
        type: 'POST',
        async: false,
        success: function (data) {
            _Data = data;
        },
        error: function () { alert('Error'); _Data = 'Error' },
    });

    return _Data;
}

function CheckTwoDate(_DateFrom, _DateTo) {

    let Status = false;

    $.ajax({
        url: "/Replenishment/CompareDates",
        type: "Post",
        async: false,
        data: { DateFrom: _DateFrom, DateTO: _DateTo },
        success: function (resp) {
            if (resp == 1) {
                if ($("#language").html() == "اللغة") {
                    AlertNotify("info", "تاريخ البداية يجب ان يكون اقل من تاريخ النهاية")
                }
                else { AlertNotify("info", "The ending date must be greater than or equal the starting date !!") }
                Status = false;
            }
            else if (resp == 2) {
                if ($("#language").html() == "اللغة") {
                    AlertNotify("info", "تاريخ النهاية يجب ان يكون اقل من او يساوي تاريخ اليوم")
                }
                else { AlertNotify("info", "The ending date must be less than or equal to today date !! !!") }
                Status = false;
            }
            else if (resp == 3) {
                if ($("#language").html() == "اللغة") {
                    AlertNotify("info", "تاريخ البداية يجب ان يكون اقل من او يساوي تاريخ اليوم")
                }
                else { AlertNotify("info", "The starting date must be less than or equal  to today date !!") }
                Status = false;
            }
            else if (resp == 4) {
                Status = true;
            }
        },
        error: function () {
            hideloader();
        }
    });

    return Status;
}

function FillDropDownMultiSelect(ddParentID, ddChildrenID, _IsFromCentralizedLookUp) {

    let ChildID = '#' + ddChildrenID;
    let ParentID = '#' + ddParentID;
    let SelectedChildrenValues = $(ChildID).val();
    if (SelectedChildrenValues == null) {
        SelectedChildrenValues = [];
    }

    $(ChildID).empty().change();

    if ($(ParentID).val() != null) {

        showloader();

        $.ajax({
            url: '/LockUp/GetChildrensbyPaentLst',
            data: { ParentIdLst: $(ParentID).val(), IsFromCentralizedLookUp: _IsFromCentralizedLookUp },
            type: 'POST',
            async: false,
            success: function (data) {

                let Options = "";
                for (var item of data) {

                    Options += "<option value=" + item.ID + ">" + item.Name + "</option>";
                }
                $(ChildID).append(Options);
                $(ChildID).val(SelectedChildrenValues).change();
                hideloader();
            },
            error: function () { alert('Error'); },
        });
    }
}