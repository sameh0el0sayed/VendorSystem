var selectedItem = [];
$(function () {
    
    $('#jstree').on('changed.jstree', function (e, data) {
        var i, j;
        var IsEdit = false;
        if ($('#IsEdit').is(':checked'))
            IsEdit = true;
        for (i = 0, j = data.selected.length; i < j; i++) {
            //Fetch the Id.
            var id = data.selected[i];
            //Remove the ParentId.
            if (id.indexOf('-') != -1) {
                id = id.split("-")[1];
            }
            //Add the Node to the JSON Array.
            selectedItem.push({
                text: data.instance.get_node(data.selected[i]).text,
                textEn: data.instance.get_node(data.selected[i]).text,
                id: id,
                parent: data.node.parents[0],//parent
                IsEdit: IsEdit
            });
        }
        if ($('#IsEdit').is(':checked')) {
            $('#Name').val(selectedItem[0].text);
            $('#NameEn').val(selectedItem[0].text);
        }
        else {
            $('#Name').val("");
            $('#NameEn').val("");
        }
        //Serialize the JSON Array and save in HiddenField.
        $('#selectedItem').val(JSON.stringify(selectedItem));
    }).jstree({
        "core": {
            "multiple": false,
            "themes": { "variant": "large" },
            "data": JSON.parse('@Html.Raw(Json.Encode(ViewBag.Json))') /*@Html.Raw(ViewBag.Json)*/
            }, "checkbox": { "keep_selected_style": true },
        // "plugins": ["wholerow", "checkbox"],
        /////////////////////
        multiSelect: false,
        cacheItems: true,
        'open-icon': 'ace-icon tree-minus',
        'close-icon': 'ace-icon tree-plus',
        'itemSelect': true,
        'folderSelect': false,
        'selected-icon': 'ace-icon fa fa-check',
        'unselected-icon': 'ace-icon fa fa-times',
        loadingHTML: '<div class="tree-loading"><i class="ace-icon fa fa-refresh fa-spin blue"></i></div>'
        /////////////////////////////////
    });
});
function Save()
{ 
    
    if ($("#Name").val() == "" || $("#NameEn").val() == "") {
        if ($("#language").html() == "اللغة") {
            AlertNotify("info", "عفوا لابد من ادخال اسم المرجع ");
        }
        else {
            AlertNotify("info", "Sorry, You must enter refrence name!");
            return false;
        }
    }
    //if (!IsExit) {
    //    $("#CloseModel").click();
    //    if ($("#language").html() == "اللغة") {AlertNotify("info","هذا البنك موجود مسبقا في قاعدة البيانات");
    //        return false;}
    //    else {AlertNotify("info","This Bank Is Already Exist");
    //        return false;}

    //    return false;
    //}
    StopSave("BtnSave");
    var selectedItems ;
    if($('#IsEdit').is(':checked')){
        selectedItems = {
            text:$('#Name').val(),// data.instance.get_node(data.selected[i]).text,
            textEn:$('#NameEn').val(),// data.instance.get_node(data.selected[i]).text,
            id:selectedItem[0].id,
            parent:selectedItem[0].parent,// data.node.parents[0],//parent
            IsEdit:true
        };
    }
    else{
        selectedItems = {
            text:$('#Name').val(),// data.instance.get_node(data.selected[i]).text,
            textEn:$('#NameEn').val(),// data.instance.get_node(data.selected[i]).text,
            id:selectedItem[0].id,
            parent:selectedItem[0].id,// data.node.parents[0],//parent
            IsEdit:false
        };
    }
    var data = { selectedItems};
    $.ajax({
        type:"POST",
        cache:false,
        async:true,
        datatype: 'json',
        contenttype:'application/json; charset=utf-8',
        url:'/LockUp/Index',
        //  data: JSON.stringify(data),
        data:data,
        success:function(data){
            if ($("#language").html() == "اللغة") {AlertNotify("success","تم الحفظ بنجاح");}
            else {AlertNotify("success","Saved Successfully");}            
            setTimeout(function(){ location.reload();},1000)
            if (data==1) {
                $("#CloseModel").click();
                if ($("#language").html() == "اللغة") {AlertNotify("success","تم الحفظ بنجاح");}
                else {AlertNotify("success","Saved Successfully");}            
                setTimeout(function(){ location.reload();},1000)
            }
            else {
                $("#CloseModel").click();
                if ($("#language").html() == "اللغة") {AlertNotify("info","من فضلك املا البيانات المطلوبة");}
                else {AlertNotify("info","Please fill in required fields");}
            }
        },
        error:function(data){ alert ('sdfghjkjhgfdsa');
        }
    })
}
//$("#IsEdit").change(function () {
//    var i, j;
//    var selectedItems = [];
//    if ($('#IsEdit').is(':checked')) {
//        for (i = 0, j = data.selected.length; i < j; i++) {
//            //Fetch the Id.
//            var id = data.selected[i];
//            //Remove the ParentId.
//            if (id.indexOf('-') != -1) {
//                id = id.split("-")[1];
//            }
//            //Add the Node to the JSON Array.
//            selectedItems.push({
//                text: data.instance.get_node(data.selected[i]).text,
//                textEn: data.instance.get_node(data.selected[i]).texttextEn,
//                id: id,
//                parent: data.node.parents[0],//parent
//                IsEdit: IsEdit
//            });
//        }
//        $('#Name').val(selectedItems[0].text);
//        $('#NameEn').val(selectedItems[0].textEn);
//    }
//    else {
//        $('#Name').val("");
//        $('#NameEn').val("");
//    }
//});
//jstree-anchor jstree-clicked  class when chose node from tree