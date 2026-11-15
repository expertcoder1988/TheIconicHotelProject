function hideRooms() {

    $('#c21').hide();
    $('#c22').hide();
    $('#c31').hide();
    $('#c32').hide();
    $('#c41').hide();
    $('#c42').hide();
    $('#c51').hide();
    $('#c52').hide();

    $('#room2').hide();
    $('#room3').hide();
    $('#room4').hide();
    $('#room5').hide();
    $("#cmbRoom2, #cmbRoom3, #cmbRoom4, #cmbRoom5").val(0);
    //$("#cmbRoom2Child, #cmbRoom3Child, #cmbRoom4Child,#cmbRoom5Child").val(0);
    //$("#cmbRoom2Age1,#cmbRoom2Age2,#cmbRoom3Age1,#cmbRoom3Age2,#cmbRoom4Age1,#cmbRoom4Age2,#cmbRoom5Age1,#cmbRoom5Age2").val(0);
}

function changeroom() {
   
    var r = $('#ddlRooms').val();
    switch (r) {
        case '1':

            $('#c21').fadeOut(300);
            $('#c22').fadeOut(300);
            $('#c31').fadeOut(300);
            $('#c32').fadeOut(300);
            $('#c41').fadeOut(300);
            $('#c42').fadeOut(300);
            $('#c51').fadeOut(300);
            $('#c52').fadeOut(300);

            $('#room2').fadeOut(300);
            $('#room3').fadeOut(300);
            $('#room4').fadeOut(300);
            $('#room5').fadeOut(300);

            $("#cmbRoom2, #cmbRoom3, #cmbRoom4, #cmbRoom5").val(0);
            //$("#cmbRoom2Child, #cmbRoom3Child, #cmbRoom4Child,#cmbRoom5Child").val(0);
            //$("#cmbRoom2Age1,#cmbRoom2Age2,#cmbRoom3Age1,#cmbRoom3Age2,#cmbRoom4Age1,#cmbRoom4Age2,#cmbRoom5Age1,#cmbRoom5Age2").val(0);
            break;
        case '2':
            $('#c31').fadeOut(300);
            $('#c32').fadeOut(300);
            $('#c41').fadeOut(300);
            $('#c42').fadeOut(300);
            $('#c51').fadeOut(300);
            $('#c52').fadeOut(300);

            $('#room5').fadeOut(300);
            $('#room3').fadeOut(300);
            $('#room4').fadeOut(300);
            $('#room2').fadeIn(300);

            $("#cmbRoom3, #cmbRoom4, #cmbRoom5").val(0);
            //$("#cmbRoom3Child, #cmbRoom4Child,#cmbRoom5Child").val(0);
            //$("#cmbRoom3Age1,#cmbRoom3Age2,#cmbRoom4Age1,#cmbRoom4Age2,#cmbRoom5Age1,#cmbRoom5Age2").val(0);
            break;
        case '3':
            $('#c41').fadeOut(300);
            $('#c42').fadeOut(300);
            $('#c51').fadeOut(300);
            $('#c52').fadeOut(300);

            $('#room4').fadeOut(300);
            $('#room5').fadeOut(300);
            $('#room2').fadeIn(300);
            $('#room3').fadeIn(300);

            $("#cmbRoom4, #cmbRoom5").val(0);
            //$("#cmbRoom4Child,#cmbRoom5Child").val(0);
            //$("#cmbRoom4Age1,#cmbRoom4Age2,#cmbRoom5Age1,#cmbRoom5Age2").val(0);
            break;
        case '4':
            $('#c51').fadeOut(300);
            $('#c52').fadeOut(300);

            $('#room5').fadeOut(300);
            $('#room2').fadeIn(300);
            $('#room3').fadeIn(300);
            $('#room4').fadeIn(300);

            $("#cmbRoom5").val(0);
            //$("#cmbRoom5Child").val(0);
            //$("#cmbRoom5Age1,#cmbRoom5Age2").val(0);

            break;
        case '5':
            $('#room2').fadeIn(300);
            $('#room3').fadeIn(300);
            $('#room4').fadeIn(300);
            $('#room5').fadeIn(300);
            break;
        default:
            alert("No Room available");
    }

}

function childShow1(){
    var c = $('#cmbRoom1Child').val();
    switch(c){
        case '0':
            $('#c11').fadeOut(300);
            $('#c12').fadeOut(300);
            break;
        case '1':
            $('#c12').fadeOut(300);
            $('#c11').fadeIn(300);
             break;
         case '2':
             $('#c11').fadeIn(300);
             $('#c12').fadeIn(300);
             break;
    }
}
function childShow2() {
    var c = $('#cmbRoom2Child').val();
    switch (c) {
        case '0':
            $('#c21').fadeOut(300);
            $('#c22').fadeOut(300);
            break;
        case '1':
            $('#c22').fadeOut(300);
            $('#c21').fadeIn(300);
            break;
        case '2':
            $('#c21').fadeIn(300);
            $('#c22').fadeIn(300);
            break;
    }
}
function childShow3() {
    var c = $('#cmbRoom3Child').val();
    switch (c) {
        case '0':
            $('#c31').fadeOut(300);
            $('#c32').fadeOut(300);
            break;
        case '1':
            $('#c32').fadeOut(300);
            $('#c31').fadeIn(300);
            break;
        case '2':
            $('#c31').fadeIn(300);
            $('#c32').fadeIn(300);
            break;
    }
}
function childShow4() {
    var c = $('#cmbRoom4Child').val();
    switch (c) {
        case '0':
            $('#c41').fadeOut(300);
            $('#c42').fadeOut(300);
            break;
        case '1':
            $('#c42').fadeOut(300);
            $('#c41').fadeIn(300);
            break;
        case '2':
            $('#c41').fadeIn(300);
            $('#c42').fadeIn(300);
            break;
    }
}
function childShow5() {
    var c = $('#cmbRoom5Child').val();
    switch (c) {
        case '0':
            $('#c51').fadeOut(300);
            $('#c52').fadeOut(300);
            break;
        case '1':
            $('#c52').fadeOut(300);
            $('#c51').fadeIn(300);
            break;
        case '2':
            $('#c51').fadeIn(300);
            $('#c52').fadeIn(300);
            break;
    }
}
