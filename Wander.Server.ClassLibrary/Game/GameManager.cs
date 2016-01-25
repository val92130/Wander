using System;
using System.Diagnostics;
using System.Threading;
using System.Timers;
using Microsoft.AspNet.SignalR;
using Wander.Server.ClassLibrary.Hubs;
using Wander.Server.ClassLibrary.Services;
using Timer = System.Timers.Timer;

namespace Wander.Server.ClassLibrary
{
    public class GameManager
    {
        public static int DefaultUnemployedEarningPoints = 2;
        private readonly Timer _randomRainTimer = new Timer();
        private readonly Timer _updateTimer = new Timer();
        private readonly IHubContext context;


        public GameManager()
        {
            context = GlobalHost.ConnectionManager.GetHubContext<GameHub>();

            _updateTimer.Interval = 2000;
            _updateTimer.Elapsed += Update;

            _randomRainTimer.Interval = 2000;
            _randomRainTimer.Elapsed += RainEvent;

            var tickThread = new Thread(() =>
            {
                var t = new Timer();
                t.Interval = 200;
                t.Elapsed += (sender, e) => { ServiceProvider.GetHookService().GetHooks().ForEach(x => x.OnTick()); };
                t.Start();
            });

            tickThread.Start();
        }

        public bool IsDay { get; private set; }

        public bool IsRaining { get; private set; }

        private void RainEvent(object sender, ElapsedEventArgs e)
        {
            ToggleRain();
        }

        public void ToggleRain()
        {
            var connectedPlayers = ServiceProvider.GetPlayerService().GetAllPlayersServer();
            var r = new Random();

            var nextRain = 0;
            if (IsRaining)
            {
                nextRain = r.Next(2*60*1000, 10*60*1000);
            }
            else
            {
                nextRain = r.Next(30*1000, 2*60*1000);
            }
            var next = DateTime.Now.AddMilliseconds(nextRain);
            IsRaining = !IsRaining;
            Debug.Print((IsRaining
                ? "Its raining ! Stopping rain at :  " + next
                : "Its not raining, next rain at : " + next));

            for (var i = 0; i < connectedPlayers.Count; i++)
            {
                context.Clients.Client(connectedPlayers[i].SignalRId).setRain(IsRaining);
            }

            _randomRainTimer.Interval = nextRain;
        }

        public void ForceStartRain(int seconds)
        {
            if (seconds <= 0) return;
            Debug.Print("Forcing rain for : " + seconds + " seconds");
            var connectedPlayers = ServiceProvider.GetPlayerService().GetAllPlayersServer();
            var milli = seconds*1000;
            IsRaining = true;
            for (var i = 0; i < connectedPlayers.Count; i++)
            {
                context.Clients.Client(connectedPlayers[i].SignalRId).setRain(IsRaining);
            }
            _randomRainTimer.Stop();
            _randomRainTimer.Interval = milli;
            _randomRainTimer.Start();
        }

        public void ForceStopRain()
        {
            Debug.Print("Forcing rain to stop");
            if (IsRaining) ToggleRain();
        }

        public void ForceNight(int seconds)
        {
            if (seconds <= 0) return;
            Debug.Print("Forcing night for : " + seconds + " seconds");
            IsDay = false;
            _updateTimer.Stop();
            _updateTimer.Interval = seconds*1000;
            _updateTimer.Start();
        }

        public void ForceDay(int seconds)
        {
            if (seconds <= 0) return;
            Debug.Print("Forcing day for : " + seconds + " seconds");
            IsDay = true;
            _updateTimer.Stop();
            _updateTimer.Interval = seconds*1000;
            _updateTimer.Start();
        }


        public void Start()
        {
            _updateTimer.Start();
            _randomRainTimer.Start();
        }


        private void Update(object sender, ElapsedEventArgs e)
        {
            var now = DateTime.Now;
            IsDay = (now.Hour < 18 && now.Hour >= 8);
        }
    }
}