using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Wander.Server.Model;
using Wander.Server.Model.Players;
using Wander.Server.Services;

namespace Wander.StressTest.App
{
    internal class Simulation
    {
        private int _connectionCount;
        private Dictionary<HubConnection, IHubProxy> _connections = new Dictionary<HubConnection, IHubProxy>();
        private string _hubName, _host;
        private List<UserModel> _users;
        Timer _moveTimer, _chatTimer;
        public Simulation(string host, string hubName, int connectionCount)
        {
            if (host == null) throw new ArgumentNullException("host");
            if (hubName == null) throw new ArgumentNullException("hubName");
            
            _users = new List<UserModel>();
            _host = host;
            _hubName = hubName;

            Console.WriteLine("Initializing simulation of " + connectionCount + " players");
            _connectionCount = connectionCount;

            _moveTimer = new Timer();
            _moveTimer.Interval = 1000;
            Console.WriteLine("Each player's position will be updated every " + _moveTimer.Interval + " ms");
            _moveTimer.Elapsed += MovePlayers;

            _chatTimer = new Timer();
            _chatTimer.Interval = 10000;
            Console.WriteLine("Each player will send a random message every " + _chatTimer.Interval + " ms");
            _chatTimer.Elapsed += SendMessages;
        }

        private async void SendMessages(object sender, ElapsedEventArgs e)
        {
            int j = 0;
            Random r = new Random();
            foreach (KeyValuePair<HubConnection, IHubProxy> entry in _connections)
            {
                await entry.Value.Invoke("SendPublicMessage", RandomString(r.Next(1,10)));
                j++;
            }
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        async private void MovePlayers(object sender, ElapsedEventArgs e)
        {
            int j = 0;
            Random r = new Random();

            foreach (KeyValuePair<HubConnection, IHubProxy> entry in _connections)
            {
                Vector2 vec = new Vector2(r.Next(0, 2000), r.Next(0, 2000));
                await entry.Value.Invoke("UpdatePosition", vec, EPlayerDirection.Down);
                Console.WriteLine("Moving player : " + j + " to position : " + vec);
                j++;
            }
        }

        public async void Start()
        {

            for (int i = 0; i < _connectionCount; i++)
            {
                var hubCo = new HubConnection(_host);

                var hubProx = hubCo.CreateHubProxy(_hubName);

                _connections.Add(hubCo, hubProx);
                try
                {
                    await hubCo.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Couldnt connect to host, exiting : Debug - " + e.Message);
                    return;
                }
                
                Console.WriteLine("Connected to hub : " + i);
            }

            GenerateAllUsers();
            int j = 0;
            foreach (KeyValuePair<HubConnection, IHubProxy> entry in _connections)
            {
                await entry.Value.Invoke("DeleteUser");
                await entry.Value.Invoke("RegisterUser", _users[j]);
                Console.WriteLine("Registered user : " + j);
                await entry.Value.Invoke("Connect", _users[j]);
                Console.WriteLine("Connected user : " + j);
                j++;
            }

            Console.WriteLine("Moving players...");
            _moveTimer.Start();
            _chatTimer.Start();

        }

        private void GenerateAllUsers()
        {
            for (int i = 0; i < _connectionCount; i++)
            {
                UserModel u = new UserModel()
                {
                    Email = "testUserEmail" + i + "@live.fr",
                    Login = "testUserLogin" + i,
                    Password = "testUserPassword" + i,
                    Sex = 1
                };
                _users.Add(u);
            }
        }


        public async void DeleteAllUsers()
        {
            int j = 0;
            foreach (KeyValuePair<HubConnection, IHubProxy> entry in _connections)
            {
                await entry.Value.Invoke("DeleteUser");
                Console.WriteLine("Deleted user : " + j);
                j++;
            }
        }
    }
}
