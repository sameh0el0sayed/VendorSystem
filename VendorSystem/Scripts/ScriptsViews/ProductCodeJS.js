function DownloadCurrentStatus() {

    showloader();
    $.ajax({
        url: '/ProductCode/DownloadCurrentStatus',
        type: 'Post',
        success: function (data) {
            if (data.Status == "Done") {
                window.open(data.FilePath, '_blank');
            }
            else {
                AlertMe('error', data.FilePath, data.FilePath);
            }
            hideloader();
        },
        error: function () { hideloader(); alert('Error'); },
    });

}

function CheckValidation() {
    if ($("#btnUpload").val() == "") {
        AlertMe('info', 'Please choose a file', 'من فضلك اختر الملف');
        return false;
    }

    $("#ModalConfirm").modal();
}

function Save() {

    $("#bntCloseModalConfirm").click();
    StopSave("Save");
    showloader();

    var FileName = $("#btnUpload")[0].value.split(/(\\|\/)/g).pop();
    if (FileName != "" && FileName.indexOf('xls') > -1 ) {
        var formData = new FormData();
        var File = $("#btnUpload")[0].files[0];
        formData.append(File.name, File);
        $.ajax({
            url: '/ProductCode/UploadAndSaveInternalCode',
            type: 'POST',
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (data) {

                if (data == "Done") {
                    AlertMe('success', 'Saved Succefully', 'تم الحفظ بنجاح');
                    EnableSave("Save");
                }
                else {
                    AlertMe('danger', data, data);
                }
                hideloader();
            },
            error: function () { hideloader(); },
        });
    }
    $("#btnUpload")[0].value = "";
}