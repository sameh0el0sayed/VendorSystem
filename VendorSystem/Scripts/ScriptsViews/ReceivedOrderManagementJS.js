var URL = "/OrderManagement/Received";
var DataTable;
var tblRecievedPOs;
let DocumnetNumber = "";
let IsShippingPo = false;
let IsRejected = false;
let MasterID = 0;

$(function () {


    if ($("#language").html() == "  اللغة") {
        $("#ExpectedDeliveredDate").datepicker({
            format: 'dd/mm/yyyy', showClose: true, showClear: true, keepInvalid: true
        }).on('dp.change', function () {
        });
        $("#ExpectedDeliveredDate").css("direction", "ltr");
        $("#ExpectedDeliveredDate").css("text-align", "right");
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
            },
            iDisplayLength: -1,
            aLengthMenu: [
                 [-1], ["All"]
            ]
        });
        tblRecievedPOs = $("#tblReceivedPosData").DataTable({
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
    }
    else {
        $("#ExpectedDeliveredDate").css("direction", "ltr");
        $("#ExpectedDeliveredDate").css("text-align", "left");

        $("#ExpectedDeliveredDate").datepicker({
            format: 'dd/mm/yyyy', showClose: true, showClear: true, keepInvalid: true
        }).on('dp.change', function () {
        });
        DataTable = $("#tbShowData").DataTable({
            iDisplayLength: -1,
            aLengthMenu: [
                 [-1], ["All"]
            ]
        });
        tblRecievedPOs = $("#tblReceivedPosData").DataTable({
        });
    }

    $('#StoreId').attr('disabled', 'disabled');
    $('#PartnerId').attr('disabled', 'disabled');
});

 //Sameh Code 28062021 
function Search() {
    showloader();
    tblRecievedPOs.clear().draw();
    $.ajax({
        url: "/OrderManagement/GetPOsPerStatusID",
        type: "Post",
        data: { StatusID: 2 },
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                tblRecievedPOs.row.add([
                       data[i].DocumentNumber,
                       data[i].StoreName,
                       data[i].SentDate,
                       data[i].ExpectedDeliveredDate,
                       data[i].CustomerCode,
                       "<span class=' glyphicon glyphicon-edit edit'  style='cursor: pointer;' title='" + data[i].DocumentNumber + "' id=" + data[i].ID + " onclick='Edit(this)'>"
                ]).draw(false);
            }
            hideloader();
        },
        error: function (data) {
            hideloader();
        }
    });
    $("#ModalShowReceivedPosData").modal();
}

function Edit(Element) {
    MasterID = Element.id;
    console.log(MasterID);
    $.ajax({
        url: "/OrderManagement/GetPoByMasterID",
        type: "Post",
        data: { _MasterID: MasterID },
        success: function (data) {

            $("#btnPrintReceivedOrder").show();

            DataTable.clear().draw();
            $("#CloseModalShowReceivedPosData").click();
            $("#DocumentNumber").val(data[0].DocumentNumber);
            $("#StoreId").val(data[0].StoreId).change().attr('disabled', true);
            $("#CustomerCode").val(data[0].CustomerCode); 
            $("#RouteName").val(data[0].RouteName).attr('disabled', true);
            $("#RegionName").val(data[0].RegionName).attr('disabled', true);
            $("#TerritoryName").val(data[0].TerritoryName).attr('disabled', true);
            $("#ExpectedDeliveredDate").datepicker('setDate', data[0].ExpectedDeliveredDate);

            let Item;
            for (var i = 0; i < data.length; i++) {
                Item = data[i];
                DataTable.row.add([
                  Item.InternalCode,
                  Item.BarCode,
                  Item.ProductNa,
                  Item.SystemQty,
                  Item.ApprovedQty,
                 "<input type='number'  step='1' onChange='SaveInDatabase()'   class='ShippedQty form-control'  value='" + Item.ShippedQty + "'>", //ShippedQty
                 "<input type='number' min=0 step='0.5' onChange='SaveInDatabase()'  class='VendorUnitPrice form-control'  value='" + Item.VendorUnitPrice + "'>"
                ]).draw(false);
            }
            Summation(DataTable);
            $("#DivInsertData").show();
            $("#btnShipped").show();
            $("#btnReject").show();

            hideloader();
        },
        error: function (data) {
        }
    })
}



