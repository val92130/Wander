
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
    var table = $('table.display').DataTable();

    $scope.sent_messages = -1;
    $scope.online_players = -1;
    $scope.registered_players = -1;
    $scope.house_boughts = -1;
    $scope.players = {};
    $scope.currentPage = "overview";
    $scope.availablePages = ["overview", "chat", "players", "weather"];

    $scope.login = function (user) {
        if (typeof(user) !== "undefined" && typeof (user.pseudo) != "undefined" && typeof(user.password) != "undefined") {
            console.log('User clicked register', user);
            hub.invoke("ConnectAdmin", {Login:user.pseudo, Password:user.password}).done(function(res) {
                connected = res;
                if (res) {
                    $("#loginModal").modal("hide");
                    $(".hideOverlay").fadeOut();
                    $scope.updateConnectedPlayers();
                    update();
                }
            });

        }
        
    };

    $scope.broadcastMessage = function (msg) {
        if (typeof (msg.text) !== "undefined") {
            if (msg.text.length >= 1) {
                hub.invoke("BroadcastMessageAdmin", msg.text);
                msg.text = "";
            }
        }
    }

    $scope.setPage = function(pageName)
    {
        if ($.inArray(pageName, $scope.availablePages) !== -1) {        
            $scope.currentPage = pageName;
            hideAllPagesExcept(pageName);

        }
        
    }

    function hideAllPages() {
        for (var i = 0; i < $scope.availablePages.length; i++) {
            $("#page-" + $scope.availablePages[i]).hide();
        }
    }

    function hideAllPagesExcept(page) {
        for (var i = 0; i < $scope.availablePages.length; i++) {
            if ($scope.availablePages[i] === page) {
                $("#page-" + $scope.availablePages[i]).show();
                continue;
            }
            $("#page-" + $scope.availablePages[i]).hide();
        }
    }

    $scope.setPage($scope.currentPage);

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

        $scope.$apply();
    }

    $scope.updateConnectedPlayers = function () {
        $("#playerTable").html("");
        hub.invoke("GetAllPlayersAdmin").done(function (players) {
            if (players === null) return;
            $scope.players = players;
            $("#selectPlayers").html("");
            table.clear();

            for (var i = 0; i < players.length; i++) {
                var p = players[i];
                table.row.add([
                p.Pseudo,
                (p.Sex == 1 ? "male" : "female"),
                p.Email,
                p.UserId,
                p.Direction,
                '(' + Math.round(p.Position.X) + " : " + Math.round(p.Position.Y) + ')',
                p.Account,
                p.Points,
                p.HouseId,
                p.Job.JobDescription,
                p.Properties,
                 (p.IsBanned ? '<td><button class="btn btn-info" onclick="setBan(' + p.UserId + ',false)">Unban</button></td>' :
                     '<td><button class="btn btn-danger" onclick="setBan(' + p.UserId + ',true)">Ban</button></td>')

                                ]).draw(false);
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
        $('#chatBody').scrollTop($('#chatBody')[0].scrollHeight);
    });


}
]);

setBan = function (userId, value) {
    if (typeof (userId) === "number" && typeof (value) === "boolean") {
        hub.invoke((value ? "BanPlayer" : "UnBanPlayer"), userId).done(function () {
            $.notify("Player " + (value ? "banned" : "unbanned"), "info");
        });
    }
}

$(document).ready(function () {
    $('.nav li a').click(function (e) {

        $('.nav li').removeClass('active');

        var $parent = $(this).parent();
        if (!$parent.hasClass('active')) {
            $parent.addClass('active');
        }
        e.preventDefault();
    });
});






