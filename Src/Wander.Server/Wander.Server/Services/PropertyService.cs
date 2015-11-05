using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wander.Server.Services
{
    using System.Data.SqlClient;

    using Wander.Server.Model;
    using Wander.Server.Model.Players;

    public class PropertyService: IPropertyService
    {

       public  List<ServerPropertyModel> GetProperties()
        {

            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = string.Format("SELECT * from dbo.ListProperties");
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    List<ServerPropertyModel> Properties  = new List<ServerPropertyModel>();
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ServerPropertyModel propertyModel = new ServerPropertyModel();
                        propertyModel.PropertyId = Convert.ToInt32(reader["ListPropertyId"]);
                        propertyModel.PropertyName = (reader["NameProperty"]).ToString(); 
                        propertyModel.PropertyDescription = (reader["PropertyDescription"]).ToString();
                        propertyModel.Threshold = Convert.ToInt32(reader["Threshold"]);
                        propertyModel.Price = Convert.ToInt32(reader["Price"]);
                        Properties.Add(propertyModel);
                    }

                    conn.Close();

                    return Properties;
                }
            }
        }

       

        public List<ServerPropertyModel> GetUserProperties( ServerPlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = ("SELECT l.ListPropertyId, l.NameProperty, l.PropertyDescription, l.Threshold, l.Price from dbo.UserProperties p JOIN dbo.Users u on p.UserId = u.UserId JOIN dbo.ListProperties l ON l.ListPropertyId = p.UserPropertyId WHERE u.UserId = @id");
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    List<ServerPropertyModel> Properties = new List<ServerPropertyModel>();
                    conn.Open();
                    cmd.Parameters.AddWithValue("@id", user.UserId);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ServerPropertyModel propertyModel = new ServerPropertyModel();
                        propertyModel.PropertyId = Convert.ToInt32(reader["ListPropertyId"]);
                        propertyModel.PropertyName = (reader["NameProperty"]).ToString();
                        propertyModel.PropertyDescription = (reader["PropertyDescription"]).ToString();
                        propertyModel.Threshold = Convert.ToInt32(reader["Threshold"]);
                        propertyModel.Price = Convert.ToInt32(reader["Price"]);
                        Properties.Add(propertyModel);
                    }

                    conn.Close();

                    return Properties;
                }
            }
        }

        public void BuyProperty(string connectionId, ServerPropertyModel property)
        {
            if (connectionId == null) throw new ArgumentException("parameter user is null");
            if (property == null) throw new ArgumentException("parameter property is null");
            int count = 0, threshold = -1;
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = string.Format("SELECT COUNT (u.ListPropertyId) as count, l.Threshold FROM dbo.ListProperties l JOIN UserProperties u ON u.ListPropertyId = l.ListPropertyId WHERE u.ListPropertyId = @PropertyId GROUP BY Threshold");
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@PropertyId",property.PropertyId);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        count = Convert.ToInt32(reader["count"]);
                        threshold = Convert.ToInt32(reader["Threshold"]);
                    }
                    
                    conn.Close();
                }
            }

            if  (count < threshold || (count == 0 && threshold == -1) || threshold == 0)
            {
                var properties = GetUserProperties(connectionId);

                bool alreadyHas = properties.FirstOrDefault(x => x.PropertyId == property.PropertyId) != null;
                int money = ServiceProvider.GetUserService().GetUserBankAccount(connectionId);
                int id = ServiceProvider.GetPlayerService().GetPlayer(connectionId).UserId;
                if (!alreadyHas && money >= property.Price)
                {
                    using (SqlConnection conn = SqlConnectionService.GetConnection())
                    {
                        string query =
                    "INSERT INTO dbo.UserProperties (UserId, ListPropertyId) values ( @UserId, @ListPropertyId) ";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            conn.Open();

                            cmd.Parameters.AddWithValue("@UserId",id);
                            cmd.Parameters.AddWithValue("@ListPropertyId", property.PropertyId);

                            cmd.ExecuteNonQuery();

                            conn.Close();
                        }
                    }
                    int remainingMoney = money - property.Price;
                    ServiceProvider.GetUserService().SetUserBankAccount(connectionId, remainingMoney);
                }
            }
            

        }

        public void MakePropertyInSell(string connectionId, ServerPropertyModel property, int price)
        {
            if (connectionId == null) throw new ArgumentException("parameter user is null");
            if (property == null) throw new ArgumentException("parameter property is null");
            if (price <= 0) throw new ArgumentException("price must be greater than 0");

            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                var properties = GetUserProperties(connectionId);
                bool alreadyHas = properties.FirstOrDefault(x => x.PropertyId == property.PropertyId) != null;
               
                int id = ServiceProvider.GetPlayerService().GetPlayer(connectionId).UserId;
                if (alreadyHas)
                {
                    {
                        string query =
                    "INSERT INTO dbo.PropertiesToSell (UserId, ListPropertyId, Price) values ( @UserId, @ListPropertyId, @Price) "; 
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            conn.Open();
                            cmd.Parameters.AddWithValue("@UserId", id);
                            cmd.Parameters.AddWithValue("@ListPropertyId", property.PropertyId);
                            cmd.Parameters.AddWithValue("@Price", price);
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }   
                    } 
                }
            }

        }

        public void BuyPropertyFromUser(string connectionId, string connectionId2, ServerPropertyModel property)
        {
            if (connectionId == null) throw new ArgumentException("parameter user is null");
            if (connectionId2 == null) throw new ArgumentException("parameter user2 is null");
            if (property == null) throw new ArgumentException("parameter property is null");
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                var properties = GetUserProperties(connectionId);
                bool alreadyHas = properties.FirstOrDefault(x => x.PropertyId == property.PropertyId) != null;
                int id = ServiceProvider.GetPlayerService().GetPlayer(connectionId).UserId;
                int id2 = ServiceProvider.GetPlayerService().GetPlayer(connectionId2).UserId;
                int moneyUser1 = ServiceProvider.GetUserService().GetUserBankAccount(connectionId2);
                int moneyUser2 = ServiceProvider.GetUserService().GetUserBankAccount(connectionId2);
                if (alreadyHas && moneyUser2 >= property.Price)
                {
                    string query =
                        "UPDATE dbo.UserProperties SET UserId = @UserId WHERE ListPropertyId = @propertyUserId "
                        + "AND UserId = @UserId1" ;
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@UserId", id2);
                        cmd.Parameters.AddWithValue("@UserId1", id);
                        cmd.Parameters.AddWithValue("@propertyUserId", property.PropertyId);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    int remainingMoneyUser1 = moneyUser2 + property.Price;
                    int remainingMoneyUser2 = moneyUser2 - property.Price;
                    ServiceProvider.GetUserService().SetUserBankAccount(connectionId, remainingMoneyUser1);
                    ServiceProvider.GetUserService().SetUserBankAccount(connectionId2, remainingMoneyUser2);
                }
            }
        }

        public List<ServerPropertyModel> GetUserProperties(string ConnectionId)
        {
            if (ConnectionId == null) throw new ArgumentException("there is no id");
            
            ServerPlayerModel user = ServiceProvider.GetPlayerService().GetPlayer(ConnectionId);
            List<ServerPropertyModel> property = ServiceProvider.GetPropertiesService().GetUserProperties(user);

            return property;
        }

    }
    
}