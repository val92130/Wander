$("#msgForm").submit(function (e) {
    var values;
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
hub.on("MessageReceived", function (message, sender) {
    $("#chatBox").animate({ scrollTop: $('#chatBox').prop("scrollHeight") }, 1000);
    $("#chatBox").append('<div id="msgBox"><div id="msgContent">' + sender + ' : ' + message + ' </div></div>');
});
//# sourceMappingURL=Messaging.js.map