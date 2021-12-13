let AllCustomers = _AllCustomers; // _AllCustomers it be filled from script in index page

let _ID = -1;
let CustomersDataTable;

$(function () {

    if ($("#language").html() == "اللغة") {
        CustomersDataTable = $("#tblShowCustomersData").DataTable({
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
        CustomersDataTable = $("#tblShowCustomersData").DataTable({});
    }


    InitiatMap();
    FillTable(AllCustomers);
});

function CheckValidation() {


    //if ($("#txtName").val().trim().length < 3) {
    //    AlertMe('danger', 'Please insert valid name at least 3 char', 'من فضلك ادخل اسم صحيح على الاقل 3 احرف')
    //    return false;
    //}

    //if ($("#txtNameEng").val().trim().length < 3) {
    //    AlertMe('danger', 'Please insert valid English name at least 3 char', 'من فضلك ادخل اسم باللغه الانجليزيه صحيح على الاقل 3 احرف')
    //    return false;
    //}

    //if ($("#ddCountry").val() == "") {
    //    AlertMe('danger', 'Please choose country', 'من فضلك اختر الدوله')
    //    return false;
    //}

    //if ($("#ddProvince").val() == "") {
    //    AlertMe('danger', 'Please choose Province', 'من فضلك اختر الاقليم')
    //    return false;
    //}

    //if ($("#ddCity").val() == "") {
    //    AlertMe('danger', 'Please choose City', 'من فضلك اختر المدينه')
    //    return false;
    //}

    $("#ModalConfirm").modal();
}

function Save() {

    showloader();

    $("#bntCloseModalConfirm").click();

    let _CustomerVM =
    {
        ID: _ID,
        StoreName: $("#txtStoreName").val().trim(),
        StoreNameEng: $("#txtStoreNameEng").val().trim(),
        CompanyName: $("#txtCompanyName").val().trim(),
        CompanyNameEng: $("#txtCompanyNameEng").val().trim(),
        CountryID: $("#ddCountry").val(),
        ProvinceID: $("#ddProvince").val(),
        CityID: $("#ddCity").val(),
        RegionID: $("#ddRegion").val(),
        TerritoryID: $("#ddTerritory").val(),
        Phone: $("#txtPnone").val().trim(),
        Mobile: $("#txtMobile").val().trim(),
        Address: $("#txtAddress").val().trim(),
        VatNumber: $("#txtVatNumber").val().trim(),
        CommercialRegistration: $("#txtCommercialRegistration").val().trim(),
        VisitScheduleID: $("#ddVisitSchedule").val(),
        StoreTypeID: $("#ddStoreType").val(),
        StoreCategoryID: $("#ddStoreCategory").val(),
        StoreClassificationID: $("#ddStoreClassification").val(),
        IsActive: $("#Active").prop('checked'),
    }

    $.ajax({
        url: '/Customers/Save',
        data: { CustomerVM: _CustomerVM },
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
    CustomersDataTable.clear().draw();

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

        CustomersDataTable.row.add([
                        data[i].CompanyName,
                        data[i].StoreName,
                        data[i].CustomerCode,
                        data[i].CityName,
                        data[i].Address,
                        data[i].Mobile,
                        Status,
                        "<span class=' glyphicon glyphicon-edit edit' id='" + data[i].ID + "'  title='" + Edit + "' onclick='Edit(this)'> "
        ]).draw(false);
    }
}

function Edit(element) {
    _ID = element.id;
    $("#btnCloseModalShowCustomersData").click();
    showloader();

    $.ajax({
        url: '/Customers/GetCustomerByID',
        data: { ID: _ID },
        type: 'POST',
        async: false,
        success: function (data) {

            $("#txtStoreName").val(data.StoreName).change();
            $("#txtStoreNameEng").val(data.StoreNameEng).change();
            $("#txtCompanyName").val(data.CompanyName).change();
            $("#txtCompanyNameEng").val(data.CompanyNameEng).change();
            $("#ddCountry").val(data.CountryID).change();
            $("#ddProvince").val(data.ProvinceID).change();
            $("#ddCity").val(data.CityID).change();
            $("#ddRegion").val(data.RegionID).change();
            $("#ddTerritory").val(data.TerritoryID).change();
            $("#txtPnone").val(data.Phone).change();
            $("#txtMobile").val(data.Mobile).change();
            $("#txtAddress").val(data.Address);
            $("#txtVatNumber").val(data.VatNumber);
            $("#txtCommercialRegistration").val(data.CommercialRegistration);
            $("#ddVisitSchedule").val(data.VisitScheduleID);
            $("#ddStoreType").val(data.StoreTypeID).change();
            $("#ddStoreCategory").val(data.StoreCategoryID).change();
            $("#ddStoreClassification").val(data.StoreClassificationID).change();
            $("#txtAssingRoutes").val(data.AssingRoutes);
            $("#txtCustomerCode").val(data.CustomerCode);
            $("#Active").prop('checked', data.IsActive);


            //$("#Longitude").val(RowData.Longitude);
            //$("#Latitude").val(RowData.Latitude);
            //addmarker($("#Latitude").val(), $("#Longitude").val());

            hideloader();
        },
        error: function () { alert('Error'); },
    });

}