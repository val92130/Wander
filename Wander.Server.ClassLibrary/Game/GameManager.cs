using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Timers;
using Microsoft.AspNet.SignalR;
using Wander.Server.ClassLibrary.Hubs;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Players;
using Wander.Server.ClassLibrary.Services;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace Wander.Server.ClassLibrary
{
    public class GameManager
    {
        IHubContext context;
        bool _isDay;
        System.Timers.Timer _updateTimer = new System.Timers.Timer();
        System.Timers.Timer _randomRainTimer = new System.Timers.Timer();
        public static int DefaultUnemployedEarningPoints = 2;
        bool _isRaining = false;
        

        public GameManager()
        {
            context = GlobalHost.ConnectionManager.GetHubContext<GameHub>();

            _updateTimer.Interval = 2000;
            _updateTimer.Elapsed += Update;

            _randomRainTimer.Interval = 2000;
            _randomRainTimer.Elapsed += RainEvent;

            var r = ServiceProvider.GetMapService();

            Thread tickThread = new Thread(() =>
            {
                System.Timers.Timer t = new System.Timers.Timer();
                t.Interval = 200;
                t.Elapsed += (sender, e) =>
                {
                    ServiceProvider.GetHookService().GetHooks().ForEach(x => x.OnTick());
                };
                t.Start();


            });

            tickThread.Start();
        }

        private void RainEvent(object sender, ElapsedEventArgs e)
        {
            ToggleRain();
        }

        public void ToggleRain()
        {
            List<ServerPlayerModel> connectedPlayers = ServiceProvider.GetPlayerService().GetAllPlayersServer();
            Random r = new Random();

            int nextRain = 0;
            if (_isRaining)
            {
                nextRain = r.Next(2 * 60 * 1000, 10 * 60 * 1000);
            }
            else
            {
                nextRain = r.Next(30 * 1000, 2 * 60 * 1000);
            }
            DateTime next = DateTime.Now.AddMilliseconds(nextRain);
            _isRaining = !_isRaining;
            Debug.Print((_isRaining
                ? "Its raining ! Stopping rain at :  " + next
                : "Its not raining, next rain at : " + next));

            for (int i = 0; i < connectedPlayers.Count; i++)
            {
                context.Clients.Client(connectedPlayers[i].SignalRId).setRain(_isRaining);
            }

            _randomRainTimer.Interval = nextRain;
        }

        public void ForceStartRain(int seconds)
        {
            if (seconds <= 0) return;
            Debug.Print("Forcing rain for : " + seconds + " seconds");
            List<ServerPlayerModel> connectedPlayers = ServiceProvider.GetPlayerService().GetAllPlayersServer();
            int milli = seconds*1000;
            _isRaining = true;
            for (int i = 0; i < connectedPlayers.Count; i++)
            {
                context.Clients.Client(connectedPlayers[i].SignalRId).setRain(_isRaining);
            }
            _randomRainTimer.Stop();
            _randomRainTimer.Interval = milli;
            _randomRainTimer.Start();
        }

        public void ForceStopRain()
        {
            Debug.Print("Forcing rain to stop");
            if (_isRaining) ToggleRain();
        }

        public void ForceNight(int seconds)
        {          
            if (seconds <= 0) return;
            Debug.Print("Forcing night for : " + seconds + " seconds");
            _isDay = false;
            _updateTimer.Stop();
            _updateTimer.Interval = seconds*1000;
            _updateTimer.Start();

        }

        public void ForceDay(int seconds)
        {
            if (seconds <= 0) return;
            Debug.Print("Forcing day for : " + seconds + " seconds");
            _isDay = true;
            _updateTimer.Stop();
            _updateTimer.Interval = seconds * 1000;
            _updateTimer.Start();
        }


        public void Start()
        {
            _updateTimer.Start();
            _randomRainTimer.Start();
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

        public bool IsRaining
        {
            get { return _isRaining; }
        }




    }
}
