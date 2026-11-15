$(document).ready(function () {
    // Datepicker initialization
    $("#ExpiryDate").datepicker({
        dateFormat: 'yy-mm-dd'
    });

    // Function to load voucher data if editing
    var id = $("#VoucherId").val();
    if (id > 0) {
        LoadVoucherData(id);
    }

    // AJAX function to load voucher data by ID
    function LoadVoucherData(id) {
        try {
            $.ajax({
                url: '/Voucher/Details/' + id,
                type: 'GET',
                dataType: 'json',
                success: function (data) {
                    $('#Code').val(data.Code);
                    $('#DiscountAmount').val(data.DiscountAmount);
                    $('#ExpiryDate').val(data.ExpiryDate); 
                    $('#IsActive').val(data.IsActive.toString()); 
                    $('#CategoryId').val(data.CategoryId); 
                    $('#Remarks').val(data.Remarks);
                },
                error: function (err) {
                    console.log('Error loading voucher data: ' + err.status + " : " + err.statusText);
                }
            });
        } catch (e) {
            console.log("Error loading voucher data: " + e.message);
        }
    }


    // Function to save voucher data
    function SaveVoucher() {
        try {
            var expiryDate = $('#ExpiryDate').val();
            // Ensure the date is in a proper format
            var formattedExpiryDate = expiryDate ? new Date(expiryDate).toISOString() : null;

            var voucher = {
                Code: $('#Code').val(),
                DiscountAmount: $('#DiscountAmount').val(),
                ExpiryDate: formattedExpiryDate,
                IsActive: $('#IsActive').val() === "true",
                CategoryId: $('#CategoryId').val(),
                Remarks: $('#Remarks').val()
            };

            // Debugger statement added here
            console.log(voucher);

            $.ajax({
                url: '/Voucher/Create',
                type: 'POST',
                dataType: 'json',
                data: JSON.stringify(voucher),
                contentType: 'application/json',
                success: function (response) {
                    console.log('Success:', response);
                    alert("Voucher saved successfully.");
                    window.location.href = '/Voucher/Index'; // Redirect to voucher list
                },
                error: function (xhr, status, error) {
                    console.log('Error:', xhr.status, '-', xhr.statusText);
                    alert("Error saving voucher: " + xhr.status + " - " + xhr.statusText);
                }
            });

        } catch (e) {
            console.log("Error saving voucher: " + e.message);
        }
    }

    // Save button click event handler
    $('#AddVoucher').click(function (e) {
        e.preventDefault(); // Prevent default form submission
        SaveVoucher();
    });

    // Initialize datepicker
    $('#ExpiryDate').datepicker({
        format: 'yyyy-mm-dd',
        autoclose: true
    });


    // Function to update voucher data
    function UpdateVoucher() {
        try {
            var expiryDate = $('#ExpiryDate').val();
            // Ensure the date is in a proper format
            var formattedExpiryDate = expiryDate ? new Date(expiryDate).toISOString() : null;

            var voucher = {
                VoucherId: $('#VoucherId').val(),
                Code: $('#Code').val(),
                DiscountAmount: $('#DiscountAmount').val(),
                ExpiryDate: formattedExpiryDate,
                IsActive: $('#IsActive').val() === "true",
                CategoryId: $('#CategoryId').val(),
                Remarks: $('#Remarks').val()
            };

            $.ajax({
                url: '/Voucher/Edit',
                type: 'POST',
                dataType: 'json',
                data: JSON.stringify(voucher),
                contentType: 'application/json',
                success: function (response) {
                    console.log('Success:', response);
                    alert("Voucher updated successfully.");
                    window.location.href = '/Voucher/Index'; // Redirect to voucher list
                },
                error: function (xhr, status, error) {
                    console.log('Error:', xhr.status, '-', xhr.statusText);
                    alert("Error updating voucher: " + xhr.status + " - " + xhr.statusText);
                }
            });

        } catch (e) {
            console.log("Error updating voucher: " + e.message);
        }
    }


    // Update button click event handler
    $('#UpdateVoucher').click(function (e) {
        e.preventDefault(); // Prevent default form submission
        UpdateVoucher();
    });


    // Delete button click event handler
    $('#btnDelete').click(function (e) {
        e.preventDefault(); // Prevent default form submission
        var id = $('#VoucherId').val();
        DeleteVoucher(id);
    });

    // Function to delete voucher by ID
    function DeleteVoucher(id) {
        try {
            $.ajax({
                url: '/Voucher/DeleteConfirmed/' + id, // Ensure the URL matches your route
                type: 'POST',
                headers: {
                    'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                },
                success: function (response) {
                    alert("Voucher deleted successfully.");
                    window.location.href = '/Voucher/Index'; // Redirect to voucher list
                },
                error: function (err) {
                    alert("Error deleting voucher: " + err.statusText);
                }
            });
        } catch (e) {
            console.log("Error deleting voucher: " + e.message);
        }
    }

});
