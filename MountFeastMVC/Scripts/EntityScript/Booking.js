$(document).ready(function () {
    if ($('#AvoidReload').val() != "True") {
        // alert("g");
        LoadCategories();
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
                    $("select#Properties").prop('selectedIndex', 1);
                    //document.getElementById('#Properties').selectedIndex = 1;
                    //}
                },
                error: function (err) {
                    //  alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            window.location.href = '/Booking/Booking';
        }
    })

    function LoadCategories() {
        try {
            $.ajax({
                url: '/Property/LoadCategories',
                type: 'GET',
                dataType: 'json',
                contentType: 'application/json',
                success: function (data) {
                   // console.log(data);
                    $('#RoomCategory').empty().append('<option selected="selected" value="-1">Please Select</option>');
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
            window.location.href = '/Booking/Booking';
        }
    }

    function LoadRoomNums() {
        try {
            if ($("#Properties")[0].selectedIndex == 0) {
                alert("Please select a Property");
                return;
            }
            var propertyID = new Object();
            propertyID = $('#Properties').val();

            var _txtCheckIn = new Object();
            _txtCheckIn = $('#CheckIn').val();

            var _txtCheckOut = new Object();
            _txtCheckOut = $('#CheckOut').val();

            var roomCate = new Object();
            roomCate = $('#RoomCategory').val();

            $.ajax({
                url: '/Booking/LoadRoomNums',
                type: 'GET',
                dataType: 'json',
                contentType: 'application/json',
                data: 'propertyID=' + propertyID + '&CheckIn=' + _txtCheckIn + '&CheckOut=' + _txtCheckOut + '&Roomcate=' + roomCate,
                success: function (data) {
                  // console.log(data);
                    $('#RoomNum').empty().append('<option selected="selected" value="-1">Please Select</option>');
                    //if (data.length > 0) {

                    $.each(data, function (i, item) {
                        $('#RoomNum').append($('<option></option>').attr('value', item.RoomNums).text(item.RoomNums));
                    });
                    //document.getElementById('#PropertyTypes').selectedIndex = 1;
                    //}
                },
                error: function (err) {
                    //  alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            alert(e);
            window.location.href = '/Booking/Booking';
        }
    }

    function LoadRoomNRate() {
        try {
            if ($("#Properties")[0].selectedIndex == 0) {
                alert("Please select a Property");
                return;
            }
            var propertyID = new Object();
            propertyID = $('#Properties').val();

            var _txtCheckIn = new Object();
            _txtCheckIn = $('#CheckIn').val();

            var _txtCheckOut = new Object();
            _txtCheckOut = $('#CheckOut').val();

            var roomCate = new Object();
            roomCate = $('#RoomCategory').val();

            $.ajax({
                url: '/Booking/LoadRoomRate',
                type: 'GET',
                dataType: 'json',
                contentType: 'application/json',
                data: 'propertyID=' + propertyID + '&CheckIn=' + _txtCheckIn + '&CheckOut=' + _txtCheckOut + '&Roomcate=' + roomCate,
                success: function (data) {
                    //console.log(data);
                    //console.log(JSON.stringify(data));
                    var res = data;
                    const myArray = res.split(",");
                    //console.log(myArray);
                    $('#AdultRate').val(myArray[0]);
                    
                    $('#ChildRate').val(myArray[1]);
                    //if (data.length > 0) {

                    //$.each(data, function (i, item) {
                    //    $('#AdultRate').val(item.Adult);
                    //    $('#ChildRate').val(item.Children);
                    //});
                    //document.getElementById('#PropertyTypes').selectedIndex = 1;
                    //}
                },
                error: function (err) {
                    //  alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            alert(e);
            window.location.href = '/Booking/Booking';
        }
    }

    $("#Adults").change(function () {
        var adultrate = $('#AdultRate').val();
        var childrate = $('#ChildRate').val();
        var adults = $('#Adults').val();
        var child = $('#Children').val();
        var tax = $('#RoomTax').val();
        var ExtraCleaning = $('#ExtraCleaning').val();

        var total = (adults * adultrate) + (child * childrate) + parseInt(tax) + parseInt(ExtraCleaning);
        $('#TotalRoomRate').val(total);
    });

    $("#Children").change(function () {
        var adultrate = $('#AdultRate').val();
        var childrate = $('#ChildRate').val();
        var adults = $('#Adults').val();
        var child = $('#Children').val();
        var tax = $('#RoomTax').val();
        var ExtraCleaning = $('#ExtraCleaning').val();

        var total = (adults * adultrate) + (child * childrate) + parseInt(tax) + parseInt(ExtraCleaning);
        $('#TotalRoomRate').val(total);
    });

    $("#RoomTax").change(function () {
        var adultrate = $('#AdultRate').val();
        var childrate = $('#ChildRate').val();
        var adults = $('#Adults').val();
        var child = $('#Children').val();
        var tax = $('#RoomTax').val();
        var ExtraCleaning = $('#ExtraCleaning').val();

        var total = (adults * adultrate) + (child * childrate) + parseInt(tax) + parseInt(ExtraCleaning);
        $('#TotalRoomRate').val(total);
    });

    $("#ExtraCleaning").change(function () {
        var adultrate = $('#AdultRate').val();
        var childrate = $('#ChildRate').val();
        var adults = $('#Adults').val();
        var child = $('#Children').val();
        var tax = $('#RoomTax').val();
        var ExtraCleaning = $('#ExtraCleaning').val();

        var total = (adults * adultrate) + (child * childrate) + parseInt(tax) + parseInt(ExtraCleaning);
        $('#TotalRoomRate').val(total);
    });

    function SaveData() {
        try {
            if ($("#Properties")[0].selectedIndex == 0) {
                alert("Please select a Property");
                return;
            }
            if ($("#RoomCategory")[0].selectedIndex == 0) {
                alert("Please select a Room Category");
                return;
            }

            // alert($('#hdFeatures').val());
            var pBI = new Object();
            pBI.RoomCatCode = $('#RoomCategory').val();
            pBI.PaxTitle = $('#PaxTitle').val();
            pBI.PaxFName = $('#PaxFName').val();
            pBI.PaxLName = $('#PaxLName').val();
            pBI.PaxEmail = $('#PaxEmail').val();
            pBI.PaxPhoneNumber = $('#PaxPhoneNumber').val();
            pBI.CheckIn = $('#CheckIn').val();
            pBI.CheckOut = $('#CheckOut').val();
            pBI.NoOfRooms = $('#NoOfRooms').val();
            pBI.Adults = $('#Adults').val();
            pBI.Children = $('#Adults').val();
            pBI.RoomNum = $('#RoomNum').val();
            pBI.SupplierID = $('#SupplierID').val();
            pBI.RoomTax = $('#RoomTax').val();
            pBI.RoomRate = $('#TotalRoomRate').val();
            pBI.TotalRoomRate = $('#TotalRoomRate').val();
            pBI.HotelCode = $('#Properties').val();
            pBI.ExtraCleaning = $('#ExtraCleaning').val();
            
            //alert(pRO.Features);

            $.ajax({
                url: '/Booking/Save',
                type: 'POST',
                dataType: 'json',
                data: JSON.stringify(pBI),
                contentType: 'application/json',
                success: function () {
                     alert("Booking done Succesfully Updated");
                    window.location.href = '/Booking/Booking';
                    //loadRooms();
                },
                error: function (err) {
                    alert("Booking done Succesfully Updated");
                    window.location.href = '/Booking/Booking';
                },
            });
        } catch (e) {
            //  alert(e);
            window.location.href = '/Booking/Booking';

        }
    }



    $('#btnSave').click(function () {

        SaveData();
        window.location.href = '/Booking/Booking';
    })

    $('#RoomCategory').change(function () {
        if ($("#CheckIn").val() == "") {
            alert("Please select CheckIn Date");
            $("#RoomCategory")[0].selectedIndex = 0;
            return;
        }
        if ($("#CheckOut").val() == "") {
            alert("Please select a CheckOut Date");
            $("#RoomCategory")[0].selectedIndex = 0;
            return;
        }
        if ($("#Properties")[0].selectedIndex !== 0) {
            LoadRoomNums();
            LoadRoomNRate();
        }
      
    });

});