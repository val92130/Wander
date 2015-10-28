$("#msgForm").submit(function (e) {
    

    var values: {};

    var $inputs = $('#msgForm :input');

    var values = {};
    $inputs.each(function () {
        values[this.name] = $(this).val();
    });

    var msg = values["message"];

    hub.invoke("SendPublicMessage", msg);
    $('#msgForm :input').val('');
    e.preventDefault();
});

$("#hideChatBoxBtn").click(function () {
    $(".panel-body").slideToggle("slow");
});

hub.on("MessageReceived", function (msg) {
    $(".panel-body").animate({ scrollTop: $('.panel-body').prop("scrollHeight") }, 1000);

    var img = msg.Sex == 1 ? "user-boy.png" : "user-girl.png"; 
    $("#chatBox").append('<li class="left clearfix"> <span class="chat-img pull-left"> <img src=" Content/' + img + '" alt="User Avatar" class="img-circle" /> </span> <div class="chat-body clearfix"> <div class="header"> <strong class="primary-font">' + msg.UserName + ' </strong> <small class="pull-right text-muted"> <span class="glyphicon glyphicon-time"></span> ' + msg.Hour + ' </small> </div > <p> ' + msg.Content +' </p> </div> </li>');
});