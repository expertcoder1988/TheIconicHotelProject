$(document).ready(function () {
    if ($('#AvoidReload').val() !== "True") {
        // Initial setup or other operations if required
        // loadCalendar();
    }

    // Event handler for navigating through calendar months/years
    $('body').on('click', 'button.fc-prev-button', function () {
        // Handle previous button click if needed
    });

    $('body').on('click', 'button.fc-next-button', function () {
        // Handle next button click if needed
    });

    // Load properties into dropdown on page load
    $.ajax({
        url: '/Property/Properties',
        type: 'GET',
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            $('#AvoidReload').val("True");
            $('#Properties').empty().append('<option selected="selected" value="0">Please select</option>');
            $.each(data, function (i, item) {
                $('#Properties').append($('<option></option>').attr('value', item.Id).text(item.PropertyName));
            });
            $("#Properties").prop('selectedIndex', 1); // Set default selection
            loadCal($('#Properties').val()); // Load calendar for the default property
        },
        error: function (err) {
            console.error(err.status + " : " + err.statusText);
        }
    });

    $('#Properties').change(function () {
        var propertyID = $(this).val();
        if (propertyID !== "0") {
            loadCal(propertyID); // Load calendar for the selected property
        } else {
            $('#calendar').fullCalendar('destroy'); // Clear the calendar if no property is selected
        }
    });


    $('#Properties').change(function () {
        var propertyID = $(this).val();
        if (propertyID != "0") {
            loadCal(propertyID); // Load calendar for the selected property
        } else {
            $('#calendar').fullCalendar('destroy'); // Clear the calendar if no property is selected
        }
    });

    $('#CalendarViewDropdown').change(function () {
        var selectedView = $(this).val();
        $('#calendar').fullCalendar('changeView', selectedView); // Change the calendar view
    });

    function loadCal(propId) {
        // Destroy existing calendar instance if it exists
        $('#calendar').fullCalendar('destroy');

        $.ajax({
            url: "/Calander/GetEventsNew",
            type: 'GET',
            dataType: 'json',
            contentType: 'application/json',
            data: { propertyID: propId },
            success: function (data) {
                initThemeChooser({
                    init: function (themeSystem) {
                        $('#calendar').fullCalendar({
                            schedulerLicenseKey: 'GPL-My-Project-Is-Open-Source',
                            defaultDate: $('#calendar').fullCalendar('getDate'),
                            editable: true,
                            aspectRatio: 1.8,
                            theme: true,
                            themeSystem: 'jquery-ui',
                            scrollTime: '00:00',
                            header: {
                                left: 'prev,next',
                                center: 'title',
                                right: 'customWeek,agendaSevenDays'
                            },
                            firstDay: 1,
                            defaultView: 'customWeek',
                            views: {
                                customWeek: {
                                    type: 'timeline',
                                    duration: { days: 31 },
                                    buttonText: 'Week View'
                                },
                                month: {
                                    type: 'month',
                                    duration: { days: 31 },
                                    buttonText: 'Month View'
                                },
                                agendaSevenDays: {
                                    type: 'agenda',
                                    duration: { days: 7 },
                                    buttonText: '7 Days Agenda'
                                }
                            },
                            slotLabelFormat: ['MMMM YYYY', 'D'],
                            timeFormat: 'YYYY, MM, DD',
                            navLinks: true,
                            resourceAreaWidth: '17%',
                            resourceLabelText: 'Rooms List',
                            resources: $.map(data, function (item, i) {
                                return {
                                    id: item.RoomNo,
                                    category: item.ProductName,
                                    title: item.ProductName + ' - Room No.' + item.RoomNo
                                };
                            }),
                            eventSources: [
                                {
                                    // Blocked Rooms
                                    events: $.map(data, function (item) {
                                        if (item.BookingStatus == "Blocked") {
                                            return {
                                                id: item.EventID,
                                                resourceId: item.RoomNo,
                                                start: new Date(moment(item.StartDate).format('MM/DD/YYYY')),
                                                end: new Date(moment(item.EndDate).format('MM/DD/YYYY')),
                                                title: "Blocked",
                                                description: "Room is unavailable for these dates."
                                            };
                                        }
                                    }),
                                    backgroundColor: 'gray',  // Set background color to gray for blocked rooms
                                    borderColor: 'gray',
                                    textColor: 'white'
                                },
                                {
                                    events: $.map(data, function (item) {
                                        if (item.BookingStatus == "Paid") {
                                            return {
                                                id: item.EventID,
                                                resourceId: item.RoomNo,
                                                start: new Date(moment(item.StartDate).format('MM/DD/YYYY')),
                                                end: new Date(moment(item.EndDate).format('MM/DD/YYYY')),
                                                title: item.EventName,
                                                description: "<b>Source: </b>" + item.PropertySource + "<br/><br/><b>Booking No: </b>" + item.ConfirmationNo
                                            };
                                        }
                                    }),
                                    backgroundColor: 'green',
                                    borderColor: 'green',
                                    textColor: 'white'
                                },
                                {
                                    events: $.map(data, function (item) {
                                        if (item.BookingStatus == "Booked") {
                                            return {
                                                id: item.EventID,
                                                resourceId: item.RoomNo,
                                                start: new Date(moment(item.StartDate).format('MM/DD/YYYY')),
                                                end: new Date(moment(item.EndDate).format('MM/DD/YYYY')),
                                                title: item.EventName,
                                                description: "<b>Source: </b>" + item.PropertySource + "<br/><br/><b>Booking No: </b>" + item.ConfirmationNo
                                            };
                                        }
                                    }),
                                    backgroundColor: 'red',
                                    borderColor: 'red',
                                    textColor: 'white'
                                },
                                {
                                    events: $.map(data, function (item) {
                                        if (item.BookingStatus == "CheckIn") {
                                            return {
                                                id: item.EventID,
                                                resourceId: item.RoomNo,
                                                start: new Date(moment(item.StartDate).format('MM/DD/YYYY')),
                                                end: new Date(moment(item.EndDate).format('MM/DD/YYYY')),
                                                title: item.EventName,
                                                description: "<b>Source: </b>" + item.PropertySource + "<br/><br/><b>Booking No: </b>" + item.ConfirmationNo
                                            };
                                        }
                                    }),
                                    backgroundColor: 'blue',
                                    borderColor: 'blue',
                                    textColor: 'white'
                                }
                            ],
                            eventMouseover: function (data, event, view) {
                                var tooltip =
                                    '<div class="tooltiptopicevent" style="width:auto;height:auto;background:#feb811;position:absolute;z-index:10001;padding:10px 10px 10px 10px ;  line-height: 200%;">' +
                                    '<b>title:</b> ' + data.title + '</br>' +
                                    '<b>start:</b> ' + ': ' + data.start.format('YYYY-MM-DD') + '</br>' +
                                    '<b>End:</b> ' + ': ' + data.end.format('YYYY-MM-DD') + '</br>' +
                                    data.description + '</div>';

                                $("body").append(tooltip);
                                $(this).mouseover(function (e) {
                                    $(this).css('z-index', 10000);
                                    $('.tooltiptopicevent').fadeIn('500');
                                    $('.tooltiptopicevent').fadeTo('10', 1.9);
                                }).mousemove(function (e) {
                                    $('.tooltiptopicevent').css('top', e.pageY + 10);
                                    $('.tooltiptopicevent').css('left', e.pageX + 20);
                                });
                            },
                            eventMouseout: function (data, event, view) {
                                $(this).css('z-index', 8);
                                $('.tooltiptopicevent').remove();
                            },
                            eventClick: function (calEvent, jsEvent, view) {
                                $('#myModal #eventTitle').text(calEvent.title);
                                var $description = $('<div/>');
                                var $link = "../EditBooking.aspx?BookingID=" + calEvent.id;
                                $('#ed').attr('href', $link);
                                $description.append($('<p/>').html('<b>Start:</b>' + calEvent.start.format('DD-MMM-YYYY HH:mm a')));

                                if (calEvent.end != null) {
                                    $description.append($('<p/>').html('<b>End:</b>' + calEvent.end.format('DD-MMM-YYYY HH:mm a')));
                                }

                                $description.append($('<p/>').html('<b>Source:</b>' + calEvent.description));
                                $('#myModal #pDetails').empty().html($description);
                                $('#myModal').modal('show');
                            },
                            eventAfterAllRender: function (view) {
                                var viewStartDate = 4 * 12 * (moment().weekday() - 1);
                                $('.fc-body .fc-time-area .fc-scrollpane').children('div').scrollLeft(viewStartDate * view.options.slotWidth);
                            }
                        });
                    },
                    change: function (themeSystem) {
                        $('#calendar').fullCalendar('option', 'themeSystem', themeSystem);
                    }
                });
            }
        });
    }

});
