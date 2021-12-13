var DataTable;
var ContactDataTable;
var ID = 0;
var edit;
var delet;
var Active;
var InActive;
var ActiveClass;
var FormType = 1; // Save Or Update
var PartnerContact;
var alreadyexist;
var PartnerId;
var PartnerName = '';
var PartnerCode;
var Mobile;
var Fax;
var ProvinceId;
var City;
var District;
var Terriory;
var CompanyId;
var tblProducts;
var tblSearchProducts;
var UploadFile = 0;
var URL = "/Partner/index";
$(function () {
    $("#Price_List_Id").val($("#Price_List_Id option:first").val());
    if ($("#language").html() == "اللغة") {
        $("#Modelspan").css('float', 'left');
        $("#Catspan").css('float', 'left');
        ContactDataTable = $("#tbContactData").DataTable({
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
            },
            "columnDefs": [{ "targets": [1], visible: false }, ]
        });

        ContactDataTable.on('order.dt search.dt', function () {
            ContactDataTable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();

        $("#arbicestatuse").show();
        //////////////////////////////////////////////////////////////////
        tblProducts = $("#tblProducts").DataTable({
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
            },
            "columnDefs": [{ "targets": [0], visible: false }]
        });

        tblSearchProducts = $("#tblSearchProducts").DataTable({
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
            },
            "columnDefs": [{ "targets": [0], visible: false }],
            "searching": false,
            "Length": false
        });

    } else {
        $("#Modelspan").css('float', 'right');
        $("#Catspan").css('float', 'right');
        ContactDataTable = $("#tbContactData").DataTable({
            "columnDefs": [{ "targets": [1], visible: false }, ]
        });
        ContactDataTable = $("#tbContactData").DataTable();

        ContactDataTable.on('order.dt search.dt', function () {
            ContactDataTable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();
        /////////////////////////////////////////////////////////////////
        tblProducts = $("#tblProducts").DataTable({
            "columnDefs": [{ "targets": [0], visible: false }]
        });
        tblSearchProducts = $("#tblSearchProducts").DataTable({
            "columnDefs": [{ "targets": [0], visible: false }],
            "searching": false,
            "Length": false
        });
    }
    if (Partners != null)
        CompanyId = Partners[0].CompanyId;
})
$('#ShowPartnerData').on('shown.bs.modal', function (e) {
    if ($("#language").html() == "اللغة") {
        edit = "تعديل";
        delet = "تغيير الحالة";
        Active = "متاح";
        InActive = "غير متاح";

        DataTable = $("#tbShowData").DataTable({
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
            }, "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]]
        });

        DataTable.on('order.dt search.dt', function () {
            DataTable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();
    }
    else {
        Active = "Active";
        InActive = " InActive";
        edit = "edit";
        delet = "Change Status";

        DataTable = $("#tbShowData").DataTable({ "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]] });
        DataTable.on('order.dt search.dt', function () {
            DataTable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();
    }
    FillTable(Partners);
});
function FillTable(data) {
    DataTable.clear().draw();
    for (var i = 0; i < data.length; i++) {
        var IsActive;
        if (data[i].Active == 1) {
            IsActive = Active;
            ActiveClass = "  spangreen";
        }
        else {
            IsActive = InActive;
            ActiveClass = "  spanred";
        }
        DataTable.row.add(["",
             data[i].Partner_Name,
             data[i].PartnerTypeName,
              data[i].Address,
               data[i].Email,
                data[i].Mobile,
                  data[i].Fax,
                      IsActive,
                      "<span class=' glyphicon glyphicon-edit edit'  title='" + edit + "' onclick='Edit(" + i + ")'> "
        ]).draw(false);
    }
}
$("#Partner_Name").change(function () {

    SessionState(URL);
    var format = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/;
    if (format.test($("#Partner_Name").val())) {
        $("#Partner_Name").val("");
        if ($("#language").html() == "اللغة") { AlertNotify("info", "غير مسموح بادخال العلامات الخاصة"); }
        else { AlertNotify("info", "You Can not Insert Special Characters "); }
        return false;
    }
    $.ajax({
        url: "/Partner/CheckName",
        type: "GET",
        data: { Name: $("#Partner_Name").val() },
        success: function (data) {
            if (data == 1 && FormType == 1) {
                $("#Partner_Name").val('');
                if ($("#language").html() == "اللغة") {
                    AlertNotify("info", "عفوا لقد ادخلت اسم  موجود  من قبل ");
                }
                else { AlertNotify("info", "Sorry, You Entered Existing  Name!"); }
            }
            if (data == 1 && FormType == 2 && $("#Partner_Name").val() != PartnerName) {
                $("#Partner_Name").val('');
                if ($("#language").html() == "اللغة") {
                    AlertNotify("info", "عفوا لقد ادخلت اسم  موجود  من قبل ");
                }

                else { AlertNotify("info", "Sorry, You Entered Existing  Name!"); }
            }
        },
        error: function (data) { }
    });
});
$("#Contact_Person").change(function () {
    var format = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/;
    if (format.test($("#Contact_Person").val())) {
        $("#Contact_Person").val("");
        if ($("#language").html() == "اللغة") { AlertNotify("info", "غير مسموح بادخال العلامات الخاصة"); }
        else { AlertNotify("info", "You Can not Insert Special Characters "); }
    }

});
function ColorBalance() {
    var inputVal = document.getElementById("Starting_Balance");
    // inputVal.setAttribute('BackColor', 'Red');
    if (inputVal.value < 0) {
        inputVal.style.backgroundColor = "red";
        inputVal = document.getElementById("Starting_Balance").style.backgroundColor = 'red';
    }
    else {
        inputVal.style.backgroundColor = "yellow";
    }
}
function Edit(index) {
    SessionState(URL);

    var RowData = Partners[index];
    ID = RowData.Partner_Id;
    PartnerId = RowData.Partner_Id;
    PartnerName = RowData.Partner_Name;
    PartnerCode = RowData.PartnerCode;
    Mobile = RowData.Mobile;
    Fax = RowData.Fax;
    NID = RowData.NID;
    $("#CustomerCode").val(RowData.CustomerCode)
    //$("#City").val(RowData.City),
    $("#Street").val(RowData.Street),
    $("#BuildingNumber").val(RowData.BuildingNumber),
    $("#FlatNumber").val(RowData.FlatNumber),
    ///////////
    $("#Partner_Name").val(RowData.Partner_Name);
    $("#NID").val(RowData.NID);
    $("#Address").val(RowData.Address);
    $("#PartnerCode").val(RowData.PartnerCode).attr("disabled", "disabled");;
    $("#Email").val(RowData.Email);
    $("#Mobile").val(RowData.Mobile);
    $("#Phone1").val("");
    $("#Phone2").val(RowData.Phone2);
    $("#Fax").val(RowData.Fax);
    $("#Website").val(RowData.Website);
    $("#Contact_Person").val("");
    $("#Tax_Record_Id").val(RowData.Tax_Record_Id);
    $("#Commercial_Id").val(RowData.Commercial_Id);
    $("#Bank_Acc1").val(RowData.Bank_Acc1);
    $("#Acc1_Bank_Id").val(RowData.Acc1_Bank_Id).change();
    $("#DepartmentId").val(RowData.DepartmentId).change();

    $("#Bank_Acc2").val(RowData.Bank_Acc2);
    $("#Acc2_Bank_Id").val(RowData.Acc2_Bank_Id).change();
    $("#Group_Id").val(1);
    $("#Partner_Type").val(RowData.Partner_Type).change();
    $("#Apply_Tax").val(1);
    $("#Multi_Shipping").val(1);
    $("#Multi_Billing").val(1);
    $("#Payment_Term").val(RowData.Payment_Term).change();
    //document.getElementById("Starting_Balance").style.color = "red";
    $("#Starting_Balance").val(RowData.Starting_Balance);
    document.getElementById("Starting_Balance").style.backgroundColor = 'red';
    // document.getElementById('Starting_Balance').onchange= ColorBalance
    // ColorBalance()
    //$("#Starting_Balance").css('background-color', 'red');
    //var input=document.getElementById("Starting_Balance")
    //input.style.backgroundColor = "yellow";
    $("#Type").val(RowData.Type).change();
    $("#Price_List_Id").val(RowData.Price_List_Id).change();
    $("#Zone").val(RowData.Zone);
    $("#PartnerSegmentation").val(RowData.PartnerSegmentation).change();

    $("#CountryId").val(RowData.CountryId).change();
    setTimeout(function () { $("#City_Id").val(RowData.City_Id).change(); }, 2000)


    $("#Country").val(RowData.CountryId).change();
    ProvinceId = RowData.ProvinceId;
    City = RowData.CityId;
    District = RowData.DistrictId;
    Terriory = RowData.TerritoryId;
    ///////////
    if (RowData.Active == 1 || RowData.Active == true) {
        $("#Active").prop('checked', true).change();
    }
    else {
        $("#Active").prop('checked', false);
    }

    if (RowData.IsPostpon == 1 || RowData.IsPostpon == true) {
        $("#IsPostpond").prop('checked', true);
    }
    else {
        $("#IsPostpond").prop('checked', false);
    }
    if (RowData.IsDeliveryMan == 1 || RowData.IsDeliveryMan == true) {
        $("#IsDeliveryMan").prop('checked', true);
    }
    else {
        $("#IsDeliveryMan").prop('checked', false);
    }
    if (RowData.Multi_Billing == 1) {
        $("#MultiBilling").prop('checked', true);
    }
    else {
        $("#MultiBilling").prop('checked', false);
    }

    if (RowData.Multi_Shipping == 1) {
        $("#MultiShipping").prop('checked', true);
    }
    else {
        $("#MultiShipping").prop('checked', false);
    }
    if (RowData.Apply_Tax == 1) {
        $("#ApplyTax").prop('checked', true);
    }
    else {
        $("#ApplyTax").prop('checked', false);
    }
    //////////////////////////////////
    if (RowData.IsManufacturer == 1 || RowData.IsManufacturer == true) {
        $("#IsManufacturer").prop('checked', true).change();
    }
    else {
        $("#IsManufacturer").prop('checked', false);
    }
    $('#VatNumber').val(RowData.VatNumber);
    $('#Longitude').val(RowData.Longitude);
    $('#Latitude').val(RowData.Latitude);

    FormType = 2;
    $("#CloseShowdataModel").click();
    $.ajax({
        url: "/Partner/GetPartnerContacts",
        type: "GET",
        data: { ID: PartnerId },
        success: function (data) {
            ;
            ContactDataTable.clear().draw();
            for (var i = 0; i < data.PartnerContacts.length; i++) {
                ContactDataTable.row.add(["",
                    data.PartnerContacts[i].ID,
                     data.PartnerContacts[i].Contact,
                     data.PartnerContacts[i].Phone,
                      data.PartnerContacts[i].Contact_PersonPosition,
                       data.PartnerContacts[i].Contact_PersonMail,
                         "<span class='btn btn-info' onclick='Remove(this)'><i class='glyphicon glyphicon-remove'></i></span>"
                ]).draw(false);
            }
        },
        error: function (data) {
            
        }
    })
    ////////////////Linked Products////////////////
    $.ajax({
        url: "/Partner/GetPartnerLinkedProducts",
        type: "Post",
        data: { PartnerCode: PartnerCode },
        success: function (data) {
            tblProducts.clear().draw();
            InsertIntoProductTable(data);
        },
        error: function (data) {
 
        }
    })

}
function PassID(index) {

    ID = index;
}
function New() {
    $("#NID").val("");
    $("#CustomerCode").val("");
    $("#DepartmentId").val("").change();
    //ContactDataTable.destroy();
    ContactDataTable.clear().draw();
    ContactDataTable.search('').draw();
    $("#Active").prop("checked", false);
    $("#Partner_Name").val("");
    $("#Address").val("");
    $("#Email").val("");
    $("#Mobile").val("");
    $("#Phone1").val("");
    $("#Phone2").val("");
    $("#Fax").val("");
    $("#Website").val("");
    $("#Contact_Person").val("");
    $("#Tax_Record_Id").val("");
    $("#Commercial_Id").val("");
    $("#Bank_Acc1").val("");
    $("#Acc1_Bank_Id").val("").change();
    $("#Bank_Acc2").val("");
    $("#Acc2_Bank_Id").val("").change();
    $("#Group_Id").val(1);
    $("#Partner_Type").val("").change();
    $("#Apply_Tax").val(1);
    $("#Multi_Shipping").val(1);
    $("#Multi_Billing").val(1);
    $("#Payment_Term").val("").change();
    $("#Starting_Balance").val("");
    $("#Type").val("").change();
    $("#Price_List_Id").val("").change();
    $("#City_Id").val("").change();
    $("#Zone").val("");
    ID = 0;
    FormType = 1;
    $("#PartnerCode").val("").attr('disabled', false);
    $("#IsPostpond").prop("checked", false);
    $("#IsDeliveryMan").prop("checked", false);
    /////////////

    //   Area 
    $("#City").val("");
    $("#Street").val("");
    $("#BuildingNumber").val("");
    $("#FlatNumber").val("");
    ////////
    $("#Country").val("").change();
    $("#Province").val("").change();
    $("#City").val("").change();
    $("#District").val("").change();
    $("#Terriory").val("").change();
    ProvinceId = null;
    City = null;
    District = null;
    Terriory = null;

    //Link Products to Vendor
    $("#Category").val("").change();
    $("#SubCategory").val("").change();
    tblProducts.clear().draw();
    tblProducts.search('').draw();
    tblSearchProducts.clear().draw();
    tblSearchProducts.search('').draw();
    /////////////////////////////
    $('#VatNumber').val('');
    $('#Longitude').val('');
    $('#Latitude').val('');
    $('#IsManufacturer').prop("checked", false);
}
function CheckValidation() {
    if ($("#Country").val() == "" && CompanyId == 'MNM' && ($('#Partner_Type').val() == 2 || $('#Partner_Type').val() == 3)) {
        $("#CloseModel").click();
        if ($("#language").html() == "اللغة") {
            AlertNotify("info", "عفوا يجب ادخال البلد ");
        }
        else { AlertNotify("info", "Sorry, you must enter Country!"); }
        return false;
    }
    if ($("#DepartmentId").val() == "" && CompanyId == 'MNM' && ($('#Partner_Type').val() == 4)) {
        $("#CloseModel").click();
        if ($("#language").html() == "اللغة") {
            AlertNotify("info", "عفوا يجب اختيار القسم الخاص بالموظف ");
        }
        else { AlertNotify("info", "Sorry, you must select the employee`s department!"); }
        return false;
    }
    if ($("#Address").val() == "" && CompanyId == 'MNM') {
        $("#CloseModel").click();
        if ($("#language").html() == "اللغة") {
            AlertNotify("info", "عفوا يجب ادخال العنوان بالتفصيل ");
        }
        else { AlertNotify("info", "Sorry, you must enter detailed address!"); }
        return false;
    }
    if (PartnerCode == "") {
        $("#CloseModel").click();
        if ($("#language").html() == "اللغة") {
            AlertNotify("info", "عفوا يجب ادخال كود الشريك ");
        }
        else { AlertNotify("info", "Sorry, you must enter partner code!"); }
        return false;
    }
    var Form = $("#Form");
    Form.validate();
    if (Form.valid()) {
        $("#ModalConfirm").modal();
    }
    else {
        if ($("#language").html() == "اللغة") { AlertNotify("info", "من فضلك املا البيانات المطلوبة"); }
        else { AlertNotify("info", "Please fill in required fields"); }
    }
}
function Save() {
    SessionState(URL);
    FillData();
    if (PartnerContact.length == 0 && $('#Partner_Type').val() == 1) {
        $("#CloseModel").click();
        if ($("#language").html() == "اللغة") { AlertNotify("info", "لابد من ادخال شخص مفوض علي الاقل للحفظ"); }
        else { AlertNotify("info", "Insert Contact Person For Save"); }
        return false;
    }
    if ($("#NID").val() == "" && $('#Partner_Type').val() == 4) {
        $("#CloseModel").click();
        if ($("#language").html() == "اللغة") { AlertNotify("info", "لابد من ادخال رقم البطاقه"); }
        else { AlertNotify("info", "must insert NID !!"); }
        return false;
    }
    StopSave("BtnSave");
    ///////
    var ApplyTax = 0;
    if ($('#ApplyTax').is(':checked'))
        ApplyTax = 1;

    var MultiShipping = 0;
    if ($('#MultiShipping').is(':checked'))
        MultiShipping = 1;

    var MultiBilling = 0;
    if ($('#MultiBilling').is(':checked'))
        MultiBilling = 1;

    var Active = false;
    if ($('#Active').is(':checked'))
        Active = true;

    var IsPostpon = false;
    if ($('#IsPostpond').is(':checked'))
        IsPostpon = true;
  
    var IsDeliveryMan = false;
    if ($('#IsDeliveryMan').is(':checked'))
        IsDeliveryMan = true;
    ////Linked Products
    var _LinkedProducts = [];
    var Rows = tblProducts.rows().nodes();
    Rows.each(function (index) {
        var Row = tblProducts.row(index);
        var Data = Row.data();
        _LinkedProducts.push({ID: Data[0], Barcode: Data[1], Name: Data[2] });
    });
    //////////////////////
    var IsManufacturer = false;
    if ($('#IsManufacturer').is(':checked'))
        IsManufacturer = true;
    ////
    var PartnerVM = {
        Partner_Id: ID,
        Partner_Name: $("#Partner_Name").val(),
        Address: $("#Address").val(),
        Email: $("#Email").val(),
        Mobile: $("#Mobile").val(),
        Phone1: $("#Phone1").val(),
        Phone2: $("#Phone2").val(),
        Fax: $("#Fax").val(),
        Website: $("#Website").val(),
        Contact_Person: $("#Contact_Person").val(),
        Tax_Record_Id: $("#Tax_Record_Id").val(),
        Commercial_Id: $("#Commercial_Id").val(),
        Bank_Acc1: $("#Bank_Acc1").val(),
        Acc1_Bank_Id: $("#Acc1_Bank_Id").val(),
        Bank_Acc2: $("#Bank_Acc2").val(),
        Acc2_Bank_Id: $("#Acc2_Bank_Id").val(),
        Group_Id: 1,
        Partner_Type: $("#Partner_Type").val(),
        Apply_Tax: 1,
        Multi_Shipping: 1,
        Multi_Billing: 1,
        Payment_Term: $("#Payment_Term").val(),
        Starting_Balance: $("#Starting_Balance").val(),
        Type: $("#Type").val(),
        Price_List_Id: $("#Price_List_Id").val(),
        Zone: $("#Zone").val(),
        City_Id: $("#City_Id").val(),
        FormType: FormType,
        PartnerContacts: PartnerContact,
        Active: Active,
        Apply_Tax: ApplyTax,
        Multi_Billing: MultiBilling,
        Multi_Shipping: MultiShipping,
        PartnerCode: $("#PartnerCode").val(),
        PartnerSegmentation: $("#PartnerSegmentation").val(),
        IsPostpond: IsPostpon,
        IsDeliveryMan: IsDeliveryMan,
        /////////  
        City: $("#City").val(),
        Street: $("#Street").val(),
        BuildingNumber: $("#BuildingNumber").val(),
        FlatNumber: $("#FlatNumber").val(),
        NID: $("#NID").val(),
        ////////////////
        CountryId: $("#Country").val(),
        CityId: $("#City").val(),
        ProvinceId: $("#Province").val(),
        DistrictId: $("#District").val(),
        TerritoryId: $("#Terriory").val(),
        CustomerCode: $("#CustomerCode").val(),
        DepartmentId: $("#DepartmentId").val(),
        LinkedProducts: _LinkedProducts,
        ////////////////////
        IsManufacturer: IsManufacturer,
        Longitude: $('#Longitude').val(),
        Latitude:$('#Latitude').val(),
        VatNumber: $('#VatNumber').val()
    };
    var data = { PartnerVM, UploadFile: UploadFile};
    $.ajax({
            type: "POST",
            cache: false,
            async: true,
            datatype: 'json',
            contenttype: 'application/json; charset=utf-8',
            url: '/Partner/Save',
        //  data: JSON.stringify(data),
            data: data,
            success: function(data) {
            ;
            if (data==1) {
                $("#CloseModel").click();
                if ($("#language").html() == "اللغة") { AlertNotify("success", "تم الحفظ بنجاح"); }
    else { AlertNotify("success", "Saved Successfully"); }
                setTimeout(function () { location.reload(); }, 2000)
    }
    else {
                EnableSave("BtnSave");
                $("#CloseModel").click();
                if ($("#language").html() == "اللغة") { AlertNotify("info", "من فضلك املا البيانات المطلوبة"); }
                else { AlertNotify("info", "Please fill in required fields"); }

            }
            },
        error: function (data) {
            EnableSave("BtnSave");
        }
    })
    EnableSave("BtnSave");

}
function FillData() {
    PartnerContact = [];
    var Rows = ContactDataTable.rows({ 'search': 'applied' }).nodes();
    Rows.each(function (index) {
        var Row = ContactDataTable.row(index);
        var Data = Row.data();
        PartnerContact.push({
            ID: Data[1],
            Contact: Data[2],
            Phone: Data[3],
            Contact_PersonPosition: Data[4],
            Contact_PersonMail: Data[5]
        });
    })
}
function Delete() {
    SessionState(URL);
    $.ajax({
        url: '/Partner/Delete',
        data: { ID: ID },
        success: function (data) {
            if (data == 1) {
                $("#CloseShowdataModel").click();
                $("#CloseDeleteModel").click();
                if ($("#language").html() == "اللغة") { AlertNotify("success", "تم تغيير الحالة بنجاح"); }
                else { AlertNotify("success", "Status Changed Successfully"); }
                setTimeout(function () { location.reload(); }, 2000)
            }
            else {
                $("#CloseShowdataModel").click();
                $("#CloseDeleteModel").click();
                if ($("#language").html() == "اللغة") { AlertNotify("info", "حدث خطا اثناء الحفظ تاكد من الاتصال بالانترنت"); }
                else { AlertNotify("info", "Error Happen While Save"); }
                setTimeout(function () { location.reload(); }, 2000)
            }
        },
        error: function (data) { }
    })
}
function isNumberKey(evt) {

    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 44 || charCode > 57))
        return false;
    return true;
}

$("#CloseShowdataModel").click(function () {
    DataTable.destroy();
})
$("#Xbtn").click(function () {
    DataTable.destroy();
})
//$("#Phone1").change(function () {

// $.ajax({
//   url: "/Partner/CheckPhone",
//   type: "GET",
// data: { Number: $("#Phone1").val() },
// success: function (data) {
// if (data == 1) {
//   $("#Phone1").val('');
// if ($("#language").html() == "اللغة") {
//  AlertNotify("info","عفوا لقد ادخلت رقم   موجود  من قبل ");}

// else{AlertNotify("info","Sorry, You Entered Existing  Number!");}
// }
//  },
// error: function (data) { }
//  });
//})
$("#PartnerCode").change(function () {
    SessionState(URL);
    $.ajax({
        url: "/Partner/CheckPartnerCode",
        type: "GET",
        data: { PartnerCode: $("#PartnerCode").val() },
        success: function (data) {

            if (data == 1 && FormType == 1) {
                $("#PartnerCode").val('');
                if ($("#language").html() == "اللغة") {
                    AlertNotify("info", "عفوا لقد ادخلت كود شريك  موجود  من قبل ");
                }
                else { AlertNotify("info", "Sorry, You Entered Existing partner code!"); }
            }
            if (data == 1 && FormType == 2 && $("#PartnerCode").val() != PartnerCode) {
                $("#PartnerCode").val('');
                if ($("#language").html() == "اللغة") {
                    AlertNotify("info", "عفوا لقد ادخلت كود شريك  موجود  من قبل ");
                }
                else { AlertNotify("info", "Sorry, You Entered Existing partner code!"); }
            }
        },
        error: function (data) { }
    });
})
$("#Phone2").change(function () {
    SessionState(URL);
    $.ajax({
        url: "/Partner/CheckPhone",
        type: "GET",
        data: { Number: $("#Phone2").val() },
        success: function (data) {
            if (data == 1) {
                $("#Phone2").val('');
                if ($("#language").html() == "اللغة") {
                    AlertNotify("info", "عفوا لقد ادخلت رقم   موجود  من قبل ");
                }

                else { AlertNotify("info", "Sorry, You Entered Existing  Number!"); }
            }
        },
        error: function (data) { }
    });
})
$("#Mobile").change(function () {
    SessionState(URL);
    $.ajax({
        url: "/Partner/CheckPhone",
        type: "GET",
        data: { Number: $("#Mobile").val() },
        success: function (data) {
            if (data == 1 && FormType == 1) {
                $("#Mobile").val('');
                if ($("#language").html() == "اللغة") {
                    AlertNotify("info", "عفوا لقد ادخلت رقم   موجود  من قبل ");
                }
                else { AlertNotify("info", "Sorry, You Entered Existing  Number!"); }
            }
            if (data == 1 && FormType == 2 && $("#Mobile").val() != Mobile) {
                $("#Mobile").val('');
                if ($("#language").html() == "اللغة") {
                    AlertNotify("info", "عفوا لقد ادخلت رقم   موجود  من قبل ");
                }
                else { AlertNotify("info", "Sorry, You Entered Existing  Number!"); }
            }
        },
        error: function (data) { }
    });
})
$("#Fax").change(function () {
    SessionState(URL);
    $.ajax({
        url: "/Partner/CheckFax",
        type: "GET",
        data: { Number: $("#Fax").val() },
        success: function (data) {
            if (data == 1 && FormType == 1) {
                $("#Fax").val('');
                if ($("#language").html() == "اللغة") {
                    AlertNotify("info", "عفوا لقد ادخلت رقم   موجود  من قبل ");
                }
                else { AlertNotify("info", "Sorry, You Entered Existing  Number!"); }
            }
            if (data == 1 && FormType == 2 && $("#Fax").val() != Fax) {
                $("#Fax").val('');
                if ($("#language").html() == "اللغة") {
                    AlertNotify("info", "عفوا لقد ادخلت رقم   موجود  من قبل ");
                }
                else { AlertNotify("info", "Sorry, You Entered Existing  Number!"); }
            }
        },
        error: function (data) { }
    });
})
$("#Tax_Record_Id").change(function () {
    SessionState(URL);
    $.ajax({
        url: "/Partner/CheckTax",
        type: "GET",
        data: { Number: $("#Tax_Record_Id").val() },
        success: function (data) {
            if (data == 1) {
                $("#Tax_Record_Id").val('');
                if ($("#language").html() == "اللغة") {
                    AlertNotify("info", "عفوا لقد ادخلت رقم   موجود  من قبل ");
                }

                else { AlertNotify("info", "Sorry, You Entered Existing  Number!"); }
            }
        },
        error: function (data) { }
    });
})
$("#Commercial_Id").change(function () {
    SessionState(URL);
    $.ajax({
        url: "/Partner/CheckCommerical",
        type: "GET",
        data: { Number: $("#Commercial_Id").val() },
        success: function (data) {
            if (data == 1) {
                $("#Commercial_Id").val('');
                if ($("#language").html() == "اللغة") {
                    AlertNotify("info", "عفوا لقد ادخلت رقم   موجود  من قبل ");
                }

                else { AlertNotify("info", "Sorry, You Entered Existing  Number!"); }
            }
        },
        error: function (data) { }
    });
})
$("#Partner_Type").change(function () {
    document.getElementById('Div_NotEmployee').style.display = "initial";
    document.getElementById('Div_IsPostpond').style.display = "initial";
    document.getElementById('DivIsDeliveryMan').style.display = "none";
    document.getElementById('Div_LinkProductsToVendors').style.display = "none";
    document.getElementById('DivIsManufacturer').style.display = 'none';

    if ($('#Partner_Type').val() == "1") {
        document.getElementById('supplieratributes').style.display = "initial";
        document.getElementById('Div_LinkProductsToVendors').style.display = "block";
        document.getElementById('DivIsManufacturer').style.display = 'block';
    }
    else if ($('#Partner_Type').val() == "3" || $('#Partner_Type').val() == "2") {
        document.getElementById('supplieratributes').style.display = "none";
    }
    else if ($('#Partner_Type').val() == "4") {
        document.getElementById('supplieratributes').style.display = "none";
        document.getElementById('Div_NotEmployee').style.display = "none";
        document.getElementById('Div_IsPostpond').style.display = "none";
        document.getElementById('DivIsDeliveryMan').style.display = "initial";
        Payment_Term
    }

})
$("#Insert").click(function () {
    alreadyexist = 0;
    if ($("#Contact_Person").val() != "") {
        if ($("#Phone1").val() != "") {
            for (var i = 0; i < ContactDataTable.rows().data().length; i++) {
                if (ContactDataTable.rows().data()[i][2] == $("#Phone1").val()) {
                    alreadyexist = 1;
                    $("#Phone1").val("");
                    $("#Contact_Person").val("");
                    if ($("#language").html() == "اللغة") { AlertNotify("info", "رقم التليفون موجود مسبقا"); }
                    else { AlertNotify("info", "Phone number allready exist "); }
                }
            }
            if (alreadyexist == 0) {
                ContactDataTable.row.add(["",
                                     0,
                                        $("#Contact_Person").val(),
                                        $("#Phone1").val(),
                                        $("#Contact_PersonPosition").val(),
                                        $("#Contact_PersonMail").val(),
                                        "<span class='btn btn-info' onclick='Remove(this)'><i class='glyphicon glyphicon-remove'></i></span>"
                ]).draw(false);
            }
            $("#Phone1").val("");
            $("#Contact_Person").val("");
            $("#Contact_PersonPosition").val("");
            $("#Contact_PersonMail").val("");
        }
        else {
            if ($("#language").html() == "اللغة") { AlertNotify("info", "يجب ادخال رقم الهاتف"); }
            else { AlertNotify("info", "Must insert phone  number!!"); }
        }
    }
    else {
        if ($("#language").html() == "اللغة") { AlertNotify("info", "يجب ادخال شخص الاتصال"); }
        else { AlertNotify("info", "Must insert contact person"); }
    }
})
function Remove(Element) {
    ContactDataTable
      .row($(Element).parents('tr'))
      .remove()
      .draw();
}

function GetChildrens(dataId, resultId) {
    $.ajax({
        url: "/Partner/GetChlidreens",
        type: "GET",
        data: { ParentId: $(dataId).val(), ID: ID },
        success: function (data) {
            $(resultId).empty();
            if ($("#language").html() == "اللغة") {
                $(resultId).append("<option selected value=''>اختر</option>")
            }
            else {
                $(resultId).append("<option selected value=''>Please Select</option>")
            }
            $(resultId).val($(resultId + " option:first").val()).change();
            for (var i = 0; i < data.length; i++) {
                if ($("#language").html() == "اللغة") {
                    $(resultId).append("<option value=" + data[i].ID + ">" + data[i].Name + "</option>")
                }
                else { $(resultId).append("<option value=" + data[i].ID + ">" + data[i].NameEn + "</option>") }
            }
        },
        error: function (data) { }
    });
}

$("#Country").change(function () {
    SessionState(URL);
    $.ajax({
        url: "/Partner/GetChlidreens",
        type: "GET",
        data: { ParentId: $("#Country").val() },
        success: function (data) {
            $("#Province").empty();
            if ($("#language").html() == "اللغة") {
                $("#Province").append("<option selected value=''>اختر</option>")
            }
            else {
                $("#Province").append("<option selected value=''>Please Select</option>")
            }
            for (var i = 0; i < data.length; i++) {
                if ($("#language").html() == "اللغة") {
                    $("#Province").append("<option value=" + data[i].ID + ">" + data[i].Name + "</option>")
                }
                else { $("#Province").append("<option value=" + data[i].ID + ">" + data[i].NameEn + "</option>") }
            }
            $("#Province").val(ProvinceId).change();
        },
        error: function (data) { }
    });
})

$("#Province").change(function () {
    SessionState(URL);
    $.ajax({
        url: "/Partner/GetChlidreens",
        type: "GET",
        data: { ParentId: $("#Province").val() },
        success: function (data) {
            $("#City").empty();
            if ($("#language").html() == "اللغة") {
                $("#City").append("<option selected value=''>اختر</option>")
            }
            else {
                $("#City").append("<option selected value=''>Please Select</option>")
            }
            for (var i = 0; i < data.length; i++) {
                if ($("#language").html() == "اللغة") {
                    $("#City").append("<option value=" + data[i].ID + ">" + data[i].Name + "</option>")
                }
                else { $("#City").append("<option value=" + data[i].ID + ">" + data[i].NameEn + "</option>") }
            }
            $("#City").val(City).change();
        },
        error: function (data) { }
    });
});
$("#City").change(function () {
    SessionState(URL);
    $.ajax({
        url: "/Partner/GetChlidreens",
        type: "GET",
        data: { ParentId: $("#City").val() },
        success: function (data) {
            $("#District").empty();
            if ($("#language").html() == "اللغة") {
                $("#District").append("<option selected value=''>اختر</option>")
            }
            else {
                $("#District").append("<option selected value=''>Please Select</option>")
            }
            for (var i = 0; i < data.length; i++) {
                if ($("#language").html() == "اللغة") {
                    $("#District").append("<option value=" + data[i].ID + ">" + data[i].Name + "</option>")
                }
                else { $("#District").append("<option value=" + data[i].ID + ">" + data[i].NameEn + "</option>") }
            }
            $("#District").val(District).change();
        },
        error: function (data) { }
    });
});
$("#District").change(function () {
    SessionState(URL);
    $.ajax({
        url: "/Partner/GetChlidreens",
        type: "GET",
        data: { ParentId: $("#District").val() },
        success: function (data) {
            $("#Terriory").empty();
            if ($("#language").html() == "اللغة") {
                $("#Terriory").append("<option selected value=''>اختر</option>")
            }
            else {
                $("#Terriory").append("<option selected value=''>Please Select</option>")
            }
            for (var i = 0; i < data.length; i++) {
                if ($("#language").html() == "اللغة") {
                    $("#Terriory").append("<option value=" + data[i].ID + ">" + data[i].Name + "</option>")
                }
                else { $("#Terriory").append("<option value=" + data[i].ID + ">" + data[i].NameEn + "</option>") }
            }
            $("#Terriory").val(Terriory).change();
        },
        error: function (data) { }
    });
});

$("#NID").change(function () {
    SessionState(URL);
    $.ajax({
        url: "/Partner/CheckNID",
        type: "GET",
        data: { Number: $("#NID").val() },
        success: function (data) {
            if (data == 1 && FormType == 1) {
                $("#NID").val('');
                if ($("#language").html() == "اللغة") {
                    AlertNotify("info", "عفوا لقد ادخلت رقم بطاقه موجود  من قبل ");
                }
                else { AlertNotify("info", "Sorry, You Entered Existing NID !"); }
            }
            if (data == 1 && FormType == 2 && $("#NID").val() != NID) {
                $("#NID").val('');
                if ($("#language").html() == "اللغة") {
                    AlertNotify("info", "عفوا لقد ادخلت رقم بطاقه موجود من قبل ");
                }
                else { AlertNotify("info", "Sorry, You Entered Existing  NID!"); }
            }
        },
        error: function (data) { }
    });
})
$('#ItemsFile').on('change', function () {

    var FileName = $("#ItemsFile")[0].value.split(/(\\|\/)/g).pop();
    if (FileName != "") {
        var formData = new FormData();
        var File = $("#ItemsFile")[0].files[0];
        formData.append(File.name, File);
        UploadFileAndReadFromIT(formData);
    }
    $("#ItemsFile")[0].value = "";
});
function UploadFileAndReadFromIT(_formData, ItemType) {

    SessionState(URL);
    showloader();
    $.ajax({
        url: '/Partner/Upload',
        type: 'POST',
        data: _formData,
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {
            ;
            if (data.Active == 1) {
                hideloader();
                if ($("#language").html() == "اللغة") { AlertNotify("success", "تم ادراج الشركاء بنجاح"); }
                else { AlertNotify("success", "Employee inserted successfully"); }
                setTimeout(function () { location.reload(); }, 2000)
            }
            else {
                hideloader();
                var ItemError = "";
                if ($("#language").html() == "اللغة") {
                    ItemError = "يوجد خطأ بالبيانات في هذه الصفوف:                         " + data.Address;
                }
                else {
                    ItemError = "in correct data in these rows:             " + data.Address;
                }

                $("#Note").val(ItemError);
                $('#ModalDamagedCodes').modal("show");;
                setTimeout(function () {
                }, 20000)
            }
        },
        error: function () { hideloader(); },
    });
}

function Download() {
    window.open("/Content/Excell/Partners Template.xlsx");
}
function saveTextAsFile(textToWrite, fileNameToSaveAs) {
    textToWrite = textToWrite.replace(/\n/g, "\r\n"); // To retain the Line breaks.
    var textFileAsBlob = new Blob([textToWrite], { type: 'text/plain' });
    var downloadLink = document.createElement("a");
    downloadLink.download = fileNameToSaveAs;
    downloadLink.innerHTML = "Download File";
    if (window.webkitURL != null) {
        // Chrome allows the link to be clicked
        // without actually adding it to the DOM.
        downloadLink.href = window.webkitURL.createObjectURL(textFileAsBlob);
    }
    else {
        // Firefox requires the link to be added to the DOM
        // before it can be clicked.
        downloadLink.href = window.URL.createObjectURL(textFileAsBlob);
        downloadLink.onclick = destroyClickedElement;
        downloadLink.style.display = "none";
        document.body.appendChild(downloadLink);
    }
    downloadLink.click();
}
////////////////Link Product To Vendor///////////////////////////
$("#Category").change(function () {
    SessionState(URL);

    if ($("#Category").val() != null && $("#Category").val().length > 0) {
        var CategoryList = $("#Category").val() == null ? [] : $("#Category").val();
        var CompanyId = $("#SuperMarket").val();
        $.ajax({
            url: "/SalesByItemOrCategoryRpt/GetChlidreens",
            type: "Post",
            data: { ParentId: CategoryList, CompanyId: CompanyId },
            success: function (data) {
                var options = "";
                let SubCategoryValues = $("#SubCategory").val() == null ? [] : $("#SubCategory").val();
                let NewValues = [];
                if (SubCategoryValues.length > 0) {
                    for(item of data) {
                        let IsExist = false;
                        for(Code of SubCategoryValues) {
                            if (Code == item.ID) {
                                IsExist = true; break;
                            }
                        }
                        if (IsExist) {
                            NewValues.push(item.ID);
                            SubCategoryValues.unshift(item.ID);
                        }
                        if ($("#language").html() == "اللغة") {
                            options += `<option value="${item.ID}">${item.Name}</option>`;
                        }
                        else {
                            options += `<option value="${item.ID}">${item.NameEn}</option>`;
                        }
                    }
                }
                else {
                    for(item of data) {
                        if ($("#language").html() == "اللغة") {
                            options += `<option value="${item.ID}">${item.Name}</option>`;
                        }
                        else {
                            options += `<option value="${item.ID}">${item.NameEn}</option>`;
                        }
                    }
                }

                $("#SubCategory").html(options);
                $("#SubCategory").val(NewValues).change();
                //for (var i = 0; i < data.length; i++) {
                //    if ($("#language").html() == "اللغة") {
                //        $("#SubCategory").append("<option value=" + data[i].ID + ">" + data[i].Name + "</option>")
                //    }
                //    else { $("#SubCategory").append("<option value=" + data[i].ID + ">" + data[i].NameEn + "</option>") }
                //}
            },
            error: function (data) { }
        });

    }
    else {
        $("#SubCategory").html("");
        $("#SubCategory").val([]).change()
        $("#Items").html("");
        $("#Items").val([]).change()
    }
});
$("#btnInsertItem").click(function () {
    GetProdoctsDropdown();
});



$("#btnSearchProductCodeOrName").click(function () {
    SessionState();
    let ProductCodeOrName = $("#ProductCodeOrName").val();
    $.ajax({
        url: "/Partner/GetProductByCodeOrName",
        type: "Post",
        data: { ProductCodeOrName: ProductCodeOrName },
        success: function (data) {

            $('#tblSearchProducts').DataTable().clear();
            $('#tblSearchProducts').DataTable().destroy();

            if ($("#language").html() == "اللغة") {
                tblSearchProducts = $('#tblSearchProducts').DataTable({
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
                    },
                    "columnDefs": [{ "targets": [0], visible: false }],
                    "searching": false,
                    "Length": false,
                    data: data,
                    columns: [
                      { 'data': 'ID' },
                      { 'data': 'Barcode' },
                      { 'data': 'Name' },
                      { 'data': 'CheckBox' }
                    ]

                });
            }
            else {
                tblSearchProducts = $("#tblSearchProducts").DataTable({
                    "columnDefs": [{ "targets": [0], visible: false }],
                    "searching": false,
                    "Length": false,
                    data: data,
                    columns: [
                      { 'data': 'ID' },
                      { 'data': 'Barcode' },
                      { 'data': 'Name' },
                      { 'data': 'CheckBox' }
                    ]
                });
            } 
        },
        error: function (data) { }
    });
});
$("#selectallProducts").change(function () {
    if ($("#selectallProducts").is(':checked')) {
        var Rows = tblSearchProducts.rows({ 'search': 'applied' }).nodes();
        Rows.each(function (index) {
            $(index).find('.Select').prop('checked', true);
        })
    } else {
        var Rows = tblSearchProducts.rows({ 'search': 'applied' }).nodes();
        Rows.each(function (index) {
            $(index).find('.Select').prop('checked', false);

        })
    }
});
$("#btnDone").click(function () {

    var AllSelectedProducts = [];
    var Rows = tblSearchProducts.rows({ 'search': 'applied' }).nodes();
    Rows.each(function (index) {
        if ($(index).find('.Select').is(':checked')) {
            var Data = tblSearchProducts.row(index).data();

            AllSelectedProducts.push({ ID: Data.ID, Barcode: Data.Barcode, Name: Data.Name });
        }
    });

    InsertIntoProductTable(AllSelectedProducts);
    $("#selectallProducts").prop('checked', true);
    tblSearchProducts.clear().draw();
    tblSearchProducts.search('').draw();
    $("#CloseModalSearchProducts").click();

});
function ChagneStatusOfCheckedBox(e) { 
    debugger
    if (!($(e).is(':checked'))) {
        $("#selectallProducts").prop('checked', false);

    }
    else {
        if ($("#tblSearchProducts td input:checkbox:not(:checked)").length == 0) {
            $("#selectallProducts").prop('checked', true);
        }
    }
};
function RemoveProduct(e) {
    tblProducts
      .row($(e).parents('tr'))
      .remove()
      .draw();
}
function GetProdoctsDropdown() {
    var CategoriesIds = $("#Category").val() == null ? [] : $("#Category").val();
    var SubCategoriesIds = $("#SubCategory").val() == null ? [] : $("#SubCategory").val();
    var Result;

    if (CategoriesIds.length < 1) {
        if ($("#language").html() == "اللغة") { Result = confirm("هل تريد اضافة جميع المنتجات ؟!"); }
        else { Result = confirm("Are you want to add all products ?!", "Harry Potter"); }
        if (Result == false) {
            return false;
        }
    }
    $.ajax({
        url: "/Partner/GetItemsByCategoryAndSubCategory",
        type: "Post",
        data: { CategoriesIds: CategoriesIds, SubCategoriesIds: SubCategoriesIds },
        success: function (data) {
            InsertIntoProductTable(data);
        },
        error: function (data) { }
    });

}
function InsertIntoProductTable(data) {
    var IsExist = false;
    for (var item of data) {
        var Rows = tblProducts.rows().nodes();
        Rows.each(function (index) {
            var Row = tblProducts.row(index);
            var Data = Row.data();
            if (Data[0] == item.ID) {
                IsExist = true;
                return;
            }
        });
        if (IsExist == false) {
            tblProducts.row.add([item.ID, item.Barcode, item.Name,
                                "<span class='btn btn-info' onclick='RemoveProduct(this)'><i class='glyphicon glyphicon-remove'></i></span>"
            ]).draw(false);
        }
    }
}

$('#LinkedProductsVendorsFile').on('change', function () {
    var FileName = $("#LinkedProductsVendorsFile")[0].value.split(/(\\|\/)/g).pop();
    if (FileName != "") {
        var formData = new FormData();
        var File = $("#LinkedProductsVendorsFile")[0].files[0];
        formData.append(File.name, File);
        LinkedProductsVendors_UploadFileAndReadFromIT(formData);
    }
    else {
        UploadFile = 0;
        $("#LinkedProductsVendorsFile")[0].value = "";

    }
});
function LinkedProductsVendors_UploadFileAndReadFromIT(_formData, ItemType) {

    SessionState(URL);
    showloader();
    $.ajax({
        url: '/Partner/UploadLinkedProductsVendors',
        type: 'POST',
        data: _formData,
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data.Status == 1) {
                hideloader();
                if ($("#language").html() == "اللغة") { AlertNotify("success", "تم الادراج بنجاح"); }
                else { AlertNotify("success", "inserted successfully"); }
                UploadFile = 1;
            }
            else {
                UploadFile = 0;
                $("#LinkedProductsVendorsFile")[0].value = "";
                hideloader();
                var ItemError = "";
                if ($("#language").html() == "اللغة") {
                    for (var i of data.Result) {
                        ItemError +=  i.PartnerCode;
                    }
                }
                else {
                    for (var i of data.Result) {
                        ItemError +=  i.PartnerCode;
                    }
                }

                $("#Note").val(ItemError);
                $('#ModalDamagedCodes').modal("show");;
            }
        },
        error: function () { hideloader(); },
    });
}