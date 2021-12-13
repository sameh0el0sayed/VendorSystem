let AllDistributors = _AllDistributors; // _AllDistributors it be filled from script in index page

let _Code = '';
let DistributorsDataTable;

$(function () {

    if ($("#language").html() == "اللغة") {
        DistributorsDataTable = $("#tblShowDistributorsData").DataTable({
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
        DistributorsDataTable = $("#tblShowDistributorsData").DataTable({});
    }



    FillTable(AllDistributors);
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

    if ($("#ddCountry").val() == "") {
        AlertMe('danger', 'Please choose country', 'من فضلك اختر الدوله')
        return false;
    }

    if ($("#ddProvince").val() == "") {
        AlertMe('danger', 'Please choose Province', 'من فضلك اختر الاقليم')
        return false;
    }

    if ($("#ddCity").val() == "") {
        AlertMe('danger', 'Please choose City', 'من فضلك اختر المدينه')
        return false;
    }

    $("#ModalConfirm").modal();
}

function Save() {

    showloader();

    $("#bntCloseModalConfirm").click();

    let _DistributorVM =
    {
        Code: _Code,
        Name: $("#txtName").val().trim(),
        NameEng: $("#txtNameEng").val().trim(),
        Address: $("#txtAddress").val().trim(),
        CountryID: $("#ddCountry").val(),
        ProvinceID: $("#ddProvince").val(),
        CityID: $("#ddCity").val(),
        RegionID: $("#ddRegion").val(),
        TerritoryID: $("#ddTerritory").val(),
        IsActive: $("#Active").prop('checked'),
        Mobile: $("#txtMobile").val().trim()
    }

    $.ajax({
        url: '/Distributors/Save',
        data: { DistributorVM: _DistributorVM },
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
    DistributorsDataTable.clear().draw();

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

        DistributorsDataTable.row.add([
                        data[i].Name,
                        data[i].Code,
                        data[i].RoutesCount,
                        data[i].CityName,
                        data[i].Address,
                        data[i].Mobile,
                        Status,
                        "<span class=' glyphicon glyphicon-edit edit' id='" + data[i].Code + "'  title='" + Edit + "' onclick='Edit(this)'> "
        ]).draw(false);
    }
}

function Edit(element) {
    _Code = element.id;
    $("#btnCloseModalShowDistributorsData").click();
    showloader();

    $.ajax({
        url: '/Distributors/GetDistributorByCode',
        data: { Code: _Code },
        type: 'POST',
        async: false,
        success: function (data) {


            $("#txtCode").val(_Code).change();
            $("#txtName").val(data.Name).change();
            $("#txtNameEng").val(data.NameEng);
            $("#txtAddress").val(data.Address);
            $("#ddCountry").val(data.CountryID).change();
            $("#ddProvince").val(data.ProvinceID).change();
            $("#ddCity").val(data.CityID).change();
            $("#ddRegion").val(data.RegionID).change();
            $("#ddTerritory").val(data.TerritoryID).change();
            $("#txtMobile").val(data.Mobile);
            $("#Active").prop('checked', data.IsActive)
            $("#txtRoutesCount").val(data.RoutesCount);

            hideloader();
        },
        error: function () { alert('Error'); },
    });

}