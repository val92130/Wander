using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Timers;
using Microsoft.AspNet.SignalR;
using Wander.Server.Hubs;
using Wander.Server.Lib.Model;
using Wander.Server.Model;
using Wander.Server.Services;

namespace Wander.Server
{
    public class GameManager
    {
        DateTime _time;
        Timer _payTimer = new Timer();
        Timer _alertTimer = new Timer();
        Timer _taxTimer = new Timer();
        IHubContext context;
        int _intervalMinutes = 15;
        bool _isDay;
        Timer _updateTimer = new Timer();
        Timer _randomMoneyDelivery = new Timer();
        Timer _randomRainTimer = new Timer();
        public static int DefaultUnemployedEarningPoints = 2;
        List<MoneyBag> _moneyBags;
        bool _isRaining = false;
        public GameManager()
        {
            context = GlobalHost.ConnectionManager.GetHubContext<GameHub>();
            _time = DateTime.Now;
            _payTimer.Interval = 1000 * 60 * _intervalMinutes;
            _payTimer.Elapsed += DeliverPayEvent;

            _alertTimer.Interval = 1000 * 60 * 2; // We remain the player every two minutes
            _alertTimer.Elapsed += Alert;

            _updateTimer.Interval = 2000;
            _updateTimer.Elapsed += Update;

            _randomMoneyDelivery.Interval = 1000 * 60 * 20;
            _randomMoneyDelivery.Elapsed += DeliverMoneyEvent;

            _randomRainTimer.Interval = 2000;
            _randomRainTimer.Elapsed += RainEvent;

            this._taxTimer.Interval = 1000 * 60 * 20;
            this._taxTimer.Elapsed += this.TakeTaxEvent;

            _moneyBags = new List<MoneyBag>();

        }

        private void RainEvent(object sender, ElapsedEventArgs e)
        {
            List<ServerPlayerModel> connectedPlayers = ServiceProvider.GetPlayerService().GetAllPlayersServer();
            Random r = new Random();

            int nextRain = 0;
            if (_isRaining)
            {
                nextRain = r.Next(2*60*1000, 10 * 60 * 1000);
            }
            else
            {
                nextRain = r.Next (30 * 1000, 2 * 60 * 1000);
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

        public void Start()
        {

            _payTimer.Start();
            _alertTimer.Start();
            _updateTimer.Start();
            _randomMoneyDelivery.Start();
            _randomRainTimer.Start();
            this._taxTimer.Start();
        }

        private void DeliverMoneyEvent(object sender, ElapsedEventArgs e)
        {
            List<ServerPlayerModel> connectedPlayers = ServiceProvider.GetPlayerService().GetAllPlayersServer();
            Random r = new Random();
            int mapSize = 250*16;

            int randomX = r.Next(0, mapSize);
            int randomY = r.Next(0, mapSize);
            int ammount = r.Next(0, 200);

            MoneyBag moneyBag = new MoneyBag(new Vector2(randomX, randomY), ammount );
            for (int i = 0; i < connectedPlayers.Count; i++)
            {
                context.Clients.Client(connectedPlayers[i].SignalRId)
                    .notify(Helper.CreateNotificationMessage("Delivering free money ! Find it on the map ", EMessageType.info));
                context.Clients.Client(connectedPlayers[i].SignalRId)
                    .addMoneyBag(moneyBag);

                _moneyBags.Add(moneyBag);
            }
        }

        public void RemoveMoneyBag(int id)
        {
            _moneyBags.RemoveAll(x => x.Id == id);
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
        private void TakeTaxEvent(object sender, ElapsedEventArgs e)
        {
            _time = DateTime.Now;
            List<ServerPlayerModel> connectedPlayers = ServiceProvider.GetPlayerService().GetAllPlayersServer();

            for (int i = 0; i < connectedPlayers.Count; i++)
            {
                context.Clients.Client(connectedPlayers[i].SignalRId)
                    .notify(Helper.CreateNotificationMessage("You have to pay your TAX ! ", EMessageType.success));
                ServiceProvider.GetUserService().GetTax(connectedPlayers[i]);
                ServiceProvider.GetUserService().GetTax(connectedPlayers[i]);
            }
        }
        public List<MoneyBag> MoneyBags
        {
            get
            {
                return _moneyBags;
            }
        }

        public void BeTheMaster(string connectionId)
        {
            
        }


    }
}