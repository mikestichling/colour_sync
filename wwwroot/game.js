
var game = $.connection.gameHub;
var tableName = "";
var userName = "";

game.client.updateUsers = function (users) {
    document.getElementById("users").innerHTML = "";
    for(var i = 0; i < users.length; i++)
    {
        $("#users").append('<li>' + users[i].Name + '</li>');
    }
    $("#tableName").val(tableName);
    $("#userName").val(userName);
    $("#start").removeAttr("disabled");
};

game.client.updateServerMessage = function(number){
    document.getElementById("serverMessages").innerHTML = "Waiting for " + number + " to pick";
};

game.client.updateTimer = function (count) {
    $(".drinkers").hide();  
    document.getElementById("gameStatus").innerHTML = "Starting in " + (4 - count);
};

game.client.startGame = function(config){
    document.getElementById("drinkers-list").innerHTML = "";
    document.getElementById("gameStatus").innerHTML = "Pick colour!";
    $(".colour").hide();
   
    for(var i = 0; i < config.Colours.length; i++)
    {
        $("." + config.Colours[i]).show();
    }
    $(".colours").show();
    
};

game.client.gameComplete = function(colourBreakdown, loosers, slowestPlayer){
    document.getElementById("drinkers-list").innerHTML = "";
    document.getElementById("serverMessages").innerHTML = "";
    document.getElementById("colour-breakdown").innerHTML = "";
    $(".drinkers").show();
    if (colourBreakdown)
    {
        for(var i = 0; i < colourBreakdown.length; i++)
        {
            //<div class="drinker green">[ ]</div><label>bob, mike</label>
            var players = "";
            for(var p = 0; p < colourBreakdown[i].Players.length; p++)
            {
                players = colourBreakdown[i].Players[p].Name + ", ";
            }
            $("#colour-breakdown").append('<div class="drinker ' + colourBreakdown[i].Move + '">[ ]</div><label>' + players + '</label>');
        }
    }
    if (loosers)
    {
        var announceSlowestPlayer = true;
        for(var i = 0; i < loosers.length; i++)
        {
            if (slowestPlayer.Id != loosers[i].Id)
            {
                announceSlowestPlayer = false;    
            }

            $("#drinkers-list").append('<li>' + loosers[i].Name + '</li>');
        }
        if (announceSlowestPlayer)
        {
            $("#drinkers-list").append('<li>' + slowestPlayer.Name + ' (slowest) </li>');
        }
    }
    else
    {
        $("#drinkers-list").append('<li>' + slowestPlayer.Name + ' (slowest) </li>');
    }
    $("#next").removeAttr("disabled");
};

$("#join").click(function () {
    tableName = $("#tableName").val().toLowerCase();
    userName = $("#userName").val().toLowerCase();
    $(".game-buttons").show();
    game.server.joinTable(tableName, userName);
});

$(".colour").click(function () {
    var colourPicked = $(this).attr("class").substring(7, $(this).attr("class").length);
    document.getElementById("gameStatus").innerHTML = "You picked: <div class='drinker " + colourPicked +"'>[ ]</div>" ;
    game.server.pickColour(tableName, colourPicked);
    $(".colours").hide();
});

$("#start").click(function () {
    $(".drinkers").hide();  
    $("#start").attr("disabled", "disabled");
    $(".colours").hide();
    game.server.startGame(tableName);
});

$("#next").click(function () {
    $(".drinkers").hide();
    $(".colours").hide();
    document.getElementById("drinkers-list").innerHTML = "";
    $("#next").attr("disabled", "disabled");
    game.server.nextRound(tableName);
});

$("#leave").click(function () {
    tableName = $("#tableName").val().toLowerCase();
    $(".game-buttons").hide();
    document.getElementById("users").innerHTML = "Left!";
    $("#start").attr("disabled", "disabled");
    game.server.leaveTable(tableName); 
});

$.connection.hub.logging = true;
$.connection.hub.start().done(function () {
    $("#status").append('Connected');
});

$.connection.hub.connectionSlow(function() {
    $("#status").append(' Connection Slow');
});

$.connection.hub.reconnecting(function() {
    $("#status").append(' Reconnecting');
    $("#start").attr("disabled", "disabled");
});

$.connection.hub.reconnected(function() {
     $("#status").append(' Reconnected :)');
     $("#start").removeAttr("disabled");
     
});

$.connection.hub.disconnected(function() {
   //setTimeout(function() {
   //    $.connection.hub.start();
   //}, 5000); // Restart connection after 5 seconds.
   $("#status").append(' Disconnected :(');
   $("#start").attr("disabled", "disabled");
});

window.onbeforeunload = function (e) {
    $.connection.hub.stop();
};

$(".hidden-at-start").hide();