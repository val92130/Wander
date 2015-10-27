using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wander.Server.Model;
using System.Linq;

namespace Wander.Server.Services
{
   

    public class PlayerService : IPlayerService
    {
        static List<PlayerModel> Players = new List<PlayerModel>(); 

        /// <summary>
        /// Add a new player to the Players list if it doesnt already exists
        /// </summary>
        /// <param name="signalRId"></param>
        /// <param name="userId"></param>
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

        /// <summary>
        /// Remove a specified player using the SignalRId if it exists
        /// </summary>
        /// <param name="SignalRId">User's Connection Id</param>
        public void RemovePlayer(string SignalRId)
        {
            lock (Players)
            {
                PlayerModel p = Players.FirstOrDefault(x => x.SignalRId == SignalRId);
                if (p != null)
                {
                    Players.Remove(p);
                }
            }
        }

        /// <summary>
        /// Gets a player from a specified signalRId if it exists
        /// </summary>
        /// <param name="signalRId"></param>
        /// <returns></returns>
        public PlayerModel GetPlayer(string signalRId)
        {
            lock (Players)
            {
                return Players.FirstOrDefault(x => x.SignalRId == signalRId);   
            }           
        }

    }
}