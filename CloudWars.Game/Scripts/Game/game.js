//The current match data
var matchData;
var isPlaying = false;
var unitsUrl;
var gamesUrl;
var soundUrl;
var gameControllerUrl = '';

//The game engine
var game = {

    /* Methods called from  the server */
    setClientId: function (id) {
        clientId = id;
    },

    doNothing: function () { },

    challengeAccepted: function (matchId) {
        if (!isPlaying) {
            if (confirm('Your challenge has been accepted, do you want to go to the list of available games?')) {
                loadUrlPost(gamesUrl);
            }
        }
    },

    challengeRejected: function (player) {
        if (!isPlaying) {
            showConfirmWindow('Challenge', 'The player ' + player + ' has rejected your challenge.');
        }
    },

    goToGame: function (matchId) {
        if (!isPlaying) {
            if (confirm('Your have accepted the challenge, do you want to go to the list of available games?')) {
                loadUrlPost(gamesUrl);
            }
        }
    },

    otherPlayerNotOnline: function () {
        showConfirmWindow('Challenge', 'The other player is not online, if it is your turn, you can move, remember that you can come back at any time to continue the game going to "My Games"');        
    },

    inviteToMatch: function (matchId, player) {
        if (!isPlaying) {
            showConfirmWindow('Game', 'The player ' + player + ' is waiting for you to start the challenge');
        }
    },

    startMatch: function () {
    },

    otherPlayerIsReady: function () {
        if (isPlaying) {
            showConfirmWindow('Game', 'The other player is ready.');
        }
    },

    identityError: function () {
        isPlaying = false;
        $('#mainContainer').html('');
        showConfirmWindow('Error!', 'Cannot identify your session, please logout and login again, if that does not work you may have to review your firewall settings.');
    },

    newChallenge: function () {
        if (!isPlaying) {
            showConfirmWindow('New Challenge', 'You have a new challenge, go to "My Challenges" to check the details');
        }
    },

    matchReady: function (gameData) {
        matchData = gameData;
        isPlaying = true;
        var allSet = true;
        //display the units and so on
        for (x in matchData.Units) {
            var u = matchData.Units[x];
            //skip death units
            if (u.Health <= 0)
                continue;
            if (u.Col == 0 || u.Row == 0) {
                var unit = '<div class="draggable" id="' + u.UnitId + '"><img src="' + unitsUrl + '' + u.Name + '.jpg" /><span class ="health" id="health_' + u.UnitId + '">' + u.Health + '/' + u.MaxHealth + '</span></div>';
                $('#units').append(unit);
                allSet = false;
            }
            else {
                //append to the proper row and col
                var unit = '<div class="draggable" id="' + u.UnitId + '"><img src="' + unitsUrl + '' + u.Name + '.jpg"/><span class ="health" id="health_' + u.UnitId + '">' + u.Health + '/' + u.MaxHealth + '<span></div>';
                $('#' + u.Row + '_' + u.Col).append(unit);
            }
        }

        if (!allSet) {
            //enable d&d
            $('.droppable').droppable({
                drop: function (event, ui) {
                    $(ui.draggable).removeAttr('style');
                    $(ui.draggable).appendTo(this);
                    //set the row and col
                    var id = $(ui.draggable).attr('id');
                    for (x in matchData.Units) {
                        var u = matchData.Units[x];
                        if (u.UnitId == id) {
                            u.Row = parseInt($(this).attr('row'), 10);
                            u.Col = parseInt($(this).attr('col'), 10);
                            //move the unit in the server
                            game.playerMoveTo(u.UnitId, u.Row, u.Col);
                        }
                    }
                }
            });
            $(".draggable").draggable({ revert: 'invalid', helper: 'clone' }); //seems like its not working
        }

        //load sounds
        var baseSound = matchData.PlayerId == matchData.Player1 ? 'rebel' : 'empire';
        $('#sounds').append('<audio id="sndLost"> <source src="' + soundUrl + '' + baseSound + 'Lost.wav" type="audio/wav"/></audio>');
        $('#sounds').append('<audio id="sndShotMade"> <source src="' + soundUrl + '' + baseSound + 'ShotMade.wav" type="audio/wav"/></audio>');
        $('#sounds').append('<audio id="sndAttackMissed"> <source src="' + soundUrl + '' + baseSound + 'AttackMissed.wav" type="audio/wav"/></audio>');
        $('#sounds').append('<audio id="sndShotMissed"> <source src="' + soundUrl + '' + baseSound + 'ShotMissed.wav" type="audio/wav"/></audio>');
        $('#sounds').append('<audio id="sndTakeDamage"> <source src="' + soundUrl + '' + baseSound + 'TakeDamage.wav" type="audio/wav"/></audio>');
        $('#sounds').append('<audio id="sndWin"> <source src="' + soundUrl + '' + baseSound + 'Win.wav" type="audio/wav"/></audio>');

        //move sounds to outter div
        $('#allSounds').html('');
        $('#sounds').appendTo('#allSounds');

        game.setTurn();
    },

    sucessfulAttack: function (row, col) {
        showMessage('alert-success', '<strong>Sucessful Attack !</strong> Is your turn to attack.');
        $('#attack_' + row + '_' + col).addClass('made');
        matchData.Turn = matchData.PlayerId;
        playSound('#sndShotMade');
    },

    takeDamage: function (damage) {
        showMessage('alert-error', '<strong>Your oponent has hit one of your units.</strong>');
        for (x in matchData.Units) {
            var u = matchData.Units[x];
            if (u.UnitId == damage.UnitId) {
                u.Health = damage.HealthAfterAttack;
                $('#health_' + u.UnitId).html(u.Health + '/' + u.MaxHealth);
                if (u.Health == 0) {
                    $('#' + damage.UnitId).hide('slow');
                    $('#' + damage.UnitId).remove();
                }
            }
        }
        matchData.Turn = '';
        playSound('#sndTakeDamage');
    },

    attackMissed: function (row, col) {
        showMessage('alert-error', '<strong>You missed your shot!</strong>');
        $('#attack_' + row + '_' + col).removeClass('made');
        $('#attack_' + row + '_' + col).addClass('missed');
        matchData.Turn = '';
        playSound('#sndAttackMissed');
    },

    shotMissed: function () {
        showMessage('alert-info', '<strong>Your oponent has missed!</strong> now is your turn to attack');
        matchData.Turn = matchData.PlayerId;
        playSound('#sndShotMissed');
    },

    endMatch: function (winner) {
        isPlaying = false;
        $('#mainContainer').html('');
        if (winner == matchData.PlayerId) {
            showMessage('alert-success', '<strong>You are the winner!</strong>');
            showConfirmWindow('Game', 'You have won the game!');
            playSound('#sndWin');
        }
        else {
            showMessage('alert-error', '<strong>You have lost the game</strong>');
            showConfirmWindow('Game', 'You have lost the game');
            playSound('#sndLost');
        }
    },

    receiveMessage: function (message) {
        var newMsg = '<div class="wp90 block bold message">' + matchData.OtherPlayerName + ': ' + message + '</div>';
        $('#chatConversation').prepend(newMsg);
    },

    pauseMatch: function () {
        //the other player is off line, pause the match
        //not implemented
    },

    /* END Methods called from  the server */

    /* Calls to the server */

    challengePlayer: function (playerId) {
        $.post(
                gameControllerUrl + 'challengePlayer',
                { playerId: playerId },
                function () {
                    //show message
                    showConfirmWindow('Challenge', 'You have challenge the other player, wait until the other player accepts.');
                });
    },

    acceptChallenge: function (challengeId) {
        $.post(gameControllerUrl + 'acceptChallenge',
            { challengeId: challengeId },
            function () { });
    },


    rejectChallenge: function (challengeId) {
        $.post(gameControllerUrl + 'rejectChallenge',
            { challengeId: challengeId },
            function () { });
    },


    playerReady: function () {
        $.post(
            gameControllerUrl + 'playerReady',
           {
               matchId: matchData.MatchId,
               playerId: matchData.PlayerId
           },
           function () {
               if (!game.allUnitsSet())
                   $(".draggable").draggable("option", "disabled", true);
           });
    },


    playMatch: function (matchId) {
        $.post(gameControllerUrl + 'playMatch',
            { matchId: matchId },
            function () { });
    },


    playerMoveTo: function (unitId, row, col) {
        $.post(gameControllerUrl + 'playerMoveTo',
            {
                matchId: matchData.MatchId,
                unitId: unitId,
                row: row,
                col: col
            },
            function () { });
    },


    playerAttack: function (row, col) {
        $.post(gameControllerUrl + 'playerAttack',
            {
                matchId: matchData.MatchId,
                playerId: matchData.PlayerId,
                row: row,
                col: col
            },
            function () { });
    },


    playerChat: function (message) {
        $.post(
                gameControllerUrl + 'playerChat',
                {
                    message: message,
                    oponentClientId: matchData.OponentClientId
                },
                function () {
                    var newMsg = '<div class="wp90 block message">' + matchData.MyPlayerName + ': ' + message + '</div>';
                    $('#chatConversation').prepend(newMsg);
                }
            );
    },

    inviteToJoin: function () {
        $.post(gameControllerUrl + 'inviteToJoin',
            {
                matchId: matchData.MatchId,
                oponentClientId: matchData.OponentClientId
            },
            function () { });
    },


    initialize: function (liveId) {
        //disable D&D
        $.post(
            gameControllerUrl + 'initialize',
            {
                liveId: liveId,
                latitude: latitude,
                longitude: longitude
            },
            function () {
                $('.needInitialization').show('slow');
            });
    },


    /* END Calls to the server */


    /* Other methods */

    allUnitsSet: function () {
        for (x in matchData.Units) {
            var u = matchData.Units[x];
            if (u.Col == 0 || u.Row == 0) return false;
        }
        return true;
    },


    setTurn: function () {
        if (matchData.Turn == matchData.PlayerId) {
            //show alert
            showMessage('alert-info', '<strong>Is your turn to attack</strong>');
        }
        else {
            showMessage('alert-info', '<strong>Waiting for the other player to attack</strong>');
        }
    },

    /* END Other methods */
};
