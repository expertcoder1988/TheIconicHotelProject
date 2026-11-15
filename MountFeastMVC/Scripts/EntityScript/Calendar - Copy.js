$(document).ready(function () {
    if ($('#AvoidReload').val() != "True") {
        // alert("g");
        //loadCalendar();
    }

    $(function () {
        try {
            $.ajax({
                url: '/Property/Properties',
                type: 'GET',
                dataType: 'json',
                contentType: 'application/json',
                success: function (data) {
                    // console.log("K");
                    $('#AvoidReload').val("True");
                    // alert("f");
                    //var ddlPropertyTypes = $("[id*=PropertyTypes]");
                    $('#Properties').empty().append('<option selected="selected" value="0">Please select</option>');
                    //if (data.length > 0) {

                    $.each(data, function (i, item) {
                        $('#Properties').append($('<option></option>').attr('value', item.Id).text(item.PropertyName));
                    });
                  //  $("select#Properties").prop('selectedIndex', 1);
                    //document.getElementById('#PropertyTypes').selectedIndex = 1;
                    //}
                },
                error: function (err) {
                    //  alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            window.location.href = '/Property/RateSlab';
        }
    })


    $('#Properties').change(function () {
        //loadRooms();
        loadCalendar();
    });

    function loadCalendar() {
        try {
            var propertyID = new Object();
            propertyID = $('#Properties').val();

            $.ajax({
                url: "/Calander/GetEvents",
                type: 'GET',
                dataType: 'json',
                contentType: 'application/json',
                data: 'propertyID=' + propertyID,
                success: function (data) {
                    //console.log(data);

                    if (data == "No record found") {
                        var divShowData = document.getElementById('showData');
                        divShowData.innerHTML = "No record found";
                        return;
                    }
                    var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
                    //var monthDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
                    var weekday = ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"];

                    var myData = JSON.parse(data);
                    // Extract value from table header. 
                    // ('Book ID', 'Book Name', 'Category' and 'Price')
                    var col = [];
                    for (var i = 0; i < myData.length; i++) {
                        for (var key in myData[i]) {
                            if (col.indexOf(key) === -1) {
                                col.push(key);
                            }
                        }
                    }

                    // Create a table.
                    var table = document.createElement("table");

                    // Create table header row using the extracted headers above.
                    var tr = table.insertRow(-1);                   // table row.

                   // console.log(moment("2024-02", "YYYY-MM").daysInMonth());
                    var date = new Date();
                    var time = new Date(date.getTime());
                    time.setMonth(date.getMonth() + 1);
                    time.setDate(0);
                    var days = time.getDate() > date.getDate() ? time.getDate() - date.getDate() : 0;
                    var currentMonth = date.getMonth() + 1;
                    //console.log(days);

                    //Month and Year
                    for (var i = 0; i < col.length; i++) {
                        var th = document.createElement("th");      // table header.
                        
                        var m = col[i].split('/');
                        var _daysInMonth = moment(m[2] + "-" + m[0], "YYYY-MM").daysInMonth()
                       
                        if (i == 0) {
                            th.innerHTML = col[i];
                            th.setAttribute("rowspan", "3");
                        }
                        else if (m[0] == currentMonth) {
                            th.innerHTML = monthNames[m[0] - 1] + " " + m[2];
                            th.setAttribute("colspan", days+1);
                            th.style.textAlign = "left";
                            i = i + days+1;
                           // console.log(days+1);
                        }
                        else {
                            th.innerHTML = monthNames[m[0] - 1] + " " + m[2];;
                            th.setAttribute("colspan", _daysInMonth);
                            th.style.textAlign = "left";
                            i = i + _daysInMonth;
                            //console.log(_daysInMonth);
                        }                      
                        
                        tr.appendChild(th);
                        
                    }

                    //Dates
                    var tr = table.insertRow(-1);                   // table row.
                    for (var i = 1; i < col.length; i++) {
                        var th = document.createElement("th");      // table header.
                        var m = col[i].split('/');

                        const d = new Date(col[i]);
                        let day = d.getDay();

                        if (weekday[day] == "Su" || weekday[day] == "Sa") {
                            th.style.backgroundColor = "skyblue";
                        }

                        th.innerHTML = m[1];
                        //th.setAttribute("colspan", "2");
                        // console.log(col[i]);
                        tr.appendChild(th);
                    }

                    //Day Names
                    var tr = table.insertRow(-1);                   // table row.
                    for (var i = 1; i < col.length; i++) {
                        var th = document.createElement("th");      // table header.
                        //var m = col[i].split('/');
                        const d = new Date(col[i]);
                        let day = d.getDay();

                        if (weekday[day] == "Su" || weekday[day] == "Sa") {
                            th.style.backgroundColor = "skyblue";
                        }
                        th.innerHTML = weekday[day];
                        tr.appendChild(th);
                    }

                    // add json data to the table as rows.
                    for (var i = 0; i < myData.length; i++) {

                        tr = table.insertRow(-1);

                        for (var j = 0; j < col.length; j++) {
                            var tabCell = tr.insertCell(-1);
                            if (myData[i][col[j]] == "red") {
                                tabCell.style.backgroundColor = "red";
                                //  tabCell.style.width = "100px";
                            }
                            else if (myData[i][col[j]] == "green") {
                                //tabCell.innerHTML = myData[i][col[j]];
                                tabCell.style.backgroundColor = "green";
                                // tabCell.style.width = "100px";
                            }
                            else if (myData[i][col[j]] == "blue") {
                                //tabCell.innerHTML = myData[i][col[j]];
                                tabCell.style.backgroundColor = "blue";
                                // tabCell.style.width = "100px";
                            }
                            else {
                               // tabCell.innerHTML = myData[i][col[j]];
                                tabCell.style.backgroundColor = "silver";
                            }

                            // console.log(myData[i][col[j]]);
                        }
                    }

                    // Now, add the newly created table with json data, to a container.
                    var divShowData = document.getElementById('showData');
                    divShowData.innerHTML = "";
                    divShowData.appendChild(table);
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

});