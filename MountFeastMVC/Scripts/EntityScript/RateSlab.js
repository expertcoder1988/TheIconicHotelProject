$(document).ready(function () {
    if ($('#AvoidReload').val() != "True") {
        // alert("g");
      //  loadRooms();
    }

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
                  //  $("select#Properties").prop('selectedIndex', 1);
                    //}
                },
                error: function (err) {
                    //  alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            window.location.href = '/Property/RateSlab';
        }
    })
   

    $('#rooms').change(function () {
      //  LoadMinPrice($('#rooms').val());
        loadRoomRates();
    });

    $('#Properties').change(function () {
        loadRooms();        
    });

    function loadRooms() {
        try {
            var propertyID = new Object();
            propertyID = $('#Properties').val();

            $.ajax({
                url: '/Property/LoadpropertyCategories',
                type: 'GET',
                dataType: 'json',
                contentType: 'application/json',
                data: 'propertyID=' + propertyID,
                success: function (data) {
                    //console.log(data);
                    $('#rooms').empty().append('<option selected="selected" value="0">Please Select Room</option>');
                    //if (data.length > 0) {

                    $.each(data, function (i, item) {
                        $('#rooms').append($('<option></option>').attr('value', item.CateId).text(item.Category));
                    });
                },
                error: function (err) {
                    alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            alert(e.status + " : " + e.statusText);
            window.location.href = '/Property/RateSlab';
        }
    }

    function loadRoomRates() {
        try {
            var roomID = new Object();
            roomID = $('#rooms').val();
           
            $.ajax({
                url: '/Property/loadRoomsRateSlab',
                type: 'GET',
                dataType: 'json',
                contentType: 'application/json',
                data: 'roomID=' + roomID,
                success: function (data) {
                   // console.log(data);
                    $("#RatePlan").empty();
                    var trHTML = '';
                    trHTML = '<tr><th style=display:none>Room Caterogy ID</th><th>Room Category</th><th>Room Rate</th><th>Discount</th><th>Adult Price</th><th>' +
                            'Child Price</th > <th>Days</th><th>Slab Start Date</th><th>Slab End Date</th><th></th><th></th></tr > ';

                    if (data.length > 0) {
                        
                        $.each(data, function (i, item) {
                            var sDate = moment(item.StartDate); //new Date(parseInt(item.StartDate.substr(6)));
                            var eDate = moment(item.EndDate);
                            var desc = item.Days;
                            //console.log(desc);
                            if (desc == null) {
                                desc = "";
                            }
                          //  console.log(myDate.fomat('DD-MM-YYYY'));
                            trHTML += '<tr><td style=display:none>' + item.RateSlabId + '</td><td>' + item.RoomDescription + '</td>' +
                                "<td>" + item.RoomRate + "</td>" + "<td>" + item.Discount + "</td>" +
                                "<td>" + item.ExtraAdultPrice + "</td><td>" + item.ExtraChildPrice + "</td><td>" + desc + "</td>" +                               
                                "<td>" + sDate.format('DD-MM-YYYY') + "</td>" +
                                "<td>" + eDate.format('DD-MM-YYYY') + "</td>" +
                                "<td><input type='button' ID='" + item.RateSlabId + "' class='Edit btn btn-primary' value='Edit' /></td >" +
                                "<td><input type='button' ID='" + item.RateSlabId + "' class='Delete btn btn-danger' value='Delete' /></td ></tr > ";
                        });
                        $('#RatePlan').append(trHTML);
                    }
                },
                error: function (err) {
                    alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            alert(e.status + " : " + e.statusText);
            window.location.href = '/Property/RateSlab';
        }
    }

    $("table").on("click", ".Edit", function (ev) {
        var id = this.id;
        //alert(id);
        //window.location.href = '/Property/Features?id=' + id;
        
        $('#RateSlabId').val(id);
        LoadRateSlab(id);
    })

    $("table").on("click", ".Delete", function (ev) {
        // ev.preventDefault();
        var id = this.id;
        if (confirm("Are you sure to delete this rate?")) {
            DeleteRateSlab(id);
        }
        else {
            return false;
        }
    });


    function DeleteRateSlab(id) {

        try {
            $.ajax({
                url: '/Property/DeleteRateSlab/',
                type: 'POST',
                dataType: 'json',
                data: 'rateID=' + id,
                //contentType: 'application/json',
                success: function (data) {
                    alert("Rate Slab removed successfully.");
                    loadRoomRates();
                },
                error: function (err) {
                    alert("Rate Slab removed successfully.");
                    loadRoomRates();
                    //  alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            window.location.href = '/Property/Rooms';
        }
    }

    $('#btnSave').click(function () {

        SaveData();
        loadRoomRates();
    })


    function SaveData() {
        try {          

            if ($('#nightRate').val() == "") {
                alert("Please provide per night rate");
                return;
            }

            //if ($('#NormalPrice').val() == "") {
            //    alert("Please provide adult price");
            //    return;
            //}

            if ($('#Discount').val() == "") {
                alert("Please provide discount");
                return;
            }

            if ($('#StartDate').val() == "") {
                alert("Please select start date");
                return;
            }

            if ($('#EndDate').val() == "") {
                alert("Please select end date");
                return;
            }

            if ($('#StartDate').val() == $('#EndDate').val()) {
                alert("Please select valid date range");
                return;
            }

            if ($("#rooms")[0].selectedIndex == 0) {
                alert("Please select a room category");
                return;
            }

            var Days = "";
            $("#dvDays input[type='checkbox']:checked").each(function () {
                Days = Days + this.value + ",";
            });

            if (Days == "") {
                alert("Please select atleast a single day.");
                return;
            }

            var propertyID = new Object();
            propertyID = $('#Properties').val();
            var sdate = moment.utc($('#StartDate').val());
            var edate = moment.utc($('#EndDate').val());
            var po = new Object();
            po.Id = $('#RateSlabId').val();
            po.RoomCategoryCode = $('#rooms').val();
            po.RoomRate = $('#nightRate').val();
            po.propertyID = propertyID;
            po.ExtraAdultPrice = $('#NormalPrice').val();
            po.ExtraChildPrice = $('#ChildPrice').val();
            po.Discount = $('#Discount').val();
            po.StartDate = sdate.format('YYYY-MM-DD');
            po.EndDate = edate.format('YYYY-MM-DD');
            po.Days = Days;

            //  alert(propertyID);
            
            $.ajax({
                url: '/Property/AddRateSlabs',
                type: 'POST',
                dataType: 'json',
                data: JSON.stringify(po),
                contentType: 'application/json',
                success: function () {
                   alert("New Rate Plan Succesfully Added.");
                    // window.location.href = '/Property/Rooms';
                    loadRoomRates();
                    ClearAll();
                },
                error: function (err) {
                   // alert(err.status + " : " + err.statusText);
                    //loadRooms(); //window.location.href = '/Property/Rooms';
                    loadRoomRates();
                },
            });
        } catch (e) {
            //alert(e);
            window.location.href = '/Property/RateSlab';

        }
    }

    function LoadRateSlab(id) {

        try {
            $("#dvDays").find('input[type=checkbox]').each(function () {
                // some staff
                this.checked = false;
            });
            $.ajax({
                url: '/Property/loadRateSlab/',
                type: 'GET',
                dataType: 'json',
                data: 'rateSlabId=' + id,
                contentType: 'application/json',
                success: function (data) {
                   // console.log(data);
                    if (data.length > 0) {
                        $.each(data, function (i, item) {
                            var sDate = moment(item.StartDate); //new Date(parseInt(item.StartDate.substr(6)));
                            var eDate = moment(item.EndDate);

                            // alert(item.PropertyTypeId);
                            $('#propertyID').val(item.PropertyID);
                            $('#rooms').val(item.RoomCategoryCode);
                            $('#nightRate').val(item.RoomRate);
                            $('#NormalPrice').val(item.ExtraAdultPrice);
                            $('#ChildPrice').val(item.ExtraChildPrice);
                            $('#Discount').val(item.Discount);
                            $('#StartDate').val(sDate.format('YYYY-MM-DD'));
                            $('#EndDate').val(eDate.format('YYYY-MM-DD'));
                            $("#hdDays").val(item.Days);
                            var str = "";
                            // console.log(item.Features);
                            if (item.Days && typeof item.Days !== 'undefined') {
                                str = (item.Days).slice(0, -1);
                                var values = str.toString().split(',');
                                $("#dvDays").find('[value=' + values.join('], [value=') + ']').prop("checked", true);
                            }
                        });
                    }
                },
                error: function (err) {
                    //  alert(err.status + " : " + err.statusText);
                }
            });
        } catch (e) {
            window.location.href = '/Property/RateSlab';
        }
    }

    function LoadMinPrice(id) {

        try {
            $.ajax({
                url: '/Property/LoadMinPrice/',
                type: 'GET',
                dataType: 'json',
                data: 'roomId=' + id,
                contentType: 'application/json',
                success: function (data) {
                   // console.log(data);
                    if (data.length > 0) {
                        $.each(data, function (i, item) {
                            $('#minPrice').val(item.MinimumPrice);
                        });
                    }
                },
                error: function (err) {
                    //  alert(err.status + " : " + err.statusText);
                }
            });
        } catch (e) {
            window.location.href = '/Property/RateSlab';
        }
    }


    function ClearAll() {
        $('#NormalPrice').val() = "";
        $('#Discount').val() = "";
        $('#StartDate').val() = "";
        $('#EndDate').val() = "";
        $('#RateSlabId').val() = -1;
    }
});