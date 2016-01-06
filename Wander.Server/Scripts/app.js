var connection = $.hubConnection();
var hub = connection.createHubProxy('GameHub');

connection.start(function () {
    $(".overlay").fadeOut("slow");
    console.log("connected to hub");

});

$("#main-container").show("slide", { easing: "easeOutBack", duration: 1000 });


var gameApp = angular.module('gameApp', [
'gameManager'
]);
var gameManager = angular.module('gameManager', []);

gameManager.controller('gameManager', ['$scope',
function($scope) {
    $scope.isConnected = false;
    $scope.overlayVisible = false;
    $scope.playersModalBody = "";
    $scope.userPseudo = "";
    $scope.currentUser = {};

    $scope.openModal = function(modalId) {
        $("#" + modalId).modal();
    }

    $scope.hideModal = function(modalId) {
        $("#" + modalId).modal('hide');
    }

    $scope.disconnect = function() {
        hub.invoke("Disconnect").done(function () {
            OnLogout();
        });
    }

    $scope.onLogout = function() {
        $('input').each(function () {
            $(this).trigger('blur');
        });
        $scope.userPseudo = "";
        $("#bottom_navbar").fadeOut("slow");
        $scope.isConnected = false;

        deleteGame();
    }

    $scope.onLogin = function () {
        $scope.isConnected = true;
        $('input').each(function () {
            $(this).trigger('blur');
        });
        $scope.hideModal('loginModal');

        $("#bottom_navbar").fadeIn("slow");
        getInfos();
        createGame();
    }

    $scope.showPlayers = function(){
        if (!$scope.isConnected) return;
        hub.invoke("GetConnectedPlayers").done(function (players) {
            if (players != null) {
                $("#playersModalBody").text("");
                var text = "";
                for (var i = 0; i < players.length; i++) {
                    text += '<tr class="success"> <td>' + players[i].UserName + '</td> <td>' + (players[i].Sex == 1 ? "male" : "female") + '</td> <td>' + "X : " + Math.round(players[i].Position.X) + " Y : " + Math.round(players[i].Position.Y) + '</td> <td><button onclick=sendPrivateMessage("' + players[i].UserName + '") class="btn btn-success">Send private message</button></td> </tr>';
                }
                $scope.playersModalBody = text;
                $scope.openModal("playersModal");
            }
        });
    }


    $scope.login = function (user) {
        showOverlay();
        hub.invoke("Connect", { Login: user.login, Password: user.password }).done(function () {
            $(".overlay").fadeOut("slow");
        });
    }

    $scope.register = function(user) {
        if (checkInput(user.pseudo, 4) && checkInput(user.password, 4) && user.password == user.passwordConfirm && checkInput(user.email, 3) && (user.sex == 0 || user.sex == 1)) {
            hub.invoke("RegisterUser", { Login: user.pseudo, Password: user.password, Email: user.email, Sex: user.sex }).done(function (response) {
                hideOverlay();
                if (response) {
                    $('#signUpModal').modal('hide');
                    hideOverlay();
                    hub.invoke("Connect", { Login: user.pseudo, Password: user.password });
                }
            });
        } else {
            $.notify("Incorrect form", "error");
            hideOverlay();
        }
    }

    $scope.showMyInfos = function() {
        $("#my_infos_box").slideToggle();

        if ($("#msgFooter").css('display') != 'none') {
            $("#msgFooter").hide();
        }
    }

    $scope.showChat = function() {
        $("#msgFooter").slideToggle();
        if ($("#msgFooter").css('display') != 'none') {
            $(".panel-body").animate({ scrollTop: $('.panel-body').prop("scrollHeight") }, 20);
            $("#sendMsgInput").focus();
            unreadMsg = 0;
            $("#chat_btn").css("color", "#CDCDCD");
        }
        if ($("#my_infos_box").css('display') != 'none') {
            $("#my_infos_box").hide();
        }
    }

    hub.on("onConnected", function (pseudo) {
        $scope.onLogin();
        $scope.isConnected = true;
        $scope.userPseudo = pseudo;
    });

    hub.on("forceDisconnect", function () {
        $scope.onLogout();
    });


    function showOverlay() {
        $(".overlay").fadeIn("slow");
    }

    function hideOverlay() {
        $(".overlay").fadeOut("slow");

    }

    function checkInput(input, minLength) {
        return (typeof(input) !== "undefined" && input != null && input !== "" && input.length >= minLength);
    }

    function getInfos() {
        hub.invoke("GetPlayerInfo").done(function (user) {
            if (user != null) {
                updateInfos(user);
            }
        });
    }

    function updateInfos(user) {
        $scope.currentUser = user;

        $("#propertyListOption").empty();
        for (var i = 0; i < user.Properties.length; i++) {
            $("#propertyListOption").append('<option value="' + user.Properties[i].PropertyId + '">' + user.Properties[i].PropertyName + '</option>');
        }
    }


}
]);

hub.on("notify", function (message) {
    $.notify(message.Content, message.MessageType);
});