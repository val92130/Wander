
var connection = $.hubConnection();
var hub = connection.createHubProxy('GameHub');

connection.start(function () {
    $(".overlay").fadeOut("slow");
    console.log("connected to hub");

});
