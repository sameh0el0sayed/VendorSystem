$(function () {
    if ($("#language").html() == "اللغة") {
        $("#DateFrom").datepicker({
            format: 'dd/mm/yyyy', showClose: true, showClear: true, keepInvalid: true, autoclose: true, todayHighlight: true//, endDate: new Date()//, maxDate: 0
        }).on('dp.change', function () { });
        // $("#DateFrom").datepicker("option", "maxDate", 0);
        $("#DateTO").datepicker({ format: 'dd/mm/yyyy', showClose: true, showClear: true, keepInvalid: true, autoclose: true, todayHighlight: true }).on('dp.change', function () {
        });
        $("#DateFrom").css("direction", "ltr");
        $("#DateFrom").css("text-align", "right");
        $("#DateTO").css("direction", "ltr");
        $("#DateTO").css("text-align", "right");
    }
    else {
        $("#DateFrom").datepicker({ format: 'dd/mm/yyyy', showClose: true, showClear: true, keepInvalid: true, autoclose: true, todayHighlight: true }).on('dp.change', function () { });
        $("#DateTO").datepicker({ format: 'dd/mm/yyyy', showClose: true, showClear: true, keepInvalid: true, autoclose: true, todayHighlight: true }).on('dp.change', function () { });

        $("#DateFrom").css("direction", "ltr");
        $("#DateFrom").css("text-align", "right");
        $("#DateTO").css("direction", "ltr");
        $("#DateTO").css("text-align", "right");
    }
})

$("#ddRoute").change(function () {

    $("#ddCustomer").empty().change();
    if ($("#ddRoute").val() == "") return;

    showloader();
    $.ajax({
        url: "/Replenishment/GetCustomerByRouteID",
        type: "Post",
        data: { RouteID: $("#ddRoute").val() },
        success: function (Data) {

            let Options = "";
            let selectText = "";

            if ($("#language").html() == "اللغة") { selectText="اختار" }
            else { selectText = "Select" }
            Options += "<option value=" + "" + ">" + selectText + "</option>";

            for (var item of Data) {
                Options += "<option value=" + item.ID + ">" + item.Name + "</option>";
            }
            $("#ddCustomer").append(Options);
            hideloader();
            $("#ddCustomer").val('').change();
        },
        error: function (data) { alert('error'); }
    });

});

function filldataTable() {
    if ($('#DateFrom').val() == "" || $('#DateFrom').val() == null) {
        if ($("#language").html() == "اللغة") { AlertNotify("info", "يجب اختيار تاريخ البدايه "); }
        else { AlertNotify("info", "You must choose Start Date"); }
        return;
    }
    if ($('#DateTO').val() == "" || $('#DateTO').val() == null) {
        if ($("#language").html() == "اللغة") { AlertNotify("info", "يجب اختيار تاريخ النهاية "); }
        else { AlertNotify("info", "You must choose End Date"); }
        return;
    }

    //Sameh Edit
    //if ($('#ddRoute').val() == null) {
    //    if ($("#language").html() == "اللغة") { AlertNotify("info", "يجب اختيار المسار   "); }
    //    else { AlertNotify("info", "You must choose the route "); }
    //    return;
    //}

    let DateFrom = $("#DateFrom").val();
    let DateTO = $("#DateTO").val();

    if ((DateFrom == null && DateTO == null) || CheckTwoDate(DateFrom, DateTO)) {
        showloader();
        $.ajax({
            url: "/Replenishment/GetReplenishData",
            type: "Post",
            async: false,
            data: {
                DateFrom: $("#DateFrom").val(),
                DateTO: $('#DateTO').val(),
                RouteID: $('#ddRoute').val(),
                CustDtlID: $('#ddCustomer').val()
            },
            success: function (data) {
            },
            error: function () {

            }
        });
        $.ajax({
            url: '/Replenishment/Download_Excel',
            type: 'POST',
            cache: false,
            contentType: false,
            processData: false,
            success: function (data) {
                if (data.Status == "Done") {
                    if (data != "") {
                        window.open(data.FilePath, '_blank');
                    }
                }
                else {
                    if ($("#language").html() == "اللغة") {
                        AlertNotify("info", 'حدث خطأ اثناء تحميل الملف')
                    }
                    else {
                        AlertNotify("info", 'Error occure while downloading the file')
                    }
                }
                hideloader();
            },
            error: function () { },
        });
    }
}
