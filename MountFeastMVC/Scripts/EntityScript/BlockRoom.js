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
        //  loadRoomNums();


        loadNumberOfRooms();
        LoadBlockedRooms();
        loadRoomsByCategory();
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

    function loadNumberOfRooms() {
        try {
            var roomID = new Object();
            roomID = $('#rooms').val();

            $.ajax({
                url: '/Property/loadNumberOfRooms',
                type: 'GET',
                dataType: 'json',
                contentType: 'application/json',
                data: 'CateId=' + roomID,
                success: function (data) {
                    // console.log(data);
                    $('#TotalRooms').val(data);
                    // $('#RoomNum').val(data);
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

    function loadRoomsByCategory() {
        try {
            const roomCategoryID = $('#rooms').val();

            //if (!roomCategoryID) {
            //    // Handle the case where no category is selected
            //    $('#RoomNum').empty().append('<option value="">Select Room</option>');
            //    return;
            //}

            $.ajax({
                url: '/Property/LoadRoomsByCategory',
                type: 'GET',
                dataType: 'json',
                data: { CateId: roomCategoryID },
                success: function (data) {
                    // Clear existing options
                    $('#RoomNum').empty();

                    if (data.length > 0) {
                        // Populate dropdown with room descriptions
                        data.forEach(function (room) {
                            $('#RoomNum').append($('<option>', {
                                value: room.RoomNumber1,
                                text: room.RoomNumber1
                            }));
                        });

                        // Auto-select the first room number if not already selected
                        const selectedValue = $('#RoomNum').val();
                        if (!selectedValue) {
                            $('#RoomNum').val(data[0].RoomNumber1);
                        }
                    } else {
                        // Handle the case where no rooms are returned
                        $('#RoomNum').append($('<option>', {
                            value: '',
                            text: 'No rooms available'
                        }));
                    }
                },
                error: function (err) {
                    alert(err.status + " : " + err.statusText);
                }
            });
        } catch (e) {
            alert("An error occurred: " + e.message);
        }
    }

    // Attach the event listener
    $('#rooms').change(function () {
        loadRoomsByCategory();
    });


    function loadRoomNums() {
        try {
            var roomID = new Object();
            roomID = $('#rooms').val();

            $.ajax({
                url: '/Property/loadRoomNums',
                type: 'GET',
                dataType: 'json',
                contentType: 'application/json',
                data: 'CateId=' + roomID,
                success: function (data) {
                    // console.log(data);
                    $('#RoomNum').empty().append('<option selected="selected" value="0">Please Select Room</option>');
                    //if (data.length > 0) {

                    $.each(data, function (i, item) {
                        $('#RoomNum').append($('<option></option>').attr('value', item.RoomNumber).text(item.RoomNumber));
                    });
                },
                error: function (err) {
                    alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            alert(e.status + " : " + e.statusText);
            window.location.href = '/Property/BlockRoom';
        }
    }

    $("table").on("click", ".Edit", function (ev) {
        var id = this.id;
        //alert(id);
        //window.location.href = '/Property/Features?id=' + id;

        //$('#RateSlabId').val(id);
        UnBlockRooms(id);
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


    function UnBlockRooms(id) {

        try {
            $.ajax({
                url: '/Property/UnBlockRooms/',
                type: 'POST',
                dataType: 'json',
                data: 'Id=' + id,
                //contentType: 'application/json',
                success: function (data) {
                    alert("Selected Rooms Unblocked successfully.");
                    LoadBlockedRooms();
                },
                error: function (err) {
                    alert("Selected Rooms Unblocked successfully.");
                    LoadBlockedRooms();
                    //  alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            window.location.href = '/Property/Rooms';
        }
    }

    $('#btnSave').click(function () {
        SaveData();
    })

    function SaveData() {
        try {
            if ($("#Properties")[0].selectedIndex === 0) {
                alert("Please select a Property");
                return;
            }

            if ($("#rooms")[0].selectedIndex === 0) {
                alert("Please select a room category");
                return;
            }

            if ($('#StartDate').val() === "") {
                alert("Please select a start date");
                return;
            }

            if ($('#EndDate').val() === "") {
                alert("Please select an end date");
                return;
            }

            var propertyID = $('#Properties').val();
            var sdate = moment.utc($('#StartDate').val());
            var edate = moment.utc($('#EndDate').val());
            var br = {
                Id: 0,
                CateId: $('#rooms').val(),
                RoomNum: $('#RoomNum option:selected').text(),
                PropertyId: propertyID,
                StartDate: sdate.format('YYYY-MM-DD'),
                EndDate: edate.format('YYYY-MM-DD')
            };

            $.ajax({
                url: '/Property/BlockRooms',
                type: 'POST',
                dataType: 'json',
                data: JSON.stringify(br),
                contentType: 'application/json',
                success: function (response) {
                    if (response.success) {
                        alert(response.message); // Display success message
                        LoadBlockedRooms();
                        ClearAll();
                    } else {
                        alert(response.message); // Display validation or error message
                    }
                },
                error: function () {
                    alert("An unexpected error occurred while blocking the room.");
                    LoadBlockedRooms();
                }
            });
        } catch (e) {
            console.error("Unexpected error:", e);
            window.location.href = '/Property/RateSlab';
        }
    }


    //function SaveData() {
    //    try {
    //        if ($("#Properties")[0].selectedIndex == 0) {
    //            alert("Please select a Property");
    //            return;
    //        }

    //        if ($("#rooms")[0].selectedIndex == 0) {
    //            alert("Please select a room category");
    //            return;
    //        }

    //        //if ($("#RoomNum")[0].selectedIndex == 0) {
    //        //    alert("Please select a room");
    //        //    return;
    //        //}

    //        if ($('#StartDate').val() == "") {
    //            alert("Please select start date");
    //            return;
    //        }

    //        if ($('#EndDate').val() == "") {
    //            alert("Please select end date");
    //            return;
    //        }

    //        var propertyID = $('#Properties').val();
    //        var sdate = moment.utc($('#StartDate').val());
    //        var edate = moment.utc($('#EndDate').val());
    //        var br = {
    //            Id: 0,
    //            CateId: $('#rooms').val(),
    //            RoomNum: $('#RoomNum option:selected').text(), // Get RoomDescription
    //            PropertyId: propertyID,
    //            StartDate: sdate.format('YYYY-MM-DD'),
    //            EndDate: edate.format('YYYY-MM-DD')
    //        };

    //        $.ajax({
    //            url: '/Property/BlockRooms',
    //            type: 'POST',
    //            dataType: 'json',
    //            data: JSON.stringify(br),
    //            contentType: 'application/json',
    //            success: function () {
    //                alert("Room successfully blocked.");
    //                loadRoomRates();
    //                ClearAll();
    //            },
    //            error: function (err) {
    //                LoadBlockedRooms();
    //            },
    //        });
    //    } catch (e) {
    //        window.location.href = '/Property/RateSlab';
    //    }
    //}

    function LoadBlockedRooms() {
        try {
            var roomID = $('#rooms').val();
            var roomNum = $('#RoomNum').val(); // Get the RoomNum value as a string

            console.log('Room ID:', roomID);
            console.log('Room Number:', roomNum); // Debug RoomNum value

            $.ajax({
                url: '/Property/LoadBlockedRooms/',
                type: 'GET',
                dataType: 'json',
                data: {
                    RoomCode: roomID,
                    RoomNumber: roomNum // Pass RoomNum as part of the request
                },
                success: function (data) {
                    console.log('Data received:', data);
                    $("#RatePlan").empty();
                    var trHTML = `
                    <tr>
                        <th style="display:none">Room Category ID</th>
                        <th>Room Category</th>
                        <th>Room Num</th>
                        <th>Slab Start Date</th>
                        <th>Slab End Date</th>
                        <th></th>
                        <th></th>
                    </tr>`;

                    if (data.length > 0) {
                        $.each(data, function (i, item) {
                            var sDate = moment(item.StartDate);
                            var eDate = moment(item.EndDate);
                            trHTML += `
                            <tr>
                                <td style="display:none">${item.Id}</td>
                                <td>${item.Category}</td>
                                <td>${item.RoomNumber}</td>
                                <td>${sDate.format('DD-MM-YYYY')}</td>
                                <td>${eDate.format('DD-MM-YYYY')}</td>
                                <td><input type="button" ID="${item.Id}" class="Edit btn btn-primary" value="Unblock" /></td>
                                <td><input type="button" ID="${item.Id}" style="display:none" class="Delete btn btn-danger" value="Delete" /></td>
                            </tr>`;
                        });
                        $('#RatePlan').append(trHTML);
                    }
                },
                error: function (err) {
                    console.error('Error:', err);
                }
            });
        } catch (e) {
            console.error('Exception:', e);
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