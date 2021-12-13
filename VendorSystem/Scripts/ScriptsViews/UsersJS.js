let AllUsers = _AllUsers; // _AllUsers it be filled from script in index page

let _ID = -1;
let UsersDataTable;

$(function () {

    if ($("#language").html() == "اللغة") {
        UsersDataTable = $("#tblShowUsersData").DataTable({
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
        UsersDataTable = $("#tblShowUsersData").DataTable({});
    }



    FillTable(AllUsers);

    setTimeout(function () {
        $("#txtPassword").val('').change();
        $("#txtUserName").val('').change();
    }, 600);

});

$("#IsManufacture").change(function () {

    if ($("#IsManufacture").prop('checked')) {
        $("#dropDistributor").val('').change();
        StopSave("dropDistributor");
    }
    else {
        EnableSave("dropDistributor");
    }

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

    if ($("#dropRole").val() == "") {
        AlertMe('danger', 'Please choose Role', 'من فضلك اختر الصلاحية')
        return false;
    }

    if ($("#txtUserName").val().trim().length < 1) {
        AlertMe('danger', 'Please insert  User Name ', 'من فضلك ادخل اسم مستخدم ')
        return false;
    }

    if ($("#txtPassword").val().length < 1) {
        AlertMe('danger', 'Please insert Password ', 'من فضلك ادخل كلمه السر ')
        return false;
    }

    if ($("#txtPassword").val() != $("#txtConfirmPassword").val()) {
        AlertMe('danger', 'Password Not Matched ', 'كلمة السر غير متطابقة')
        return false;
    }

    if ($("#dropDistributor").val() == '' && $("#IsManufacture").prop('checked') != true) {
        AlertMe('danger', 'Please choose The Distributor', 'من فضلك اختر الموزع')
        return false;
    }


    $("#ModalConfirm").modal();
}

function Save() {

    showloader();
    $("#bntCloseModalConfirm").click();

    let _UserVM =
    {
        ID: _ID,
        Name: $("#txtName").val().trim(),
        NameEng: $("#txtNameEng").val().trim(),
        RoleID: $("#dropRole").val(),
        UserName: $("#txtUserName").val(),
        Password: $("#txtPassword").val(),
        Mobile: $("#txtMobile").val(),
        IsActive: $("#Active").prop('checked'),
        IsManufacture: $("#IsManufacture").prop('checked'),
        DistributorCode: $("#dropDistributor").val()
    };

    $.ajax({
        url: '/User/Save',
        data: { UserVM: _UserVM },
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
    UsersDataTable.clear().draw();

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

        UsersDataTable.row.add([
                        data[i].Name,
                        data[i].UserName,
                        data[i].RoleName,
                        data[i].Mobile,
                        Status,
                        "<span class=' glyphicon glyphicon-edit edit' id='" + data[i].ID + "'  title='" + Edit + "' onclick='Edit(this)'> "
        ]).draw(false);
    }
}

function Edit(element) {
    _ID = element.id;
    $("#btnCloseModalShowUsersData").click();
    showloader();

    $.ajax({
        url: '/User/GetUserByID',
        data: { ID: _ID },
        type: 'POST',
        async: false,
        success: function (data) {

            $("#txtName").val(data.Name);
            $("#txtNameEng").val(data.NameEng);
            $("#dropRole").val(data.RoleId).change();
            $("#dropDistributor").val(data.DistributorCode).change();


            $("#txtUserName").val(data.User_Name);
            $("#txtPassword").val(data.Password);
            $("#txtConfirmPassword").val(data.Password);
            $("#txtMobile").val(data.Mobile);
            $("#Active").prop('checked', data.IsActive);
            $("#IsManufacture").prop('checked', data.IsManufacture).change();

            StopSave("txtPassword");
            StopSave("txtConfirmPassword");

            hideloader();
        },
        error: function () { alert('Error'); },
    });

}