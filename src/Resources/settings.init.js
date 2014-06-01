$(document).ready(function () {
    $('button[id$="btnGetLocation"]').click(function () {
        //Fetching location example http://jsbin.com/ebeyaz/4/edit
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(locationSuccess, locationError);
        }
    });
});

function locationError(msg) {
    alert("Umable to fetch your current location!");
}

function locationSuccess(position) {

    $('input[id$="txtCenterLatitude"]').val(position.coords.latitude);
    $('input[id$="txtCenterLongitude"]').val(position.coords.longitude);
    
}
