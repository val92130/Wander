using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Players;
using Wander.Server.ClassLibrary.Services.Interfaces;

namespace Wander.Server.ClassLibrary.Services
{
    public class AdminService : IAdminService
    {
        private readonly List<AdminModel> _admins = new List<AdminModel>();

        public bool ConnectAdmin(string pseudo, string password, string connectionId)
        {
            var value = false;
            var id = -1;
            if (pseudo == null || password == null)
                throw new ArgumentException("parameter user is null");

            if (_admins.FirstOrDefault(x => x.ConnectionId == connectionId) != null) return false;
            var candidate = _admins.FirstOrDefault(x => x.Pseudo == pseudo);
            if (candidate != null) DisconnectAdmin(candidate.ConnectionId);

            using (var conn = SqlConnectionService.GetConnection())
            {
                var query =
                    "SELECT UserId from dbo.Users WHERE UserLogin = @Login AND UserPassword = @Password AND Admin = 1";
                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@Login", pseudo);
                    cmd.Parameters.AddWithValue("@Password", Helper.Sha1Encode(password));

                    var data = cmd.ExecuteScalar();
                    if (data != null)
                    {
                        id = (int) data;
                        value = true;
                    }

                    conn.Close();
                }
            }

            if (value && id != -1)
            {
                _admins.Add(new AdminModel {ConnectionId = connectionId, Id = id, Pseudo = pseudo});
            }

            return value;
        }

        public bool DisconnectAdmin(string connectionId)
        {
            var candidate = _admins.FirstOrDefault(x => x.ConnectionId == connectionId);
            if (candidate != null)
            {
                _admins.Remove(candidate);
                return true;
            }
            return false;
        }

        public bool IsAdmin(int userId)
        {
            using (var conn = SqlConnectionService.GetConnection())
            {
                var query = "SELECT Admin from dbo.Users WHERE UserId = @id AND Admin = 1";
                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@id", userId);

                    var data = cmd.ExecuteReader();
                    var value = data.HasRows;
                    conn.Close();
                    return value;
                }
            }
        }

        public bool IsAdminConnected(string connectionId)
        {
            return _admins.FirstOrDefault(x => x.ConnectionId == connectionId) != null;
        }

        public int GetPlayersTotalCount()
        {
            using (var conn = SqlConnectionService.GetConnection())
            {
                var query = "SELECT Count(*) from dbo.Users";
                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    var data = cmd.ExecuteScalar();
                    conn.Close();
                    if (data != null)
                    {
                        return (int) data;
                    }
                    return -1;
                }
            }
        }

        public int GetOnlinePlayersCount()
        {
            return ServiceProvider.GetPlayerService().GetAllPlayersServer().Count;
        }

        public bool BanPlayer(int playerId)
        {
            using (var conn = SqlConnectionService.GetConnection())
            {
                var query = "UPDATE dbo.Users SET Banned = 1  WHERE UserId = @userId";
                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@userId", playerId);


                    var lines = cmd.ExecuteNonQuery();
                    conn.Close();
                    return lines != 0;
                }
            }
        }

        public bool UnBanPlayer(int playerId)
        {
            using (var conn = SqlConnectionService.GetConnection())
            {
                var query = "UPDATE dbo.Users SET Banned = 0  WHERE UserId = @userId";
                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@userId", playerId);


                    var lines = cmd.ExecuteNonQuery();
                    conn.Close();
                    return lines != 0;
                }
            }
        }

        public int GetMessagesCount()
        {
            return ServiceProvider.GetMessageService().GetAllMessages().Count;
        }

        public List<ChatMessageModel> GetAllMessages()
        {
            return ServiceProvider.GetMessageService().GetAllMessages();
        }

        public int GetBoughtsHouseCount()
        {
            var count = -1;
            using (var conn = SqlConnectionService.GetConnection())
            {
                var query = "SELECT COUNT(*) from dbo.UserProperties";
                using (var cmd = new SqlCommand(query, conn))
                {
                    var Properties = new List<ServerPropertyModel>();
                    conn.Open();
                    var data = cmd.ExecuteScalar();
                    if (data != null)
                    {
                        count = (int) data;
                    }
                    conn.Close();

                    return count;
                }
            }
        }

        public List<AdminPlayerModel> GetAllPlayers()
        {
            var playerList = new List<AdminPlayerModel>();
            var connectedPlayers = ServiceProvider.GetPlayerService().GetAllPlayersServer();
            for (var i = 0; i < connectedPlayers.Count; i++)
            {
                var mdl = GetPlayerInfo(connectedPlayers[i].SignalRId);
                playerList.Add(mdl);
            }
            return playerList;
        }

        public List<AdminUserModel> GetAllUsers()
        {
            var usersId = ServiceProvider.GetUserService().GetAllUsersId();
            var users = new List<AdminUserModel>();
            for (var i = 0; i < usersId.Count; i++)
            {
                var id = usersId[i];
                var u = new AdminUserModel();
                var model = ServiceProvider.GetUserService().GetAllUserInfos(id);
                u.UserId = id;
                u.Account = model.Account;
                u.Email = model.Email;
                u.IsBanned = ServiceProvider.GetUserService().IsBanned(id);
                u.Job = model.Job;
                u.Points = model.Points;
                u.Position = ServiceProvider.GetUserService().GetLastPosition(id);
                u.Properties = model.Properties;
                u.Pseudo = model.UserName;
                u.Sex = model.Sex;
                users.Add(u);
            }
            return users;
        }

        public AdminPlayerModel GetPlayerInfo(int userId)
        {
            var mdl = new AdminPlayerModel();
            var candidate = ServiceProvider.GetPlayerService().GetPlayer(userId);
            return GetPlayerInfo(candidate.SignalRId);
        }

        public AdminPlayerModel GetPlayerInfo(string connectionId)
        {
            var mdl = new AdminPlayerModel();

            var candidate = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (candidate == null) return null;
            var userInfo = ServiceProvider.GetUserService().GetAllUserInfos(connectionId);
            mdl.Account = userInfo.Account;
            mdl.Email = userInfo.Email;
            mdl.Job = userInfo.Job;
            mdl.Points = userInfo.Points;
            mdl.Sex = userInfo.Sex;
            mdl.Properties = userInfo.Properties;
            mdl.MapId = candidate.MapId;
            mdl.Direction = candidate.Direction;
            mdl.Position = candidate.Position;
            mdl.Pseudo = candidate.Pseudo;
            mdl.UserId = candidate.UserId;
            mdl.SignalRId = candidate.SignalRId;
            mdl.IsBanned = ServiceProvider.GetUserService().IsBanned(mdl.SignalRId);
            return mdl;
        }

        public bool SetDay(bool value)
        {
            throw new NotImplementedException();
        }

        public bool SetRain(bool value)
        {
            throw new NotImplementedException();
        }

        public List<AdminModel> GetAllAdmins()
        {
            return _admins;
        }
    }
}