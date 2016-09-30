
var colour = $.connection.colourHub;
var groupName = "";

colour.client.displayColour = function (colour) {
    $("#colour").css( "background-color", colour );
};

colour.client.joined = function (connectionId) {
    $("#users").append('<li><strong>' + connectionId + '</strong>: joined</li>');
}

colour.client.left = function (connectionId) {
    $("#users").append('<li><strong>' + connectionId + '</strong>: left</li>');
}

$("#start").click(function () {
    colour.server.start(groupName);
});

$("#stop").click(function () {
    colour.server.stop();
});

$("#join").click(function () {
    groupName = $("#groupName").val();
    colour.server.joinGroup(groupName);
});

$("#leave").click(function () {
    groupName = $("#groupName").val();
    colour.server.leaveGroup(groupName);
});


$.connection.hub.logging = true;
$.connection.hub.start().done(function () {
    $("#users").append('<li>Connected</li>');
});
