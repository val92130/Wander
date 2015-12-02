using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using Microsoft.AspNet.SignalR;
using Wander.Server.ClassLibrary.Hubs;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Players;
using Wander.Server.ClassLibrary.Services;

namespace Wander.Server.ClassLibrary
{
    public class GameManager
    {
        DateTime _time;
        Timer _payTimer = new Timer();
        Timer _alertTimer = new Timer();
        Timer _taxTimer = new Timer();

        Timer _questionTimer = new Timer();
        IHubContext context;
        int _intervalMinutes = 15;
        bool _isDay;
        Timer _updateTimer = new Timer();
        Timer _randomRainTimer = new Timer();
        public static int DefaultUnemployedEarningPoints = 2;
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


            _randomRainTimer.Interval = 2000;
            _randomRainTimer.Elapsed += RainEvent;

            this._taxTimer.Interval = 1000 * 60 * 20;
            this._taxTimer.Elapsed += this.TakeTaxEvent;

            this._questionTimer.Interval = 30000;
            this._questionTimer.Elapsed += AskQuestion;
        }

        private void AskQuestion(object sender, ElapsedEventArgs e)
        {
            _time = DateTime.Now;
            List<ServerPlayerModel> connectedPlayers = ServiceProvider.GetPlayerService().GetAllPlayersServer();

            for (int i = 0; i < connectedPlayers.Count; i++)
            {
                context.Clients.Client(connectedPlayers[i].SignalRId)
                    .notify(Helper.CreateNotificationMessage("We have to test your Competence ", EMessageType.info));
               var question = ServiceProvider.GetQuestionService().GetRandomQuestion(connectedPlayers[i].SignalRId);
                if (question == null) continue;
                this.context.Clients.Client(connectedPlayers[i].SignalRId).sendQuestionToClient(new {Question = question.Question, QuestionId =question.QuestionId });
            }
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

        public void Start()
        {
            _payTimer.Start();
            _alertTimer.Start();
            _updateTimer.Start();
            _randomRainTimer.Start();
            this._taxTimer.Start();
            this._questionTimer.Start();
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
                    .notify(Helper.CreateNotificationMessage("You have to pay your TAX ! ", EMessageType.info));
                ServiceProvider.GetUserService().PayTax(connectedPlayers[i]);
            }
        }

    }
}
