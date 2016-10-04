
var game = $.connection.gameHub;
var tableName = "";
var userName = "";
game.client.updateUsers = function (users) {
    document.getElementById("users").innerHTML = "";
    for(var i = 0; i < users.length; i++)
    {
        $("#users").append('<li>' + users[i].Name + '</li>');
    }
};

$("#join").click(function () {
    tableName = $("#tableName").val();
    userName = $("#userName").val();
    game.server.joinTable(tableName, userName);
});

$("#leave").click(function () {
    tableName = $("#tableName").val();
    game.server.leaveTable(tableName);
    document.getElementById("users").innerHTML = "Left!";
});

$.connection.hub.logging = true;
$.connection.hub.start().done(function () {
    $("#status").append('Connected');
});

window.onbeforeunload = function (e) {
    $.connection.hub.stop();
};
