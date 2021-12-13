let AllRoles = _AllRoles; // _AllRoles it be filled from script in index page

let _ID = -1;
let RolesDataTable;

$(function () {

    if ($("#language").html() == "اللغة") {
        RolesDataTable = $("#tblShowRolesData").DataTable({
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
        RolesDataTable = $("#tblShowRolesData").DataTable({});
    }



    FillTable(AllRoles);

    setTimeout(function () {
        $("#txtPassword").val('').change();
        $("#txtRoleName").val('').change();
    }, 600);

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

    $("#ModalConfirm").modal();
}

function Save() {

    showloader();
    $("#bntCloseModalConfirm").click();

    let _RoleVM =
    {
        ID: _ID,
        Name: $("#txtName").val().trim(),
        NameEng: $("#txtNameEng").val().trim(),
        IsActive: $("#Active").prop('checked')
    };

    $.ajax({
        url: '/Role/Save',
        data: { RoleVM: _RoleVM },
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
    RolesDataTable.clear().draw();

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

        RolesDataTable.row.add([
                        data[i].Name,
                        Status,
                        "<span class=' glyphicon glyphicon-edit edit' id='" + data[i].ID + "'  title='" + Edit + "' onclick='Edit(this)'> "
        ]).draw(false);
    }
}

function Edit(element) {
    _ID = element.id;
    $("#btnCloseModalShowRolesData").click();
    showloader();

    $.ajax({
        url: '/Role/GetRoleByID',
        data: { ID: _ID },
        type: 'POST',
        async: false,
        success: function (data) { 

            $("#txtName").val(data.Name);
            $("#txtNameEng").val(data.NameEng);
            $("#Active").prop('checked', data.IsActive);
            hideloader();
        },
        error: function () { alert('Error'); },
    });

}