$(document).ready(function () {
    if ($('#AvoidReload').val() != "True") {
        // alert("g");
        loadPropertyTypes();
        loadCountries();
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
                    if (data.length > 0) {
                        var trHTML = '';
                        $.each(data, function (i, item) {
                            trHTML += '<tr><td>' + item.PropertyID + '</td><td>' + item.PropertyName + '</td>' +
                                "<td><input type='button' ID='" + item.PropertyID + "' class='basicInfo btn btn-primary' value='Basic Info' /></td >" +
                                "<td><input type='button' ID='" + item.PropertyID + "' class='address btn btn-success' value='Address' /></td>" +
                                "<td><input type='button' ID='" + item.PropertyID + "' class='feature btn btn-warning' value='Features' /></td>" +
                                "<td><input type='button' ID='" + item.PropertyID + "' class='Picture btn btn-default' value='Main Picture' /></td>" +
                                "<td><input type='button' ID='" + item.PropertyID + "' class='delete btn btn-danger' value='Delete' /></td></tr>";
                        });
                        $('#properties').append(trHTML);
                    }
                },
                error: function (err) {
                    alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            window.location.href = '/Property/Index';
        }
    })

    function loadPropertyTypes() {
        try {
            $.ajax({
                url: '/Property/PropertyType',
                type: 'GET',
                dataType: 'json',
                contentType: 'application/json',
                success: function (data) {
                    // console.log(data);
                    $('#AvoidReload').val("True");
                    //  alert("f");
                    //var ddlPropertyTypes = $("[id*=PropertyTypes]");
                    $('#PropertyTypes').empty().append('<option selected="selected" value="0">Please select</option>');
                    //if (data.length > 0) {

                    $.each(data, function (i, item) {
                        $('#PropertyTypes').append($('<option></option>').attr('value', item.Id).text(item.propertyType));
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

    function loadCountries() {
        try {
            $.ajax({
                url: '/Property/Countries',
                type: 'GET',
                dataType: 'json',
                contentType: 'application/json',
                success: function (data) {
                   
                    $('#Country').empty().append('<option selected="selected" value="0">Select Your Country</option>');
                    //if (data.length > 0) {

                    $.each(data, function (i, item) {
                        $('#Country').append($('<option></option>').attr('value', item.Code).text(item.Name));
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

    $('#doaddnew').click(function () {
        try {
            //alert(PaxFName);
            if (confirm("Are you sure to Add new property?")) {
                //alert("f")
                $.ajax({
                    url: '/Property/AddProperty',
                    type: 'POST',
                    dataType: 'json',
                    //data: ko.toJSON(this),
                    contentType: 'application/json',
                    success: function () {
                        alert("New Property Add Succesfully");
                        window.location.href = '/Property/Index';
                    },
                    error: function (err) {
                        // alert(err.status + " : " + err.statusText);
                        window.location.href = '/Property/Index';
                    },
                });
            }
            else {
                return false;
            }
            
        } catch (e) {
            // alert(e);slice
            window.location.href = '/Property/Index';

        }
    })

    $("table").on("click", ".basicInfo", function (ev) {
        // ev.preventDefault();
        $('#basicInfoModal').modal('show');

        var id = this.id;
        LoadData(id);
    });

    $("table").on("click", ".address", function (ev) {
        // ev.preventDefault();
        $('#AddressModal').modal('show');

        var id = this.id;
        LoadData(id);
    });

    $("table").on("click", ".delete", function (ev) {
        // ev.preventDefault();
        var id = this.id;
        if (confirm("Are you sure to delete this property?")) {
            DeleteProperty(id);
        }
        else {
            return false;
        }
       
    });

    function DeleteProperty(id) {

        try {
            $.ajax({
                url: '/Property/DeleteProperty/',
                type: 'POST',
                dataType: 'json',
                data: 'propertyID=' + id,
                //contentType: 'application/json',
                success: function (data) {
                    alert("Property removed successfully.");
                    window.location.href = '/Property/Index';
                },
                error: function (err) {
                    alert("Property removed successfully.");
                    window.location.href = '/Property/Index';
                    //  alert(err.status + " : " + err.statusText);
                }

            });
        } catch (e) {
            window.location.href = '/Property/Index';
        }
    }

    function SaveData(fileName) {
        try {
           
           
            var pPY = new Object();
            pPY.PropertyId = $('#propertyID').val();
            pPY.PropertyTypeId = $('#PropertyTypes').val();
            pPY.PropertyName = $('#propertyName').val();
            pPY.Phone = $('#Phone').val();
            pPY.Fax = $('#Fax').val();
            pPY.Email = $('#Email').val();
            pPY.Website = $('#Website').val();
            pPY.ContactPerson = $('#ContactPerson').val();
            pPY.CurrencyCode = $('#CurrencyCode').val();
            pPY.CurrencySymbol = $('#CurrencySymbol').val();
            pPY.BookingType = $('#BookingType').val();
            pPY.Address = $('#Address').val();
            pPY.CountryCode = $('#Country').val();
            pPY.State = $('#State').val();
            pPY.City = $('#City').val();
            pPY.PostalCode = $('#PostalCode').val();
            pPY.Latitude = $('#Latitude').val();
            pPY.Longitude = $('#Longitude').val();
            pPY.Features = $('#hdFeatures').val();
            pPY.Logo = $("#hdLogo").val();
            
            if (typeof fileName !== undefined) {
                pPY.MainPic = fileName;
            }
           
            $.ajax({
                url: '/Property/AddPropertyBasicInfo',
                type: 'POST',
                dataType: 'json',
                data: JSON.stringify(pPY),
                contentType: 'application/json',
                success: function () {
                    alert("Property Succesfully Updated");
                    window.location.href = '/Property/Index';
                },
                error: function (err) {
                     //alert(err.status + " : " + err.statusText);
                    window.location.href = '/Property/Index';
                },
            });
        } catch (e) {
             //alert(e);
          //  window.location.href = '/Property/Index';

        }
    }

    $('#UpdateBasicInfo').click(function () {
      //  UploadLogo();
        SaveData();
    })

    $('#UpdateAddress').click(function () {
        SaveData();
    })

    $('#UpdateFeatures').click(function () {
        var features = "";
        $("#dvFeatures input[type='checkbox']:checked").each(function () {
            features = features + this.value + ",";
        });
        // console.log(features);
        $('#hdFeatures').val(features);
        SaveData();
        features = "";
    })

    $('#closeAddress').click(function () {
        $('#AddressModal').modal('hide');
    })

    $('#closeBasic').click(function () {
        $('#basicInfoModal').modal('hide');
    })

    $('#closeFeatures').click(function () {
        $('#FaturesModal').modal('hide');
    })

    $('#closePic').click(function () {
        $('#PicModal').modal('hide');
    })

    $("table").on("click", ".feature", function (ev) {
        var id = this.id;
        //alert(id);
        //window.location.href = '/Property/Features?id=' + id;
        $("#dvFeatures").find('input[type=checkbox]').each(function () {
            // some staff
            this.checked = false;
        });
        $("#hdFeatures").val();
        $('#FaturesModal').modal('show');
        LoadData(id);
    })

    $("table").on("click", ".Picture", function (ev) {
        var id = this.id;
        //alert(id);
        //window.location.href = '/Property/Features?id=' + id;

        $('#PicModal').modal('show');
        LoadData(id);
    })

    function LoadData(id) {
        
        try {
            $("#hdFeatures").val();
            $.ajax({
                url: '/Property/Property/',
                type: 'GET',
                dataType: 'json',
                data: 'propertyID=' + id,
                contentType: 'application/json',
                success: function (data) {
                    // console.log(data);
                    //if ($('#AvoidReload').val() != "True") {
                    //  alert("g");

                    // }
                    if (data.length > 0) {


                        $.each(data, function (i, item) {
                            // alert(item.PropertyTypeId);
                            $('#propertyID').val(item.PropertyID);

                            $('#propertyName').val(item.PropertyName);
                            $('#Fax').val(item.Fax);
                            $('#Website').val(item.Website);
                            $('#CurrencyCode').val(item.CurrencyCode);

                            if (item.BookingType == 1) {
                                $('#BookingType').val("True");
                            }
                            else {
                                $('#BookingType').val("False");
                            }

                            $('#PropertyTypes').val(item.PropertyTypes);
                            $('#Phone').val(item.Phone);
                            $('#Email').val(item.Email);
                            $('#ContactPerson').val(item.ContactPerson);
                            $('#CurrencySymbol').val(item.CurrencySymbol);
                            $("#PropertyTypes").val(item.PropertyTypeId);
                            $('#propertyID1').val(item.PropertyID);

                            $('#propertyName').val(item.PropertyName);
                            $('#Fax').val(item.Fax);
                            $('#Website').val(item.Website);
                            $('#CurrencyCode').val(item.CurrencyCode);

                            if (item.BookingType == 1) {
                                $('#BookingType').val("True");
                            }
                            else {
                                $('#BookingType').val("False");
                            }

                            $('#City').val(item.City);
                            $('#Address').val(item.Address);
                            $('#State').val(item.State);
                            $('#PostalCode').val(item.PostalCode);
                            $('#Latitude').val(item.Latitude);
                            $("#Longitude").val(item.Longitude);
                            $("#hdFeatures").val(item.Features);
                            $("#Country").val(item.CountryCode);
                            $("#hdLogo").val(item.Logo);
                            var str = "";
                           // console.log(item.Features);
                            if (item.Features && typeof item.Features !== 'undefined') {
                                str = (item.Features).slice(0, -1);
                                var values = str.toString().split(',');
                                $("#dvFeatures").find('[value=' + values.join('], [value=') + ']').prop("checked", true);
                            }                            
                            //alert(item.CountryCode);                           
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

    $('#PicUpload').click(function () {

        // Checking whether FormData is available in browser  
        if (window.FormData !== undefined) {

            var fileUpload = $("#propertyPic").get(0);
            var files = fileUpload.files;

            var ext = $('#propertyPic').val().split('.').pop().toLowerCase();
            var arrayExtensions = ["jpg", "jpeg", "png", "bmp", "gif"];

            if (arrayExtensions.lastIndexOf(ext) == -1) {
                alert("Wrong extension type. Allowed extension types are jpg,jpeg,png,bmp,gif");
                $("#image").val("");
                return;
            }

            var picsize = files[0].size;
            //alert(picsize);
            if (picsize > 1000000) {
                alert("Picture size should less the or equal to 1MB.")
                return;
            }

            // Create FormData object  
            var fileData = new FormData();
            var fileName = $('#propertyID').val() + "-main." + ext;

            // Looping over all files and add it to FormData object  
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i], fileName);
            }

            // Adding one more key to FormData object  
            fileData.append('username', "Manas");

            //fileData.append("-", + $('#propertyID').val());
            SaveData(fileName);
            $.ajax({
                url: '/Property/UploadFiles',
                type: "POST",
                contentType: false, // Not to set any content header  
                processData: false, // Not to process data  
                data: fileData,
                success: function (result) {
                    
                    alert(result);
                },
                error: function (err) {
                    alert(err.statusText);
                }
            });
        } else {
            alert("FormData is not supported.");
        }
    })

    $('#LogoUpload').click(function () {
        UploadLogo();
    })

    function UploadLogo() {
        // Checking whether FormData is available in browser  
        if (window.FormData !== undefined) {

            var fileUpload = $("#propertyLogo").get(0);
            var files = fileUpload.files;

            var ext = $('#propertyLogo').val().split('.').pop().toLowerCase();
            var arrayExtensions = ["jpg", "jpeg", "png", "bmp", "gif"];

            if (arrayExtensions.lastIndexOf(ext) == -1) {
                alert("Wrong extension type. Allowed extension types are jpg,jpeg,png,bmp,gif");                
                return;
            }

            var picsize = files[0].size;
            //alert(picsize);
            if (picsize > 1000000) {
                alert("Picture size should less the or equal to 1MB.")
                return;
            }

            // Create FormData object  
            var fileData = new FormData();
            var fileName = "Logo." + ext;

            // Looping over all files and add it to FormData object  
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i], fileName);
            }

            // Adding one more key to FormData object  
            fileData.append('username', "mex");

            $("#hdLogo").val(fileName);

            //fileData.append("-", + $('#propertyID').val());
           SaveData();
            $.ajax({
                url: '/Property/UploadFiles',
                type: "POST",
                contentType: false, // Not to set any content header  
                processData: false, // Not to process data  
                data: fileData,
                success: function (result) {
                        alert("Logo uploaded");
                },
                error: function (err) {
                   // alert(err.statusText);
                }
            });
        } else {
            alert("FormData is not supported.");
        }
    }
    
});









////////// Knocoout JS //////////////////////


////$(document).ready(function () {
////    ko.applyBindings(new Properties());
////});

////function Properties() {

////    var self = this;
////    //PropertyTypeId = ko.observable();

////    var choices = [
////        { id: 0, name: "Apartment" },
////        { id: 1, name: "Bed and Breakfast" },
////        { id: 2, name: "Cabin" },
////        { id: 3, name: "Guesthouse" },
////        { id: 4, name: "Apartment"},
////        { id: 5, name: "Bed and Breakfast"},
////        { id: 6, name: "Cabin"},
////        { id: 7, name: "Guesthouse"},
////        { id: 8, name: "Hostel"},
////        { id: 9, name: "Hotel"},
////        { id: 10, name: "House"},
////        { id: 11, name: "Villa"},
////        { id: 12, name: "Aparthotel"},
////        { id: 13, name: "Barn"},
////        { id: 14, name: "Boat"},
////        { id: 15, name: "Boutique Hotel"},
////        { id: 16, name: "Building"},
////        { id: 17, name: "Bungalow"},
////        { id: 18, name: "Camping"},
////        { id: 19, name: "Caravan"},
////        { id: 20, name: "Casa Particular"},
////        { id: 21, name: "Castle"},
////        { id: 22, name: "Chacara"},
////        { id: 23, name: "Chalet"},
////        { id: 24, name: "Chateau"},
////        { id: 25, name: "Condo"},
////        { id: 26, name: "Cottage"},
////        { id: 27, name: "Cycladic House"},
////        { id: 28, name: "Estate"},
////        { id: 29, name: "Farmhouse"},
////        { id: 30, name: "Heritage Hotel"},
////        { id: 31, name: "House Boat"},
////        { id: 32, name: "Lodge"},
////        { id: 33, name: "Loft"},
////        { id: 34, name: "Mas"},
////        { id: 35, name: "Mill"},
////        { id: 36, name: "Mobile Home"},
////        { id: 37, name: "Recreational Vehicle"},
////        { id: 38, name: "Resort"},
////        { id: 39, name: "Riad"},
////        { id: 40, name: "Serviced Apartment"},
////        { id: 41, name: "Studio" },
////        { id: 42, name: "Tent"},
////        { id: 43, name: "Tour"},
////        { id: 44, name: "Tower"},
////        { id: 45, name: "Townhome"},
////        { id: 46, name: "Tree House"},
////        { id: 47, name: "Yacht" }
////    ];

////    self.initialize = function () {        
////       // self.propTypes();

////        self.PropertyID = ko.observable(),
////            self.PropertyName = ko.observable();
////            self.propertyName = ko.observable();

////            self.Fax = ko.observable();
////            self.Website = ko.observable();
////            self.CurrencyCode = ko.observable();
////            self.BookingType = ko.observable();
////            self.Phone = ko.observable();
////            self.Email = ko.observable();
////            self.ContactPerson = ko.observable();
////            self.CurrencySymbol = ko.observable();

////        self.Address = ko.observable("");
////        self.Country = ko.observable("");
////        self.State = ko.observable("");
////        self.City = ko.observable("");
////        self.PostalCode = ko.observable("");
////        self.Latitude = ko.observable("");
////        self.Longitude = ko.observable("");

////            self.currentProperty = ko.observable();
////        self.Properties = ko.observableArray([]);
////        self.LoadProperties();
////        propertyTypes1 = ko.observable(choices);

////        name = ko.observable();
////        id = ko.observable();

////    }

////    function Property_Type(name, id) {        
////        self.name = name;
////        self.id = id;
////        //alert(id);
////    };

////    //self.propTypes = function () {        
////    //    propertyTypes = ko.observableArray([

////    //    ]);


////      //  alert(propertyTypes()[1][0]);
////   // };

////    self.showBasicInfo = function (vm) {
////        self.currentProperty(vm);
////        $('#basicInfoModal').modal('show');
////    };

////    self.showFeatures = function (vm) {
////        window.location.href = '/Property/Features';
////    };

////    self.showIndex = function (vm) {
////        window.location.href = '/Property/Index';
////    };

////    self.showPropertyForm = function (vm) {
////        try {
////            self.currentProperty(vm);
////            $('#AddressModal').modal('show');
////        } catch (e) {
////           // alert(e);
////            // window.location.href = '/Property/Index';
////        }
////    };

////    self.LoadProperties = function () {
////        try {
////            $.ajax({
////                url: '/Property/Properties',
////                type: 'GET',
////                dataType: 'json',
////                contentType: 'application/json',
////                success: function (data) {
////                    //console.log(data);
////                    if (data.length > 0) {
////                        self.Properties(data);//Here we are assigning values to KO Observable array
////                    }
////                },
////                error: function (err) {
////                  //  alert(err.status + " : " + err.statusText);
////                }

////            });
////        } catch (e) {
////            window.location.href = '/Property/Index';
////        }
////    };



////    self.createProperty = function () {
////        try {
////            //alert(PaxFName);
////            $.ajax({
////                url: '/Property/AddProperty',
////                type: 'POST',
////                dataType: 'json',
////                data: ko.toJSON(this),
////                contentType: 'application/json',
////                success: function () {
////                    alert("New Property Add Succesfully");
////                    window.location.href = '/Property/Index';
////                },
////                error: function (err) {
////                    // alert(err.status + " : " + err.statusText);
////                    window.location.href = '/Property/Index';
////                },
////            });
////        } catch (e) {
////           // alert(e);
////            window.location.href = '/Property/Index';

////        }
////    };

////    self.updateProperty = function () {
////        try {
////         //   alert(self.propertyType);
////            $.ajax({
////                url: '/Property/AddPropertyBasicInfo',
////                type: 'POST',
////                dataType: 'json',
////                data: ko.toJSON(this),
////                contentType: 'application/json',
////                success: function () {
////                    alert("Property Succesfully Updated");
////                    window.location.href = '/Property/Index';
////                },
////                error: function (err) {
////                    // alert(err.status + " : " + err.statusText);
////                    window.location.href = '/Property/Index';
////                },
////            });
////        } catch (e) {
////          //  alert(e);
////            window.location.href = '/Property/Index';

////        }
////    };

////    self.initialize();

////    //self.propertyType.subscribe((selected) => {
////    //    console.log(self.propertyType());
////    //});
////}

