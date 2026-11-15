$(document).ready(function () {
    
$(function () {
    LoadCategories();
})

    function LoadCategories() {
        try {
            $.ajax({
                url: '/Property/LoadCategories',
                type: 'GET',
                dataType: 'json',
                contentType: 'application/json',
                success: function (data) {
                    $("#cate").empty();
                    var trHTML = '';
                    trHTML = '<tr><th>ID</th><th>Room Category</th><th></th></tr>';
                    if (data.length > 0) {

                        $.each(data, function (i, item) {
                            trHTML += '<tr><td>' + item.CateId + '</td><td>' + item.Category + '</td>' +
                                "<td><input type='button' ID='" + item.CateId + "' class='delete btn btn-danger' value='Delete' /></td></tr>";
                        });
                        $('#cate').append(trHTML);
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

    $("table").on("click", ".delete", function (ev) {
        // ev.preventDefault();
        var id = this.id;
        if (confirm("Are you sure to delete this category?")) {
            DeleteCategory(id);
        }
        else {
            return false;
        }
    });

    function DeleteCategory(id) {

        try {
            $.ajax({
                url: '/Property/DeleteCategory/',
                type: 'POST',
                dataType: 'json',
                data: 'cateId=' + id,
                //contentType: 'application/json',
                success: function (data) {
                    alert("Category removed successfully.");
                    LoadCategories();
                },
                error: function (err) {
                    alert("Category removed successfully.");
                    LoadCategories();
                    //  alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            window.location.href = '/Property/Rooms';
        }
    }


$('#btnSaveCategory').click(function () {
    try {
       
        if ($("#txtRoomType").val() == "") {
            alert("Please provide category");
            return;
        }
        var txtRoomType = new Object();
        txtRoomType = $('#txtRoomType').val();
        $.ajax({
            url: '/Property/AddRoomCategory',
            type: 'POST',
            dataType: 'json',
            data: 'cate=' + txtRoomType,
            //contentType: 'application/json',
            success: function () {
                alert("New Category Add Succesfully");
                window.location.href = '/Property/NewRoomCategory';
            },
            error: function (err) {
                //   alert(err.status + " : " + err.statusText);
                window.location.href = '/Property/NewRoomCategory';
            },
        });
    } catch (e) {
        // alert(e);
        window.location.href = '/Property/NewRoomCategory';

    }
})

});