$("#ddCountry").change(function () {

    $("#ddProvince").empty().change();

    if ($("#language").html() == "اللغة") {
        $("#ddProvince").append("<option selected value=''>اختر</option>")
    }
    else {
        $("#ddProvince").append("<option selected value=''>Please Select</option>")
    }

    if ($("#ddCountry").val() != "" && $("#ddCountry").val() != null) {
        let Options = "";

        var Data = GetChildrenDataByParentID($("#ddCountry").val(), true);

        if (Data == 'Error') return;

        for (var item of Data) {

            Options += "<option value=" + item.ID + ">" + item.Name + "</option>";
        }
        $("#ddProvince").append(Options);

    }

});

$("#ddProvince").change(function () {

    $("#ddCity").empty().change();

    if ($("#language").html() == "اللغة") {
        $("#ddCity").append("<option selected value=''>اختر</option>")
    }
    else {
        $("#ddCity").append("<option selected value=''>Please Select</option>")
    }

    if ($("#ddProvince").val() != "" && $("#ddProvince").val() != null) {
        let Options = "";

        var Data = GetChildrenDataByParentID($("#ddProvince").val(), true);


        for (var item of Data) {

            Options += "<option value=" + item.ID + ">" + item.Name + "</option>";
        }
        $("#ddCity").append(Options);
    }

});

$("#ddCity").change(function () {

    $("#ddRegion").empty().change();

    if ($("#language").html() == "اللغة") {
        $("#ddRegion").append("<option selected value=''>اختر</option>")
    }
    else {
        $("#ddRegion").append("<option selected value=''>Please Select</option>")
    }

    if ($("#ddCity").val() != "" && $("#ddCity").val() != null) {
        let Options = "";

        var Data = GetChildrenDataByParentID($("#ddCity").val(), true);


        for (var item of Data) {

            Options += "<option value=" + item.ID + ">" + item.Name + "</option>";
        }
        $("#ddRegion").append(Options);
    }

});

$("#ddRegion").change(function () {

    $("#ddTerritory").empty().change();

    if ($("#language").html() == "اللغة") {
        $("#ddTerritory").append("<option selected value=''>اختر</option>")
    }
    else {
        $("#ddTerritory").append("<option selected value=''>Please Select</option>")
    }

    if ($("#ddRegion").val() != "" && $("#ddRegion").val() != null) {
        let Options = "";

        var Data = GetChildrenDataByParentID($("#ddRegion").val(), true);


        for (var item of Data) {

            Options += "<option value=" + item.ID + ">" + item.Name + "</option>";
        }
        $("#ddTerritory").append(Options);
    }

});