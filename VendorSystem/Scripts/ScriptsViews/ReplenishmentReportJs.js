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

$("#ddRegion").change(function () {

    $("#ddTerritory").empty().change();

    if ($("#language").html() == "اللغة") {
        $("#ddTerritory").append("<option selected value=''>اختر</option>")
    }
    else {
        $("#ddTerritory").append("<option selected value=''>Please Select</option>")
    }

    if ($("#ddRegion").val() == "" || $("#ddRegion").val() == null) return;

    let Options = "";

    var Data = GetChildrenDataByParentID($("#ddRegion").val(), true);


    for (var item of Data) {

        Options += "<option value=" + item.ID + ">" + item.Name + "</option>";
    }
    $("#ddTerritory").append(Options);

});

$("#ddTerritory").change(function () {

    $("#ddRoute").empty().change();

    if ($("#language").html() == "اللغة") {
        $("#ddRoute").append("<option selected value=''>اختر</option>")
    }
    else {
        $("#ddRoute").append("<option selected value=''>Please Select</option>")
    }

    if ($("#ddTerritory").val() == "" || $("#ddTerritory").val() == null) return;

    let Options = "";

    $.ajax({
        url: '/ReplenishmentReport/GetRouteByTerritoryID',
        data: { TerritoryID: $("#ddTerritory").val() },
        type: 'POST',
        success: function (data) {

            for (var item of data) {

                Options += "<option value=" + item.ID + ">" + item.Name + "</option>";
            }
            $("#ddRoute").append(Options);
            hideloader();
        },
        error: function () { alert('Error'); _Data = 'Error' },
    });
});

$("#ddRoute").change(function () {

    $("#ddCustomer").empty().change();
    if ($("#language").html() == "اللغة") {
        $("#ddCustomer").append("<option selected value=''>اختر</option>")
    }
    else {
        $("#ddCustomer").append("<option selected value=''>Please Select</option>")
    }

    if ($("#ddRoute").val() == "" || $("#ddRoute").val() == null) return;

    showloader();
    $.ajax({
        url: "/ReplenishmentReport/GetCustomerByRouteID",
        type: "Post",
        data: { RouteID: $("#ddRoute").val() },
        success: function (Data) {

            let Options = "";
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

function DownloadFile() {
    let _DateFrom = $("#DateFrom").val();
    let _DateTO = $("#DateTO").val();

    if ((_DateFrom == "" && _DateTO == "") || CheckTwoDate(_DateFrom, _DateTO)) {
        showloader();
        $.ajax({
            url: '/ReplenishmentReport/Download_Excel',
            type: 'POST',
            data: {
                DateFrom: _DateFrom, DateTO: _DateTO, RegionID: $("#ddRegion").val(),
                TerritoryID: $("#ddTerritory").val(), RouteID: $("#ddRoute").val(), CustDtlID: $("#ddCustomer").val()
            },
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
