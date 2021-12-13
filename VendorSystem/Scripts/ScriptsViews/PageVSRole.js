var DataTable;
var ID = 0;
var edit;
var delet;
var Active;
var InActive;
var IsExit = true;
var FormType = 1; // Save Or Update
var PagesVSRoleViewModelData = [];
var URL = "/PagesVSRole/index";
let x = '';
$(function () {
    if ($("#language").html() == "اللغة") {
        edit = "تعديل";

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
            },
            "columnDefs": [
               { "targets": [1], visible: false },
                 { "targets": [8], visible: false },
            ]


        });
        DataTable.on('order.dt search.dt', function () {
            DataTable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();
    }
    else {
        Active = "Active";
        InActive = "Inactive";
        edit = "edit";
        DataTable = $("#tbShowData").DataTable({
            "columnDefs": [
                    { "targets": [1], visible: false },
                      { "targets": [8], visible: false },
            ]
        });

        DataTable.on('order.dt search.dt', function () {
            DataTable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();
    }
    //FillTable(AppPages);
});

$("#selectallRole").change(function () {

    if ($("#selectallRole").is(':checked')) {
        var Rows = DataTable.rows({ 'search': 'applied' }).nodes();
        Rows.each(function (index) {
            $(index).find('#save').prop('checked', true);
            $(index).find('#saveandpost').prop('checked', true);
            $(index).find('#search').prop('checked', true);
            $(index).find('#print').prop('checked', true);
            $(index).find('#Show').prop('checked', true);
            $(index).find('#SelectAll').prop('checked', true);

        })
    } else {
        var Rows = DataTable.rows({ 'search': 'applied' }).nodes();
        Rows.each(function (index) {
            $(index).find('#save').prop('checked', false);
            $(index).find('#saveandpost').prop('checked', false);
            $(index).find('#search').prop('checked', false);
            $(index).find('#print').prop('checked', false);
            $(index).find('#Show').prop('checked', false);
            $(index).find('#SelectAll').prop('checked', false);

        })
    }
});
$("#RoleId").change(function () {
    if ($("#RoleId").val() == "") {
        DataTable.clear().draw();
        return false;
    }
    showloader();
    ;
    $.ajax({
        type: "POST",
        url: "/PagesVSRole/GetPageVsRoleForEdit",
        data: { RoleId: $("#RoleId").val() },
        success: function (data) {
            ;
            DataTable.clear().draw();

            //var SaveCheckBox, saveandpostCheckBox,ShowCheckBox,PrintCheckBox,searchCheckBox;
            for (var i = 0; i < data.length; i++) {

                var SaveCheckBox = "<label><input id='save' name='switch-field-1' class='ace ace-switch ace-switch-2 check'  type='checkbox' /><span class='lbl'></span></label>";
                if (data[i].Save == true) {
                    SaveCheckBox = "<label><input id='save' name='switch-field-1' class='ace ace-switch ace-switch-2 check' checked  type='checkbox' /><span class='lbl'></span></label>";
                }
                var saveandpostCheckBox = "<label><input id='saveandpost' name='switch-field-1' class='ace ace-switch ace-switch-2 check'  type='checkbox' /><span class='lbl'></span></label>";
                if (data[i].SAveAndPost == true) {
                    saveandpostCheckBox = "<label><input id='saveandpost' name='switch-field-1' class='ace ace-switch ace-switch-2 check' checked  type='checkbox' /><span class='lbl'></span></label>";
                }

                var ShowCheckBox = "<label><input id='Show' name='switch-field-1' class='ace ace-switch ace-switch-2 check'  type='checkbox' /><span class='lbl'></span></label>";
                if (data[i].show == true) {
                    ShowCheckBox = "<label><input id='Show' name='switch-field-1' class='ace ace-switch ace-switch-2 check' checked  type='checkbox' /><span class='lbl'></span></label>";
                }
                var searchCheckBox = "<label><input id='search' name='switch-field-1' class='ace ace-switch ace-switch-2 check'  type='checkbox' /><span class='lbl'></span></label>";
                if (data[i].Search == true) {
                    searchCheckBox = "<label><input id='search' name='switch-field-1' class='ace ace-switch ace-switch-2 check' checked  type='checkbox' /><span class='lbl'></span></label>";
                }
                var PrintCheckBox = "<label><input id='print' name='switch-field-1' class='ace ace-switch ace-switch-2 check'  type='checkbox' /><span class='lbl'></span></label>";
                if (data[i].Print == true) {
                    PrintCheckBox = "<label><input id='print' name='switch-field-1' class='ace ace-switch ace-switch-2 check' checked  type='checkbox' /><span class='lbl'></span></label>";
                }
                var AllCheced = "<label><input id='SelectAll' onchange='Selectall(this)' name='switch-field-1' class='ace ace-switch ace-switch-2 check'  type='checkbox' /><span class='lbl'></span></label>"
                if (data[i].Save == true && data[i].show == true && data[i].SAveAndPost == true && data[i].Search == true && data[i].Print == true) {
                    AllCheced = "<label><input id='SelectAll' onchange='Selectall(this)' name='switch-field-1' class='ace ace-switch ace-switch-2 check' checked  type='checkbox' /><span class='lbl'></span></label>"
                }
                if ($("#language").html() == "اللغة") {
                    DataTable.row.add(["",
                       data[i].ID,
                      data[i].Name,
                      ShowCheckBox,
                      SaveCheckBox,
                      saveandpostCheckBox,
                      searchCheckBox,
                     PrintCheckBox,
                     data[i].PageID,
                    AllCheced// "<label><input id='SelectAll' onchange='Selectall(this)' name='switch-field-1' class='ace ace-switch ace-switch-2 check'  type='checkbox' /><span class='lbl'></span></label>"
                    ]).draw(false);
                }
                else {
                    DataTable.row.add(["",
                       data[i].ID,
                      data[i].NameEng,
                      ShowCheckBox,
                      SaveCheckBox,
                      saveandpostCheckBox,
                      searchCheckBox,
                     PrintCheckBox,
                     data[i].PageID,
                    AllCheced// "<label><input id='SelectAll' onchange='Selectall(this)' name='switch-field-1' class='ace ace-switch ace-switch-2 check'  type='checkbox' /><span class='lbl'></span></label>"
                    ]).draw(false);
                }
            }



            hideloader();
        },
        error: function (data) { hideloader(); }
    });
})
function Selectall(e) {

    if ($(e).is(':checked')) {
        $(e).parent().parent().prev().prev().prev().prev().prev().children().children().prop("checked", true);
        $(e).parent().parent().prev().prev().prev().prev().children().children().prop("checked", true);
        $(e).parent().parent().prev().prev().prev().children().children().prop("checked", true);
        $(e).parent().parent().prev().prev().children().children().prop("checked", true);
        $(e).parent().parent().prev().children().children().prop("checked", true);
    }
    else {
        $(e).parent().parent().prev().prev().prev().prev().prev().children().children().prop("checked", false);
        $(e).parent().parent().prev().prev().prev().prev().children().children().prop("checked", false);
        $(e).parent().parent().prev().prev().prev().children().children().prop("checked", false);
        $(e).parent().parent().prev().prev().children().children().prop("checked", false);
        $(e).parent().parent().prev().children().children().prop("checked", false);
    }


}

function FillTable(data) {
    DataTable.clear().draw();
    debugger;
    if ($("#language").html() == "اللغة") {
        for (var i = 0; i < data.length; i++) {

            DataTable.row.add(["",
                ID,
                 data[i].Name,
                "<label><input id='save' name='switch-field-1' class='ace ace-switch ace-switch-2 check'  type='checkbox' /><span class='lbl'></span></label>",
                  "<label><input id='saveandpost' name='switch-field-1' class='ace ace-switch ace-switch-2 check'  type='checkbox' /><span class='lbl'></span></label>",
                   "<label><input id='search' name='switch-field-1' class='ace ace-switch ace-switch-2 check'  type='checkbox' /><span class='lbl'></span></label>",
                    "<label><input id='print' name='switch-field-1' class='ace ace-switch ace-switch-2 check'  type='checkbox' /><span class='lbl'></span></label>",
                    data[i].ID

            ]).draw(false);
        }
    }
    else {
        for (var i = 0; i < data.length; i++) {

            DataTable.row.add(["",
                ID,
                 data[i].NameEng,
                "<label><input id='save' name='switch-field-1' class='ace ace-switch ace-switch-2 check'  type='checkbox' /><span class='lbl'></span></label>",
                  "<label><input id='saveandpost' name='switch-field-1' class='ace ace-switch ace-switch-2 check'  type='checkbox' /><span class='lbl'></span></label>",
                   "<label><input id='search' name='switch-field-1' class='ace ace-switch ace-switch-2 check'  type='checkbox' /><span class='lbl'></span></label>",
                    "<label><input id='print' name='switch-field-1' class='ace ace-switch ace-switch-2 check'  type='checkbox' /><span class='lbl'></span></label>",
                    data[i].ID

            ]).draw(false);
        }
    }

}
function FillData() {
    var Active = false;
    if ($('#Active').is(':checked'))
        Active = true;
    var Rows = DataTable.rows({ 'search': 'applied' }).nodes();
    Rows.each(function (index) {
        var Row = DataTable.row(index);
        var Data = Row.data();

        var Save = false;
        if ($(index).find('#save').is(':checked'))
            Save = true;
        var saveandpost = false;
        if ($(index).find('#saveandpost').is(':checked'))
            saveandpost = true;
        var search = false;
        if ($(index).find('#search').is(':checked'))
            search = true;
        var Show = false;
        if ($(index).find('#Show').is(':checked'))
            Show = true;
        var print = false;
        if ($(index).find('#print').is(':checked'))
            print = true;
        PagesVSRoleViewModelData.push({
            ID: Data[1],
            RoleId: $('#RoleId').val(),
            PageId: Data[8],
            save: Save,
            saveandpost: saveandpost,
            Show: Show,
            search: search,
            Print: print,
            Active: Active,

        });
    })
}
function Edit(index) {
    var RowData = Users[index];
    ID = RowData.ID;

    $("#Name").val(RowData.Name);
    $("#User_Name").val(RowData.User_Name);
    $("#Password").val(RowData.Password);
    $("#RuleId").val(RowData.RuleId).change();
    if (RowData.Active == 1) {
        $("#Active").prop('checked', true);
    }
    else {
        $("#Active").prop('checked', false);
    }
    FormType = 2;
    $("#CloseShowdataModel").click();
}
function PassID(RowID) {
    ID = RowID;
}
function New() {
    ID = 0;
    FormType = 1;
    $("#RoleId").val("").change();
    DataTable.clear().draw();
}
function CheckValidation() {
    var Form = $("#Form");
    if (!IsExit) {
        $("#CloseModel").click();
        if ($("#language").html() == "اللغة") { AlertNotify("info", "موجود مسبقا في قاعدة البيانات"); }
        else { AlertNotify("info", "This Model Is Already Exist"); }
        return false;
    }
    Form.validate();
    if ($('#RoleId').val() != "") {
        if (Form.valid()) {
            $("#ModalConfirm").modal();
        }
        else {
            if ($("#language").html() == "اللغة") { AlertNotify("info", "من فضلك املا البيانات المطلوبة"); }
            else { AlertNotify("info", "Please fill in required fields"); }
        }
    }
    else {
        if ($("#language").html() == "اللغة") { AlertNotify("info", "من فضلك ادخل اسم الصلاحيه "); }
        else { AlertNotify("info", "Please fill in Role name"); }
    }
}
function Save() {

    FillData();
    StopSave("BtnSave");
    var PagesVSRoleListViewModel = {
        PagesVSRoleViewModelData: PagesVSRoleViewModelData

    };
    var data = {
        PagesVSRoleListViewModel

        };

    $.ajax({
            type: "POST",
            async: true,
            datatype: 'json',
            contenttype: 'application/json; charset=utf-8',
            url: '/PagesVSRole/Save',
        //  data: JSON.stringify(data),
        data: data,
        success: function (data) {
            if (data == 1) {
                $("#CloseModel").click();
                if ($("#language").html() == "اللغة") { AlertNotify("success", "تم الحفظ بنجاح"); }
                else { AlertNotify("success", "Saved Successfully"); }
                setTimeout(function () { location.reload(); }, 1000)
            }
            else if (data == 0) {
                EnableSave("BtnSave");
                $("#CloseModel").click();
                if ($("#language").html() == "اللغة") { AlertNotify("info", "من فضلك املا البيانات المطلوبة"); }
                else { AlertNotify("info", "Please fill in required fields"); }

            }
            else if (data == 2) {
                EnableSave("BtnSave");
                $("#CloseModel").click();
                if ($("#language").html() == "اللغة") { AlertNotify("info", "يوجد مشكلة بالحفظ الرجاء الاتصال بالدعم الفني"); }
                else { AlertNotify("info", "There is a problem with saving, please contact Technical Support"); }

            }

        },
        error: function (data) {
            EnableSave("BtnSave");
        }
    })

}
$("#CloseShowdataModel").click(function () {
    DataTable.destroy();

})
$("#xbtn").click(function () {
    DataTable.destroy();

})
