
var connection = $.hubConnection();
var hub = connection.createHubProxy('GameHub');
var connected = false;

$("#loginModal").modal();

connection.start().done(function () {
    console.log("connected to hub");

});

hub.on("notify", function (message) {
    console.log(message);
    $.notify(message.Content, message.MessageType);
});

console.log("creating app");

var adminApp = angular.module('adminApp', [
'manager'
]);

var manager = angular.module('manager', []);

manager.controller('manager', ['$scope',
function ($scope) {
    $scope.sent_messages = -1;
    $scope.online_players = -1;
    $scope.registered_players = -1;
    $scope.house_boughts = -1;
    $scope.players = {};
    $scope.broadcast_message = "";

    $scope.login = function (user) {
        if (typeof(user) !== "undefined" && typeof (user.pseudo) != "undefined" && typeof(user.password) != "undefined") {
            console.log('User clicked register', user);
            hub.invoke("ConnectAdmin", {Login:user.pseudo, Password:user.password}).done(function(res) {
                connected = res;
                if (res) {
                    $("#loginModal").modal("hide");
                    update();
                }
            });

        }
        
    };

    $scope.broadcastMessage = function () {
        alert($scope.broadcast_message);
    }

    setInterval(function () {
        update();
    }, 5000);

    function update() {
        if (hub.connection.state !== 1 || !connected) return;
        console.log("updating..");
        hub.invoke("GetConnectedPlayersNumber").done(function (nbr) {
            $scope.online_players = nbr;
            console.log(nbr);
        });

        hub.invoke("GetRegisteredPlayersNumber").done(function(nbr) {
            $scope.registered_players = nbr;
        });

        hub.invoke("GetHouseBoughtsCount").done(function (nbr) {
            $scope.house_boughts = nbr;
        });

        hub.invoke("GetMessagesCount").done(function (nbr) {
            $scope.sent_messages = nbr;
        });

        hub.invoke("GetAllPlayersAdmin").done(function (players) {
            if (players === null) return;
            $scope.players = players;
            $("#selectPlayers").html("");
            for (var i = 0; i < players.length; i++) {
                $("#selectPlayers").append("<option>" + players[i].Pseudo + "</option>")
            }
        });

        $scope.$apply();
    }

    hub.on("MessageReceived", function (msg) {
        var txt = '<li class="left clearfix">' +
            '<span class="chat-img pull-left">' +
            '<img src="http://placehold.it/50/55C1E7/fff" alt="User Avatar" class="img-circle" />' +
            '</span>' +
            '<div class="chat-body clearfix">' +
            '<div class="header">' +
            '<strong class="primary-font">' + msg.UserName + '</strong>' +
            '<small class="pull-right text-muted">' +
            '<i class="fa fa-clock-o fa-fw"></i> ' + msg.Hour +
            '</small>' +
            '</div>' +
            '<p>' +
            msg.Content +
            '</p>' +
            '</div>' +
            '</li>';
        $("#chatbox").append(txt);
    });


}
]);



