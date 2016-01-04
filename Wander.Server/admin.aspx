<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="Wander.Server.admin" %>
<!DOCTYPE html>
<html lang="en" ng-app="adminApp">
   <head>
      <meta charset="utf-8">
      <meta http-equiv="X-UA-Compatible" content="IE=edge">
      <meta name="viewport" content="width=device-width, initial-scale=1">
      <meta name="description" content="">
      <meta name="author" content="">
      <link href='https://fonts.googleapis.com/css?family=Quicksand:700,400' rel='stylesheet' type='text/css'>
      <title>Wander administration</title>

       <link href="Content/bootstrap.min.css" rel="stylesheet" />
      <!-- Custom CSS -->
      <link href="Content/Admin/css/sb-admin-2.css" rel="stylesheet">
      <link href="Content/Admin/css/style.css" rel="stylesheet" />
      <!-- Custom Fonts -->
      <link href="Content/Admin/bower_components/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css">
   </head>
   <body ng-controller="manager">
      <div id="wrapper">
         <!-- Navigation -->
         <nav class="navbar navbar-default navbar-static-top" role="navigation" style="margin-bottom: 0">
            <div class="navbar-header">
               <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
               <span class="sr-only">Toggle navigation</span>
               <span class="icon-bar"></span>
               <span class="icon-bar"></span>
               <span class="icon-bar"></span>
               </button>
               <a class="navbar-brand" href="index.html">Wander administration</a>
            </div>
            <!-- /.navbar-header -->
            <ul class="nav navbar-top-links navbar-right">
               <!-- /.dropdown -->
               <li class="dropdown">
                  <a class="dropdown-toggle" data-toggle="dropdown" href="#">
                  <i class="fa fa-user fa-fw"></i>  <i class="fa fa-caret-down"></i>
                  </a>
                  <ul class="dropdown-menu dropdown-user">
                     <li class="divider"></li>
                     <li><a href="#"><i class="fa fa-sign-out fa-fw"></i> Logout</a>
                     </li>
                  </ul>
                  <!-- /.dropdown-user -->
               </li>
               <!-- /.dropdown -->
            </ul>
            <!-- /.navbar-top-links -->
            <div class="navbar-default sidebar" role="navigation">
               <div class="sidebar-nav navbar-collapse">
                  <ul class="nav" id="side-menu">
                     <li class="sidebar-search">
                        <div class="input-group custom-search-form">
                           <input type="text" class="form-control" placeholder="Search...">
                           <span class="input-group-btn">
                           <button class="btn btn-default" type="button">
                           <i class="fa fa-search"></i>
                           </button>
                           </span>
                        </div>
                        <!-- /input-group -->
                     </li>
                     <li>
                        <a href="#"><i class="fa fa-dashboard fa-fw"></i> Dashboard</a>
                     </li>
                  </ul>
               </div>
               <!-- /.sidebar-collapse -->
            </div>
            <!-- /.navbar-static-side -->
         </nav>
         <div id="page-wrapper">
            <div class="row">
               <div class="col-lg-12">
                  <h1 class="page-header">Dashboard</h1>
               </div>
               <!-- /.col-lg-12 -->
            </div>
            <!-- /.row -->
            <div class="row">
               <div class="col-lg-3 col-md-6">
                  <div class="panel panel-primary">
                     <div class="panel-heading">
                        <div class="row">
                           <div class="col-xs-3">
                              <i class="fa fa-users fa-5x"></i>
                           </div>
                           <div class="col-xs-9 text-right">
                              <div class="huge" ng-bind="online_players">26</div>
                              <div>Players online</div>
                           </div>
                        </div>
                     </div>
                  </div>
               </div>
               <div class="col-lg-3 col-md-6">
                  <div class="panel panel-green">
                     <div class="panel-heading">
                        <div class="row">
                           <div class="col-xs-3">
                              <i class="fa fa-user fa-5x"></i>
                           </div>
                           <div class="col-xs-9 text-right">
                              <div class="huge" ng-bind="registered_players">12</div>
                              <div>Players registered</div>
                           </div>
                        </div>
                     </div>
                  </div>
               </div>
               <div class="col-lg-3 col-md-6">
                  <div class="panel panel-yellow">
                     <div class="panel-heading">
                        <div class="row">
                           <div class="col-xs-3">
                              <i class="fa fa-shopping-cart fa-5x"></i>
                           </div>
                           <div class="col-xs-9 text-right">
                              <div class="huge" ng-bind="house_boughts">124</div>
                              <div>House bought</div>
                           </div>
                        </div>
                     </div>
                  </div>
               </div>
               <div class="col-lg-3 col-md-6">
                  <div class="panel panel-red">
                     <div class="panel-heading">
                        <div class="row">
                           <div class="col-xs-3">
                              <i class="fa fa-envelope-o fa-5x"></i>
                           </div>
                           <div class="col-xs-9 text-right">
                              <div class="huge" ng-bind="sent_messages">13</div>
                              <div>Messages sent</div>
                           </div>
                        </div>
                     </div>
                  </div>
               </div>
            </div>
             
             <hr />
             <h2>Players </h2>
             <form class="form-inline" novalidate>
              <div class="form-group">
                <select class="form-control" id="selectPlayers">
                  <option>1</option>
                </select>
              </div>
              
              <button type="button" class="btn btn-success">View infos</button>
              <button type="button" class="btn btn-danger">Ban</button>
              <button type="button" class="btn btn-info">Unban</button>
            </form>
             <hr />
            <!-- /.row -->
            <div class="row">
               <!-- /.col-lg-8 -->
               <div class="col-lg-4">
                  <!-- /.panel -->
                  <div class="chat-panel panel panel-default">
                     <div class="panel-heading">
                        <i class="fa fa-comments fa-fw"></i>
                        Players chat
                     </div>
                     <!-- /.panel-heading -->
                     <div class="panel-body">
                        <ul class="chat" id="chatbox">

                        </ul>
                     </div>
                     <!-- /.panel-body -->
                     <div class="panel-footer">
                        <div class="input-group">
                           <input id="btn-input" type="text" ng-bind="broadcast_message" class="form-control input-sm" placeholder="Type your broadcast message here..." />
                           <span class="input-group-btn">
                           <button class="btn btn-warning btn-sm" id="btn-chat" ng-click="broadcastMessage()">
                           Send
                           </button>
                           </span>
                        </div>
                     </div>
                     <!-- /.panel-footer -->
                  </div>
                  <!-- /.panel .chat-panel -->
               </div>
               <!-- /.col-lg-4 -->
            </div>
            <!-- /.row -->
         </div>
         <!-- /#page-wrapper -->
      </div>
      <div id="loginModal" class="modal fade" role="dialog">
         <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
               <div class="modal-header">
                  <button type="button" class="close" data-dismiss="modal">&times;</button>
                  <h4 class="modal-title">Login</h4>
               </div>
               <div class="modal-body">
                  <form novalidate>
                     <div class="form-group">
                        <label for="pseudo">Pseudo</label>
                        <input type="text" class="form-control" ng-model="user.pseudo" id="pseudo" placeholder="Pseudo">
                     </div>
                     <div class="form-group">
                        <label for="password">Password</label>
                        <input type="password" class="form-control" ng-model="user.password" id="password" placeholder="Password">
                     </div>
                     <button type="submit" ng-click="login(user)" class="btn btn-success">Login</button>
                  </form>
               </div>
            </div>
         </div>
      </div>
      <!-- /#wrapper -->
      <!-- jQuery -->
      <script src="Scripts/jquery-1.9.1.min.js"></script>
      <script src="Scripts/jquery.signalR-2.2.0.min.js"></script>
      <script src="Scripts/notify.min.js"></script>
      <script src="Scripts/bootstrap.min.js"></script>
      <script src="Content/Admin/js/angular.min.js"></script>
      <script src="Content/Admin/js/app.js"></script>
   </body>
</html>