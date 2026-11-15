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
            window.location.href = '/User/Index';
        }
    })

    $("table").on("click", ".Edit", function (ev) {
        // ev.preventDefault();
   
        var id = this.id;
      //   alert(id);
        window.location.href = '/User/UserEdit?id='+id;
    });

    $("table").on("click", ".Reset", function (ev) {
        // ev.preventDefault();

        var id = this.id;
        $('#Id').val(id);
        alert($('#Id').val());
        $('#PasswordReset').modal('show');
    });

    $('#ResetPassword').click(function () {
        //  var id = this.id;
        SaveData();
    })

    $('#closeRoomNum').click(function () {
        //  var id = this.id;

        $('#PasswordReset').modal('hide');
    })

    $("table").on("click", ".Block", function (ev) {
        // ev.preventDefault();
        var id = this.id;
        if (confirm("Are you sure to block this user?")) {
            DeleteRoom(id);
        }
        else {
            return false;
        }

    });

    function DeleteRoom(id) {

        try {
            $.ajax({
                url: '/User/DeleteUser/',
                type: 'POST',
                dataType: 'json',
                data: 'userID=' + id,
                //contentType: 'application/json',
                success: function (data) {
                    alert("User blocked successfully.");
                    LoadUsers();
                },
                error: function (err) {
                    alert("User blocked successfully.");
                    LoadUsers();
                    //  alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            window.location.href = '/User/LoadUsers';
        }
    }

    function SaveData() {
        try {

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

            var propertyID = new Object();
            propertyID = $('#Properties').val();

            var Access = "";
            $("#dvAccess input[type='checkbox']:checked").each(function () {
                Access = Access + this.value + ",";
            });
            // console.log(features);
            $('#hdAccess').val(Access);

            var po = new Object();
          
            po.PropertyID = propertyID;
            po.Id = $('#Id').val();
            po.vPassword = $('#password').val();
          

            // alert($('#Country').val());
            $.ajax({
                url: '/User/ResetPassword',
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


    $("#cPassword").change(function () {
        if ($('#password').val() != $('#cPassword').val()) {
            $('#msg').val() = "Password and Confirm Password Not Match";
            $('#cPassword').show();
            $('#cPassword').css('border-color', 'red');
            $('#cPassword').focus();
        }
    });

    function LoadUsers() {
        try {
            var propertyID = new Object();
            propertyID = $('#Properties').val();

            $.ajax({
                url: '/User/LoadUsers',
                type: 'GET',
                dataType: 'json',
                contentType: 'application/json',
                data: 'propertyID=' + propertyID,
                success: function (data) {
                    // console.log(data);
                    $("#userList").empty();
                    var trHTML = '';
                    trHTML = '<tr> <th>User ID</th><th>First Name</th><th>Last Name</th><th>User Id</th><th>Email</th><th>Mobile Num</th><th>Date Created</th><th></th><th></th><th></th></tr>';
                    if (data.length > 0) {

                        $.each(data, function (i, item) {
                            var sDate = moment(item.DateCreated);
                            trHTML += '<tr><td>' + item.UserId + '</td><td>' + item.FirstName + '</td>' +
                                '<td>' + item.LastName + '</td>' +
                                '<td>' + item.UserName + '</td><td>' + item.Email + '</td>' +
                                '<td>' + item.MobileNo + '</td>' +
                                '<td>' + sDate.format('DD-MM-YYYY') + '</td>' +
                                "<td><input type='button' id='" + item.UserId + "' class='Edit btn btn-primary' value='View / Edit' /></td>" +
                                "<td><input type='button' id='" + item.UserId + "' class='Reset btn btn-warning' value='Reset Password' /></td>" +
                                "<td><input type='button' id='" + item.UserId + "' class='Block btn btn-danger' value='Block' /></td></tr>";
                        });
                        $('#userList').append(trHTML);
                    }
                },
                error: function (err) {
                    alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            alert(e.status + " : " + e.statusText);
            window.location.href = '/User/LoadUsers';
        }
    }

    $('#Properties').change(function () {
        LoadUsers();
    });


});