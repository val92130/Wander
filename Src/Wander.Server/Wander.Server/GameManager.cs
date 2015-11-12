﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Web;
using Microsoft.AspNet.SignalR;
using Wander.Server.Hubs;
using Wander.Server.Model;
using Wander.Server.Services;

namespace Wander.Server
{
    public class GameManager
    {
        DateTime _time;
        Timer _payTimer = new Timer();
        Timer _alertTimer = new Timer();
        IHubContext context;
        int _intervalMinutes = 15;
        bool _isDay;
        Timer _updateTimer = new Timer();
        public static int DefaultUnemployedEarningPoints = 2;
        public GameManager()
        {
            context = GlobalHost.ConnectionManager.GetHubContext<GameHub>();
            _time = DateTime.Now;
            _payTimer.Interval = 1000 * 60 * _intervalMinutes;
            _payTimer.Elapsed += DeliverPayEvent;
            _payTimer.Start();

            _alertTimer.Interval = 1000 * 60 * 2; // We remain the player every two minutes
            _alertTimer.Elapsed += Alert;

            _updateTimer.Interval = 2000;
            _updateTimer.Elapsed += Update;
        }

        private void Update(object sender, ElapsedEventArgs e)
        {
            DateTime now = DateTime.Now;
            _isDay = (now.Hour < 18 && now.Hour >= 8);
        }

        public bool IsDay
        {
            get
            {
                return _isDay;
            }
        }

        public void Start()
        {
            
            _payTimer.Start();
            _alertTimer.Start();
            _updateTimer.Start();
        }

        private void Alert(object sender, ElapsedEventArgs e)
        {
            List<ServerPlayerModel> connectedPlayers = ServiceProvider.GetPlayerService().GetAllPlayersServer();
            TimeSpan interval = DateTime.Now - _time;
            int remainingMinutes = _intervalMinutes - interval.Minutes;
            if (remainingMinutes == 0) return;
            for (int i = 0; i < connectedPlayers.Count; i++)
            {
                context.Clients.Client(connectedPlayers[i].SignalRId)
                    .notify(Helper.CreateNotificationMessage("Next pay coming in :  " + remainingMinutes + " minutes", EMessageType.info));
            }
        }

        private void DeliverPayEvent(object sender, ElapsedEventArgs e)
        {
            _time = DateTime.Now;
            List<ServerPlayerModel> connectedPlayers = ServiceProvider.GetPlayerService().GetAllPlayersServer();

            for (int i = 0; i < connectedPlayers.Count; i++)
            {
                context.Clients.Client(connectedPlayers[i].SignalRId)
                    .notify(Helper.CreateNotificationMessage("Pay received ! ", EMessageType.success));
                ServiceProvider.GetUserService().DeliverPay(connectedPlayers[i]);
                ServiceProvider.GetUserService().DeliverPoints(connectedPlayers[i]);
            }
        }
    }
}