function Summation(MyDatatable) {
    var sum;
    MyDatatable.columns('.sum ').every(function () {
        var column = this;
        let IsComma = false;
        sum = column.data().reduce(function (a, b) {
  
            if (b.indexOf('input') > -1) {
                b = $(b).val();
            }
 
            for (var i = 0; i < a.length; i++) {
                if (a[i] == ",") {
                    IsComma = true;
                }
            }

           
            if (IsComma) {
                a = parseFloat(a.replace(/,/g, ''), 10);
                IsComma = false;
            }
            else {
                a = parseFloat(a, 10);
                IsComma = false;
            }


            if (isNaN(a)) { a = 0; }
            for (var i = 0; i < b.length; i++) {
                if (b[i] == ",") {
                    IsComma = true;
                }
            }
            if (IsComma) {
                b = parseFloat(b.replace(/,/g, ''), 10);
                IsComma = false;
            }
            else {
                b = parseFloat(b, 10);
                IsComma = false;
            }
            if (isNaN(b)) { b = 0; }
            return a + b;
        });
       
        if (isNaN(sum)) {
            if (sum.indexOf('input') > -1) {
                sum = $(sum).val();
            }
        }
         sum = parseFloat(sum).toFixed(2)
        if ($("#language").html() == "اللغة") { $(column.footer()).html('الاجمالى: ' + sum); }
        else { $(column.footer()).html('Total:   ' + sum); }
    });
}

$("#CategoryId").change(function () {
    $("#ProductId").empty();
    if ($("#CategoryId").val() == '') {
        return false;
    }
    FillDropDownProduct();
})

function FillDropDownProduct() {

    $.ajax({
        url: "/OrderManagement/GetProducts",
        type: "Post",
        async: false,
        data: { CategoryId: $("#CategoryId").val() },
        success: function (data) {

            if ($("#language").html() == "  اللغة") {
                $("#ProductId").append("<option selected value=''>اختر</option>")
            }
            else {
                $("#ProductId").append("<option selected value=''>Please Select</option>")
            }
            $("#ProductId").val($("#ProductId option:first").val()).change();
            for (var i = 0; i < data.length; i++) {
                if ($("#language").html() == "  اللغة") {
                    $("#ProductId").append("<option value=" + data[i].ID + ">" + data[i].Name + "</option>")
                }
                else { $("#ProductId").append("<option value=" + data[i].ID + ">" + data[i].Name + "</option>") }
            }
        },
        error: function (data) { }
    });
}

$("#Barcode").on('keypress', function (e) {
    if (e.which == 13) {
        alreadyexist = 0;

        if ($("#Barcode").val() == "") {
            return;
        }
        showloader();
        for (var i = 0; i < DataTable.rows().data().length; i++) {
            if (DataTable.rows().data()[i][1] == $("#Barcode").val()) {
                alreadyexist = 1;
                //means bring the cell no 6 of row no header+1 and so on 
                var OldQuantity = parseFloat($("#tbShowData tr:nth-child(" + (i + 1) + ") td:nth-child(6) input").val());
                $("#tbShowData tr:nth-child(" + (i + 1) + ") td:nth-child(6) input").val(OldQuantity + 1).change();
                $("#Barcode").val("");
                hideloader();
                break;
            }
        }
        //لو المنتج مش موجود في الجدول
        if (alreadyexist == 0) {

            InsertNewRecode($("#Barcode").val());
            $("#Barcode").val("");
        }
    }
})

function InsertNewRecode(Barcdoe) {
    $.ajax({
        type: "Post",
        url: '/OrderManagement/GetPriceAndProductAttributeVM',
        data: { Barcode: Barcdoe },
        success: function (Data) {

            if (Data == 0) {
                hideloader();
                AlertMe('info', 'this product does not exist!', 'هذا المنتج غير موجود')

                return false;
            }

            DataTable.row.add([
               Data.InternalCode,
            Barcdoe,
            Data.ProductName,
            0, // System Qty
            0, //ApprovedQty,
           "<input type='number' step='1' onChange='SaveInDatabase()'  class='ShippedQty form-control'  value='" + 1 + "'>",
           "<input type='number'  min=0 step='0.5' onChange='SaveInDatabase()' class='VendorUnitPrice form-control' value='" + Data.SellPrice + "'>"
            ]).draw(false);

            hideloader();
        },
        error: function () {
            hideloader();
        }
    });
}

