console.log("loaded");
var $;
var isConnected = false;
var userPseudo;
var currentUser;
$(document).ready(function () {
    $("#logoutBtn").hide();
    $("#msgFooter").hide();
    $("#my_infos_box").hide();
    $("#main-container").show("slide", { easing: "easeOutBack", duration: 1000 });
    $("#loginBtn").click(function () {
        $("#loginModal").modal();
    });
    $("#registerBtn").click(function () {
        $("#signUpModal").modal();
    });
    $("#logoutBtn").click(function () {
        hub.invoke("Disconnect").done(function () {
            OnLogout();
        });
    });
    $("#loginForm").submit(function (e) {
        $(".overlay").fadeIn("slow");
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
            $(".overlay").fadeOut("slow");
        });
    });
    $("#registerForm").submit(function (e) {
        $(".overlay").fadeIn("slow");
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
            hub.invoke("RegisterUser", { Login: login, Password: password, Email: email, Sex: sex }).done(function (response) {
                $(".overlay").fadeOut("slow");
                if (response) {
                    $('#signUpModal').modal('hide');
                    $(".overlay").fadeOut("slow");
                    hub.invoke("Connect", { Login: login, Password: password });
                }
            });
        }
        else {
            $.notify("Incorrect form", "error");
            $(".overlay").fadeOut("slow");
        }
    });
    $("#playersBtn").click(function () {
        if (!isConnected)
            return;
        hub.invoke("GetConnectedPlayers").done(function (players) {
            if (players != null && players != undefined) {
                $("#playersModalBody").text("");
                for (var i = 0; i < players.length; i++) {
                    var val = '<tr class="success"> <td>' + players[i].UserName + '</td> <td>' + (players[i].Sex == 1 ? "male" : "female") + '</td> <td>' + "X : " + Math.round(players[i].Position.X) + " Y : " + Math.round(players[i].Position.Y) + '</td> <td><button onclick=sendPrivateMessage("' + players[i].UserName + '") class="btn btn-success">Send private message</button></td> </tr>';
                    $("#playersModalBody").append(val);
                }
                $("#playersModal").modal();
            }
        });
    });
    function OnLogin() {
        $('input').each(function () {
            $(this).trigger('blur');
        });
        $('#loginModal').modal('hide');
        $("#loginBtn").hide();
        $("#registerBtn").hide();
        $("#logoutBtn").show();
        $("#bottom_navbar").fadeIn("slow");
        GetInfos();
        $("#box-message-container").show();
        $("#box-info-container").show();
        $("#playersBtn").show();
        createGame();
    }
    function OnLogout() {
        $('input').each(function () {
            $(this).trigger('blur');
        });
        $("#loginBtn").show();
        $("#registerBtn").show();
        $("#logoutBtn").hide();
        $("#msgFooter").fadeOut("slow");
        $("#my_infos_box").fadeOut("slow");
        $("#labelPseudo").text("");
        $("#bottom_navbar").fadeOut("slow");
        $("#playersBtn").hide();
        isConnected = false;
        deleteGame();
    }
    function GetInfos() {
        hub.invoke("GetPlayerInfo").done(function (user) {
            if (user != null && user != undefined) {
                updateInfos(user);
            }
        });
    }
    function updateInfos(user) {
        currentUser = user;
        $("#jobLabel").text(user.Job.JobDescription);
        $("#salaryLabel").text(user.Job.Salary + " €");
        $("#userNameLabel").text(user.UserName);
        $("#sexLabel").text(user.Sex == 1 ? "Male" : "Female");
        $("#accountLabel").text(user.Account + " €");
        $("#pointsLabel").text(user.Points);
        $("#propertyListOption").empty();
        for (var i = 0; i < user.Properties.length; i++) {
            $("#propertyListOption").append('<option value="' + user.Properties[i].PropertyId + '">' + user.Properties[i].PropertyName + '</option>');
        }
    }
    hub.on("notify", function (message) {
        $.notify(message.Content, message.MessageType);
    });
    hub.on("onConnected", function (pseudo) {
        OnLogin();
        isConnected = true;
        $("#labelPseudo").text(pseudo);
        userPseudo = pseudo;
    });
    hub.on("forceDisconnect", function () {
        OnLogout();
    });
    function checkInput(input, minLength) {
        return (input != null && input != "" && input.length >= minLength);
    }
    $("#sellPropertyBtn").click(function () {
        $("#sell-property-title").text("Sell property n° " + $('#propertyListOption').val() + " ?");
        $("#sellPropertyModal").modal();
        console.log($('#propertyListOption').val());
        $("#hiddenPropertyId").attr("value", $('#propertyListOption').val());
    });
    $("#sellPropertyForm").submit(function (e) {
        var propertyId = $("#hiddenPropertyId").attr("value");
        var price = $("#priceInput").val();
        hub.invoke("SellProperty", propertyId, price).done(function () {
            $("#sellPropertyModal").modal("hide");
        });
        e.preventDefault();
    });
    $("#my_infos").click(function () {
        $("#my_infos_box").slideToggle();
        if ($("#msgFooter").css('display') != 'none') {
            $("#msgFooter").hide();
        }
    });
    $("#chat_btn").click(function () {
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
    });
    $("#heading_box_msg").click(function () {
        $("#msgFooter").hide();
    });
    $("#heading_box_info").click(function () {
        $("#my_infos_box").hide();
    });
    $("#rulesBtn").click(function () {
        $("#rulesModal").modal();
    });
    setInterval(function () {
        if (isConnected) {
            GetInfos();
        }
    }, 15000);
});
