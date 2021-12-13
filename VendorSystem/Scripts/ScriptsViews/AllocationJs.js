let DT_ShowProductQty;
let ProductQtyLst;
$(function () {

    $("#ExpectedDeliveredDate").datepicker({
        format: 'dd/mm/yyyy', showClose: true, showClear: true, keepInvalid: true, autoclose: true, todayHighlight: true
    }).on('dp.change', function () {
    });

    if ($("#language").html() == "اللغة") {


        $("#ExpectedDeliveredDate").css("direction", "ltr");
        $("#ExpectedDeliveredDate").css("text-align", "right");

        DT_ShowProductQty = $("#tblShowProductQtyData").DataTable({
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
            ],
            "columnDefs": [{ "targets": [5], visible: false }]
        });

    } else {

        $("#ExpectedDeliveredDate").css("direction", "ltr");
        $("#ExpectedDeliveredDate").css("text-align", "left");



        DT_ShowProductQty = $("#tblShowProductQtyData").DataTable({
            iDisplayLength: -1,
            aLengthMenu: [
                [-1], ["All"]
            ],
            "columnDefs": [{ "targets": [5], visible: false }]
        });
    }
});

function GitAllProductDataQty() {

    if ($("#ddRegion").val() == "") {
        AlertMe('danger', 'Please choose the Region', 'من فضلك اختر المنطقة ');
        return;
    }

    if ($("#ExpectedDeliveredDate").val() == "") {
        AlertMe('danger', 'Please choose the expected delivery date', 'من فضلك اختر تاريخ التوصيل المتوقع ');
        return;
    }

    StopSave("ddRegion");
    StopSave("ddTerritory");
    StopSave("ddRoute");
    StopSave("ExpectedDeliveredDate");
    StopSave("btnSearch");
    DT_ShowProductQty.clear().draw();
    Reset(DT_ShowProductQty);
    showloader();
    $.ajax({
        url: '/Allocation/GetAllocationData',
        data: {
            RegionID: $("#ddRegion").val(),
            TerritoryID: $("#ddTerritory").val(),
            RouteID: $("#ddRoute").val(),
            ExpectedDeliveryDate: $("#ExpectedDeliveredDate").val()
        },
        type: 'POST',
        success: function (data) {
            if (data.length == 0) {
                EnableSave("ddRegion");
                EnableSave("ddTerritory");
                EnableSave("ddRoute");
                EnableSave("ExpectedDeliveredDate");
                EnableSave("btnSearch");
            }

            for (var i = 0; i < data.length; i++) {
                DT_ShowProductQty.row.add([
                    data[i].Barcode,
                    data[i].InternalCode,
                    data[i].Name,
                    data[i].TotalNeededQty,
                    '<input type="number" min="0" value="' + data[i].ShippedQty + '" class="ShippedQty" />',
                    data[i].ShippedQty
                ]).draw(false);
            }
            Summation(DT_ShowProductQty);
            hideloader();

        },
        error: function () { alert('Error'); hideloader(); },
    });
}

function CheckValidation() {
    ProductQtyLst = [];
    let IsValid = true;
    let _ShippedQty = 0;

    var Rows = DT_ShowProductQty.rows({ 'search': 'applied' }).nodes();
    Rows.each(function (index) {
        let Row = DT_ShowProductQty.row(index);
        let Data = Row.data();
        _ShippedQty = $(index).find('.ShippedQty').val();

        if (_ShippedQty == "") {
            AlertMe('info', 'برجاء ادخال كميه  للمنتج: ' + Data[0] + '', 'Please insert qty for barcode: ' + Data[0]);
            IsValid = false;
            return;
        }
        else if (Number(_ShippedQty) < 0) {
            AlertMe('info', 'برجاء ادخال كميه اكبر من الصفر للمنتج: ' + Data[0] + '', 'Please insert qty larger than zero for barcode: ' + Data[0]);
            IsValid = false;
            return;
        }
        if ($(index).find('.ShippedQty').val() != Data[5]) {
            ProductQtyLst.push({
                BarCode: Data[0],
                ShippedQty: _ShippedQty,
                TotalNeededQty: Data[3],
                InternalCode: Data[1]
            });
        }
    })

    if (ProductQtyLst == null) {
        ProductQtyLst = {}
            }

    if (IsValid) {
        $("#ModalConfirm").modal();
    }
}

function Save() {

    showloader();

    $("#bntCloseModalConfirm").click();

    $.ajax({
        url: '/Allocation/Save',
        data: { AllocationVMLst: ProductQtyLst },
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

$("#ddRegion").change(function () {

    $("#ddTerritory").empty().change();

    if ($("#language").html() == "اللغة") {
        $("#ddTerritory").append("<option selected value=''>اختر</option>")
    }
    else {
        $("#ddTerritory").append("<option selected value=''>Please Select</option>")
    }

    if ($("#ddRegion").val() != "" && $("#ddRegion").val() != null) {
        let Options = "";

        var Data = GetChildrenDataByParentID($("#ddRegion").val(), true);


        for (var item of Data) {

            Options += "<option value=" + item.ID + ">" + item.Name + "</option>";
        }
        $("#ddTerritory").append(Options);
    }

});


$("#ddTerritory").change(function () {

    $("#ddRoute").empty().change();

    if ($("#language").html() == "اللغة") {
        $("#ddRoute").append("<option selected value=''>اختر</option>")
    }
    else {
        $("#ddRoute").append("<option selected value=''>Please Select</option>")
    }

    if ($("#ddTerritory").val() != "" && $("#ddTerritory").val() != null) {
        let Options = "";

        $.ajax({
            url: '/Allocation/GetRouteByTerritoryID',
            data: { TerritoryID: $("#ddTerritory").val() },
            type: 'POST',
            async: false,
            success: function (data) {

                for (var item of data) {

                    Options += "<option value=" + item.ID + ">" + item.Name + "</option>";
                }
                $("#ddRoute").append(Options);
                hideloader();
            },
            error: function () { alert('Error'); _Data = 'Error' },
        });


    }
});

function Summation(MyDatatable) {
    MyDatatable.columns('.sum').every(function () {
        var column = this;
        let IsComma = false;  
        var sum = column.data().reduce(function (a, b) {
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
        sum = parseInt(sum).toFixed(0)
        if ($("#language").html() == "اللغة") { $(column.footer()).html('الاجمالى: ' + sum); }
        else { $(column.footer()).html('Total:   ' + sum); }
    });
}
function Reset(MyDatatable) {
    MyDatatable.columns('.sum').every(function () {
        var column = this;
        var iscomma = false;
        var isiscommaa = false;
        ;
        var sum = 0;
        if ($("#language").html() == "اللغة") { $(column.footer()).html('الاجمالى: ' + sum); }
        else { $(column.footer()).html('Total: ' + sum); }
    });
}