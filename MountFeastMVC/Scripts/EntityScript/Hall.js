$(document).ready(function () {
    //if ($('#AvoidReload').val() != "True") {
    // alert("g");
    //}
    $("#prcent").prop("checked", true);
    $('#ddlProperty').change(function () {
        //loadRooms();
        LoadHalls();
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

    function LoadHalls() {
        try {
            var propertyID = new Object();
            propertyID = $('#ddlProperty').val();

            $.ajax({
                url: '/Property/LoadHalls',
                type: 'GET',
                dataType: 'json',
                data: 'propertyID=' + propertyID,
                contentType: 'application/json',
                success: function (data) {
                    // console.log(data);
                    if (data.length > 0) {
                        var trHTML = '';
                        var detected = "%";
                        $('#Halls').empty()
                        trHTML += '<thead><tr><th>Id</th><th>Hall Name</th><th>Short Code</th><th>Covered Area</th><th>Capacity</th><th>Image</th><th></th><th></th></tr></thead>';
                        $.each(data, function (i, item) {
                            if (item.DetectedAs == true) {
                                detected = "%";
                            }
                            else {
                                detected = "Amount";
                            }


                            trHTML += '<tr><td>' + item.Id + '</td><td class=HallName_' + item.Id + '>' + item.HallName + '</td>' +
                                '<td class=ShortCode_' + item.Id + ' > ' + item.ShortCode + '</td > ' +
                                '<td class=CoveredArea_' + item.Id + '>' + item.CoveredArea + '</td><td class=Capacity_' + item.Id + '>' + item.Capacity + '</td>' +
                                '<td class=image_' + item.Id + '>' + item.Image + '</td>' +
                                "<td><input type='button' ID='" + item.Id + "' class='basicInfo btn btn-primary' value='Edit' /></td >" +
                                "<td><input type='button' ID='" + item.Id + "' class='deactivate btn btn-danger' value='Deactivate' /></td></tr>";
                        });
                        $('#Halls').append(trHTML);
                    }
                },
                error: function (err) {
                    alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            window.location.href = '/Property/Index';
        }
    }

    function PicUpload() {

        // Checking whether FormData is available in browser  
       
    }

    function loadRooms() {
        try {
            var propertyID = new Object();
            propertyID = $('#ddlProperty').val();

            $.ajax({
                url: '/Property/loadRooms',
                type: 'GET',
                dataType: 'json',
                contentType: 'application/json',
                data: 'propertyID=' + propertyID,
                success: function (data) {
                    $('#rooms').empty().append('<option selected="selected" value="0">Please select</option>');
                    if (data.length > 0) {

                        $.each(data, function (i, item) {
                            $('#rooms').append($('<option></option>').attr('value', item.RoomCode).text(item.RoomDescription));
                        });

                    }
                },
                error: function (err) {
                    alert(err.status + " : " + err.statusText);

                }

            });
        } catch (e) {
            alert(e.status + " : " + e.statusText);
            window.location.href = '/Property/Taxes';
        }
    }

    function SaveData() {
        try {
            alert(value);
            //alert($('#ShortCode').val());

            var value = document.getElementById('HallName').value;
            if (value === '') {
                alert("Please Provide Hall Name");
                return;
            }

            if (window.FormData !== undefined) {

                var fileUpload = $("#propertyPic").get(0);
                var files = fileUpload.files;

                var ext = $('#propertyPic').val().split('.').pop().toLowerCase();
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
                var fileName = $('#HallName').val() + "." + ext;

                // Looping over all files and add it to FormData object  
                for (var i = 0; i < files.length; i++) {
                    fileData.append(files[i].name, files[i], fileName);
                }

                // Adding one more key to FormData object  
                // fileData.append('username', "Manas");

                //fileData.append("-", + $('#propertyID').val());
                $('#filename').val(fileName);
                $.ajax({
                    url: '/Property/UploadFiles',
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
            
            var pRO = new Object();
            pRO.id = $('#HallId').val();
            pRO.PropertyID = $('#ddlProperty').val();
            pRO.HallName = value;
            pRO.ShortCode = $('#ShortCode').val();
            pRO.CoveredArea = $('#CoveredArea').val();
            pRO.Capacity = $('#Capacity').val();
            pRO.Image = $('#filename').val();
            pRO.status = true;


            // alert($('#Country').val());
            $.ajax({
                url: '/Property/AddNewHall',
                type: 'POST',
                dataType: 'json',
                data: JSON.stringify(pRO),
                contentType: 'application/json',
                success: function () {
                    alert("Property Succesfully Updated");
                    //window.location.href = '/Property/Taxes';
                    $('#HallId').val(0);
                    LoadHalls();
                    $('#HallName').val();
                    $('#Capacity').val();
                    $('#CoveredArea').val();
                    $('#filename').val();
                },
                error: function (err) {
                    // alert(err.status + " : " + err.statusText);
                    //window.location.href = '/Property/Taxes';
                    $('#TaxId').val(0);
                    LoadHalls();

                },
            });
        } catch (e) {
            alert(e);
            window.location.href = '/Property/Halls';

        }
    }

    $('#Save').click(function () {
        SaveData();
    })

    $("table").on("click", ".basicInfo", function (ev) {
        // ev.preventDefault();
        $('#basicInfoModal').modal('show');

        var id = this.id;
        //alert($(".DetectedAs_" + id).html());
        $('#HallId').val(id);
        $('#HallName').val($("." + id).html());
        $('#ShortCode').val($(".ShortCode" + id).html());
        $('#CoveredArea').val($(".CoveredArea_" + id).html());
        $('#Capacity').val($(".Capacity_" + id).html());

        
    });

    $("table").on("click", ".address", function (ev) {
        // ev.preventDefault();
        $('#AddressModal').modal('show');

        var id = this.RoomCode;
        // LoadData(id);
    });
});