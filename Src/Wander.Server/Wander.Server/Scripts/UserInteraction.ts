console.log("loaded");

var $: any;

$(document).ready(function () {

    $("#logoutBtn").hide();

    $("#loginBtn").click(function () {
        $("#loginModal").modal();
    });

    $("#registerBtn").click(function () {
        $("#signUpModal").modal();
    });

    $("#logoutBtn").click(function () {
        hub.invoke("Disconnect");
        OnLogout();
    });

    $("#loginForm").submit(function (e) {
        console.log("clicked");
        var values: {};

        var $inputs = $('#loginForm :input');

        var values = {};
        $inputs.each(function () {
            values[this.name] = $(this).val();

        });

        e.preventDefault();


        var login: any = values["login"];
        var password: any = values["pwd"];


        hub.invoke("Connect", { Login: login, Password: password }).done(function () {
            console.log("Successfuly connected");
        });

    });


    $("#registerForm").submit(function (e) {
        var values: {};

        var $inputs = $('#registerForm :input');

        var values = {};
        $inputs.each(function () {
            values[this.name] = $(this).val();
        });

        var sex: any = $('input[name=sex]:checked', '#registerForm').val();
        var login = values["login"];
        var password = values["pwd"];
        var passwordConfirm = values["pwdConfirm"];
        var email = values["email"];

        e.preventDefault();

        if (checkInput(login, 4) && checkInput(password, 4) && password == passwordConfirm && checkInput(email, 3) && (sex == 0 || sex == 1)) {
            hub.invoke("RegisterUser", { Login: login, Password: password, Email: email, Sex: sex }).done(function() {
                console.log("registered");
            });
        } else {
            alert("incorrect form");
        }
    });

    function OnLogin() {
        $('#loginModal').modal('hide');
        $("#loginBtn").hide();
        $("#registerBtn").hide();
        $("#logoutBtn").show();
    }

    function OnLogout() {
        $("#loginBtn").show();
        $("#registerBtn").show();
        $("#logoutBtn").hide();
    }

    hub.on("notify", function (message) {
        $.notify(message.Content, message.MessageType);
    });

    hub.on("onConnected", function() {
        OnLogin();
    });

    hub.on("onRegistered", function() {
        $('#signUpModal').modal('hide');
    });

    function checkInput(input, minLength) {
        return (input != null && input != "" && input.length >= minLength);
    }


});