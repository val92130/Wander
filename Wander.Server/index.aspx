<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Wander.Server.index" %>
<!DOCTYPE html>
<html>
   <head>
      <title>Wander Game</title>
      <script src="Scripts/jquery-1.9.1.min.js"></script>
      <script src="Scripts/jquery.signalR-2.2.0.min.js"></script>
      <link href="Content/bootstrap.min.css" rel="stylesheet" />
      <link href="Content/bootstrap-theme.min.css" rel="stylesheet" />
      <link href="Content/jquery-ui.theme.min.css" rel="stylesheet" />
      <link href="Content/Style.css" rel="stylesheet" />
      <script src="Scripts/bootstrap.min.js"></script>
      <script src="Scripts/notify.min.js"></script>
      <script src="Scripts/phaser.js"></script>
      <script src="Scripts/jquery-ui.min.js"></script>
      <link href='https://fonts.googleapis.com/css?family=Quicksand:700,400' rel='stylesheet' type='text/css'>
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
                  <li><a href="#logout" id="logoutBtn" style="display:none"><span class="glyphicon glyphicon-log-out" aria-hidden="true"></span> Logout</a></li>
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
      <div class="game" id="main-game">
         <div class="container" id="main-container" style="display:none">
            <div class="jumbotron" style ="height: 100%; margin-top: 5%;box-shadow: 15px 15px 40px #171414;">
               <h1 style="text-align:center">Welcome !</h1>
               <hr />
               <p class="lead" style="text-align:center">Wander is an Open-Source online multiplayer game based on C# and Javascript. <br/><br/>
                  This game allows users to wander freely in a 2D environment, to practice a virtual job, to buy a house and to tchat with other players. <br/><br/>
                  Keep in mind this game is still in development ! <br/><br/>
               </p>
               <button class="btn btn-primary" onclick="$('#signUpModal').modal();" style="margin-left: 40%;margin-right: 40%; width:20%"><span class="glyphicon glyphicon-user" aria-hidden="true"></span> Signup now</button>
               <hr />
            </div>
         </div>
      </div>
      <div id="modals">
         <!-- LOGIN MODAL-->
         <div class="modal fade" id="loginModal" tabindex="-1" role="dialog">
            <div class="modal-dialog" role="document">
               <div class="modal-content">
                  <div class="modal-header">
                     <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                     <h4 class="modal-title">Login</h4>
                  </div>
                  <div class="modal-body">
                     <form id="loginForm">
                        <div class="form-group">
                           <span class="glyphicon glyphicon-user" aria-hidden="true"></span>
                           <label for="login">Login:</label>
                           <input type="text" class="form-control" id="login" name="login" required>
                        </div>
                        <div class="form-group">
                           <span class="glyphicon glyphicon-cog" aria-hidden="true"></span>
                           <label for="pwd">Password:</label>
                           <input type="password" class="form-control" id="pwd" name="pwd" required>
                        </div>
                        <button id="loginFormBtn" class="btn btn-success" type="submit">Login</button>
                     </form>
                  </div>
                  <div class="modal-footer">
                     <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                  </div>
               </div>
            </div>
         </div>
         <!-- SIGN UP MODAL-->
         <div class="modal fade" id="signUpModal" tabindex="-1" role="dialog" >
            <div class="modal-dialog" role="document">
               <div class="modal-content">
                  <div class="modal-header">
                     <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                     <h4 class="modal-title">Sign Up</h4>
                  </div>
                  <div class="modal-body">
                     <form id="registerForm">
                        <div class="form-group">
                           <label for="login">Login:</label>
                           <input type="text" class="form-control" name="login" required >
                        </div>
                        <div class="form-group">
                           <label for="email">Email:</label>
                           <input type="email" class="form-control" id="email" name="email" required>
                        </div>
                        <div class="form-group">
                           <label for="pwd">Password:</label>
                           <input type="password" class="form-control" name="pwd" required>
                        </div>
                        <div class="form-group">
                           <label for="pwdConfirm">Password Confirmation:</label>
                           <input type="password" class="form-control" id="pwdConfirm" name="pwdConfirm" required>
                        </div>
                        <div class="radio">
                           <label><input type="radio" name="sex" value="1" required>Male</label>
                        </div>
                        <div class="radio">
                           <label><input type="radio" name="sex" value="0" required>Female</label>
                        </div>
                        <button id="registerFormBtn" class="btn btn-success" style="width: 20%;margin-left: 40%;">Register</button>
                     </form>
                  </div>
                  <div class="modal-footer">
                     <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                  </div>
               </div>
            </div>
         </div>
         <!-- PLAYERS MODAL-->
         <div class="modal fade" id="playersModal" tabindex="-1" role="dialog">
            <div class="modal-dialog" role="document">
               <div class="modal-content">
                  <div class="modal-header">
                     <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                     <h4 class="modal-title">Players</h4>
                  </div>
                  <div class="modal-body">
                     <table class="table">
                        <thead>
                           <tr>
                              <th>UserName</th>
                              <th>Sex</th>
                              <th>Position</th>
                           </tr>
                        </thead>
                        <tbody id="playersModalBody"></tbody>
                     </table>
                  </div>
                  <div class="modal-footer">
                     <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                  </div>
               </div>
            </div>
         </div>
         <!-- JOBS MODAL-->
         <div class="modal fade" id="jobsModal" tabindex="-1" role="dialog">
            <div class="modal-dialog" role="document">
               <div class="modal-content">
                  <div class="modal-header">
                     <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                     <h4 class="modal-title">Jobs</h4>
                  </div>
                  <div class="modal-body">
                     <table class="table">
                        <thead>
                           <tr>
                              <th>Description</th>
                              <th>Salary</th>
                              <th>Necessary Points</th>
                              <th>Earning Points</th>
                              <th>Limit</th>
                              <th>Apply</th>
                           </tr>
                        </thead>
                        <tbody id="jobsModalBody">
                           <tr>
                              <td>Cook</td>
                              <td>120</td>
                              <td>10</td>
                              <td>5</td>
                              <td>200</td>
                              <td><button onclick="alert('e')">Test</button></td>
                           </tr>
                        </tbody>
                     </table>
                  </div>
                  <div class="modal-footer">
                     <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                  </div>
               </div>
            </div>
         </div>
         <!-- PROPERTY MODAL-->
         <div class="modal fade" id="propertyModal" tabindex="-1" role="dialog">
            <div class="modal-dialog" role="document">
               <div class="modal-content">
                  <div class="modal-header">
                     <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                     <h4 class="modal-title">Buy Property</h4>
                  </div>
                  <div class="modal-body">
                     <table class="table">
                        <thead>
                           <tr>
                              <th>Name</th>
                              <th>Description</th>
                              <th>Limit</th>
                              <th>Price</th>
                              <th>Buy</th>
                           </tr>
                        </thead>
                        <tbody id="propertyModalBody"></tbody>
                     </table>
                  </div>
                  <div class="modal-footer">
                     <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                  </div>
               </div>
            </div>
         </div>
         <!-- SELL PROPERTY MODAL-->
         <div class="modal fade" id="sellPropertyModal" tabindex="-1" role="dialog">
            <div class="modal-dialog" role="document">
               <div class="modal-content">
                  <div class="modal-header">
                     <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                     <h4 class="modal-title" id="sell-property-title">Sell Property</h4>
                  </div>
                  <div class="modal-body">
                     <form id="sellPropertyForm">
                        <div class="form-group">
                           <label for="priceInput">Price</label>
                           <input type="number" class="form-control" id="priceInput">
                           <input type="hidden" id="hiddenPropertyId" value="" />
                        </div>
                        <button type="submit" class="btn btn-danger">Sell</button>
                     </form>
                  </div>
                  <div class="modal-footer">
                     <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                  </div>
               </div>
            </div>
         </div>
          
          <!-- BUY DRUG MODAL-->
         <div class="modal fade" id="buyDrugModal" tabindex="-1" role="dialog">
            <div class="modal-dialog" role="document">
               <div class="modal-content">
                  <div class="modal-header">
                     <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                     <h4 class="modal-title">Buy drugs</h4>
                  </div>
                  <div class="modal-body" style="text-align: center">
                     <form id="buyDrugsForm">
                        <div class="form-group">
                           <input type="hidden" id="sellerPseudo" value="" />
                        </div>
                        <button type="submit" class="btn btn-danger">Buy drugs !</button>
                     </form>
                  </div>
                  <div class="modal-footer">
                     <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                  </div>
               </div>
            </div>
         </div>
      </div>
      <div class="container" id="box-message-container" style="display: none; height:0px">
         <%Response.WriteFile("includes/message-box.html"); %>
      </div>
      <div class="container" id="box-info-container" style="display: none;height:0px">
         <%Response.WriteFile("includes/info-box.html"); %>
      </div>
      <nav class="navbar navbar-trans navbar-fixed-bottom" role="navigation" style="position: relative; display:none" id="bottom_navbar">
         <div class="container">
            <div class="navbar-header">
               <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#navbar-collapsible-bottom">
               <span class="sr-only">Toggle navigation</span>
               <span class="icon-bar"></span>
               <span class="icon-bar"></span>
               <span class="icon-bar"></span>
               </button>
            </div>
            <div class="navbar-collapse collapse" id="navbar-collapsible-bottom">
               <ul class="nav navbar-nav navbar-left">
                  <li><a href="#chat" id="chat_btn"><span class="glyphicon glyphicon-comment" aria-hidden="true"></span> Public chat</a></li>
                  <li><a href="#register" id="my_infos"><span class="glyphicon glyphicon-home" aria-hidden="true"></span> My Infos</a></li>
               </ul>
            </div>
         </div>
      </nav>
   </body>
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
   <script src="Scripts/Game/SoundManager.js"></script>
   <script src="Scripts/Game/Torch.js"></script>
</html>