using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Wander.Server.ClassLibrary.Model;
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
            throw new NotImplementedException();
        }

        public bool UnBanPlayer(int playerId)
        {
            throw new NotImplementedException();
        }
    }
}
