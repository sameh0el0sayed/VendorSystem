var URL = "/OrderManagement/Received";
var DataTable;
var tblPOData;
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
            }
        });
        tblPOData = $("#tblPOData").DataTable({
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
        DataTable = $("#tbShowData").DataTable({});
        tblPOData = $("#tblPOData").DataTable({});
    }

    $('#StoreId').attr('disabled', 'disabled');
    $('#PartnerId').attr('disabled', 'disabled');
});

function Search() {
    showloader();
    tblPOData.clear().draw();
    $.ajax({
        url: "/OrderManagement/GetPOsPerStatusID",
        type: "Post",
        data: { StatusID: _Status },
        success: function (data) {

            if (_Status == 4 || _Status == 6) {
                for (var i = 0; i < data.length; i++) {
                    tblPOData.row.add([
                           data[i].DocumentNumber,
                           data[i].StoreName,
                           data[i].SentDate,
                           data[i].ShippedDate,
                           data[i].ExpectedDeliveredDate,
                           data[i].CustomerCode,
                           "<span class=' glyphicon glyphicon-edit edit'  style='cursor: pointer;' title='" + data[i].DocumentNumber + "' id=" + data[i].ID + " onclick='Edit(this)'>"
                    ]).draw(false);
                }
            }
            else {
                for (var i = 0; i < data.length; i++) {
                    tblPOData.row.add([
                           data[i].DocumentNumber,
                           data[i].StoreName,
                           data[i].SentDate,
                           data[i].ShippedDate,
                           data[i].RecievedDate,
                           data[i].ExpectedDeliveredDate,
                           data[i].CustomerCode,
                           "<span class=' glyphicon glyphicon-edit edit'  style='cursor: pointer;' title='" + data[i].DocumentNumber + "' id=" + data[i].ID + " onclick='Edit(this)'>"
                    ]).draw(false);
                }
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
      
    $.ajax({
        url: "/OrderManagement/GetPoByMasterID",
        type: "Post",
        data: { _MasterID: MasterID },
        success: function (data) {

            $("#btnPrintDeliveredOrder").show();
            $("#btnPrintShippedOrder").show();

            DataTable.clear().draw();
            $("#CloseModalShowReceivedPosData").click();
            $("#DocumentNumber").val(data[0].DocumentNumber);
            $("#StoreId").val(data[0].StoreId).change();
            $("#ExpectedDeliveredDate").val(data[0].ExpectedDeliveredDate).change();
            $("#CustomerCode").val(data[0].CustomerCode);
            $("#RouteName").val(data[0].RouteName).attr('disabled', true);
            $("#RegionName").val(data[0].RegionName).attr('disabled', true);
            $("#TerritoryName").val(data[0].TerritoryName).attr('disabled', true);
             
            let Item;
            let Row = [];


            if (_Status == 4 || _Status == 6) {
                for (var i = 0; i < data.length; i++) {
                    Item = data[i];
                    Row = [
                            Item.InternalCode,
                            Item.BarCode,
                            Item.ProductNa,
                            Item.SystemQty,
                            Item.ApprovedQty,
                            Item.ShippedQty,
                            Item.VendorUnitPrice
                    ];

                    DataTable.row.add(Row).draw(false);
                }
            }
            else {

                for (var i = 0; i < data.length; i++) {
                    Item = data[i];
                    Row = [
                       Item.InternalCode,
                       Item.BarCode,
                       Item.ProductNa,
                       Item.SystemQty,
                       Item.ApprovedQty,
                       Item.ShippedQty,
                       Item.DeliveredQty,
                       Item.VendorUnitPrice,
                       Item.MarketUnitPrice
                    ];

                    DataTable.row.add(Row).draw(false);
                }
            }


            hideloader();
        },
        error: function (data) {
        }
    })

}

function PrintDeliveredOrder() {
    showloader();
    window.open("/OrderManagement/PrintDelivereddOrder?MasterID=" + MasterID + "", '_blank');
    hideloader();
}

function PrintShippedOrder() {
    showloader();
    window.open("/OrderManagement/PrintShippedOrder?MasterID=" + MasterID + "", '_blank');
    hideloader();
}

