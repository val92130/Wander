var connection = $.hubConnection();
var hub = connection.createHubProxy('GameHub');

connection.start(function() {
    console.log("connected to hub");
});
