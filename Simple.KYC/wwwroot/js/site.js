function UploadPhoto(instance) {
    var fileUpload = $("#photo").get(0);
    var data = new FormData();
    var reader = new FileReader();
    reader.onload = function (e) {
        $('#preview').attr('src', e.target.result);
    }
    reader.readAsDataURL(fileUpload.files[0]);
    data.append(fileUpload.files[0].name, fileUpload.files[0]);
    $.ajax({
        type: "POST",
        url: "/api/Upload",
        contentType: false,
        processData: false,
        data: data,
        success: function (ids) {
            instance.invokeMethodAsync("SetFaceID", ids)
                .then((result) => {
                    console.log(result);
                });
        },
        error: function () {

        }
    });
    return true;
}


