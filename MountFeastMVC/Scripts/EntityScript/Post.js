$(document).ready(function () {
    if ($('#AvoidReload').val() != "True") {

    }

    $(function () {
        $('.summernote').summernote();
    })

    $("table").on("click", ".ChangeRoom", function (ev) {
        // ev.preventDefault();
        $('#ChangeRoomModal').modal('show');

        var id = this.id;
        LoadRoomNums(id);
        $('#BookingID').val(id);
    });

    $('#closePic').click(function () {
        $('#ChangeRoomModal').modal('hide');
    })

    $('#doaddnew').click(function () {
      
        SaveData();       
    })

    function SaveData(fileName) {
        try {

            if (window.FormData !== undefined) {

                var fileUpload = $("#BlogImage").get(0);
                var files = fileUpload.files;

                var fileName = $("#BlogImage").val().replace(/C:\\fakepath\\/i, '');
               // alert(fileName);

                var ext = $('#BlogImage').val().split('.').pop().toLowerCase();
                var arrayExtensions = ["jpg", "jpeg", "png", "bmp", "gif"];

                if (arrayExtensions.lastIndexOf(ext) == -1) {
                    alert("Wrong extension type. Allowed extension types are jpg,jpeg,png,bmp,gif");
                    $("#image").val("");
                    return;
                }

                var picsize = files[0].size;
                //alert(picsize);
                if (picsize > 1000000) {
                    alert("Picture size should less the or equal to 1MB.")
                    return;
                }

                // Create FormData object  
                var fileData = new FormData();
                //var fileName = $('#propertyID').val() + "-main." + ext;

                // Looping over all files and add it to FormData object  
                for (var i = 0; i < files.length; i++) {
                    fileData.append(files[i].name, files[i], fileName);
                }

                // Adding one more key to FormData object  
                fileData.append('username', "Manas");


                var pPY = new Object();
                pPY.Title = $('#Title').val();
                pPY.Slug = $('#Slug').val();
                pPY.Description = $('#Description').val();
                pPY.ShortDescriptions = $('#ShortDescriptions').val();
                pPY.MetaDescription = $('#MetaDescription').val();
                pPY.Keywords = $('#Keywords').val();
                pPY.Author = $('#Author').val();


                if (typeof fileName !== undefined) {
                    pPY.DefaultImage = fileName;
                }

                $.ajax({
                    url: '/Post/Manage',
                    type: 'POST',
                    dataType: 'json',
                    data: JSON.stringify(pPY),
                    contentType: 'application/json',
                    success: function () {
                        alert("New Post Succesfully published");
                       // window.location.href = '/Post/Index';
                    },
                    error: function (err) {
                        alert("Some Thing Went Wrong");

                        //alert(err.status + " : " + err.statusText);
                       // window.location.href = '/Post/Index';
                    },

                });

                $.ajax({
                    url: '/Post/UploadFiles',
                    type: "POST",
                    contentType: false, // Not to set any content header  
                    processData: false, // Not to process data  
                    data: fileData,
                    success: function (result) {

                        alert(result);
                    },
                    error: function (err) {
                        alert(err.statusText);
                    }
                });
            }
            else {
                alert("Please select a blog default image.");
                return;
            }
        } catch (e) {
            //alert(e);
            //  window.location.href = '/Property/Index';

        }
    }

});