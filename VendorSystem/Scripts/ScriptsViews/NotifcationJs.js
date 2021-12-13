var Statustrue;
var StatusFalse;
var DataTable;
$(function () {
    if ($("#language").html() == "اللغة") {
        Statustrue = "ظهرت";
        StatusFalse = "لم تظهر";
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
        fixedHeader: true,
       
    });
}
else {
        DataTable = $("#tbShowData").DataTable();
        Statustrue = "Seen";
        StatusFalse = "Not Seen";
}
FillTable(Notifcations);

})

function FillTable(data) {
    var Seen = StatusFalse;
    DataTable.clear().draw();
    for (var i = 0; i < data.length; i++) {
        if (data[i].Seen == true) {
            Seen = Statustrue;
        }
        else {
            Seen = StatusFalse;
        }


        DataTable.row.add([
            data[i].NotifcationMessage,
            ToJavaScriptDateTime(data[i].Date),
        Seen
        ]).draw(false);
    }

}