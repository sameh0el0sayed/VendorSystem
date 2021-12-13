let AllRouteCombinations = _AllRouteCombinations;   // _AllRouteCombinations it be filled from script in index page

let _ID = -1;
let RouteCombinationsDataTable;

$(function () {

    if ($("#language").html() == "اللغة") {
        RouteCombinationsDataTable = $("#tblShowRouteCombinationsData").DataTable({
            "language": {
                "sProcessing": "جارٍ التحميل...",
                "sLengthMenu": "أظهر _MENU_ مدخلات",
                "sZeroRecords": "لم يعثر على أية سجلات",
                "sInfo": "إظهار _START_ إلى _END_ من أصل _TOTAL_ مدخل",
                "sInfoEmpty": "يعرض 0 إلى 0 من أصل 0 سجل",
                "sInfoFiltered": "(منتقاة من مجموع _MAX_ مُدخل)",
                "sInfoPostFix": "",
                "sSearch": "ابحث:",
                "sUrl": "",
                "oPaginate": {
                    "sFirst": "الأول",
                    "sPrevious": "السابق",
                    "sNext": "التالي",
                    "sLast": "الأخير"
                }
            }
        });

    } else {
        RouteCombinationsDataTable = $("#tblShowRouteCombinationsData").DataTable({});
    }



    FillTable(AllRouteCombinations);
});

function CheckValidation() {


    if ($("#txtName").val().trim().length < 3) {
        AlertMe('danger', 'Please insert valid name at least 3 char', 'من فضلك ادخل اسم صحيح على الاقل 3 احرف')
        return false;
    }

    if ($("#txtNameEng").val().trim().length < 3) {
        AlertMe('danger', 'Please insert valid English name at least 3 char', 'من فضلك ادخل اسم باللغه الانجليزيه صحيح على الاقل 3 احرف')
        return false;
    }

    if ($("#ddDistributor").val() == "") {
        AlertMe('danger', 'Please choose Distributor', 'من فضلك اختر الموزع')
        return false;
    }

    if ($("#ddRoute").val() == "") {
        AlertMe('danger', 'Please choose Route', 'من فضلك اختر المسار')
        return false;
    }

    //if ($("#ddFirstClassification").val() == "") {
    //    AlertMe('danger', 'Please choose First Classification', 'من فضلك اختر التصنيف الاول للمنتجات')
    //    return false;
    //}

    if ($("#ddCustomers").val() == null) {
        AlertMe('danger', 'Please choose  One Customer at least', 'من فضلك اختر عميل واحد على الاقل')
        return false;
    }

    $("#ModalConfirm").modal();
}

function Save() {

    showloader();

    $("#bntCloseModalConfirm").click();

    let _RouteCombinationVM =
    {

        ID: _ID,
        Name: $("#txtName").val().trim(),
        NameEng: $("#txtNameEng").val().trim(),
        DistributorID: $("#ddDistributor").val(),
        RouteID: $("#ddRoute").val(),
        FirstClassificationID: $("#ddFirstClassification").val(),
        CustomersDtlLst: $("#ddCustomers").val() == null ? [] : $("#ddCustomers").val(),
        //FirstClassificationLst: $("#ddFirstClassification").val() == null ? [] : $("#ddFirstClassification").val(),
        //SecondClassificationLst: $("#ddSecondClassification").val() == null ? [] : $("#ddSecondClassification").val(),
        Note: $("#txtNote").val().trim(),
        IsActive: $("#Active").prop('checked')

    }

    

    $.ajax({
        url: '/RouteCombination/Save',
        data: { RouteCombinationVM: _RouteCombinationVM },
        type: 'POST',
        success: function (data) {
            if (data == "Done") {
                AlertMe('success', 'Saved Succefully', 'تم الحفظ بنجاح')
                setTimeout(function () { window.location.reload() }, 2000);
            }
            else {
                AlertMe('danger', data, data);
            }
            hideloader();
        },
        error: function () { alert('Error'); _Data = 'Error' },
    });

}

function FillTable(data) {

    RouteCombinationsDataTable.clear().draw();

    let IsActive = "Active";
    let InActive = "InActive";
    let Edit = "Edit";
    if ($("#language").html() == "اللغة") {
        IsActive = "نشط";
        InActive = "غير نشط";
        Edit = "تعديل";
    }

    let Status;
    for (var i = 0; i < data.length; i++) {

        Status = InActive;
        if (data[i].IsActive == true) {
            Status = IsActive;
        }
        RouteCombinationsDataTable.row.add([
                        data[i].Name,
                        data[i].DistributorName,
                        data[i].RouteName, 
                        Status,
                        data[i].Note,
                        "<span class=' glyphicon glyphicon-edit edit' id='" + data[i].ID + "'  title='" + Edit + "' onclick='Edit(this)'> "
        ]).draw(false);
    }
}

function Edit(element) {
    _ID = element.id;
    $("#btnCloseModalShowRouteCombinationsData").click();
    showloader();

    $.ajax({
        url: '/RouteCombination/GetRouteCombinationByID',
        data: { ID: _ID },
        type: 'POST',
        success: function (data) {


            $("#txtName").val(data.Name);
            $("#txtNameEng").val(data.NameEng);
            $("#ddDistributor").val(data.DistributorID).change();
            $("#ddRoute").val(data.RouteID).change();
            $("#ddCustomers").val(data.CustomersDtlLst).change();
            $("#txtNote").val(data.Note);
            $("#Active").prop('checked', data.IsActive)
            $("#ddFirstClassification").val(data.FirstClassificationLst).change();
            $("#ddSecondClassification").val(data.SecondClassificationLst).change();
            hideloader();
        },
        error: function () { alert('Error'); },
    });

}
 
$("#ddFirstClassification").change(function () {

    FillDropDownMultiSelect('ddFirstClassification', 'ddSecondClassification', false)
});


function UploadAndSave() {

    $("#bntCloseModalConfirmUpload").click();
    StopSave("btnUpload");
    showloader();

    var FileName = $("#btnUpload")[0].value.split(/(\\|\/)/g).pop();
    if (FileName != "" && FileName.indexOf('xls') > -1) {
        var formData = new FormData();
        var File = $("#btnUpload")[0].files[0];
        formData.append(File.name, File);
        $.ajax({
            url: '/RouteCombination/UploadAndSave',
            type: 'POST',
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (data) {

                if (data == "Done") {
                    AlertMe('success', 'Saved Succefully', 'تم الحفظ بنجاح');
                    setTimeout(function () { window.location.reload() }, 2000);
                }
                else {
                    AlertMe('danger', data, data);
                }
                EnableSave("btnUpload");
                hideloader();
            },
            error: function () { hideloader(); },
        });
    }
    else if (FileName != "") {
        AlertMe('danger', 'File Format Not Valide', 'الملف غير صالح');
    }

    $("#btnUpload")[0].value = "";
}

function DownloadCurrentStatus() {

    showloader();
    $.ajax({
        url: '/RouteCombination/DownloadCurrentStatus',
        type: 'Post',
        success: function (data) {
            if (data.Status == "Done") {
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