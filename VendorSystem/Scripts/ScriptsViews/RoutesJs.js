let AllRoutes = _AllRoutes; // _AllRoutes it be filled from script in index page

let _ID = -1;
let RoutesDataTable;

$(function () {

    if ($("#language").html() == "اللغة") {
        RoutesDataTable = $("#tblShowRoutesData").DataTable({
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
        RoutesDataTable = $("#tblShowRoutesData").DataTable({});
    }



    FillTable(AllRoutes);
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

    if ($("#txtRouteCode").val().trim().length < 1) {
        AlertMe('danger', 'Please insert valid English name at least one char', 'من فضلك ادخل كود صحيح على الاقل حرف واحد')
        return false;
    }

    if ($("#ddCountry").val() == "") {
        AlertMe('danger', 'Please choose country', 'من فضلك اختر الدوله')
        return false;
    }

    if ($("#ddProvince").val() == "") {
        AlertMe('danger', 'Please choose Province', 'من فضلك اختر الاقليم')
        return false;
    }

    $("#ModalConfirm").modal();
}

function Save() {

    showloader();

    $("#bntCloseModalConfirm").click();

    let _RouteVM =
    {
        ID: _ID,
        Name: $("#txtName").val().trim(),
        NameEng: $("#txtNameEng").val().trim(),
        CountryID: $("#ddCountry").val(),
        ProvinceID: $("#ddProvince").val(),
        CityID: $("#ddCity").val(),
        RegionID: $("#ddRegion").val(),
        TerritoryID: $("#ddTerritory").val(),
        Note: $("#txtNote").val().trim(),
        IsActive: $("#Active").prop('checked'),
        RouteCode: $("#txtRouteCode").val()

    }

    $.ajax({
        url: '/Routes/Save',
        data: { RouteVM: _RouteVM },
        type: 'POST',
        async: false,
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
    RoutesDataTable.clear().draw();

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

        RoutesDataTable.row.add([
                        data[i].Name,
                        data[i].RouteCode,
                        data[i].CountryName,
                        data[i].CityName,
                         Status,
                        data[i].Note,
                        "<span class=' glyphicon glyphicon-edit edit' id='" + data[i].ID + "'  title='" + Edit + "' onclick='Edit(this)'> "
        ]).draw(false);
    }
}

function Edit(element) {
    _ID = element.id;
    $("#btnCloseModalShowRoutesData").click();
    showloader();

    $.ajax({
        url: '/Routes/GetRouteByID',
        data: { ID: _ID },
        type: 'POST',
        async: false,
        success: function (data) {


            $("#txtName").val(data.Name).change();
            $("#txtNameEng").val(data.NameEng);
            $("#txtAddress").val(data.Address);
            $("#ddCountry").val(data.CountryID).change();
            $("#ddProvince").val(data.ProvinceID).change();
            $("#ddCity").val(data.CityID).change();
            $("#ddRegion").val(data.RegionID).change();
            $("#ddTerritory").val(data.TerritoryID).change();
            $("#txtNote").val(data.Note);
            $("#Active").prop('checked', data.IsActive)
            $("#txtRouteCode").val(data.RouteCode)

            hideloader();
        },
        error: function () { alert('Error'); },
    });

}

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
            url: '/Routes/UploadAndSave',
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
    $("#btnUpload")[0].value = "";
}