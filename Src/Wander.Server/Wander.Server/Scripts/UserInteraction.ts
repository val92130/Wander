console.log("loaded");

var $: any;

$(document).ready(function () {

    $("#loginBtn").click(function () {
        $("#loginModal").modal();
    });

    $("#registerBtn").click(function () {
        $("#signUpModal").modal();
    });

    $("#logoutBtn").click(function () {
        //logout
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

        // TO DO LOGIN TO SIGNALR
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

        // TO DO REGISTER TO SIGNALR
    });

});