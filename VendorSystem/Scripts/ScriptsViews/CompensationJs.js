
$(function () {
    if ($("#language").html() == "اللغة") {
        $("#DateFrom").datepicker({
            format: 'dd/mm/yyyy', showClose: true, showClear: true, keepInvalid: true, autoclose: true, todayHighlight: true//, endDate: new Date()//, maxDate: 0
        }).on('dp.change', function () { });

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
});


function DownloadData() {

    showloader();
    $.ajax({
        url: '/Compensation/DownloadCompensationData',
        type: 'POST',
        data: { CustomerID: $("#ddCustomer").val(), DateFrom: $("#DateFrom").val(), DateTo: $("#DateTO").val() },
        success: function (data) {
            if (data.Status == "Done") {

                //DownloadFile(data.FilePath)
                window.open(data.FilePath, '_blank');
            }
            else {
                AlertMe('error', data.FilePath, data.FilePath);
            }
            hideloader();
        },
        error: function () { hideloader(); alert('Error'); },
    });
}
//Sameh Code
function DownloadGrowthData() {

    showloader();
    $.ajax({
        url: '/Compensation/DownloadCompensationGrowthData',
        type: 'POST',
        data: { CustomerID: $("#ddCustomer").val(), DateFrom: $("#DateFrom").val(), DateTo: $("#DateTO").val() },
        success: function (data) {
            if (data.Status == "Done") {

                //DownloadFile(data.FilePath)
                window.open(data.FilePath, '_blank');
            }
            else {
                AlertMe('error', data.FilePath, data.FilePath);
            }
            hideloader();
        },
        error: function () { hideloader(); alert('Error'); },
    });
}

