using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wander.Server.Model;
using System.Linq;

namespace Wander.Server.Services
{
   

    public class PlayerService
    {
        static List<PlayerModel> Players = new List<PlayerModel>(); 
        public void AddPlayer(string signalRId, int userId)
        {
            lock (Players)
            {
                PlayerModel p = Players.FirstOrDefault(x => x.SignalRId == signalRId);
                if (p == null)
                {
                    Players.Add(new PlayerModel() {SignalRId = signalRId, UserId = userId});
                }
            }
        }
        public void RemovePlayer(string SignalRId)
        {
            PlayerModel p = Players.FirstOrDefault(x => x.SignalRId == SignalRId);
            if (p != null)
            {
                Players.Remove(p);
            }
        }

        public PlayerModel GetPlayer(string signalRId)
        {
            lock (Players)
            {
                return Players.FirstOrDefault(x => x.SignalRId == signalRId);   
            }           
        }

    }
}