var Productdata = [];
var productsDataTable;
var ID=0;
var FormType = 1;
var URL = "/Store/Index";
$(function () {
    ;
    if ($("#language").html() == "اللغة") {
        var Date= $("#Date").datetimepicker({format:'DD/MM/YYYY hh:mm A',showClose: true,showClear:true, keepInvalid:true }).on('dp.change', function(){ });
        $("#Date").css("direction", "ltr");
        $("#Date").css("text-align","right");
        productsDataTable = $("#transaction").DataTable({
            "language": { "sProcessing": "جارٍ التحميل...", "sLengthMenu": "أظهر _MENU_ مدخلات","sZeroRecords": "لم يعثر على أية سجلات","sInfo": "إظهار _START_ إلى _END_ من أصل _TOTAL_ مدخل",
                "sInfoEmpty": "يعرض 0 إلى 0 من أصل 0 سجل", "sInfoFiltered": "(منتقاة من مجموع _MAX_ مُدخل)","sInfoPostFix": "","sSearch": "ابحث:",
                "sUrl": "", "oPaginate": {"sFirst": "الأول","sPrevious": "السابق","sNext": "التالي","sLast": "الأخير"}},
            "lengthMenu": [[5,10, 25, 50, -1], [5,10, 25, 50, "All"]],}); }
    else {
        $("#Date").datetimepicker({format:'DD/MM/YYYY hh:mm A',showClose: true,showClear:true,keepInvalid:true}).on('dp.change', function(){ });
        productsDataTable = $("#transaction").DataTable({"lengthMenu": [[5,10, 25, 50, -1], [5,10, 25, 50, "All"]]});}
    ;
    document.getElementById('Po').disabled = true;
    $("#transactionType").val($("#transactionType option:first").val()).change();
    $("#AllWarehouses").val($("#AllWarehouses option:first").val()).change();
    //document.getElementById('AllWarehouses').disabled = true;
    productsDataTable.clear().draw();
        productsDataTable.row.add([
             Product.Prod_Id , Product.Barcode ,Product.Product_Name , Product.Purchase_Uom_Name,
                  "<input type='text' class='form-control Quantity'  autocomplete = 'off', maxlength = '10', onkeypress ='return isNumberKey(event)' flag='Q' value=" +Product.Qty + "  Onchange='calcTotal2(this)'/>" , 
                  "<input type='text' class='form-control Price'  autocomplete = 'off', maxlength = '10', onkeypress ='return isNumberKey(event)' flag='P'  value=" + Product.Unit_Price + "  Onchange='calcTotal2(this)'/>"
                  ,"<input type='text' class='form-control Total' readonly  autocomplete = 'off', maxlength = '10', onkeypress ='return isNumberKey(event)' flag='T' value=" + Product.Total + "  Onchange='SetHiddenQuntityVal(this)'/>",
                  "<input type='text' class='form-control Notes'  autocomplete = 'off' maxlength = '50' value='' '  Onchange='SetHiddenQuntityVal(this)'/>"
        ]).draw(false);    
})
function Save()
{ 
    var Form = $("#Form");
    Form.validate();
    if (Form.valid()) {
      
        StopSave("BtnSave"); 
        var GetProductToNotificationVM = {
            PO_DocumentNumber: $("#PO_DocumentNumber").val(),
            Date: $("#Date").val(),
            transactionType:$('#transactionType').val(),
            Reciver_WH_Id:$("#Reciver_WH_Id").val(),
            po: $("#Po").val(),
            Mo: $("#Mo").val(),
            FormType: FormType,
            ////////////////
                Prod_Id:Product.Prod_Id,
                Barcode:Product.Barcode,
                Product_Name: Product.Product_Name,
                Purchase_Uom_Name: Product.Purchase_Uom_Name,
                Qty:Product.Qty,
                Unit_Price: Product.Unit_Price,
                Total: Product.Total,
                Notes: Product.Notes,
        };
        var data = {GetProductToNotificationVM};
        $.ajax({
            type:"POST",
            cache:false,
            async:true,
            datatype: 'json',
            contenttype:'application/json; charset=utf-8',
            url:'/Store/SaveTransactionNotification',
            //  data: JSON.stringify(data),
            data:data,
            success:function(data){
                if(data==1)
                {
                    $("#CloseSaveModel").click();
                    if ($("#language").html() == "اللغة") {AlertNotify("success","تم الحفظ بنجاح")}
                    else {AlertNotify("success","Saved Successfully")}
                    setTimeout(function(){ window.open('/Store/Tranaction','_self');
                    },1000)
                    }
                else if(data==0){$("#CloseSaveModel").click();
                    if ($("#language").html() == "اللغة") {AlertNotify("info","من فضلك املا البيانات المطلوبة")}
                    else {AlertNotify("info","Please fill in required fields")}
                    EnableSave("BtnSave"); }
                else if(data==2){ $("#CloseSaveModel").click();
                    if ($("#language").html() == "اللغة") {AlertNotify("info","الكمية الموجودة لا تكفى")}
                    else {AlertNotify("info","Quantity Avilable is Not enough")}
                    EnableSave("BtnSave"); }
            },error:function(data){} })}
    else {$("#CloseSaveModel").click();
        if ($("#language").html() == "اللغة") {AlertNotify("info","من فضلك املا البيانات المطلوبة")}
        else {AlertNotify("info","Please fill in required fields")}
    }
}
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}
function calcTotal2(e) {
     
    var flage = e.getAttribute("flag")
    var tbl = document.getElementById("transaction");
    if (flage == "Q")
    {
        
        var parentTd = e.parentElement;
        var parentTr = parentTd.parentElement;
        parentTr.cells[4].children[0].value;
        //quantity exist in recieving process
        var Q = parentTr.cells[4].children[0].value;
        Q=parseFloat(Q);
        if (Q > 0 ) { 
           
            total = parentTr.cells[5].children[0].value * Q;
            parentTr.cells[6].children[0].value = "0";
            parentTr.cells[6].children[0].value = total;
            
            
            Product.Qty=parentTr.cells[4].children[0].value;
            Product.Unit_Price=parentTr.cells[5].children[0].value;
            Product.Total=parentTr.cells[6].children[0].value;
            Product.Notes=parentTr.cells[7].children[0].value;
        } 
        else {
            parentTr.cells[4].children[0].value = "0";
            if ($("#language").html() == "اللغة") {AlertNotify("success","يوجد خطأ في الكمية المدخلة")}
            else{ AlertNotify("info","There are Wrong entry in Quantity");}
        }
    }
    else if (flage == "P") {
        var parentTd = e.parentElement;
        var parentTr = parentTd.parentElement;
        parentTr.cells[5].children[0].value;
       
        var Q = parentTr.cells[4].children[0].value;
        Q=parseFloat(Q);
        if (Q > 0) {
            
            total = parentTr.cells[5].children[0].value * Q;
            parentTr.cells[6].children[0].value = "0";
            parentTr.cells[6].children[0].value = total;

            Product.Qty=parentTr.cells[4].children[0].value;
            Product.Unit_Price=parentTr.cells[5].children[0].value;
            Product.Total=parentTr.cells[6].children[0].value;
            Product.Notes=parentTr.cells[7].children[0].value;
        }
        else {
            parentTr.cells[5].children[0].value = "0";
            if ($("#language").html() == "اللغة") {AlertNotify("success","يوجد خطأ في الكمية المدخلة")}
            else{ AlertNotify("info","There are Wrong entry in Quantity");}
        
        }
    }
}
function SetHiddenQuntityVal(Element) {
   
    $(Element).next('input').val(Element.value);
}
function FormSubmit() { $('#Form').submit(); }
$("#PO_DocumentNumber").change(function () {

    $.ajax({
        url: "/Store/CheckDocumentNumber",
        type: "GET",
        data: { DocumentNumber: $("#PO_DocumentNumber").val() },
        success: function (data) {
            if (data == 1) {
                $("#PO_DocumentNumber").val('');
                if ($("#language").html() == "اللغة") {
                    AlertNotify("info","عفوا لقد ادخلت رقم وثيقة  موجود  من قبل ");}
                    
                else{AlertNotify("info","Sorry, You entered Existing Document Number!");}
            }
        },
        error: function (data) { }
    });
})


