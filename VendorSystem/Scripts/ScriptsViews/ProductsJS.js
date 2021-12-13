
let _ID = -1;
let ProductsDataTable;

$(function () {

    if ($("#language").html() == "اللغة") {
        ProductsDataTable = $("#tblShowProductsData").DataTable({
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
        ProductsDataTable = $("#tblShowProductsData").DataTable({});
    }

    $.ajax({
        url: '/Product/RetriveAllProducts',
        type: 'POST',
        success: function (data) {
            FillTable(data);
        },
        error: function () { alert('Error'); },
    });

});

$("#dropFirst").change(function () {

    $("#dropSecond").empty().change();

    if ($("#language").html() == "اللغة") {
        $("#dropSecond").append("<option selected value=''>اختر</option>")
    }
    else {
        $("#dropSecond").append("<option selected value=''>Please Select</option>")
    }

    if ($("#dropFirst").val() != "" && $("#dropFirst").val() != null) {
        let Options = "";

        var Data = GetChildrenDataByParentID($("#dropFirst").val());

        if (Data == 'Error') return;

        for (var item of Data) {

            Options += "<option value=" + item.ID + ">" + item.Name + "</option>";
        }
        $("#dropSecond").append(Options);

    }

});

$("#dropSecond").change(function () {

    $("#dropThird").empty().change();

    if ($("#language").html() == "اللغة") {
        $("#dropThird").append("<option selected value=''>اختر</option>")
    }
    else {
        $("#dropThird").append("<option selected value=''>Please Select</option>")
    }

    if ($("#dropSecond").val() != "" && $("#dropSecond").val() != null) {
        let Options = "";

        var Data = GetChildrenDataByParentID($("#dropSecond").val());


        for (var item of Data) {

            Options += "<option value=" + item.ID + ">" + item.Name + "</option>";
        }
        $("#dropThird").append(Options);
    }

});

$("#dropThird").change(function () {

    $("#dropFourth").empty().change();

    if ($("#language").html() == "اللغة") {
        $("#dropFourth").append("<option selected value=''>اختر</option>")
    }
    else {
        $("#dropFourth").append("<option selected value=''>Please Select</option>")
    }

    if ($("#dropThird").val() != "" && $("#dropThird").val() != null) {
        let Options = "";

        var Data = GetChildrenDataByParentID($("#dropThird").val());


        for (var item of Data) {

            Options += "<option value=" + item.ID + ">" + item.Name + "</option>";
        }
        $("#dropFourth").append(Options);
    }

});


function CheckValidation() {




    if ($("#txtSmallBarcode").val().trim().length < 3) {
        AlertMe('danger', 'Please insert valid Small Barcodeat least 3 char', 'من فضلك ادخل باركود الوحده الصغيرة على الاقل 3 احرف')
        return false;
    }

    if ($("#txtLargBarcode").val().trim() == $("#txtSmallBarcode").val().trim()) {
        AlertMe('danger', 'Please insert a Large Barcode different from a Small Barcode', 'من فضلك ادخل باركود الوحده الكبيره مختلفه عن باركود الوحده الصغيره')
        return false;
    }

    if ($("#txtProduct_Name").val().trim().length < 3) {
        AlertMe('danger', 'Please insert valid name at least 3 char', 'من فضلك ادخل اسم صحيح على الاقل 3 احرف')
        return false;
    }

    if ($("#txtProduct_NameEn").val().trim().length < 3) {
        AlertMe('danger', 'Please insert valid English name at least 3 char', 'من فضلك ادخل اسم باللغه الانجليزيه صحيح على الاقل 3 احرف')
        return false;
    }

    if (Number($("#txtUnitConversion").val()) <= 0) {
        AlertMe('danger', 'Please insert Unit Conversion greater than zero', 'برجاء ادخال معامل تحويل الوحده المخزنيه اكبر من الصفر')
        return false;
    }

    if (Number($("#txtReturnPercentage").val()) < 0 || $("#txtReturnPercentage").val() == "") {
        AlertMe('danger', 'Please insert Return Percentage greater than or equall zero', 'برجاء ادخال نسبة المرتجعات اكبر من او يساوى الصفر')
        return false;
    }

    if (Number($("#txtMinQty").val()) < 0 || $("#txtMinQty").val() == "") {
        AlertMe('danger', 'Please insert Min Qty greater than or equall zero', 'برجاء ادخال كميه الحد الادنى اكبر من او يساوى الصفر')
        return false;
    }

    if ( $("#dropFirst").val() == "") {
        AlertMe('danger', 'Please choose First Classification', 'من فضلك اختر التصنيف الاول')
        return false;
    }

    if ($("#dropShelfLife").val() == "") {
        AlertMe('danger', 'Please choose the Shelf Life', 'من فضلك اختر نوع الصلاحية')
        return false;
    }

    $("#ModalConfirm").modal();
}

function Save() {

    showloader();

    $("#bntCloseModalConfirm").click();

    let _ProductVM =
    {
        ID: _ID,
        BigBarcode: $("#txtLargBarcode").val().trim(),
        SmallBarcode: $("#txtSmallBarcode").val().trim(),
        Name: $("#txtProduct_Name").val().trim(),
        NameEng: $("#txtProduct_NameEn").val().trim(),
        PiecesInCatron: $("#txtUnitConversion").val(),
        Weight: $("#txtWeight").val(),
        FirstClassification: $("#dropFirst").val(),
        SecondClassification: $("#dropSecond").val(),
        ThirdClassification: $("#dropThird").val(),
        FourthClassification: $("#dropFourth").val(),
        ReturnPercentage: $("#txtReturnPercentage").val(),
        MinQty: $("#txtMinQty").val(),
        ShelfLifeID: $("#dropShelfLife").val(),
        IsActive: $("#Active").prop('checked')
    }

    $.ajax({
        url: '/Product/Save',
        data: { ProductVM: _ProductVM },
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

    ProductsDataTable.clear().draw();
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

        ProductsDataTable.row.add([
                        data[i].Name,
                        data[i].BigBarcode,
                        data[i].SmallBarcode,
                        Status,
                        "<span class=' glyphicon glyphicon-edit edit' id='" + data[i].ID + "'  title='" + Edit + "' onclick='Edit(this)'> "
        ]).draw(false);
    }
}

function Edit(element) {
    _ID = element.id;
    $("#btnCloseModalShowProductsData").click();
    showloader();

    $.ajax({
        url: '/Product/GetProductByID',
        data: { ID: _ID },
        type: 'POST',
        success: function (data) {

            $("#txtLargBarcode").val(data.BigBarcode);
            $("#txtSmallBarcode").val(data.SmallBarcode);
            $("#txtProduct_Name").val(data.Name);
            $("#txtProduct_NameEn").val(data.NameEng);
            $("#txtUnitConversion").val(data.PiecesInCatron);
            $("#txtWeight").val(data.Weight);
            $("#dropFirst").val(data.FirstClassification).change();
            $("#dropSecond").val(data.SecondClassification).change();
            $("#dropThird").val(data.ThirdClassification).change();
            $("#dropFourth").val(data.FourthClassification).change();
            $("#Active").prop('checked', data.IsActive).change();
            $("#txtReturnPercentage").val(data.ReturnPercentage);
            $("#txtMinQty").val(data.MinQty);
            $("#dropShelfLife").val(data.ShelfLifeID).change();


            if (data.IsLocked == true) {
                StopSave("txtLargBarcode");
                StopSave("txtSmallBarcode");
            }
            hideloader();
        },
        error: function () { alert('Error'); },
    });

}

function DownloadCurrentStatus() {

    showloader();
    $.ajax({
        url: '/Product/DownloadCurrentStatus',
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
            url: '/Product/UploadAndSave',
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