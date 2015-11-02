console.log("loaded");
var $;
var isConnected = false;
$(document).ready(function () {
    $("#logoutBtn").hide();
    $("#msgFooter").hide();
    $("#infoFooter").hide();
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
        var values;
        var $inputs = $('#loginForm :input');
        var values = {};
        $inputs.each(function () {
            values[this.name] = $(this).val();
        });
        e.preventDefault();
        var login = values["login"];
        var password = values["pwd"];
        hub.invoke("Connect", { Login: login, Password: password }).done(function () {
            console.log("Successfuly connected");
        });
    });
    $("#registerForm").submit(function (e) {
        var values;
        var $inputs = $('#registerForm :input');
        var values = {};
        $inputs.each(function () {
            values[this.name] = $(this).val();
        });
        var sex = $('input[name=sex]:checked', '#registerForm').val();
        var login = values["login"];
        var password = values["pwd"];
        var passwordConfirm = values["pwdConfirm"];
        var email = values["email"];
        e.preventDefault();
        if (checkInput(login, 4) && checkInput(password, 4) && password == passwordConfirm && checkInput(email, 3) && (sex == 0 || sex == 1)) {
            hub.invoke("RegisterUser", { Login: login, Password: password, Email: email, Sex: sex }).done(function () {
                console.log("registered");
            });
        }
        else {
            alert("incorrect form");
        }
    });
    $("#playersBtn").click(function () {
        hub.invoke("GetConnectedPlayers");
    });
    function OnLogin() {
        $('#loginModal').modal('hide');
        $("#loginBtn").hide();
        $("#registerBtn").hide();
        $("#logoutBtn").show();
        $("#msgFooter").fadeIn("slow");
        $("#infoFooter").fadeIn("slow");
        GetInfos();
    }
    function OnLogout() {
        $("#loginBtn").show();
        $("#registerBtn").show();
        $("#logoutBtn").hide();
        $("#msgFooter").fadeOut("slow");
        $("#infoFooter").fadeOut("slow");
        $("#labelPseudo").text("");
    }
    function GetInfos() {
        hub.invoke("GetPlayerInfo");
    }
    hub.on("notify", function (message) {
        $.notify(message.Content, message.MessageType);
    });
    hub.on("onConnected", function (pseudo) {
        OnLogin();
        isConnected = true;
        $("#labelPseudo").text(pseudo);
    });
    hub.on("onRegistered", function () {
        $('#signUpModal').modal('hide');
    });
    hub.on("forceDisconnect", function () {
        OnLogout();
    });
    hub.on("getInfos", function (user) {
        $("#jobLabel").text(user.Job.JobDescription);
        $("#salaryLabel").text(user.Job.Salary + " €");
        $("#userNameLabel").text(user.UserName);
        $("#sexLabel").text(user.Sex == 1 ? "Male" : "Female");
        $("#accountLabel").text(user.Account + " €");
        $("#pointsLabel").text(user.Points);
        $("#propertyListOption").empty();
        for (var i = 0; i < user.Properties.length; i++) {
            $("#propertyListOption").append('<option value="' + user.Properties[i].PropertyName + '">' + user.Properties[i].PropertyName + '</option>');
        }
    });
    hub.on("showConnectedPlayers", function (players) {
        $("#playersModalBody").text("");
        for (var i = 0; i < players.length; i++) {
            $("#playersModalBody").append('<tr class="success"> <td>' + players[i].UserName + '</td> <td>' + (players[i].Sex == 1 ? "male" : "female") + '</td> <td>' + "X : " + players[i].Position.X + " Y : " + players[i].Position.Y + '</td> </tr>');
        }
        $("#playersModal").modal();
    });
    function checkInput(input, minLength) {
        return (input != null && input != "" && input.length >= minLength);
    }
    $("#refreshPropertyBtn").click(function () {
        GetInfos();
        console.log("refreshing");
    });
    setInterval(function () {
        if (isConnected) {
            GetInfos();
        }
    }, 15000);
});
//# sourceMappingURL=UserInteraction.js.map