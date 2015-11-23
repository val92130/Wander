<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Wander.Server.index" %>
<!DOCTYPE html>
<html>
<head>
    <title>Wander Game</title>
    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <script src="Scripts/jquery.signalR-2.2.0.min.js"></script>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/bootstrap-theme.min.css" rel="stylesheet" />
    <link href="Content/Style.css" rel="stylesheet" />
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/notify.min.js"></script>
    <script src="Scripts/phaser.js"></script>
    <meta charset="utf-8" />
</head>
<body>

<nav class="navbar navbar-trans navbar-fixed-top" role="navigation" style="position: relative; margin-bottom: 0px">
    <div class="container">
        <div class="navbar-header">
            <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#navbar-collapsible">
                <span class="sr-only">Toggle navigation</span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
            </button>
            <a class="navbar-brand text-danger" href="#">WANDER</a>
        </div>
        <div class="navbar-collapse collapse" id="navbar-collapsible">
            <ul class="nav navbar-nav navbar-left">
                <li><a href="#login" id="loginBtn"><span class="glyphicon glyphicon-user" aria-hidden="true"></span> Login</a></li>
                <li><a href="#register" id="registerBtn"><span class="glyphicon glyphicon-floppy-disk" aria-hidden="true"></span> Register</a></li>
                <li><a href="#players" id="playersBtn"><span class="glyphicon glyphicon-list-alt" aria-hidden="true"></span> Players</a></li>
                <li><a href="#logout" id="logoutBtn"><span class="glyphicon glyphicon-log-out" aria-hidden="true"></span> Logout</a></li>
            </ul>
            <ul class="nav navbar-nav navbar-right">
                <li><a href="#logout" id="labelPseudo"></a></li>
            </ul>
        </div>
    </div>
</nav>
    
    <div class="overlay">
        <div class="sk-folding-cube">
            <div class="sk-cube1 sk-cube"></div>
            <div class="sk-cube2 sk-cube"></div>
            <div class="sk-cube4 sk-cube"></div>
            <div class="sk-cube3 sk-cube"></div>
        </div>
    </div>

    <div class="quickChatOverlay">

    </div>
    
    <div class="quickChatBox">
        <form id="quickChatForm">
            <div class="input-group">
                <span class="input-group-addon" id="basic-addon1"><span class="glyphicon glyphicon-comment" aria-hidden="true"></span></span>
                <input type="text" id="quickChatInput" class="form-control" placeholder="Message" aria-describedby="basic-addon1">
            </div>
        </form>
    </div>
    
    <div id="main">
        <div class="container" id="main-container">
            <div class="jumbotron">
                <h1>Welcome !</h1>
                <p class="lead">Wander is an Open-Source online multiplayer game based on C# and Javascript. <br/><br/>

                    This game allows users to wander freely in a 2D environment, to practice a virtual job, to buy a house and to tchat with other players. <br/><br/>

                    Keep in mind this game is still in development ! <br/><br/>

                </p>
                <p><a class="btn btn-lg btn-success" href="#" role="button" onclick="$('#signUpModal').modal();">Signup now</a></p>
            </div>
        </div> 
        <div id="main-game" style="display: none"></div>

    </div>
    <% Response.WriteFile("includes/modals.html"); %>
</body>

<div class="container" id="box-message-container" style="display: none">
    <%Response.WriteFile("includes/message-box.html"); %>
</div>

<div class="container" id="box-info-container" style="display: none">
    <%Response.WriteFile("includes/info-box.html"); %>
</div>
    
<% Response.WriteFile("includes/bottom-navbar.html"); %>
<script src="Scripts/Game/NightFilter.js"></script>
<script src="Scripts/Game/DayNightCycle.js"></script>
<script src="Scripts/Game/Map.js"></script>
<script src="Scripts/HubConnection.js"></script>
<script src="Scripts/JobManagement.js"></script>
<script src="Scripts/UserInteraction.js"></script>
<script src="Scripts/Game/EDirection.js"></script>
<script src="Scripts/Messaging.js"></script>
<script src="Scripts/Game/Player.js"></script>
<script src="Scripts/Game/MainGame.js"></script>

<script>


</script>
</html>
