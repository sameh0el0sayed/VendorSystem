let _ID = 0;
let tblShowPOsConfigurationsData; 
let tblData_Visit_Weekly;
let VisitSchedule_Data = [];
let isEdit = false;

$(function () {

    if ($("#language").html() == "اللغة") {

        tblShowPOsConfigurationsData = $("#tblShowPOsConfigurationsData").DataTable({
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
        tblData_Visit_Weekly = $("#tblData_Visit_Weekly").DataTable({
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
            searching: false,
            paging: false,
            info: false
        });
        tblData_Visit_Monthly = $("#tblData_Visit_Monthly").DataTable({
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
            "columnDefs": [{ "targets": [0], visible: false }, { "targets": [1], visible: false }],
            searching: false,
            paging: false,
            info: false
        });

    } else {

        tblShowPOsConfigurationsData = $("#tblShowPOsConfigurationsData").DataTable({});
      

        tblData_Visit_Weekly = $("#tblData_Visit_Weekly").DataTable({
            "columnDefs": [{ "targets": [0], visible: false }],
            searching: false,
            paging: false,
            info: false
        });
        tblData_Visit_Monthly = $("#tblData_Visit_Monthly").DataTable({
            "columnDefs": [{ "targets": [0], visible: false }, { "targets": [1], visible: false }],
            searching: false,
            paging: false,
            info: false
        });
    }


})






//-----------------------------------------------------------------------------------------------------------------------------------

$("#ddVisitScheduleType").change(function () {

    $("#divVisit_Weekly").hide();
    $("#divVisit_Monthly").hide();
    $("#divPercentage").hide();

    if ($("#ddVisitScheduleType").val() == 7 && isEdit == false) {
        $("#divVisit_Weekly").show();
        $("#divPercentage").show();
    }
    else if ($("#ddVisitScheduleType").val() == 28 && isEdit == false) {
        $("#divVisit_Monthly").show();
        $("#divPercentage").show();
    }

});

function InsertInto_tblData_Visit_Weekly() {

    if ($("#ddVisit_Days_Weekly").val() == "") {
        AlertMe('danger', 'please choose day', 'من فضلك اختر اليوم');
        return false;
    }

    if (Number($("#txtPercentage").val()) <= 0 || Number($("#txtPercentage").val()) > 100) {
        AlertMe('danger', 'please insert valid percentage', 'من فضلك ادخل نسبه صحيحه');
        return false;
    }

    let IsExist = false;
    let DayNumber;
    let NewDayNumber = Number($("#ddVisit_Days_Weekly").val());

    var Rows = tblData_Visit_Weekly.rows({ 'search': 'applied' }).nodes();
    Rows.each(function (index) {
        let Row = tblData_Visit_Weekly.row(index);
        let Data = Row.data();
        DayNumber = Data[0];

        if (NewDayNumber == DayNumber) {
            IsExist = true;
            return;
        }
    });

    if (IsExist == false) {
        tblData_Visit_Weekly.row.add([
            NewDayNumber,
            $("#ddVisit_Days_Weekly option:selected").text(), $("#txtPercentage").val(),
            "<span class='btn btn-info' onclick='RemoveFrom_Visit_Weekly(this)'><i class='glyphicon glyphicon-remove'></i></span>"
        ]).draw(false);
    }

    $("#ddVisit_Days_Weekly").val('0').change();
}

function RemoveFrom_Visit_Weekly(Element) {
    tblData_Visit_Weekly
        .row($(Element).parents('tr'))
        .remove()
        .draw();
} 

function InsertInto_tblData_Visit_Monthly() {

    if ($("#ddVisit_Days_Monthly").val() == "") {
        AlertMe('danger', 'please choose day', 'من فضلك اختر اليوم');
        return false;
    }

    if (Number($("#txtPercentage").val()) <= 0 || Number($("#txtPercentage").val()) > 100) {
        AlertMe('danger', 'please insert valid percentage', 'من فضلك ادخل نسبه صحيحه');
        return false;
    }

    let IsExist = false;
    let DayNumber;
    let NewDayNumber = Number($("#ddVisit_Days_Monthly").val());

    var Rows = tblData_Visit_Monthly.rows({ 'search': 'applied' }).nodes();
    Rows.each(function (index) {
        let Row = tblData_Visit_Monthly.row(index);
        let Data = Row.data();
        DayNumber = Data[1];

        if (NewDayNumber == DayNumber && $("#ddVisit_WeekNumber_Monthly").val() == Data[0]) {
            IsExist = true;
            return;
        }
    });

    if (IsExist == false) {
        tblData_Visit_Monthly.row.add([

            $("#ddVisit_WeekNumber_Monthly").val(), NewDayNumber,
            $("#ddVisit_WeekNumber_Monthly option:selected").text(),
            $("#ddVisit_Days_Monthly option:selected").text(),
            $("#txtPercentage").val(),
            "<span class='btn btn-info' onclick='RemoveFrom_Visit_Monthly(this)'><i class='glyphicon glyphicon-remove'></i></span>"
        ]).draw(false);
    }

    $("#ddVisit_Days_Monthly").val('0').change();
}

function RemoveFrom_Visit_Monthly(Element) {
    tblData_Visit_Monthly
        .row($(Element).parents('tr'))
        .remove()
        .draw();
}

//-----------------------------------------------------------------------------------------------------------------------------------
function CheckValidation() {
        
    if ($('#ddCustomers').find(':selected').data()==null) {
        AlertMe('danger', 'Please select at least one customer', 'من فضلك إختر علي الاقل عميل واحد')
        return false;
    }
    if ($("#txtName").val().trim().length < 3) {
        AlertMe('danger', 'Please insert valid name at least 3 char', 'من فضلك ادخل اسم صحيح على الاقل 3 احرف')
        return false;
    }


    if ($("#txtNameEng").val().trim().length < 3) {
        AlertMe('danger', 'Please insert valid English name at least 3 char', 'من فضلك ادخل اسم باللغه الانجليزيه صحيح على الاقل 3 احرف')
        return false;
    }
     
    let TotalPercentage = 0;
    VisitSchedule_Data = [];
    if ($("#ddVisitScheduleType").val() == 7 && isEdit==false) { //weekly

        var Rows = tblData_Visit_Weekly.rows({ 'search': 'applied' }).nodes();
        Rows.each(function (index) {
            let Row = tblData_Visit_Weekly.row(index);
            let Data = Row.data();

            TotalPercentage += Number(Data[2]);
            VisitSchedule_Data.push({ DayNumber: Data[0], OrderPercentage: Data[2] });
        });

        if (TotalPercentage != 100) {
            AlertMe('danger', 'The Total of visits percenage must be 100', 'مجموع نسب الزيارات يجب ان يكون  100');
            return false;
        }
        if (VisitSchedule_Data.length == 0) {
            AlertMe('danger', 'Please insert one day at least in Visits Days', 'يرجى ادخال يوم واحد على الاقل فى جدول ايام الزيارات');
            return false;
        }
    }
    if ($("#ddVisitScheduleType").val() == 28 && isEdit == false) { // monthly
        var Rows = tblData_Visit_Monthly.rows({ 'search': 'applied' }).nodes();
        Rows.each(function (index) {
            let Row = tblData_Visit_Monthly.row(index);
            let Data = Row.data();
            TotalPercentage += Number(Data[4]);
            VisitSchedule_Data.push({ DayNumber: Data[1], WeekNumber: Data[0], OrderPercentage: Data[4] });
        });

        if (TotalPercentage != 100) {
            AlertMe('danger', 'The Total of visits percenage must be 100', 'مجموع نسب الزيارات يجب ان يكون  100');
            return false;
        }
        if (VisitSchedule_Data.length == 0) {
            AlertMe('danger', 'Please insert one day at least in Visit Days', 'يرجى ادخال يوم واحد على الاقل فى جدول ايام الزيارات');
            return false;
        }
    }

    $("#ModalConfirm").modal();
}

function Save() { 
    StopSave("BtnSave");
    let _OrderConfigVM = [];
    if (isEdit) {
          _OrderConfigVM =
        {
            ID: _ID,
            Name: $("#txtName").val().trim(),
            NameEng: $("#txtNameEng").val().trim(),
            VisitScheduleTypeID: $("#ddVisitScheduleType").val(),
            /*IsIncludeMinQty: $("#IsIncludeMinQty").prop('checked'),*/
            IsActive: $("#Active").prop('checked'),
            VisitSchedule_Data: VisitSchedule_Data,
            CustomersDtlLst: $("#ddCustomers").val() == null ? [] : $("#ddCustomers").val(),
        }
    }
    else {
          _OrderConfigVM =
        { 
            Name: $("#txtName").val().trim(),
            NameEng: $("#txtNameEng").val().trim(),
            VisitScheduleTypeID: $("#ddVisitScheduleType").val(), 
            IsActive: $("#Active").prop('checked'),
            VisitSchedule_Data: VisitSchedule_Data,
            CustomersDtlLst: $("#ddCustomers").val() == null ? [] : $("#ddCustomers").val(),

        }
    }

    showloader();
    $.ajax({
        type: "POST",
        url: '/OrderConfig/Save',
        data: { _orderConfigVM: _OrderConfigVM },
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
        error: function (data) {
            EnableSave("BtnSave");
        }
    })
    EnableSave("BtnSave");
}

//Sameh Code 08072021 
function Search() {
    showloader();
    tblShowPOsConfigurationsData.clear().draw();
    $.ajax({
        url: "/OrderConfig/GetAllOrderConfig",
        type: "Post",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                tblShowPOsConfigurationsData.row.add([
                    data[i].Name,
                    data[i].Status,
                    "<span class=' glyphicon glyphicon-edit edit'  style='cursor: pointer;' title='" + data[i].ID + "' id=" + data[i].ID + " onclick='Edit(this)'>"
                ]).draw(false);
            }
            hideloader();
        },
        error: function (data) {
            hideloader();
        }
    });
    $("#ModalShowPOsConfigurationsData").modal();
}

function Edit(Element) {
    ID = Element.id;
    isEdit = true;
    $.ajax({
        url: "/OrderConfig/GetOrderConfigByID",
        type: "Post",
        data: { _ID: ID },
        success: function (data) {
            _ID = ID;

            $("#txtName").val(data.Name).attr('disabled', true);
            $("#txtNameEng").val(data.NameEng).attr('disabled', true);
            $("#ddVisitScheduleType").val(data.VisitScheduleTypeID).change().attr('disabled', true);
            $("#ddCustomers").val(data.CustomersDtlLst).change(); 
            $("#Active").prop('checked', data.IsActive);


            $('#ModalShowPOsConfigurationsData').modal('toggle');

            hideloader();
        },
        error: function (data) {
            hideloader();
        }

    })


}

