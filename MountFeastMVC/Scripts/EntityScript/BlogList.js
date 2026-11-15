$(document).ready(function () {
    if ($('#AvoidReload').val() != "True") {
        //// alert("g");
        //$("#dvmeals").hide();
        //$("#numberofdays").hide();
        //const today = moment();
        ////   console.log(today.format("YYYY/MM/DD"));
        ////  $("#txtCheckOut").val(today.add(30, 'days'));
        ////  $("#txtCheckIn").val(today.format("MM/DD/YYYY"));
        //document.getElementById('txtCheckIn').value = today.add(-15, 'days').format("YYYY-MM-DD");
        //document.getElementById('txtCheckOut').value = today.add(30, 'days').format("YYYY-MM-DD");
        //LoadCategories();
    }

 

    $(function () {
        try {
            $.ajax({
                url: '/Post/PostList',
                type: 'GET',
                dataType: 'json',
                contentType: 'application/json',
                success: function (data) {
                    $("#table").empty();
                 //   console.log(data);
                    var trHTML = '';
                    trHTML = '<thead><tr><th>Id</th><th>Title</th><th>Published Date</th><th></th><th></th><th></th></tr></thead>';
                    if (data.length > 0) {
                        
                        trHTML += '<tbody>';
                        $.each(data, function (i, item) {
                            var pDate = moment(item.pDate); //new Date(parseInt(item.StartDate.substr(6)));

                            trHTML += '<tr><td>' + item.Id + '</td>' +
                                '<td>' + item.Title + '</td>' +
                                '<td>' + pDate.format('DD-MM-YYYY') + '</td>' +
                                "<td><input type='button' ID='" + item.Id + "' class='Edit btn btn-primary' value='Edit' /></td>" +
                                "<td><input type='button' ID='" + item.Id + "' class='View btn btn-secondary' value='View' /></td>" +
                                "<td><input type='button' ID='" + item.Id + "' class='Delete btn btn-warning' value='Delete' /></td></tr>";

                        });
                        trHTML += '</tbody>';
                        $('#table').append(trHTML);
                    }
                },
                error: function (err) {
                    //  alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            window.location.href = '/Post/Index';
        }
    })

 

    $("table").on("click", ".Edit", function (ev) {
         var id = this.id;
        window.location.href = '/Post/Edit/' + id;
    });

    $("table").on("click", ".View", function (ev) {
        var id = this.id;
        window.open('https://thesaraihotel.com/post/' + id, '_blank');
    });

    $("table").on("click", ".Delete", function (ev) {
        // ev.preventDefault();
        var id = this.id;
        if (confirm("Are you sure to delete this post?")) {
            DeletePost(id);
        }
        else {
            return false;
        }
    });

    function DeletePost(id) {

        try {
            $.ajax({
                url: '/Post/DeletePost/',
                type: 'POST',
                dataType: 'json',
                data: 'postId=' + id,
                //contentType: 'application/json',
                success: function (data) {
                    alert("Post removed successfully.");
                    window.location.href = '/Post/Index';
                },
                error: function (err) {
                    alert("post removed successfully.");
                    window.location.href = '/Post/Index';
                }

            });
        } catch (e) {
            window.location.href = '/Post/Index';
        }
    }


});