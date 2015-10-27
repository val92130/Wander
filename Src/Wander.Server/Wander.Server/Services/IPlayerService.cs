using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wander.Server.Model;

namespace Wander.Server.Services
{
    public interface IPlayerService
    {
        void AddPlayer(string ConnectionId, int UserId);
        void RemovePlayer(string ConnectionId);
        PlayerModel GetPlayer(string ConnectionId);
    }
}