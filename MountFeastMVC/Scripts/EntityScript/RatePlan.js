$(document).ready(function () {
    if ($('#AvoidReload').val() != "True") {
        // alert("g");
        $("#dvmeals").hide();
        $("#numberofdays").hide();
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

    function loadPolicies() {
        try {
            var propertyID = new Object();
            propertyID = $('#Properties').val();

            $.ajax({
                url: '/Policy/Policies',
                type: 'GET',
                dataType: 'json',
                contentType: 'application/json',
                data: 'propertyID=' + propertyID,
                success: function (data) {
                    // console.log(data);
                    $('#cancellation').empty().append('<option selected="selected" value="0">Please Select</option>');

                    $.each(data, function (i, item) {
                        $('#cancellation').append($('<option></option>').attr('value', item.Id).text(item.Name));
                    });
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

    $('#Properties').change(function () {
        loadPolicies();
        loadRatePlans();
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
                    // console.log(data);
                    // $('##roomtypes').empty().append('<option selected="selected" value="0">Please Select</option>');
                    $('#roomtypes').empty().append('<label><span>Which room types will be bookable with this rate plan?</span></label>');
                    $.each(data, function (i, item) {
                        $('#roomtypes')
                            .append("<br>")
                            .append("<input type='checkbox' id='" + item.CateId + "' name='" + item.CateId + "' value='" + item.CateId + "'>")
                            .append("<label for='" + item.CateId + "'>" + item.Category + "</label></div>");
                    });
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

    function loadRatePlans() {
        try {
            var propertyID = new Object();
            propertyID = $('#Properties').val();

            $.ajax({
                url: '/Policy/RatePlans',
                type: 'GET',
                dataType: 'json',
                contentType: 'application/json',
                data: 'propertyID=' + propertyID,
                success: function (data) {
                    //console.log(data);
                    $("#RatePlan").empty();
                    var trHTML = '';
                    trHTML = '<tr><th>Rate Plan ID</th><th>Rate Plan Name</th><th></th><th></th></tr>';
                    if (data.length > 0) {

                        $.each(data, function (i, item) {
                            trHTML += '<tr><td>' + item.Id + '</td><td>' + item.Name + '</td>' +
                                "<td><input type='button' ID='" + item.Id + "' class='Edit btn btn-primary' value='View / Edit' /></td>" +
                                "<td><input type='button' ID='" + item.Id + "' class='Delete btn btn-danger' value='Delete' /></td></tr>";
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
            window.location.href = '/Property/Rooms';
        }
    }

    function SaveData() {
        try {

            var propertyID = new Object();
            propertyID = $('#Properties').val();
            var po = new Object();
            po.Name = $('#RatePlanName').val();
            po.PropertyID = propertyID;
            po.Id = $('#Id').val();
            //  alert(propertyID);
            if ($("#radioPrimary1").prop("checked")) {
                po.WantStayMinimumLength = true;
                po.StayMinimumLength = $('#minimumstay').val();
            }
            else if ($("#radioPrimary2").prop("checked")) {
                po.WantStayMinimumLength = false;
                po.StayMinimumLength = -1;
            }

            if ($("#radioPrimary3").prop("checked")) {
                po.WantToIncludeMeals = false;
            }
            else if ($("#radioPrimary4").prop("checked")) {
                po.WantToIncludeMeals = true;

                if ($("#Breakfast").prop("checked")) {
                    po.Breakfast = true;
                }
                else {
                    po.Breakfast = false;
                }

                if ($("#Lunch").prop("checked")) {
                    po.Lunch = true;
                }
                else {
                    po.Lunch = false;
                }

                if ($("#Dinner").prop("checked")) {
                    po.Dinner = true;
                }
                else {
                    po.Dinner = false;
                }
            }

            if ($("#radioPrimary5").prop("checked")) {
                po.DaysBeforeCheckIn = false;
                po.NumberOfDaysBeforeCheckIn = -1;
            }
            else if ($("#radioPrimary6").prop("checked")) {
                po.DaysBeforeCheckIn = true;
                po.NumberOfDaysBeforeCheckIn = $('#numofdays').val();
            }


            po.CancellationPolicy = $('#cancellation').val();
            po.Discount = $('#Discount').val();

            var roomTypes = "";
            $("#roomtypes input[type='checkbox']:checked").each(function () {
                roomTypes = roomTypes + this.value + ",";
            });
            po.RoomTypes = roomTypes;

            // alert($('#Country').val());
            $.ajax({
                url: '/Policy/AddRatePlan',
                type: 'POST',
                dataType: 'json',
                data: JSON.stringify(po),
                contentType: 'application/json',
                success: function () {
                    alert("New Rate Plan Succesfully Added.");
                    // window.location.href = '/Property/Rooms';
                    loadRatePlans();
                },
                error: function (err) {
                    //alert(err.status + " : " + err.statusText);
                    loadRatePlans(); //window.location.href = '/Property/Rooms';
                },
            });
        } catch (e) {
            //alert(e);
            window.location.href = '/Policy/RatePlan';

        }
    }

    $('#UpdateBasicInfo').click(function () {

        SaveData();
        $('#basicInfoModal').modal('hide');
    })

    $("table").on("click", ".Delete", function (ev) {
        // ev.preventDefault();
        var id = this.id;
        if (confirm("Are you sure to delete this plan?")) {
            DeleteRatePlan(id);
        }
        else {
            return false;
        }
    });

    function DeleteRatePlan(id) {

        try {
            $.ajax({
                url: '/Property/DeleteRatePlan/',
                type: 'POST',
                dataType: 'json',
                data: 'ratePlanID=' + id,
                //contentType: 'application/json',
                success: function (data) {
                    alert("Rate Plan removed successfully.");
                    loadRatePlans();
                },
                error: function (err) {
                    alert("Rate Plan removed successfully.");
                    loadRatePlans();
                    //  alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            window.location.href = '/Property/Rooms';
        }
    }

    $("input[name$='r1']").click(function () {
        var test = $(this).val();

        if (test === "1") {
            $("#long").show();
        }
        else {
            $("#long").hide();
        }

    });
    $("input[name$='r2']").click(function () {
        var test = $(this).val();

        if (test === "1") {
            $("#dvmeals").show();
        }
        else {
            $("#dvmeals").hide();
        }

    });
    $("input[name$='r3']").click(function () {
        var test = $(this).val();

        if (test === "1") {
            $("#numberofdays").show();
        }
        else {
            $("#numberofdays").hide();
        }

    });

    $('#doaddnew').click(function () {
        //  var id = this.id;
        if ($("#Properties")[0].selectedIndex == 0) {
            alert("Please select a Property");
            return;
        }
        $('#RatePlanName').val("");
        $("select#cancellation").prop('selectedIndex', 1);
        $("#dvmeals").hide();
        $("#numberofdays").hide();
        $('#basicInfoModal').modal('show');
    })

    $('#closeBasic').click(function () {
        $('#basicInfoModal').modal('hide');
    })

    $("table").on("click", ".Edit", function (ev) {
        var id = this.id;
        // alert(id);
        LoadRatePlan(id);
        $('#basicInfoModal').modal('show');
    })

    function LoadRatePlan(id) {

        try {
            //alert(id);
            $("#roomtypes").find('input[type=checkbox]').each(function () {
                // some staff
                this.checked = false;
            });
            $.ajax({
                url: '/Policy/LoadRatePlan/',
                type: 'POST',
                dataType: 'json',
                data: 'ratePlanID=' + id,
                //contentType: 'application/json',
                success: function (data) {
                    //console.log(data);
                    $.each(data, function (i, item) {
                        $('#Id').val(item.Id);
                        $('#RatePlanName').val(item.Name);

                        if (item.WantStayMinimumLength == true) {
                            $("#radioPrimary1").prop("checked", true);
                            $("#radioPrimary2").prop("checked", false);
                            $('#minimumstay').val(item.StayMinimumLength)
                            $("#long").show();
                        }
                        else {
                            $("#radioPrimary1").prop("checked", false);
                            $("#radioPrimary2").prop("checked", true);
                            $('#minimumstay').val(-1);
                            $("#long").hide();
                        }

                        if (item.WantToIncludeMeals == false) {
                            $("#radioPrimary3").prop("checked", false);
                            $("#dvmeals").hide();
                        }
                        else {
                            $("#dvmeals").show();
                            $("#radioPrimary3").prop("checked", true);

                            if (item.Breakfast == true) {
                                $("#Breakfast").prop("checked", true);
                            }
                            else {
                                $("#Breakfast").prop("checked", false);
                            }
                            if (item.Lunch == true) {
                                $("#Lunch").prop("checked", true);
                            }
                            else {
                                $("#Lunch").prop("checked", false);
                            }
                            if (item.Dinner == true) {
                                $("#Dinner").prop("checked", true);
                            }
                            else {
                                $("#Dinner").prop("checked", false);
                            }
                        }

                        if (item.DaysBeforeCheckIn == false) {
                            $("#radioPrimary5").prop("checked", true);
                            $("#radioPrimary6").prop("checked", false);
                            $('#numofdays').val(-1);
                            $("#numberofdays").hide();
                        }
                        else {
                            $("#radioPrimary5").prop("checked", false);
                            $("#radioPrimary6").prop("checked", true);
                            $('#numofdays').val(item.NumberOfDaysBeforeCheckIn);
                            $("#numberofdays").show();
                        }

                        $('#cancellation').val(item.CancellationPolicy);
                        $('#discount').val(item.Discount);

                        //loadRooms();

                        var str = "";
                       console.log(item.RoomTypes);
                        if (item.RoomTypes && typeof item.RoomTypes !== 'undefined') {
                            str = (item.RoomTypes); //.slice(0, -1);
                            str = str.replace(/,\s*$/, "");
                            console.log(str);
                            var values = str.toString().split(',');
                            console.log(values);
                            $("#roomtypes").find('[value=' + values.join('], [value=') + ']').prop("checked", true);
                        }
                })
            },
                error: function (err) {
                    //alert("Room removed successfully.");
                    loadRooms();
                    //  alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            window.location.href = '/Property/Rooms';
        }
    }
});