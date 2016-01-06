using System;
using System.Collections;
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
        private List<AdminModel> _admins = new List<AdminModel>();

        public bool ConnectAdmin(string pseudo, string password, string connectionId)
        {
            bool value = false;
            int id = -1;
            if (pseudo == null || password == null)
                throw new ArgumentException("parameter user is null");

            if (_admins.FirstOrDefault(x => x.ConnectionId == connectionId) != null) return false;
            var candidate = _admins.FirstOrDefault(x => x.Pseudo == pseudo);
            if (candidate != null) DisconnectAdmin(candidate.ConnectionId);

            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "SELECT UserId from dbo.Users WHERE UserLogin = @Login AND UserPassword = @Password AND Admin = 1";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@Login", pseudo);
                    cmd.Parameters.AddWithValue("@Password", Helper.Sha1Encode(password));

                    var data = cmd.ExecuteScalar();
                    if (data != null)
                    {
                        id = (int)data;
                        value = true;
                    }                
                    
                    conn.Close();
                   
                }
            }

            if (value && id != -1)
            {
                _admins.Add(new AdminModel() {ConnectionId = connectionId, Id = id, Pseudo = pseudo});
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
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "SELECT Admin from dbo.Users WHERE UserId = @id AND Admin = 1";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@id", userId);

                    var data = cmd.ExecuteReader();
                    conn.Close();
                    return data.HasRows;

                }
            }
        }

        public bool IsAdminConnected(string connectionId)
        {
            return _admins.FirstOrDefault(x => x.ConnectionId == connectionId) != null;
        }

        public int GetPlayersTotalCount()
        {
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "SELECT Count(*) from dbo.Users";
                using (SqlCommand cmd = new SqlCommand(query, conn))
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
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "UPDATE dbo.Users SET Banned = 1  WHERE UserId = @userId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@userId", playerId);


                    int lines = cmd.ExecuteNonQuery();
                    conn.Close();
                    return lines != 0;
                }
            }
        }

        public bool UnBanPlayer(int playerId)
        {
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "UPDATE dbo.Users SET Banned = 0  WHERE UserId = @userId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@userId", playerId);


                    int lines = cmd.ExecuteNonQuery();
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
            int count = -1;
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = string.Format("SELECT COUNT(*) from dbo.UserProperties");
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    List<ServerPropertyModel> Properties = new List<ServerPropertyModel>();
                    conn.Open();
                    var data = cmd.ExecuteScalar();
                    if (data != null)
                    {
                        count = (int)data;
                    }
                    conn.Close();

                    return count;
                }
            }
        }

        public List<AdminPlayerModel> GetAllPlayers()
        {
            List<AdminPlayerModel> playerList = new List<AdminPlayerModel>();
            var connectedPlayers = ServiceProvider.GetPlayerService().GetAllPlayersServer();
            for (int i = 0; i < connectedPlayers.Count; i++)
            {
                AdminPlayerModel mdl = GetPlayerInfo(connectedPlayers[i].SignalRId);
                playerList.Add(mdl);
            }
            return playerList;
        }

        public List<AdminUserModel> GetAllUsers()
        {
            var usersId = ServiceProvider.GetUserService().GetAllUsersId();
            List<AdminUserModel> users = new List<AdminUserModel>();
            for (int i = 0; i < usersId.Count; i++)
            {
                int id = usersId[i];
                AdminUserModel u = new AdminUserModel();
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
            AdminPlayerModel mdl = new AdminPlayerModel();           
            var candidate = ServiceProvider.GetPlayerService().GetPlayer(userId);            
            return GetPlayerInfo(candidate.SignalRId);
        }

        public AdminPlayerModel GetPlayerInfo(string connectionId)
        {
            AdminPlayerModel mdl = new AdminPlayerModel();
            
            var candidate = ServiceProvider.GetPlayerService().GetPlayer(connectionId);
            if (candidate == null) return null;
            var userInfo = ServiceProvider.GetUserService().GetAllUserInfos(connectionId);
            mdl.Account = userInfo.Account;
            mdl.Email = userInfo.Email;
            mdl.Job = userInfo.Job;
            mdl.Points = userInfo.Points;
            mdl.Sex = userInfo.Sex;
            mdl.Properties = userInfo.Properties;
            mdl.HouseId = candidate.HouseId;
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
