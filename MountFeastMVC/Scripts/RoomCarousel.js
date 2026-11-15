$(document).ready(function () {
    if ($('#AvoidReload').val() != "True") {
        // alert("g");
        //loadCalendar();
    }

    
    $("input[name='PicturesModal'").click(function () {
        var roomCate = ($j(this).data("value"));
        LoadImages(roomCate)
        $('#carouselModal').modal('show');
    });


    function LoadImages(roomCate) {
        try {
            $.ajax({
                url: '/SearchResult/RoomPics',
                type: 'GET',
                dataType: 'json',
                contentType: 'application/json',
                data: '&Roomcate=' + roomCate,
                success: function (data) {

                    $("#banner2").empty();
                    var trHTML = '<div id="carouselExampleCaptions" class="carousel slide" data-bs-ride="carousel">' + 
                        ' <div class="carousel-indicators">';    
                    if (data.length > 0) {
                        
                        $.each(data, function (i, item) {
                            if (i == 0) {
                                trHTML += "<button type='button' data-bs-target='#carouselExampleCaptions' data-bs-slide-to=" + i + "' class='active' aria-current='true' aria-label='Slide " + i + "'></button>"
                            }
                            else {
                                trHTML += "<button type='button' data-bs-target='#carouselExampleCaptions' data-bs-slide-to=" + i + "' aria-label='Slide " + i + "'></button>"
                            }
                        });
                        trHTML += "</div><div class='carousel-inner'>";
                        $.each(data, function (i, item) {
                           if (i == 0) {
                                trHTML += '<div class="carousel-item active">' +
                                    '<img class="d-block w-100 h-100" src="../Uploads/' + item.ImageName + '" alt="banner-photo"></div>';
                            }
                            else {
                                trHTML += '<div class="carousel-item">' +
                                    '<img class="d-block w-100 h-100" src="../Uploads/' + item.ImageName + '" alt="banner-photo"></div>';
                            }

                        });
                        trHTML += '</div><button class="carousel-control-prev" type="button" data-bs-target="#carouselExampleCaptions" data-bs-slide="prev">' +
                            '<span class="carousel-control-prev-icon" aria-hidden="true" ></span><span class="visually-hidden">Previous</span>' +
                            '</button>' +
                            '<button class="carousel-control-next" type="button" data-bs-target="#carouselExampleCaptions" data-bs-slide="next">' +
                            '<span class="carousel-control-next-icon" aria-hidden="true"></span>' +
                            '<span class="visually-hidden">Next</span>' +
                            '</button></div>';
                        $('#banner2').append(trHTML);
                        //console.log(trHTML);
                    }
                },
                error: function (err) {
                    alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            alert(e.status + " : " + e.statusText);
            window.location.href = '/Booking/BookingList';
        }
    }

});
