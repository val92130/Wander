<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="Wander.Server.admin" %>
<!DOCTYPE html>
<html lang="en" ng-app="adminApp">
   <head>
      <meta charset="utf-8">
      <meta http-equiv="X-UA-Compatible" content="IE=edge">
      <meta name="viewport" content="width=device-width, initial-scale=1">
      <meta name="description" content="">
      <meta name="author" content="">
      <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/s/bs/jqc-1.11.3,dt-1.10.10/datatables.min.css"/>
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
      <div class="hideOverlay" style="position: absolute; bottom: 0px; width: 100%; height: 100%; background-color: #2B2B2B; z-index: 9000"></div>
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
                <br/>
                 <ul class="nav nav-pills justified">
                  <li role="presentation" class="active"><a href="#" ng-click="setPage('overview')">Overview</a></li>
                  <li role="presentation"><a href="#" ng-click="setPage('players')">Players</a></li>
                  <li role="presentation"><a href="#" ng-click="setPage('chat')">Chat</a></li>
                  <li role="presentation"><a href="#" ng-click="setPage('weather')">Weather management</a></li>
                </ul>
                <hr/>
               <!-- /.col-lg-12 -->
            </div>
             
             <div id="content">
                 <%
                     Response.WriteFile("/Content/Admin/pages/chat.html");
                     Response.WriteFile("/Content/Admin/pages/overview.html");
                     Response.WriteFile("/Content/Admin/pages/players.html");
                     Response.WriteFile("/Content/Admin/pages/weather.html");
                 %>
             </div>

             <!-- Overview -->

            <!-- Players -->
           
         </div>
         <!-- /#page-wrapper -->
      </div>
      <div id="loginModal" class="modal fade" role="dialog" style="z-index: 9999">
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
      <script type="text/javascript" src="https://cdn.datatables.net/s/bs/jqc-1.11.3,dt-1.10.10/datatables.min.js"></script>
      <script src="Scripts/jquery.signalR-2.2.0.min.js"></script>
      <script src="Scripts/notify.min.js"></script>
      <script src="Scripts/bootstrap.min.js"></script>
      <script src="Content/Admin/js/angular.min.js"></script>
      <script src="Content/Admin/js/app.js"></script>
   </body>
</html>