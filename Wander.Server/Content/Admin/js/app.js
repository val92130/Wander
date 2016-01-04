
var connection = $.hubConnection();
var hub = connection.createHubProxy('GameHub');

$("#myModal").modal();

connection.start().done(function () {
    console.log("connected to hub");

    hub.invoke("GetConnectedPlayersNumber").done(function (nbr) {
        //$scope.online_players = nbr;
    });
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

    update();

    $scope.login = function (user) {
        if (typeof(user) !== "undefined" && typeof (user.pseudo) != "undefined" && typeof(user.password) != "undefined") {
            console.log('User clicked register', user);
            hub.invoke("ConnectAdmin", {Login:user.pseudo, Password:user.password}).done(function(res) {
                alert(res);
            });

        }
        
    };

    setInterval(function () {
        update();
    }, 5000);

    function update() {
        if (hub.connection.state !== 1) return;
        console.log("updating..");
        hub.invoke("GetConnectedPlayersNumber").done(function (nbr) {
            console.log(nbr);
        });
    }


}
]);



