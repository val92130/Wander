
var gameApp = angular.module('gameApp', [
'gameManager'
]);
var gameManager = angular.module('gameManager', []);

gameManager.controller('gameManager', ['$scope',
function($scope) {
    
    $scope.openModal = function(modalId) {
        $("#" + modalId).modal();
    }

    $scope.disconnect = function() {
        hub.invoke("Disconnect").done(function () {
            OnLogout();
        });
    }

    $scope.onLogout = function() {
        
    }

    $scope.login = function () {
        showOverlay();
        hub.invoke("Connect", { Login: user.login, Password: user.password }).done(function () {
            $(".overlay").fadeOut("slow");
        });
    }

    $scope.register = function() {
        if (checkInput(user.login, 4) && checkInput(user.password, 4) && user.password == user.passwordConfirm && checkInput(user.email, 3) && (user.sex == 0 || user.sex == 1)) {
            hub.invoke("RegisterUser", { Login: user.login, Password: user.password, Email: user.email, Sex: user.sex }).done(function (response) {
                hideOverlay();
                if (response) {
                    $('#signUpModal').modal('hide');
                    hideOverlay();
                    hub.invoke("Connect", { Login: user.login, Password: user.password });
                }
            });
        } else {
            $.notify("Incorrect form", "error");
            hideOverlay();
        }
    }

    function showOverlay() {
        $(".overlay").fadeIn("slow");
    }

    function hideOverlay() {
        $(".overlay").fadeOut("slow");

    }
}
]);