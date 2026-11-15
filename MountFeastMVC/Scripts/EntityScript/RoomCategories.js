
$(document).ready(function () {
    ko.applyBindings(new LoadRoomCategories());
});
function LoadRoomCategories() {

    var self = this;


    self.initialize = function () {
        self.HotelCode = ko.observable(),
            self.ProductID = ko.observable(),
            self.SupplierID = ko.observable(),
            self.PaxTitle = ko.observable(),
            self.PaxFName = ko.observable(),
            self.PaxLName = ko.observable(),
            self.RoomCatCode = ko.observable(),
            self.PaxEmail = ko.observable(),
            self.PaxPhoneNumber = ko.observable(),
            self.PaxDescription = ko.observable(),
            self.CheckIn = ko.observable(),
            self.CheckOut = ko.observable(),
            self.RoomRate = ko.observable(),
            self.RoomTax = ko.observable(),
            self.TotalRoomRate = ko.observable(),
            self.ExtraCleaning = ko.observable(),
            self.NoofGuest = ko.observable(),
            self.SupplierID = ko.observable(),
            self.AccomodationRent = ko.observable(),
            self.NoofGuest = ko.observable(),
            self.loadRoomCatCodes();
    }


    self.loadRoomCatCodes = function () {
        $.ajax({
            url: '/Booking/RoomCategories',
            type: "GET",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: function (xhr, status, error) {
                alert(xhr.responseText);
            },
            success: function (result) {
                // console.log(result);
                $("#RoomCatCodes").empty();
                $("#RoomCatCodes").append($('<option></option>').val(-1).html("Select Command"));
                for (var i = 0; i < result.length; i++) {
                    $("#RoomCatCodes").append($('<option></option>').val(result[i].ProductID).html(result[i].RoomCategory));
                }
            },
            cache: false
        });
    }

    self.createBooking = function () {
        try {
            //alert(PaxFName);
            $.ajax({
                url: '/Booking/Save',
                type: 'POST',
                dataType: 'json',
                data: ko.toJSON(this),
                contentType: 'application/json',
                success: function () {
                    alert("Booking done succesfully");
                },
                error: errorCallback
            });
        } catch (e) {
            alert(e);
            window.location.href = '/Booking/Booking/';

        }
    }

    //self.successCallback() = function() {
    //    alert("Booking done succesfully");
    //    window.location.href = '/Booking/Booking/';
    //}
    //self.errorCallback() = function() {
    //    // window.location.href = '/Booking/Index/';

    //}

    self.initialize();
}




