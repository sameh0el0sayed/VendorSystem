var DataTable;
var ID = 0;
var Logo;
var ProvinceId;
var City;
var District;
var Terriory;

$(function () {
    FillData();
})

function FillData() {

    var RowData = Company;
    ID = RowData.Company_Id;
    $("#Company_Id").val(RowData.Company_Id)
    $("#Company_Name").val(RowData.Company_Name);

    $("#ddCountry").val(RowData.CountryId).change();
    $("#ddProvince").val(RowData.ProvinceId).change();
    $("#ddCity").val(RowData.CityId).change();
    $("#ddRegion").val(RowData.Region).change();
    $("#ddTerritory").val(RowData.TerritoryId).change();



    $("#Company_Address").val(RowData.Company_Address);
    $("#Email").val(RowData.Email);
    $("#Website").val(RowData.Website);
    $("#Mobile").val(RowData.Mobile);
    $("#Fax").val(RowData.Fax);
    $("#Phone1").val(RowData.Phone1);
    document.getElementById("ProductPhoto").src = RowData.Logo;
    Logo = RowData.Logo;
}

function New() {
    FillData();
}


function CheckValidation() {



    $("#ModalConfirm").modal();
}

function Save() {

    StopSave("BtnSave");

    var CompanyVM = {
        Company_Id: ID,
        Company_Name: $("#Company_Name").val(),
        Company_Address: $("#Company_Address").val(),
        Email: $("#Email").val(),
        Mobile: $("#Mobile").val(),
        Phone1: $("#Phone1").val(),
        Fax: $("#Fax").val(),
        Website: $("#Website").val(),
        CountryId: $("#ddCountry").val(),
        ProvinceId: $("#ddProvince").val(),
        CityId: $("#ddCity").val(),
        Region: $("#ddRegion").val(),
        TerritoryId: $("#ddTerritory").val(),
        Logo: Logo,
    };
    var data = { CompanyVM };
    $.ajax({
        type: "POST",
        cache: false,
        async: true,
        datatype: 'json',
        contenttype: 'application/json; charset=utf-8',
        url: '/Company/Save',
        //  data: JSON.stringify(data),
        data: data,
        success: function (data) {
            ;
            if (data == 1) {
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
function isNumberKey(evt) {

    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 44 || charCode > 57))
        return false;
    return true;
}


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

    showloader();
    $.ajax({
        url: '/Company/Upload',
        type: 'POST',
        data: _formData,
        cache: false,
        contentType: false,
        processData: false,
        success: function (data) {
            hideloader();
            document.getElementById("ProductPhoto").src = data;
            Logo = data;
        },
        error: function () { hideloader(); },
    });
}

$("#CloseShowdataModel").click(function () {
    DataTable.destroy();
})

$("#Xbtn").click(function () {
    DataTable.destroy();
})


