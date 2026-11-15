$(document).ready(function () {
    if ($('#AvoidReload').val() != "True") {
        // alert("g");
        LoadCategories();
        //loadCountries();
    }

    function loadRooms() {
        try {
            var propertyID = new Object();
            propertyID = $('#ddlProperty').val();
              
            $.ajax({
                url: '/Property/LoadpropertyCategories',
                type: 'GET',
                dataType: 'json',
                contentType: 'application/json',
                data: 'propertyID=' + propertyID,
                success: function (data) {
                    $('#rooms').empty().append('<option selected="selected" value="0">Please select</option>');
                    if (data.length > 0) {

                        $.each(data, function (i, item) {
                            $('#rooms').append($('<option></option>').attr('value', item.CateId).text(item.Category));
                        });
                        
                    }
                },
                error: function (err) {
                   alert(err.status + " : " + err.statusText);
                   
                }

            });
        } catch (e) {
            alert(e.status + " : " + e.statusText);
            window.location.href = '/Property//RoomImages';
        }
    }

    function loadRoomPictures() {
        try {
            var RoomCode = new Object();
            RoomCode = $('#rooms').val();
            //  alert(propertyID);
            $.ajax({
                url: '/Property/RoomPictures',
                type: 'GET',
                dataType: 'json',
                contentType: 'application/json',
                data: 'RoomCode=' + RoomCode,
                success: function (data) {
                    //console.log(data);
                    $("#images").empty();
                    var trHTML = '';
                    trHTML = '<tr><th>S No.</th><th>Image Name</th><th>Image</th><th>Description</th><th></th><th></th>';
                    var base_url = window.location.origin;
                    if (data.length > 0) {

                        $.each(data, function (i, item) {
                            var desc = item.Description;
                           // console.log(desc);
                            if (desc == null) {
                                desc = "";
                            }

                            trHTML += '<tr><td>' + item.PicNum + '</td><td id=pic>' + item.PicName + '</td>' +
                                "<td><img src='/Uploads/" + item.PicName + "' alt='test' style='height:100px'></td >" +
                                "<td><input type='text' id='" + item.PicId + "'  class='form-control' value='" + desc + "' /></td>" +
                                "<td><input type='button' ID='" + item.PicId + "' class='Update btn btn-success' value='Update' /></td>" +
                                "<td><input type='button' ID='" + item.PicId + "' class='Delete btn btn-danger' value='Delete' /></td>";

                        });
                        $('#images').append(trHTML);
                    }
                },
                error: function (err) {
                    alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            alert(e.status + " : " + e.statusText);
            window.location.href = '/Property//RoomImages';
        }
    }

    $("table").on("click", ".Delete", function (ev) {
        // ev.preventDefault();
        var id = this.id;
        if (confirm("Are you sure to delete this picture?")) {
            DeletePic(id);
        }
        else {
            return false;
        }

    });

    function DeletePic(id) {

        try {
            $.ajax({
                url: '/Property/DeletePic/',
                type: 'POST',
                dataType: 'json',
                data: 'picID=' + id,
                //contentType: 'application/json',
                success: function (data) {
                    alert("Picture removed successfully.");
                    loadRoomPictures();
                },
                error: function (err) {
                    alert("Picture removed successfully.");
                    loadRoomPictures();
                    //  alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            window.location.href = '/Property/Rooms';
        }
    }

    $('#ddlProperty').change(function () {
        loadRooms();
    });

    $(function () {
        try {
            $.ajax({
                url: '/Property/Properties',
                type: 'GET',
                dataType: 'json',
                contentType: 'application/json',
                success: function (data) {
                    // console.log(data);
                    $('#AvoidReload').val("True");
                    //  alert("f");
                    //var ddlPropertyTypes = $("[id*=PropertyTypes]");
                    $('#ddlProperty').empty().append('<option selected="selected" value="0">Please select</option>');
                    //if (data.length > 0) {

                    $.each(data, function (i, item) {
                        $('#ddlProperty').append($('<option></option>').attr('value', item.Id).text(item.PropertyName));
                    });
                    //document.getElementById('#PropertyTypes').selectedIndex = 1;
                    //}
                },
                error: function (err) {
                    //  alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            window.location.href = '/Property/Index';
        }
    })

    $('#rooms').change(function () {
        $('#PicNum').val('false');
        LoadPicNum();
        loadRoomPictures();
    });

 
    function LoadCategories() {
        try {
            $.ajax({
                url: '/Property/LoadCategories',
                type: 'GET',
                dataType: 'json',
                contentType: 'application/json',
                success: function (data) {

                    $('#RoomCategory').empty().append('<option selected="selected" value="0">Please Select</option>');
                    //if (data.length > 0) {

                    $.each(data, function (i, item) {
                        $('#RoomCategory').append($('<option></option>').attr('value', item.CateId).text(item.Category));
                    });
                    //document.getElementById('#PropertyTypes').selectedIndex = 1;
                    //}
                },
                error: function (err) {
                    //  alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            window.location.href = '/Property/Index';
        }
    }

    $('#doaddnew').click(function () {
        try {
            //alert(PaxFName);
            if ($("#ddlProperty")[0].selectedIndex == 0) {
                alert("Please select a Property");
                return;
            }
            var propertyID = new Object();
            propertyID = $('#ddlProperty').val();
            $.ajax({
                url: '/Property/AddRoom',
                type: 'POST',
                dataType: 'json',
                data: 'propertyID=' + propertyID,
                //contentType: 'application/json',
                success: function () {
                    alert("New Room Add Succesfully");
                    window.location.href = '/Property//RoomImages';
                },
                error: function (err) {
                    //   alert(err.status + " : " + err.statusText);
                    window.location.href = '/Property//RoomImages';
                },
            });
        } catch (e) {
            // alert(e);
            window.location.href = '/Property//RoomImages';

        }
    })



    $("table").on("click", ".Update", function (ev) {
        // ev.preventDefault();
      //  $('#basicInfoModal').modal('show');
        var currentRow = $(this).closest("tr");
        var Picture = currentRow.find("td:eq(1)").text();
        var Description = currentRow.find("td:eq(3) input[type='text']").val();
        var PicNum = currentRow.find("td:eq(0)").text();

        var id = this.id;
        // alert(id);
        UpdateData(id, Picture, Description, PicNum)
    });

    $("table").on("click", ".address", function (ev) {
        // ev.preventDefault();
        $('#AddressModal').modal('show');

        var id = this.id;
        LoadData(id);
    });

    function UpdateData(id, Picture, Description, PicNum) {
        try {
            // alert(fileName);
            var rp = new Object();
            rp.Id = id;
            rp.RoomCode = $('#rooms').val();
            rp.Picture = Picture;
            rp.Description = Description;
            rp.PicNum = PicNum;
          
            // alert($('#Country').val());
            $.ajax({
                url: '/Property/UpdatePictureDesc',
                type: 'POST',
                dataType: 'json',
                data: JSON.stringify(rp),
                contentType: 'application/json',
                success: function () {
                    loadRoomPictures();
                },
                error: function (err) {
                    // alert(err.status + " : " + err.statusText);
                    loadRoomPictures();
                },
            });
        } catch (e) {
            //  alert(e);
            window.location.href = '/Property//RoomImages';

        }
    }

    $('#UpdateBasicInfo').click(function () {
        SaveData();
    })

    $('#UpdateAddress').click(function () {
        SaveData();
    })

    $('#UpdateFeatures').click(function () {
        var features = "";
        $("#dvFeatures input[type='checkbox']:checked").each(function () {
            features = features + this.value + ",";
        });
        // console.log(features);
        $('#hdFeatures').val(features);
        SaveData();
        features = "";
    })

    $('#closeAddress').click(function () {
        $('#AddressModal').modal('hide');
    })

    $('#closeBasic').click(function () {
        $('#basicInfoModal').modal('hide');
    })

    $('#closeFeatures').click(function () {
        $('#FaturesModal').modal('hide');
    })

    $('#closePic').click(function () {
        var id = this.id;

        $('#PicModal').modal('hide');
    })

    $("table").on("click", ".feature", function (ev) {
        var id = this.id;
        //alert(id);
        //window.location.href = '/Property/Features?id=' + id;

        $('#FaturesModal').modal('show');
        LoadData(id);
    })

    $("table").on("click", ".Picture", function (ev) {
        //   var id = this.id;
        //  alert(id);
        //window.location.href = '/Property/Features?id=' + id;
        //  LoadPicNum(id);
        //$('#PicModal').modal('show');
        //LoadData(id);
        // window.location.href = '/Property/Index';
        window.open('/Property/RoomImages', '_blank');
    })

    function LoadData(id) {

        try {
            $.ajax({
                url: '/Property/loadRoomDetails/',
                type: 'GET',
                dataType: 'json',
                data: 'RoomCode=' + id,
                contentType: 'application/json',
                success: function (data) {
                    // console.log(data);
                    //if ($('#AvoidReload').val() != "True") {
                    //  alert("g");

                    // }
                    if (data.length > 0) {
                        //console.log(data);

                        $.each(data, function (i, item) {

                            //alert(item.RoomCategory);

                            $('#RoomCode').val(item.RoomCode);

                            $('#RoomDescription').val(item.RoomDescription);
                            $('#RoomCategory').val(item.RoomCategory);
                            $('#BedType').val(item.BedType);
                            $('#CheckIn').val(item.CheckIn);

                            $('#CheckOut').val(item.CheckOut);
                            $('#NoOfRooms').val(item.NoOfRooms);
                            $('#Capacity').val(item.Capacity);
                            $('#Adult').val(item.Adult);
                            $('#NoOfChild').val(item.NoOfChild);
                            $("#ShortCode").val(item.ShortCode);
                            $('#MinimumPrice').val(item.MinimumPrice);

                            $('#MinimumStay').val(item.MinimumStay);
                            $('#MaximumStay').val(item.MaximumStay);
                            $('#Size').val(item.Size);

                        });

                    }
                },
                error: function (err) {
                    //  alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            window.location.href = '/Property/Index';
        }
    }

    function previewImages() {

        var preview = document.querySelector('#preview');

        if (this.files) {
            [].forEach.call(this.files, readAndPreview);
        }

        function readAndPreview(file) {

            // Make sure `file.name` matches our extensions criteria
            if (!/\.(jpe?g|png|gif)$/i.test(file.name)) {
                return alert(file.name + " is not an image");
            } // else...

            var reader = new FileReader();

            reader.addEventListener("load", function () {
                var image = new Image();
                image.height = 100;
                image.title = file.name;
                image.src = this.result;
                preview.appendChild(image);
            });

            reader.readAsDataURL(file);

        }

    }

    //document.querySelector('#roomPic').addEventListener("change", previewImages);

    function LoadPicNum() {
        try {
            var RoomCode = new Object();
            RoomCode = $('#rooms').val();
            $.ajax({
                url: '/Property/PicNum',
                type: 'GET',
                dataType: 'json',
                data: "RoomCode=" + RoomCode,
                contentType: 'application/json',
                success: function (data) {
                    //console.log(data);
                    $.each(data, function (i, item) {
                       // alert(data.PicNum);
                        $('#PicNum').val(data.PicNum);

                    });
                    // alert($('#PicNum').val());
                },
                error: function (err) {
                    //  alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            window.location.href = '/Property/RoomImages';
        }
    }


    $('#PicUpload').click(function () {
       // LoadPicNum();

        // Checking whether FormData is available in browser  
        if (window.FormData !== undefined) {

            var fileUpload = $("#roomPic").get(0);
            var files = fileUpload.files;
            //console.log(fileUpload.files);
            //alert(files.length);
            var ext = $('#roomPic').val().split('.').pop().toLowerCase();
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
            var fileName = "";      
      
            var idx = 0;
            //alert($('#PicNum').val());
            if ($('#PicNum').val() == "false") {
                idx = 1;
            }
            else {             
                idx = $('#PicNum').val();
                idx = parseInt(idx) + 1;
            }
         //   alert(idx);
            $('#PicNum').val(idx);
            for (var i = 0; i < files.length; i++) {
                fileName = $('#rooms').val() + "-" + idx + "." + ext;
                fileData.append(files[i].name, files[i], fileName);
            }

            // Adding one more key to FormData object  
            fileData.append('username', "Manas");

            //fileData.append("-", + $('#propertyID').val());
            //SaveData(fileName);
            $.ajax({
                url: '/Property/UploadRoomImages',
                type: "POST",
                contentType: false, // Not to set any content header  
                processData: false, // Not to process data  
                data: fileData,
                success: function (result) {
                    alert(result);
                    loadRoomPictures();
                },
                error: function (err) {
                    alert(err.statusText);
                }
            });
        } else {
            alert("FormData is not supported.");
        }
    })


});




