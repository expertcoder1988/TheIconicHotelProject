$(document).ready(function () {
    if ($('#AvoidReload').val() != "True") {
        // alert("g");

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
        //try {

        //    $.ajax({
        //        url: '/User/LoadAccessRights',
        //        type: 'GET',
        //        dataType: 'json',
        //        contentType: 'application/json',
        //        success: function (data) {
        //            // console.log(data);
        //            $("#accessList").empty();
        //            var trHTML = '';
        //            trHTML = '<tr> <th style="Display:none">ID</th><th>Name</th><th>Description</th><th></th></tr>';
        //            if (data.length > 0) {

        //                $.each(data, function (i, item) {
        //                    trHTML += '<tr><td style="Display:none">' + item.Id + '</td><td>' + item.Name + '</td>' +
        //                        '<td>' + item.Description + '</td>' +
        //                        "<td><input type='checkbox' ID='" + item.Id + "' class='Block btn btn-danger' value='" + item.Id + "' /></td></tr>";
        //                });
        //                $('#accessList').append(trHTML);
        //            }
        //        },
        //        error: function (err) {
        //            alert(err.status + " : " + err.statusText);
        //        }

        //    });
        //} catch (e) {
        //   // alert(e.status + " : " + e.statusText);
        //    window.location.href = '/Property/Rooms';
        //}
    })

    $('#doaddnew').click(function () {
        SaveData();
    })

    function SaveData() {
        try {          

            if ($('#firstName').val() == '') {
                $('#firstName').css('border-color', 'red');
                $('#firstName').focus();
                return false;
            }
            if ($('#lastName').val() == '') {
                $('#lastName').css('border-color', 'red');
                $('#lastName').focus();
                return false;
            }
            if ($('#userName').val() == '') {
                $('#userName').css('border-color', 'red');
                $('#userName').focus();
                return false;
            }
            if ($('#password').val() == '') {
                $('#password').css('border-color', 'red');
                $('#password').focus();
                return false;
            }
            if ($('#cPassword').val() == '') {
                $('#cPassword').css('border-color', 'red');
                $('#cPassword').focus();
                return false;
            }
            if ($('#txtOwnerEmail').val() == '') {
                $('#txtOwnerEmail').css('border-color', 'red');
                $('#txtOwnerEmail').focus();
                return false;
            }
            else {
                $('#txtOwnerEmail').css('border-color', '');
                var testEmail = /^[A-Z0-9._%+-]+@([A-Z0-9-]+\.)+[A-Z]{2,4}$/i;
                if (testEmail.test($('#txtOwnerEmail').val())) { }
                else {
                    $('#emailvalidation').show();
                    $('#txtOwnerEmail').css('border-color', 'red');
                    $('#txtOwnerEmail').focus();
                    return false;
                }

            }
            if ($('#phone').val() == '') {
                $('#phone').css('border-color', 'red');
                $('#phone').focus();
                return false;
            }

            var propertyID = new Object();
            propertyID = $('#Properties').val();

            var Access = "";
            $("#dvAccess input[type='checkbox']:checked").each(function () {
                Access = Access + this.value + ",";
            });
            // console.log(features);
            $('#hdAccess').val(Access);

            var po = new Object();
            po.vFirstName = $('#firstName').val();
            po.vLastName = $('#lastName').val();
            po.PropertyID = propertyID;
            po.Id = $('#Id').val();
            po.vUserName = $('#userName').val();
            po.vPassword = $('#password').val();
            po.vEmail = $('#txtOwnerEmail').val();
            po.vMobileNumber = $('#phone').val();
            po.AccessCodes = $('#hdAccess').val();

            // alert($('#Country').val());
            $.ajax({
                url: '/User/AddUser',
                type: 'POST',
                dataType: 'json',
                data: JSON.stringify(po),
                contentType: 'application/json',
                success: function () {
                    alert("New user created Succesfully.");
                    // window.location.href = '/Property/Rooms';
                    //loadRatePlans();
                },
                error: function (err) {
                    //alert(err.status + " : " + err.statusText);
                    alert("New user created Succesfully.");
                    //loadRatePlans(); //window.location.href = '/Property/Rooms';
                },
            });
        } catch (e) {
          //  alert(e);
            alert("New user created Succesfully.");
            window.location.href = '/User/Index';

        }
    }


    function fnCheck() {
       
    }
    $("#cPassword").change(function () {
        if ($('#password').val() != $('#cPassword').val()) {
            $('#msg').val() = "Password and Confirm Password Not Match";
            $('#cPassword').show();
            $('#cPassword').css('border-color', 'red');
            $('#cPassword').focus();
        }
    });

});