$("#ProductId").change(function () {

    if ($("#ProductId").val() == '') {
        return false;
    }
    alreadyexist = 0;

    var BarCode = $("#ProductId").val();
    showloader();
    for (var i = 0; i < DataTable.rows().data().length; i++) {
        if (DataTable.rows().data()[i][1] == BarCode) {
            alreadyexist = 1;
            //means bring the cell no 6 of row no header+1 and so on 
            var OldQuantity = parseFloat($("#tbShowData tr:nth-child(" + (i + 1) + ") td:nth-child(6) input").val());
            $("#tbShowData tr:nth-child(" + (i + 1) + ") td:nth-child(6) input").val(OldQuantity + 1).change();
            break;
        }
    }
    //لو المنتج مش موجود في الجدول
    if (alreadyexist == 0) {
        InsertNewRecode($("#ProductId").val());
    }
    hideloader();
    $("#ProductId").val("").change();
})

function SaveInDatabase() {

    let Msg = "Error";

    OrderDtls = [];
    var Rows = DataTable.rows({ 'search': 'applied' }).nodes();
    Rows.each(function (index) {
        var Row = DataTable.row(index);
        var Data = Row.data();
        var IsFree = false;
        if ($(index).find('#IsFree').is(':checked'))
            IsFree = true;
        OrderDtls.push({
            InternalCode: Data[0],
            BarCode: Data[1],
            ProductNa: Data[2],
            ShippedQty: $(index).find('.ShippedQty ').val(),
            VendorUnitPrice: $(index).find('.VendorUnitPrice ').val(),
            ExpectedDeliveredDate: $('#ExpectedDeliveredDate').val(),
            DocumentNumber: DocumnetNumber,
            IsShipping: IsShippingPo,
            MasterID: MasterID,
            IsRejected: IsRejected
        });
    })
    $.ajax({
        url: "/OrderManagement/Save",
        type: "Post",
        async: false,
        data: { OrderVM: OrderDtls },
        success: function (data) {
            Msg = data;
        },
        error: function (data) { alert('Error'); }
    });

    return Msg;
}

function CheckValidationBeforShippingPo() {

    let IsVald = false;
    var Rows = DataTable.rows({ 'search': 'applied' }).nodes();
    Rows.each(function (index) {
        var Row = DataTable.row(index);
        var Data = Row.data();
        if (parseFloat($(index).find('.ShippedQty ').val()) != 0) {
            IsVald = true;
            return;
        }
    });
    if (IsVald == false) {
        if ($("#language").html() == "  اللغة") {
            AlertNotify("info", "برجاء ادخال منتج واحد على الاقل");
        }
        else { AlertNotify("info", "Sorry, please insert one product at least "); }
        return;
    }
    $("#ModalConfirmShippPo").modal();
}

function ShippingPO() {

    IsShippingPo = true;
    var Result = SaveInDatabase();

    if (Result == "Done") {
        if ($("#language").html() == "  اللغة") {
            AlertNotify("info", "تم شحن امر الشراء  بنجاح");
        }
        else { AlertNotify("info", "direct PO  Shipped  Successfully"); }

        setTimeout(function () { window.open("/OrderManagement/PrintShippingOrder", '_blank'); }, 1000);


        setTimeout(function () { location.reload(); }, 3000);
    }
    else { AlertNotify("danger", Result); }

    $("#CloseModalConfirmShippPo").click();
}

function RejectPo() {
    IsRejected = true;
    var Result = SaveInDatabase();

    if (Result == "Done") {
        if ($("#language").html() == "  اللغة") {
            AlertNotify("info", "تم رفض امر الشراء المباشر بنجاح");
        }
        else { AlertNotify("info", "direct po rejected succefully"); }

        setTimeout(function () { location.reload(); }, 3000);
    }
    else { AlertNotify("danger", Result); }

    $("#CloseConfirmRejectPo").click();
}

function PrintReceivedOrder() {
    showloader();
    window.open("/OrderManagement/PrintReceivedOrder?MasterID=" + MasterID + "", '_blank');
    hideloader();
}