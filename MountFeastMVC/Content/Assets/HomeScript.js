$(document).ready(function () {
    if ($('#AvoidReload').val() != "True") {
        // alert("g");
       
    }

    function LoadSearchResults() {
        try {
            $.ajax({
                url: '/SearchResult/LoadCategories',
                type: 'GET',
                dataType: 'json',
                contentType: 'application/json',
                success: function (data) {

                    $('#RoomCategory').empty().append('<option selected="selected" value="-1">All</option>');
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
            window.location.href = '/Property/Index';
        }
    }

});