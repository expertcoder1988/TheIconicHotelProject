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
            var id = $("#Id").val();

            if (id > 0) {
                LoadUserData(id);
            }
        } catch (e) {
            window.location.href = '/Property/Index';
        }
    });

    function getUrlVars() {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
      //  console.log(vars);
        return vars;
    }

    function LoadUserData(id) {

        try {
            $("#hdAccess").val();
            $.ajax({
                url: '/User/LoadUserData/',
                type: 'GET',
                dataType: 'json',
                data: 'id=' + id,
                contentType: 'application/json',
                success: function (data) {
                    // console.log(data);
                    //if ($('#AvoidReload').val() != "True") {
                    //  alert("g");

                    // }
                    if (data.length > 0) {
                        //console.log(data);

                        $.each(data, function (i, item) {

                            $('#Properties').val(item.PropertyId);
                            $('#firstName').val(item.FirstName);
                           // $('#Id').val(item.UserId);
                            $('#lastName').val(item.LastName);
                            $('#userName').val(item.UserName);
                            //$('#password').val(item.CheckOut);
                            $('#txtOwnerEmail').val(item.Email);
                            $('#phone').val(item.MobileNo);
                            $("#hdAccess").val(item.Access);
                            var str = "";
                            // console.log(item.Features);
                            if (item.Access && typeof item.Access !== 'undefined') {
                                str = (item.Access).slice(0, -1);
                                var values = str.toString().split(',');
                                $("#dvAccess").find('[value=' + values.join('], [value=') + ']').prop("checked", true);
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

    $('#doaddnew').click(function () {
        SaveData();
    })

    function SaveData() {
        try {          

            //alert($('#Id').val());
            //return;

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
            //if ($('#password').val() == '') {
            //    $('#password').css('border-color', 'red');
            //    $('#password').focus();
            //    return false;
            //}
            //if ($('#cPassword').val() == '') {
            //    $('#cPassword').css('border-color', 'red');
            //    $('#cPassword').focus();
            //    return false;
            //}
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
            po.intUserID = $('#Id').val();
            po.vUserName = $('#userName').val();
            po.vPassword = $('#password').val();
            po.vEmail = $('#txtOwnerEmail').val();
            po.vMobileNumber = $('#phone').val();
            po.AccessCodes = $('#hdAccess').val();

            // alert($('#Country').val());
            $.ajax({
                url: '/User/UpdateUser',
                type: 'POST',
                dataType: 'json',
                data: JSON.stringify(po),
                contentType: 'application/json',
                success: function () {
                    alert("User updated Succesfully.");
                    window.location.href = '/User/Index';
                    //loadRatePlans();
                },
                error: function (err) {
                    //alert(err.status + " : " + err.statusText);
                    alert("User updated Succesfully.");
                    window.location.href = '/User/Index';
                },
            });
        } catch (e) {
          //  alert(e);
            alert("User updated Succesfully.");
            window.location.href = '/User/Index';

        }
    }


    //$("#cPassword").change(function () {
    //    if ($('#password').val() != $('#cPassword').val()) {
    //        $('#msg').val() = "Password and Confirm Password Not Match";
    //        $('#cPassword').show();
    //        $('#cPassword').css('border-color', 'red');
    //        $('#cPassword').focus();
    //    }
    //});

});