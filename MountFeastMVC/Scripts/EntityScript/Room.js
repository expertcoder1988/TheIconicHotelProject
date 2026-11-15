$(document).ready(function () {
    if ($('#AvoidReload').val() != "True") {
        // alert("g");
        LoadCategories();
        //loadCountries();
    }

    function loadRooms() {
        try {
            var propertyID = new Object();
            propertyID = $('#Properties').val();

            $.ajax({
                url: '/Property/loadRooms',
                type: 'GET',
                dataType: 'json',
                contentType: 'application/json',
                data: 'propertyID=' + propertyID,
                success: function (data) {
                    // console.log(data);
                    $("#rooms").empty();
                    var trHTML = '';

                    trHTML = '<tr><th>Room ID</th><th>Room Category</th><th></th><th></th></tr>';
                    if (data.length > 0) {

                        $.each(data, function (i, item) {
                            var desc = item.RoomCategoryDesc;
                            //console.log(desc);
                            if (desc == null) {
                                desc = "";
                            }
                            trHTML += '<tr><td>' + item.RoomCode + '</td><td>' + desc + '</td>' +
                                "<td><input type='button' ID='" + item.RoomCode + "' class='basicInfo btn btn-primary' value='Basic Info' /></td>" +
                                "<td><input type='button' ID='" + item.RoomCode + "' class='RoomNumbers btn btn-info' value='Room Numbers' /></td>" +
                                "<td><input type='button' ID='" + item.RoomCode + "' class='feature btn btn-warning' value='Features' /></td>" +
                                "<td><input type='button' ID='" + item.RoomCode + "' class='delete btn btn-danger' value='Delete' /></td></tr>";
                        });
                        $('#rooms').append(trHTML);
                    }
                },
                error: function (err) {
                    alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            alert(e.status + " : " + e.statusText);
            window.location.href = '/Property/Rooms';
        }
    }

    function loadRoomNumbers(id) {
        try {
            $.ajax({
                url: '/Property/RoomNumbers',
                type: 'GET',
                dataType: 'json',
                contentType: 'application/json',
                data: 'RoomCode=' + id,
                success: function (data) {
                    // Clear existing room numbers
                    $("#roomNumbers").empty();

                    // Set up table header
                    let trHTML = '<tr><th>Room Number</th><th>Floor</th><th></th></tr>';

                    if (data.length > 0) {
                        $.each(data, function (i, item) {
                            let roomNumber = item.RoomNumber || ""; // Default to empty if null
                            let floor = item.Floor || ""; // Default to empty if null
                            let status = item.Status !== true ? "Activate" : "Deactivate";

                            // Create table rows with conditional readonly attribute
                            trHTML += `<tr>
                            <td>
                                <input type="hidden" name="roomId" id="${item.roomId}" value="${item.roomId}" />
                                <input type="text" id="RoomNumber-${item.roomId}" name="RoomNumber" 
                                    class="form-control room-field" value="${roomNumber}" 
                                    ${roomNumber ? "readonly" : ""} />
                            </td>
                            <td>
                                <input type="number" id="Floor-${item.roomId}" name="floor" 
                                    class="form-control room-field" value="${floor}" 
                                    ${floor ? "readonly" : ""} />
                            </td>
                            <td>
                                <input type="button" id="${item.roomId}" 
                                    class="${status === "Activate" ? "activate btn btn-primary" : "deactivate btn btn-danger"}" 
                                    value="${status}" />
                            </td>
                        </tr>`;
                        });

                        // Append rows to the table
                        $('#roomNumbers').append(trHTML);
                    }

                    // Attach event listener to dynamically check field values
                    $(".room-field").on("input", function () {
                        toggleSaveButton();
                    });

                    // Initial check for enabling/disabling the button
                    toggleSaveButton();
                },
                error: function (err) {
                    alert(err.status + " : " + err.statusText);
                }
            });
        } catch (e) {
            alert(e.status + " : " + e.statusText);
            window.location.href = '/Property/Rooms';
        }
    }

    // Function to toggle the Save button
    function toggleSaveButton() {
        let hasEmptyFields = false;

        $(".room-field").each(function () {
            // Check only editable fields (fields without the readonly attribute)
            if (!$(this).prop("readonly") && $(this).val().trim() === "") {
                hasEmptyFields = true;
                return false; // Exit loop if an empty field is found
            }
        });

        // Enable the button if there are any editable fields or if any field is empty
        if (hasEmptyFields) {
            $("#SaveRoomNum").prop("disabled", false).show();
        } else {
            $("#SaveRoomNum").prop("disabled", false).show(); // Keep the button enabled if all fields are filled
        }
    }

    //Comment By zubair for disable and enable save button when data have, by Zubair

    //function loadRoomNumbers(id) {
    //    try {

    //        $.ajax({
    //            url: '/Property/RoomNumbers',
    //            type: 'GET',
    //            dataType: 'json',
    //            contentType: 'application/json',
    //            data: 'RoomCode=' + id,
    //            success: function (data) {
    //                //console.log(data);
    //                $("#roomNumbers").empty();
    //                var trHTML = '';
    //                trHTML = '<tr><th>Room Number</th><th>Floor</th><th></th></tr>';
    //                if (data.length > 0) {
    //                    var $roomNumber = "";
    //                    $.each(data, function (i, item) {
    //                        $roomNumber = item.RoomNumber;
    //                        if ($roomNumber == null) {
    //                            $roomNumber = "";
    //                        }
    //                        var status = "";
    //                        console.log(item.Status)
    //                        if (item.Status !== true) {
    //                            status = "Activate";
    //                        }
    //                        else {
    //                            status = "Deactivate";
    //                        }

    //                        if ($roomNumber == null) {
    //                            $("SaveRoomNum").enable = true
    //                        }
    //                        else {
    //                            $("SaveRoomNum").enable = false
    //                        }

    //                        $('#RoomCode').val(item.RoomCode);
    //                        trHTML = trHTML + "<tr><td><input type='hidden' name='roomId' id='" + item.roomId + "' value='" + item.roomId + "'/>";
    //                            if (status == "Activate") {
    //                                trHTML = trHTML + "<input type='text' id='RoomNumber-" + item.roomId + "' readonly name='RoomNumber' class='form-control' value='" + $roomNumber + "'</td>";
    //                            }
    //                            else {
    //                                trHTML = trHTML + "<input type='text' id='RoomNumber-" + item.roomId + "' name='RoomNumber' class='form-control' value='" + $roomNumber + "'</ td>";
    //                                }
    //                        if (status == "Activate") {
    //                            trHTML = trHTML + "<td><input type='number' id='Floor-" + item.roomId + "' name='floor' readonly class='form-control' value='" + item.Floor + "'</td>";
    //                                }
    //                        else {
    //                            trHTML = trHTML + "<td><input type='number' id='Floor-" + item.roomId + "' name='floor'  class='form-control' value='" + item.Floor + "'</td>";
    //                        }
    //                        if (status == "Activate") {
    //                            trHTML = trHTML + "<td><input type='button' ID='" + item.roomId + "' class='activate btn btn-primary' value='" + status + "' /></td></tr>";
    //                        }
    //                        else {
    //                            trHTML = trHTML + "<td><input type='button' ID='" + item.roomId + "' class='deactivate btn btn-danger' value='" + status + "' /></td></tr>";
    //                        }
    //                    });
    //                    $('#roomNumbers').append(trHTML);
    //                }
    //            },
    //            error: function (err) {
    //                alert(err.status + " : " + err.statusText);
    //            }

    //        });
    //    } catch (e) {
    //        alert(e.status + " : " + e.statusText);
    //        window.location.href = '/Property/Rooms';
    //    }
    //}

    $('#Properties').change(function () {
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
                    $('#Properties').empty().append('<option selected="selected" value="0">Please select</option>');
                    //if (data.length > 0) {

                    $.each(data, function (i, item) {
                        $('#Properties').append($('<option></option>').attr('value', item.Id).text(item.PropertyName));
                    });
                    document.getElementById('#Properties').selectedIndex = 1;
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

    function LoadPicNum(RoomCode) {
        try {
            $.ajax({
                url: '/Property/PicNum',
                type: 'GET',
                dataType: 'json',
                data: "RoomCode=" + RoomCode,
                contentType: 'application/json',
                success: function (data) {
                    //console.log(data);
                    $.each(data, function (i, item) {
                        $('#PicNum').val(item.PicNum);

                    });
                    //alert($('#PicNum').val());
                },
                error: function (err) {
                    //  alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            window.location.href = '/Property/Index';
        }
    }

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
                    $("select#Properties").prop('selectedIndex', 1);
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
            if ($("#Properties")[0].selectedIndex == 0) {
                alert("Please select a Property");
                return;
            }
            var propertyID = new Object();
            propertyID = $('#Properties').val();
            //alert(propertyID);

            var pPY = new Object();
            pPY.propertyID = $('#Properties').val();

            $.ajax({
                url: '/Property/AddRoom',
                type: 'POST',
                data: 'propertyID=' + propertyID,
                // contentType: 'application/json',
                success: function () {
                    loadRooms();
                    alert("New Room Add Succesfully");
                    // window.location.href = '/Property/Rooms';

                },
                error: function (err) {
                    alert(err.status + " : " + err.statusText);
                    // window.location.href = '/Property/Rooms';
                },
            });
        } catch (e) {
            alert("t");
            window.location.href = '/Property/Rooms';

        }
    })



    $("table").on("click", ".basicInfo", function (ev) {
        // ev.preventDefault();
        $('#basicInfoModal').modal('show');


        var id = this.id;
        // alert(id);
        LoadData(id);
    });

    $("table").on("click", ".address", function (ev) {
        // ev.preventDefault();
        $('#AddressModal').modal('show');

        var id = this.id;
        LoadData(id);
    });

    function SaveRoomNumber() {
        try {
            $("#roomNumbers tr").each(function (i, row) {
                //alert("f");
                var $row = $(row),
                    $Id = $row.find('input[name*="roomId"]'),
                    $RoomNumber = $row.find('input[name*="RoomNumber"]'),
                    $Floor = $row.find('input[name*="floor"]');;

                if (typeof $Id.val() === "undefined") { }
                else {
                    if ($RoomNumber.val() != "") {
                        var pRO = new Object();
                        pRO.Id = $Id.val();
                        pRO.RoomNumber1 = $RoomNumber.val();
                        pRO.Floor = $Floor.val();
                        $.ajax({
                            url: '/Property/UpdateRoomNumbers',
                            type: 'POST',
                            dataType: 'json',
                            data: JSON.stringify(pRO),
                            contentType: 'application/json',
                            success: function () {
                                alert("Property Succesfully Updated");
                                // window.location.href = '/Property/Rooms';
                                loadRooms();
                                $('#RoomNumModal').modal('hide');
                            },
                            error: function (err) {
                                // alert(err.status + " : " + err.statusText);
                                loadRooms(); //window.location.href = '/Property/Rooms';
                                $('#RoomNumModal').modal('hide');
                            },
                        });
                    }
                }

                // alert($('#Country').val());

            });
        } catch (e) {
            //  alert(e);
            window.location.href = '/Property/Rooms';

        }
    }

    function CheckCategory() {

        var Count = 0;
        try {
            // alert($('#hdFeatures').val());
            var pRO = new Object();
            pRO.PropertyID = $('#Properties').val();
            pRO.RoomCate = $('#RoomCategory').val();

            //alert(pRO.Features);

            $.ajax({
                url: '/Property/CheckCategories',
                type: 'POST',
                dataType: 'json',
                data: JSON.stringify(pRO),
                contentType: 'application/json',
                success: function (data) {
                    //   console.log(data);
                    if (data > 0) {
                        // alert(data);
                        alert("Room already exist with selected category");
                    }
                    else {
                        SaveData();
                    }

                },
                error: function (err) {
                },
            });
        } catch (e) {
            //  alert(e);
            window.location.href = '/Property/Rooms';

        }
        // console(Count);

    }

    function SaveData(fileName) {
        try {
            // alert($('#hdFeatures').val());
            var pRO = new Object();
            pRO.RoomCode = $('#RoomCode').val();
            pRO.RoomDescription = $('#RoomDescription').val();
            pRO.RoomCategory = $('#RoomCategory').val();
            pRO.BedType = $('#BedType').val();
            pRO.CheckIn = $('#CheckIn').val();
            pRO.CheckOut = $('#CheckOut').val();
            pRO.NoOfRooms = $('#NoOfRooms').val();
            pRO.Adult = $('#Adult').val();
            pRO.NoOfChild = $('#NoOfChild').val();
            pRO.ChildAge = $('#ChildAge').val();
            pRO.Capacity = $('#Capacity').val();
            pRO.ShortCode = $('#ShortCode').val();
            pRO.MinimumPrice = $('#MinimumPrice').val();
            pRO.MinimumStay = $('#MinimumStay').val();
            pRO.MaximumStay = $('#MaximumStay').val();
            pRO.Size = $('#Size').val();
            pRO.PropertyID = $('#Properties').val();
            pRO.Features = $('#hdFeatures').val();

            //alert(pRO.Features);

            $.ajax({
                url: '/Property/AddRoomBasicInfo',
                type: 'POST',
                dataType: 'json',
                data: JSON.stringify(pRO),
                contentType: 'application/json',
                success: function () {
                    // alert("Property Succesfully Updated");
                    // window.location.href = '/Property/Rooms';
                    loadRooms();
                },
                error: function (err) {
                    //  alert("2");
                    loadRooms(); //window.location.href = '/Property/Rooms';
                },
            });
        } catch (e) {
            //   alert(e);
            window.location.href = '/Property/Rooms';

        }

    }

    $('#UpdateBasicInfo').click(function () {
        //CheckCategory();
        SaveData();
        $('#basicInfoModal').modal('hide');
    })

    $('#SaveRoomNum').click(function () {
        SaveRoomNumber();
        $('#RoomNumModal').modal('hide');
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
        $('#FaturesModal').modal('hide');
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

    $('#closeRoomNum').click(function () {
        //  var id = this.id;

        $('#RoomNumModal').modal('hide');
    })

    $("table").on("click", ".feature", function (ev) {
        var id = this.id;
        //alert(id);
        $("#dvFeatures").find('input[type=checkbox]').each(function () {
            // some staff
            this.checked = false;
        });
        $("#hdFeatures").val();
        $('#FaturesModal').modal('show');
        LoadData(id);
    })

    $("table").on("click", ".RoomNumbers", function (ev) {
        var id = this.id;
        // alert(id);
        loadRoomNumbers(id);
        $('#RoomNumModal').modal('show');
    })

    $("table").on("click", ".delete", function (ev) {
        // ev.preventDefault();
        var id = this.id;
        if (confirm("Are you sure to delete this room?")) {
            DeleteRoom(id);
        }
        else {
            return false;
        }

    });

    function DeleteRoom(id) {

        try {
            $.ajax({
                url: '/Property/DeleteRoom/',
                type: 'POST',
                dataType: 'json',
                data: 'roomID=' + id,
                //contentType: 'application/json',
                success: function (data) {
                    alert("Room removed successfully.");
                    loadRooms();
                },
                error: function (err) {
                    alert("Room removed successfully.");
                    loadRooms();
                    //  alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            window.location.href = '/Property/Rooms';
        }
    }

    $("table").on("click", ".deactivate", function (ev) {
        // ev.preventDefault();
        var id = this.id;
        var status = "deactivate";
        if (confirm("Are you sure to deactivate this room?")) {
            DeleteRoomNumber(id, status);
        }
        else {
            return false;
        }

    });

    $("table").on("click", ".activate", function (ev) {
        // ev.preventDefault();
        var id = this.id;
        var status = "activate";
        if (confirm("Are you sure to activate this room?")) {
            DeleteRoomNumber(id, status);
        }
        else {
            return false;
        }

    });

    function DeleteRoomNumber(id, status) {

        try {
            //alert($('#RoomCode').val());
            $.ajax({
                url: '/Property/DeleteRoomNumber/',
                type: 'POST',
                dataType: 'json',
                data: 'roomID=' + id,
                //contentType: 'application/json',
                success: function (data) {
                    alert("Room " + status + " successfully.");
                    loadRoomNumbers($('#RoomCode1').val());
                },
                error: function (err) {
                    alert("Room " + status + " successfully.");
                    loadRoomNumbers($('#RoomCode1').val());
                    //  alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            window.location.href = '/Property/Rooms';
        }
    }

    function LoadData(id) {

        try {
            $("#hdFeatures").val();
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

                            //   alert(item.RoomCode);

                            $('#RoomCode').val(item.RoomCode);

                            $('#RoomDescription').val(item.RoomDescription);
                            $('#RoomCategory').val(item.RoomCategory);
                            $('#BedType').val(item.BedType);
                            $('#CheckIn').val(item.CheckIn);
                            $('#ChildAge').val(item.ChildAge);
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
                            $("#hdFeatures").val(item.Features);
                            var str = "";
                            // console.log(item.Features);
                            if (item.Features && typeof item.Features !== 'undefined') {
                                str = (item.Features).slice(0, -1);
                                var values = str.toString().split(',');
                                $("#dvFeatures").find('[value=' + values.join('], [value=') + ']').prop("checked", true);
                            }
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

    $('#PicUpload').click(function () {

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

            // Looping over all files and add it to FormData object  


            //var totalfiles = document.getElementById('roomPic').files.length;
            //alert(totalfiles);
            //for (var index = 0; index < totalfiles; index++) {
            //    fileName = $('#RoomCode').val() + "-" + index + "." + ext;
            //    fileData.append("roomPic[]", document.getElementById('roomPic').files[index]);
            //}
            var idx = $('#PicNum').val();
            //  alert(idx);
            if (idx == "false") {
                idx = 1;
            }

            for (var i = 0; i < files.length; i++) {
                fileName = $('#RoomCode').val() + "-" + idx + "." + ext;
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




