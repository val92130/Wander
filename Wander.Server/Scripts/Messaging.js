var unreadMsg = 0;
var quickChatToggle = false;
$("#msgForm").submit(function (e) {
    var values;
    var $inputs = $('#msgForm :input');
    var values = {};
    $inputs.each(function () {
        values[this.name] = $(this).val();
    });
    var msg = values["message"];
    sendMessage(msg);
    $('#msgForm :input').val('');
    e.preventDefault();
});
$("#hideChatBoxBtn").click(function () {
    $(".panel-body").slideToggle("slow");
});
$("#hideInfosBoxBtn").click(function () {
    $(".panel-body").slideToggle("slow");
});
hub.on("MessageReceived", function (msg) {
    var msgCount = $("#chatBox> li").length;
    if (msgCount >= 50) {
        $("#chatBox").text("");
    }
    $(".panel-body").animate({ scrollTop: $('.panel-body').prop("scrollHeight") }, 20);
    if ($("#msgFooter").css('display') == 'none') {
        if (msg.UserName != userPseudo) {
            unreadMsg += 1;
            $("#chat_btn").css("color", "green");
        }
    }
    var img = msg.Sex == 1 ? "user-boy.png" : "user-girl.png";
    $("#chatBox").append(buildChatMessage(msg.Sex, msg.UserName, msg.Content, msg.Hour));
});
hub.on("LoadMessages", function (msgList) {
    $("#chatBox").text("");
    $.each(msgList, function (i, item) {
        $("#chatBox").prepend(buildChatMessage(msgList[i].Sex, msgList[i].UserName, msgList[i].Content, msgList[i].Hour));
    });
});
$(document).keypress(function (event) {
    if (isConnected) {
        if ($('input:focus').size() == 0) {
            if (event.which == 117) {
                if (!quickChatToggle) {
                    $(".quickChatBox").fadeIn(function () {
                        $("#quickChatInput").focus();
                    });
                    $(".quickChatOverlay").fadeIn(300);
                }
                quickChatToggle = true;
            }
        }
    }
});
$("body").click(function () {
    if (quickChatToggle) {
        disableQuickChat();
    }
});
$(document).keyup(function (e) {
    if (e.keyCode == 27) {
        if (quickChatToggle) {
            disableQuickChat();
        }
    }
});
function sendMessage(msg) {
    hub.invoke("SendPublicMessage", msg);
}
function disableQuickChat() {
    if (!isConnected)
        return;
    quickChatToggle = false;
    $(".quickChatOverlay").fadeOut();
    $(".quickChatBox").fadeOut();
    $("#quickChatInput").val("");
    $("#quickChatInput").blur();
}
$("#quickChatForm").submit(function (e) {
    var msg = $("#quickChatInput").val();
    if (msg != "" && msg.length <= 95) {
        sendMessage(msg);
    }
    disableQuickChat();
    e.preventDefault();
});
function buildChatMessage(sex, username, content, hour) {
    var img = sex == 1 ? "user-boy.png" : "user-girl.png";
    return '<li class="left clearfix"> <span class="chat-img pull-left" > <img src=" Content/' + img + '" alt= "User Avatar" class="img-circle" /> </span> <div class="chat-body clearfix"> <div class="header"> <strong class="primary-font">' + username + ' </strong> <small class="pull-right text-muted" > <span class="glyphicon glyphicon-time" > </span> ' + hour + ' </small> </div > <p style="overflow-x: auto;"> ' + content + ' </p> </div> </li>';
}
