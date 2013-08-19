
//used for calls
var clientId;
var liveId;
var latitude;
var longitude;
var geoLocationInitialized = false;
var myMatchesUrl = '';

/* Helper methods */

if (navigator.geolocation && !geoLocationInitialized) {
    navigator.geolocation.getCurrentPosition(success, error, { maximumAge: Infinity, timeout: 5000 });
} else {
    alert('You need a browser that supports HTML 5');
}


function error(msg) {
    alert('Cannot determine current location');
    game.initialize(liveId);
    geoLocationInitialized = true;
}

function success(position) {
    latitude = position.coords.latitude;
    longitude = position.coords.longitude;
    game.initialize(liveId);
    geoLocationInitialized = true;
}


function showMessage(messageType, message) {
    //hide the current child
    $('#gameMessages').children().hide('slow');
    $('#gameMessages').html('<div class="alert ' + messageType + '"><a class="close" data-dismiss="alert">×</a>' + message + '</div>');
}

function playSound(element) {
    $(element)[0].play();
    try {
        var oAudio = $(element)[0];
        oAudio.currentTime = 0;
    }
    catch (e) {
    }
}

function loadUrl(url) {
    isPlaying = false;
    $.get(
        url,
        {},
        function (data) { $('#mainContainer').html(data); }
    )
}

function loadUrlPost(url) {
    isPlaying = false;
    $.post(
        url,
        {},
        function (data) { $('#mainContainer').html(data); }
    )
}

function loadStaticContent(page) {
    url = '@CDNHelper.StaticContentUrl' + page;
    isPlaying = false;
    $.get(
        url,
        {},
        function (data) { $('#mainContainer').html(data); }
    )
}

function showConfirmWindow(header, text) {
    $('#windowHeader').html(header);
    $('#windowText').html(text);
    $('#retoLanzadoModal').modal('show');
}


function IamReady(element) {
    if (game.allUnitsSet()) {
        game.playerReady();
        $(element).hide();
        $('#attackBoard').removeClass('hidden');
        $('#rightLabel').html('Tablero de ataque');
        $('#inviteToJoin, #helpMessage1').hide();
        game.setTurn();
    }
    else {
        showConfirmWindow('Game', 'You need to set all your units.');
    }
}


function SendChat() {
    //send the message
    game.playerChat($('#myMessage').val());
    //clear the content
    $('#myMessage').val('');
}


function attackCell(element) {
    var row = parseInt($(element).attr('row'));
    var col = parseInt($(element).attr('col'));
    game.playerAttack(row, col);
}

/* END Helper mehotds*/
