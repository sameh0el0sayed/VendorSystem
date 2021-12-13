let _ID = 0;
let tblShowPOsConfigurationsData;
let tblData_Rep_Weekly;
let tblData_Rep_Monthly;
//let tblData_Visit_Weekly;
//let tblData_Visit_Monthly;
let RepSchedule_Data = [];
//let VisitSchedule_Data = [];
 

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
        tblData_Rep_Weekly = $("#tblData_Rep_Weekly").DataTable({
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
        tblData_Rep_Monthly = $("#tblData_Rep_Monthly").DataTable({
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

        //tblData_Visit_Weekly = $("#tblData_Visit_Weekly").DataTable({
        //    "language": {
        //        "sProcessing": "جارٍ التحميل...",
        //        "sLengthMenu": "أظهر _MENU_ مدخلات",
        //        "sZeroRecords": "لم يعثر على أية سجلات",
        //        "sInfo": "إظهار _START_ إلى _END_ من أصل _TOTAL_ مدخل",
        //        "sInfoEmpty": "يعرض 0 إلى 0 من أصل 0 سجل",
        //        "sInfoFiltered": "(منتقاة من مجموع _MAX_ مُدخل)",
        //        "sInfoPostFix": "",
        //        "sSearch": "ابحث:",
        //        "sUrl": "",
        //        "oPaginate": {
        //            "sFirst": "الأول",
        //            "sPrevious": "السابق",
        //            "sNext": "التالي",
        //            "sLast": "الأخير"
        //        }
        //    },
        //    "columnDefs": [{ "targets": [0], visible: false }],
        //    searching: false,
        //    paging: false,
        //    info: false
        //});
        //tblData_Visit_Monthly = $("#tblData_Visit_Monthly").DataTable({
        //    "language": {
        //        "sProcessing": "جارٍ التحميل...",
        //        "sLengthMenu": "أظهر _MENU_ مدخلات",
        //        "sZeroRecords": "لم يعثر على أية سجلات",
        //        "sInfo": "إظهار _START_ إلى _END_ من أصل _TOTAL_ مدخل",
        //        "sInfoEmpty": "يعرض 0 إلى 0 من أصل 0 سجل",
        //        "sInfoFiltered": "(منتقاة من مجموع _MAX_ مُدخل)",
        //        "sInfoPostFix": "",
        //        "sSearch": "ابحث:",
        //        "sUrl": "",
        //        "oPaginate": {
        //            "sFirst": "الأول",
        //            "sPrevious": "السابق",
        //            "sNext": "التالي",
        //            "sLast": "الأخير"
        //        }
        //    },
        //    "columnDefs": [{ "targets": [0], visible: false }, { "targets": [1], visible: false }],
        //    searching: false,
        //    paging: false,
        //    info: false
        //});

    } else {

        tblShowPOsConfigurationsData = $("#tblShowPOsConfigurationsData").DataTable({});
        tblData_Rep_Weekly = $("#tblData_Rep_Weekly").DataTable({
            "columnDefs": [{ "targets": [0], visible: false }],
            searching: false,
            paging: false,
            info: false
        });
        tblData_Rep_Monthly = $("#tblData_Rep_Monthly").DataTable({
            "columnDefs": [{ "targets": [0], visible: false }, { "targets": [1], visible: false }],
            searching: false,
            paging: false,
            info: false
        });

        //tblData_Visit_Weekly = $("#tblData_Visit_Weekly").DataTable({
        //    "columnDefs": [{ "targets": [0], visible: false }],
        //    searching: false,
        //    paging: false,
        //    info: false
        //});
        //tblData_Visit_Monthly = $("#tblData_Visit_Monthly").DataTable({
        //    "columnDefs": [{ "targets": [0], visible: false }, { "targets": [1], visible: false }],
        //    searching: false,
        //    paging: false,
        //    info: false
        //});
    }
})

//----------------------------------------------------------------------------------------------------------------------------------- 

$("#ddReplenishmentScheduleType").change(function () {

    $("#divRep_Weekly").hide();
    $("#divRep_Monthly").hide();

    if ($("#ddReplenishmentScheduleType").val() == 7) {
        $("#divRep_Weekly").show();
    }
    else if ($("#ddReplenishmentScheduleType").val() == 28) {
        $("#divRep_Monthly").show();
    }

});

function InsertInto_tblData_Rep_Weekly() {

    if ($("#ddRep_Days_Weekly").val() == "") {
        AlertMe('danger', 'please choose day', 'من فضلك اختر اليوم');
        return false;
    }

    let IsExist = false;
    let DayNumber;
    let NewDayNumber = Number($("#ddRep_Days_Weekly").val());

    var Rows = tblData_Rep_Weekly.rows({ 'search': 'applied' }).nodes();
    Rows.each(function (index) {
        let Row = tblData_Rep_Weekly.row(index);
        let Data = Row.data();
        DayNumber = Data[0];

        if (NewDayNumber == DayNumber) {
            IsExist = true;
            return;
        }
    });

    if (IsExist == false) {
        tblData_Rep_Weekly.row.add([
                       NewDayNumber,
                      $("#ddRep_Days_Weekly option:selected").text(),
                     "<span class='btn btn-info' onclick='RemoveFrom_Rep_Weekly(this)'><i class='glyphicon glyphicon-remove'></i></span>"
        ]).draw(false);
    }

    $("#ddRep_Days_Weekly").val('0').change();
}

function RemoveFrom_Rep_Weekly(Element) {
    tblData_Rep_Weekly
      .row($(Element).parents('tr'))
      .remove()
      .draw();
}

function InsertInto_tblData_Rep_Monthly() {

    if ($("#ddRep_Days_Monthly").val() == "") {
        AlertMe('danger', 'please choose day', 'من فضلك اختر اليوم');
        return false;
    }

    let IsExist = false;
    let DayNumber;
    let NewDayNumber = Number($("#ddRep_Days_Monthly").val());

    var Rows = tblData_Rep_Monthly.rows({ 'search': 'applied' }).nodes();
    Rows.each(function (index) {
        let Row = tblData_Rep_Monthly.row(index);
        let Data = Row.data();
        DayNumber = Data[1];

        if (NewDayNumber == DayNumber && $("#ddRep_WeekNumber_Monthly option:selected").text() == Data[0]) {
            IsExist = true;
            return;
        }
    });

    if (IsExist == false) {
        tblData_Rep_Monthly.row.add([
             $("#ddRep_WeekNumber_Monthly").val(), NewDayNumber,
             $("#ddRep_WeekNumber_Monthly option:selected").text(),
             $("#ddRep_Days_Monthly option:selected").text(),
             "<span class='btn btn-info' onclick='RemoveFrom_Rep_Monthly(this)'><i class='glyphicon glyphicon-remove'></i></span>"
        ]).draw(false);
    }

    $("#ddRep_Days_Monthly").val('0').change();
}

function RemoveFrom_Rep_Monthly(Element) {
    tblData_Rep_Monthly
      .row($(Element).parents('tr'))
      .remove()
      .draw();
}

//-----------------------------------------------------------------------------------------------------------------------------------
//old function for visit schedule
//$("#ddVisitScheduleType").change(function () {

//    $("#divVisit_Weekly").hide();
//    $("#divVisit_Monthly").hide();
//    $("#divPercentage").hide();

//    if ($("#ddVisitScheduleType").val() == 7) {
//        $("#divVisit_Weekly").show();
//        $("#divPercentage").show();
//    }
//    else if ($("#ddVisitScheduleType").val() == 28) {
//        $("#divVisit_Monthly").show();
//        $("#divPercentage").show();
//    }

//});

//function InsertInto_tblData_Visit_Weekly() {

//    if ($("#ddVisit_Days_Weekly").val() == "") {
//        AlertMe('danger', 'please choose day', 'من فضلك اختر اليوم');
//        return false;
//    }

//    if (Number($("#txtPercentage").val()) <= 0 || Number($("#txtPercentage").val()) > 100) {
//        AlertMe('danger', 'please insert valid percentage', 'من فضلك ادخل نسبه صحيحه');
//        return false;
//    }

//    let IsExist = false;
//    let DayNumber;
//    let NewDayNumber = Number($("#ddVisit_Days_Weekly").val());

//    var Rows = tblData_Visit_Weekly.rows({ 'search': 'applied' }).nodes();
//    Rows.each(function (index) {
//        let Row = tblData_Visit_Weekly.row(index);
//        let Data = Row.data();
//        DayNumber = Data[0];

//        if (NewDayNumber == DayNumber) {
//            IsExist = true;
//            return;
//        }
//    });

//    if (IsExist == false) {
//        tblData_Visit_Weekly.row.add([
//                       NewDayNumber,
//                      $("#ddVisit_Days_Weekly option:selected").text(), $("#txtPercentage").val(),
//                     "<span class='btn btn-info' onclick='RemoveFrom_Visit_Weekly(this)'><i class='glyphicon glyphicon-remove'></i></span>"
//        ]).draw(false);
//    }

//    $("#ddVisit_Days_Weekly").val('0').change();
//}

//function RemoveFrom_Visit_Weekly(Element) {
//    tblData_Visit_Weekly
//      .row($(Element).parents('tr'))
//      .remove()
//      .draw();
//}


//function InsertInto_tblData_Visit_Monthly() {

//    if ($("#ddVisit_Days_Monthly").val() == "") {
//        AlertMe('danger', 'please choose day', 'من فضلك اختر اليوم');
//        return false;
//    }

//    if (Number($("#txtPercentage").val()) <= 0 || Number($("#txtPercentage").val()) > 100) {
//        AlertMe('danger', 'please insert valid percentage', 'من فضلك ادخل نسبه صحيحه');
//        return false;
//    }

//    let IsExist = false;
//    let DayNumber;
//    let NewDayNumber = Number($("#ddVisit_Days_Monthly").val());

//    var Rows = tblData_Visit_Monthly.rows({ 'search': 'applied' }).nodes();
//    Rows.each(function (index) {
//        let Row = tblData_Visit_Monthly.row(index);
//        let Data = Row.data();
//        DayNumber = Data[1];

//        if (NewDayNumber == DayNumber && $("#ddVisit_WeekNumber_Monthly").val() == Data[0]) {
//            IsExist = true;
//            return;
//        }
//    });

//    if (IsExist == false) {
//        tblData_Visit_Monthly.row.add([

//              $("#ddVisit_WeekNumber_Monthly").val(), NewDayNumber,
//              $("#ddVisit_WeekNumber_Monthly option:selected").text(),
//              $("#ddVisit_Days_Monthly option:selected").text(),
//              $("#txtPercentage").val(),
//              "<span class='btn btn-info' onclick='RemoveFrom_Rep_Monthly(this)'><i class='glyphicon glyphicon-remove'></i></span>"
//        ]).draw(false);
//    }

//    $("#ddVisit_Days_Monthly").val('0').change();
//}

//Sameh: this make conflict
//function RemoveFrom_Rep_Monthly(Element) {
//    tblData_Visit_Monthly
//      .row($(Element).parents('tr'))
//      .remove()
//      .draw();
//}

//-----------------------------------------------------------------------------------------------------------------------------------

$("#ddFirstClass").change(function () {

    $("#ddSecondClass").empty().change();

    if ($("#language").html() == "اللغة") {
        $("#ddSecondClass").append("<option selected value=''>اختر</option>")
    }
    else {
        $("#ddSecondClass").append("<option selected value=''>Please Select</option>")
    }

    if ($("#ddFirstClass").val() != "" && $("#ddFirstClass").val() != null) {
        let Options = "";

        var Data = GetChildrenDataByParentID($("#ddFirstClass").val());

        if (Data == 'Error') return;

        for (var item of Data) {

            Options += "<option value=" + item.ID + ">" + item.Name + "</option>";
        }
        $("#ddSecondClass").append(Options);

    }

});

$("#ddSecondClass").change(function () {

    $("#ddThirdClass").empty().change();

    if ($("#language").html() == "اللغة") {
        $("#ddThirdClass").append("<option selected value=''>اختر</option>")
    }
    else {
        $("#ddThirdClass").append("<option selected value=''>Please Select</option>")
    }

    if ($("#ddSecondClass").val() != "" && $("#ddSecondClass").val() != null) {
        let Options = "";

        var Data = GetChildrenDataByParentID($("#ddSecondClass").val());


        for (var item of Data) {

            Options += "<option value=" + item.ID + ">" + item.Name + "</option>";
        }
        $("#ddThirdClass").append(Options);
    }

});

$("#ddThirdClass").change(function () {

    $("#ddFourthClass").empty().change();

    if ($("#language").html() == "اللغة") {
        $("#ddFourthClass").append("<option selected value=''>اختر</option>")
    }
    else {
        $("#ddFourthClass").append("<option selected value=''>Please Select</option>")
    }

    if ($("#ddThirdClass").val() != "" && $("#ddThirdClass").val() != null) {
        let Options = "";

        var Data = GetChildrenDataByParentID($("#ddThirdClass").val());


        for (var item of Data) {

            Options += "<option value=" + item.ID + ">" + item.Name + "</option>";
        }
        $("#ddFourthClass").append(Options);
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

    if ($("#ddFirstClass").val() == null) {
        AlertMe('danger', 'Please choose first classification', 'من فضلك اختر التصنيف الاول')
        return false;
    }

    if (Number($("#ddVisitScheduleType").val()) > Number($("#ddReplenishmentScheduleType").val())) {
        AlertMe('danger', 'Visit schedule not correct according to replenishment schedule', 'نوع الزيارات غير صحيح بالنسبه لنوع الاستعواض');
        return false;
    }

    RepSchedule_Data = [];
    if ($("#ddReplenishmentScheduleType").val() == 7) { //weekly

        var Rows = tblData_Rep_Weekly.rows({ 'search': 'applied' }).nodes();
        Rows.each(function (index) {
            let Row = tblData_Rep_Weekly.row(index);
            let Data = Row.data();

            RepSchedule_Data.push({ DayNumber: Data[0] });
        });

        if (RepSchedule_Data.length == 0) {
            AlertMe('danger', 'Please insert one day at least in Replenishment Days', 'يرجى ادخال يوم واحد على الاقل فى جدول ايام الاستعواض');
            return false;
        }
    }
    if ($("#ddReplenishmentScheduleType").val() == 28) { // monthly
        var Rows = tblData_Rep_Monthly.rows({ 'search': 'applied' }).nodes();
        Rows.each(function (index) {
            let Row = tblData_Rep_Monthly.row(index);
            let Data = Row.data();
            RepSchedule_Data.push({ DayNumber: Data[0], WeekNumber: Data[1] });
        });

        if (RepSchedule_Data.length == 0) {
            AlertMe('danger', 'Please insert one day at least in Replenishment Days', 'يرجى ادخال يوم واحد على الاقل فى جدول ايام الاستعواض');
            return false;
        }
    }

    let TotalPercentage = 0;
    //VisitSchedule_Data = [];
    //if ($("#ddVisitScheduleType").val() == 7) { //weekly

    //    var Rows = tblData_Visit_Weekly.rows({ 'search': 'applied' }).nodes();
    //    Rows.each(function (index) {
    //        let Row = tblData_Visit_Weekly.row(index);
    //        let Data = Row.data();

    //        TotalPercentage += Number(Data[2]);
    //        VisitSchedule_Data.push({ DayNumber: Data[0], OrderPercentage: Data[2] });
    //    });

    //    if (TotalPercentage != 100) {
    //        AlertMe('danger', 'The Total of visits percenage must be 100', 'مجموع نسب الزيارات يجب ان يكون  100');
    //        return false;
    //    }
    //    if (VisitSchedule_Data.length == 0) {
    //        AlertMe('danger', 'Please insert one day at least in Visits Days', 'يرجى ادخال يوم واحد على الاقل فى جدول ايام الزيارات');
    //        return false;
    //    }
    //}
    //if ($("#ddVisitScheduleType").val() == 28) { // monthly
    //    var Rows = tblData_Visit_Monthly.rows({ 'search': 'applied' }).nodes();
    //    Rows.each(function (index) {
    //        let Row = tblData_Visit_Monthly.row(index);
    //        let Data = Row.data();
    //        TotalPercentage += Number(Data[4]);
    //        VisitSchedule_Data.push({ DayNumber: Data[1], WeekNumber: Data[0], OrderPercentage: Data[4] });
    //    });

    //    if (TotalPercentage != 100) {
    //        AlertMe('danger', 'The Total of visits percenage must be 100', 'مجموع نسب الزيارات يجب ان يكون  100');
    //        return false;
    //    }
    //    if (VisitSchedule_Data.length == 0) {
    //        AlertMe('danger', 'Please insert one day at least in Visit Days', 'يرجى ادخال يوم واحد على الاقل فى جدول ايام الزيارات');
    //        return false;
    //    }
    //}

    $("#ModalConfirm").modal();
}

function Save() {
  
    StopSave("BtnSave");
    let _ReplenishConfigVM =
    {
        ID: _ID,
        Name: $("#txtName").val().trim(),
        NameEng: $("#txtNameEng").val().trim(),
        CalcTypeID: $("#ddCalculationTypeID").val(),
        Rep_ScheduleTypeID: $("#ddReplenishmentScheduleType").val(),
        Visit_ScheduleTypeID: $("#ddVisitScheduleType").val(),
        IsIncludeMinQty: $("#IsIncludeMinQty").prop('checked'),
        IsActive: $("#Active").prop('checked'),

        RepSchedule_Data: RepSchedule_Data,
      //  VisitSchedule_Data: VisitSchedule_Data,

        FirstClassificationID: $("#ddFirstClass").val(),
        SecondClassificationID: $("#ddSecondClass").val(),
        ThirdClassificationID: $("#ddThirdClass").val(),
        FourthClassificationID: $("#ddFourthClass").val()

    }

    showloader();
    $.ajax({
        type: "POST",
        url: '/ReplenishmentConfiguration/Save',
        data: { ReplenishConfigVM: _ReplenishConfigVM },
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
        url: "/ReplenishmentConfiguration/GetAllReplenishConfig",
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
      
    $.ajax({
        url: "/ReplenishmentConfiguration/GetReplenishConfigByID",
        type: "Post",
        data: { _ID: ID },
        success: function (data) {
            _ID = ID;
                
            $("#txtName").val(data[0].Name).attr('disabled', true);
            $("#txtNameEng").val(data[0].NameEng).attr('disabled', true);
            $("#ddCalculationTypeID").val(data[0].CalcTypeID).change().attr('disabled', true);
            $("#ddReplenishmentScheduleType").val(data[0].RepScheduleTypeID).change().attr('disabled', true);
            $("#ddRep_Days_Weekly").val(data[0].ScheduleType).change().attr('disabled', true);
            $("#Active").prop('checked', data[0].IsActive);
            $("#IsIncludeMinQty").prop('checked', data[0].IsIncludeMinQty).attr('disabled', true);
            $("#ddFirstClass").val(data[0].FirstClassID).change().attr('disabled', true);
            $("#ddSecondClass").val(data[0].SecondClassID).change().attr('disabled', true);
            $("#ddThirdClass").val(data[0].ThirdClassID).change().attr('disabled', true);
            $("#ddFourthClass").val(data[0].FourthClassID).change().attr('disabled', true);

 

            $('#ModalShowPOsConfigurationsData').modal('toggle');

            hideloader();
        },
        error: function (data) {
            hideloader();
        }
       
    })

   
}

