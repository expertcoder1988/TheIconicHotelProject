$(document).ready(function () {
    if ($('#AvoidReload').val() != "True") {
        // alert("g");
        $("#collectprepayment").hide();
        loadProperties();
        //loadPolicies();

    }

    function loadProperties() {
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
    }

    $('#Properties').change(function () {
        loadPolicies();
    });

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
                    $("#Policies").empty();
                    var trHTML = '';
                    trHTML = '<tr><th>Policy ID</th><th>Policy Name</th><th></th><th></th></tr>';
                    if (data.length > 0) {

                        $.each(data, function (i, item) {
                            trHTML += '<tr><td>' + item.Id + '</td><td>' + item.Name + '</td>' +
                                "<td><input type='button' ID='" + item.Id + "' class='Edit btn btn-primary' value='View / Edit' /></td>" +
                                "<td><input type='button' ID='" + item.Id + "' class='delete btn btn-danger' value='Delete' /></td></tr>";
                        });
                        $('#Policies').append(trHTML);
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
            // alert(fileName);
            var propertyID = new Object();
            propertyID = $('#Properties').val();
            var po = new Object();
            po.Id = $('#Id').val();
            po.Name = $('#PolicyName').val();
            po.PropertyID = propertyID;
            if ($("#radioPrimary1").prop("checked")) {
                po.PeriodForFreeCharge = true;
            }
            else if ($("#radioPrimary2").prop("checked")) {
                po.PeriodForFreeCharge = false;
            }

            if ($("#radioPrimary3").prop("checked")) {
                po.ChargeGuestsBeforeTheyArrive = false;
            }
            else if ($("#radioPrimary4").prop("checked")) {
                po.ChargeGuestsBeforeTheyArrive = true;
            }

            if ($("#radioPrimary5").prop("checked")) {
                po.HowToCollectPayment = true;
            }
            else if ($("#radioPrimary6").prop("checked")) {
                po.HowToCollectPayment = false;
            }
            
            po.HowlongFreeOfCharge = $('#interval').val();
            po.GuestChargedForCancellation = $('#cancellation').val();
            po.ChargedForNoShow = $('#no-show').val();
            po.HowToReceiveThePrepayment = $('#prepayment_options').val();
            po.HowLongWillRefundTake = $('#prepayment_refund').val();
         
            // alert($('#Country').val());
            $.ajax({
                url: '/Policy/AddPolicy',
                type: 'POST',
                dataType: 'json',
                data: JSON.stringify(po),
                contentType: 'application/json',
                success: function () {
                    alert("New Policy Succesfully Added.");
                    // window.location.href = '/Property/Rooms';
                    loadPolicies();
                },
                error: function (err) {
                    // alert(err.status + " : " + err.statusText);
                    loadPolicies(); //window.location.href = '/Property/Rooms';
                },
            });
        } catch (e) {
            //  alert(e);
            window.location.href = '/Policy/Index';

        }
    }

    function SaveNonRefundable() {
        try {
            // alert(fileName);
            var propertyID = new Object();
            propertyID = $('#Properties').val();
            var po = new Object();
            po.Name = "Non-Refundable";
            po.PropertyID = propertyID;
            // alert($('#Country').val());
            $.ajax({
                url: '/Policy/AddNonRefundablePolicy',
                type: 'POST',
                dataType: 'json',
                data: JSON.stringify(po),
                contentType: 'application/json',
                success: function () {
                    //alert("Property Succesfully Updated");
                    // window.location.href = '/Property/Rooms';
                    loadPolicies();
                },
                error: function (err) {
                    // alert(err.status + " : " + err.statusText);
                    loadPolicies(); //window.location.href = '/Property/Rooms';
                },
            });
        } catch (e) {
            //  alert(e);
            window.location.href = '/Policy/Index';

        }
    }

    $('#UpdateBasicInfo').click(function () {
        SaveData();
        $('#basicInfoModal').modal('hide');
    })


    $("input[name$='r1']").click(function () {
        var test = $(this).val();

        if (test === "1") {
            $("#long").show();
            $("#noshow").show();
            $("#beforearrival").show();
            $("#prepayment").show();
            $("#collectprepayment").show();
        }
        else {
            $("#long").hide();
            $("#noshow").hide();
            $("#beforearrival").hide();
            $("#prepayment").hide();
            $("#collectprepayment").hide();
        }
       
    });
    $("input[name$='r2']").click(function () {
        var test = $(this).val();

        if (test === "1") {
            $("#collectprepayment").show();
        }
        else {
            $("#collectprepayment").hide();
        }

    });
    $("input[name$='r3']").click(function () {
        var test = $(this).val();

        if (test === "1") {
            $("#refundtake").show();
            $("#receiveprepaymen").show();
        }
        else {
            $("#refundtake").hide();
            $("#receiveprepaymen").show();
        }

    });

    $('#doaddnew').click(function () {
        //  var id = this.id;
        if ($("#Properties")[0].selectedIndex == 0) {
            alert("Please select a Property");
            return;
        }
        $('#basicInfoModal').modal('show');
    })

    $('#NonRef').click(function () {
        SaveNonRefundable();
        $('#NonRefundableModal').modal('hide');
    })

    $('#Non-Refundable').click(function () {
        if ($("#Properties")[0].selectedIndex == 0) {
            alert("Please select a Property");
            return;
        }
            $('#NonRefundableModal').modal('show');
    })

    $('#closeBasic').click(function () {
        $('#basicInfoModal').modal('hide');
    })

    $('#closeNonRef').click(function () {
        $('#NonRefundableModal').modal('hide');
    })

    $("table").on("click", ".Edit", function (ev) {
        var id = this.id;
        // alert(id);
        LoadPolicy(id);
        $('#basicInfoModal').modal('show');
    });

    function LoadPolicy(id) {

        try {
            //alert(id);
            $("#roomtypes").find('input[type=checkbox]').each(function () {
                // some staff
                this.checked = false;
            });
            $.ajax({
                url: '/Policy/loadPolicy/',
                type: 'POST',
                dataType: 'json',
                data: 'policyId=' + id,
                //contentType: 'application/json',
                success: function (data) {
                 
                    $.each(data, function (i, item) {
                        $('#Id').val(id);
                        $('#PolicyName').val(item.Name);
                        console.log(item.HowlongFreeOfCharge);
                        if (item.PeriodForFreeCharge == true) {
                            $("#long").show();
                            $("#noshow").show();
                            $("#beforearrival").show();
                            $("#prepayment").show();
                            $("#collectprepayment").show();
                            $("#receiveprepaymen").show();
                            $("#refundtake").show();

                            $('#interval').val(item.HowlongFreeOfCharge);
                            $('#cancellation').val(item.GuestChargedForCancellation);
                            $('#no-show').val(item.ChargedForNoShow);
                        }
                        else {
                            $("#long").hide();
                            $("#noshow").hide();
                            $("#beforearrival").hide();
                            $("#prepayment").hide();
                            $("#collectprepayment").hide();
                        }

                        if (item.ChargeGuestsBeforeTheyArrive == true) {
                            $("#radioPrimary3").prop("checked", true);
                            $("#collectprepayment").hide();
                        }
                        else {
                            $("#collectprepayment").show();
                            $("#radioPrimary4").prop("checked", true);

                            if (item.HowToCollectPayment == true) {
                                $("#radioPrimary5").prop("checked", true);
                            }
                            else {
                                $("#radioPrimary2").prop("checked", false);
                            }

                            $('#prepayment_options').val(item.HowToReceiveThePrepayment);
                            $('#prepayment_refund').val(item.HowLongWillRefundTake);
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

    $("table").on("click", ".delete", function (ev) {
        // ev.preventDefault();
        var id = this.id;
        if (confirm("Are you sure to delete this policy?")) {
            DeletePolicy(id);
        }
        else {
            return false;
        }
    });

    function DeletePolicy(id) {

        try {
            $.ajax({
                url: '/Policy/DeletePolicy/',
                type: 'POST',
                dataType: 'json',
                data: 'policyId=' + id,
                //contentType: 'application/json',
                success: function (data) {
                    alert("Policy removed successfully.");
                    loadPolicies();
                },
                error: function (err) {
                    alert("Policy removed successfully.");
                    loadPolicies();
                    //  alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            window.location.href = '/Property/Rooms';
        }
    }
});