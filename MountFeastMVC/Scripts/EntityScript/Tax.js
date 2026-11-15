$(document).ready(function () {
    //if ($('#AvoidReload').val() != "True") {
        // alert("g");
    //}
    $("#prcent").prop("checked", true);
    $('#ddlProperty').change(function () {
        loadRooms();
    });

    $('#rooms').change(function () {
       LoadTaxes();
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

    function LoadTaxes() {
        try {
            var propertyID = new Object();
            propertyID = $('#ddlProperty').val();

            var roomCate = new Object();
            roomCate = $('#rooms').val();

            $.ajax({
                url: '/Property/LoadTaxes',
                type: 'GET',
                dataType: 'json',
                data: 'propertyID=' + propertyID + "&roomCate=" + roomCate,
                contentType: 'application/json',
                success: function (data) {
                   // console.log(data);
                    if (data.length > 0) {
                        var trHTML = '';
                        var detected ="%";
                        $('#Taxes').empty()
                        trHTML += '<thead><tr><th>Id</th><th>Room</th><th>Tax Name</th><th>Detected As</th><th>Amount</th><th></th><th></th></tr></thead>';
                        $.each(data, function (i, item) {
                            if (item.DetectedAs == true) {
                                detected = "%";
                            }
                            else {
                                detected = "Amount";
                            }
            
                            
                            trHTML += '<tr><td>' + item.Id + '</td><td class=Room_' + item.Id + '>' + item.RoomName + '</td>' +
                                '<td class=TaxName_' + item.Id + ' > ' + item.TaxName + '</td > ' +
                                '<td class=DetectedAs_' + item.Id + '>' + detected + '</td><td class=Amount_' + item.Id + '>' + item.Amount + '</td>' +
                                '<td class="'+ item.Id + '" style="display:none">' + item.RoomCategory + '</td>' +
                                "<td><input type='button' ID='" + item.Id + "' class='basicInfo btn btn-primary' value='Edit' /></td >" +
                                "<td><input type='button' ID='" + item.Id + "' class='delete btn btn-danger' value='Delete' /></td></tr>";
                        });
                        $('#Taxes').append(trHTML);
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
            window.location.href = '/Property/Taxes';
        }
    }

    function SaveData() {
        try {
            // alert(fileName);
            var detected;
            if ($("#AmtType").html() == "%") {
                detected = true;
            }
            else {
                detected = false;
            }
            var pRO = new Object();
            pRO.id = $('#TaxId').val();
            pRO.PropertyID = $('#ddlProperty').val();
            pRO.RoomCategory = $('#rooms').val();
            pRO.TaxName = $('#TaxName').val();
            pRO.DetectedAs = detected;
            pRO.Amount = $('#Amount').val();
            pRO.status = true;

           
            // alert($('#Country').val());
            $.ajax({
                url: '/Property/AddNewTax',
                type: 'POST',
                dataType: 'json',
                data: JSON.stringify(pRO),
                contentType: 'application/json',
                success: function () {
                    alert("Property Succesfully Updated");
                    //window.location.href = '/Property/Taxes';
                    $('#TaxId').val(0);
                    LoadTaxes();
                    $('#TaxName').val();
                    $('#Amount').val();
                },
                error: function (err) {
                    // alert(err.status + " : " + err.statusText);
                    //window.location.href = '/Property/Taxes';
                    $('#TaxId').val(0);
                    LoadTaxes();
                    
                },
            });
        } catch (e) {
            //  alert(e);
            window.location.href = '/Property//RoomImages';

        }
    }

    $("table").on("click", ".delete", function (ev) {
        // ev.preventDefault();
        var id = this.id;
        if (confirm("Are you sure to delete this tax?")) {
            DeleteTax(id);
        }
        else {
            return false;
        }
    });

    function DeleteTax(id) {

        try {
            $.ajax({
                url: '/Property/DeleteTax/',
                type: 'POST',
                dataType: 'json',
                data: 'TaxId=' + id,
                //contentType: 'application/json',
                success: function (data) {
                    alert("Tax removed successfully.");
                    LoadTaxes();
                },
                error: function (err) {
                    alert("Tax removed successfully.");
                    LoadTaxes();
                    //  alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            window.location.href = '/Property/Rooms';
        }
    }

    $('#Save').click(function () {
        SaveData();
    })

    $('input:radio').click(
        function () {
            $("#AmtType").html(event.target.value)
        }
    );

    $("table").on("click", ".basicInfo", function (ev) {
        // ev.preventDefault();
        $('#basicInfoModal').modal('show');

        var id = this.id;
        //alert($(".DetectedAs_" + id).html());
        $('#TaxId').val(id);
        $('#rooms').val($("." + id).html());
        $('#TaxName').val($(".TaxName_" + id).html());
        $('#Amount').val($(".Amount_" + id).html());

        if ($(".DetectedAs_" + id).html() == "Amount") {
            $("#prcent").prop("checked", false);
            $("#amt").prop("checked", true);
        }
        else {
            $("#prcent").prop("checked", true);
            $("#amt").prop("checked", false);
        }
    });

    $("table").on("click", ".address", function (ev) {
        // ev.preventDefault();
        $('#AddressModal').modal('show');

        var id = this.RoomCategory;
       // LoadData(id);
    });
});