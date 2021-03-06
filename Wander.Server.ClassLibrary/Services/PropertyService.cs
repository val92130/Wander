﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Players;

namespace Wander.Server.ClassLibrary.Services
{
    public class PropertyService : IPropertyService
    {

        public List<ServerPropertyModel> GetProperties()
        {
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = string.Format("SELECT * from dbo.ListProperties");
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    List<ServerPropertyModel> Properties = new List<ServerPropertyModel>();
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
        public List<ServerPropertyUserModel> GetPropertiesInSell()
        {

            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = string.Format("SELECT l.ListPropertyId, l.Nameproperty, l.PropertyDescription,l.Threshold, p.Price, p.UserId from dbo.PropertiesToSell p JOIN dbo.ListProperties l on l.ListPropertyId = p.ListPropertyId ");
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    List<ServerPropertyUserModel> Properties = new List<ServerPropertyUserModel>();
                    conn.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ServerPropertyUserModel propertyModel = new ServerPropertyUserModel();
                        propertyModel.PropertyId = Convert.ToInt32(reader["ListPropertyId"]);
                        propertyModel.PropertyName = (reader["NameProperty"]).ToString();
                        propertyModel.PropertyDescription = (reader["PropertyDescription"]).ToString();
                        propertyModel.Threshold = Convert.ToInt32(reader["Threshold"]);
                        propertyModel.Price = Convert.ToInt32(reader["Price"]);
                        propertyModel.UserId = Convert.ToInt32(reader["UserId"]);
                        Properties.Add(propertyModel);
                    }

                    conn.Close();

                    return Properties;
                }
            }
        }

        public int AddProperty(ServerPropertyModel model)
        {
            if (model == null) throw new ArgumentException("parameter model is null");
            if (model.PropertyName == null || model.PropertyDescription == null) throw new ArgumentException("an attribute of the property model is null");

            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "INSERT INTO dbo.ListProperties (NameProperty, PropertyDescription, Threshold, Price) OUTPUT INSERTED.ListPropertyId values (@Name, @Des, @Thres, @Price)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Name", model.PropertyName);
                    cmd.Parameters.AddWithValue("@Des", model.PropertyDescription);
                    cmd.Parameters.AddWithValue("@Thres", model.Threshold);
                    cmd.Parameters.AddWithValue("@Price", model.Price);

                    int lgn = (int)cmd.ExecuteScalar();

                    conn.Close();

                    return lgn;
                }
            }

        }

        public bool DeleteProperty(int id)
        {
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "DELETE from dbo.ListProperties WHERE ListPropertyId = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Id", id);

                    int lgn = cmd.ExecuteNonQuery();
                    conn.Close();

                    return lgn != 0;
                }
            }
        }

        public bool DeleteProperty(ServerPropertyModel model)
        {
            if (model == null) throw new ArgumentException("parameter model is null");
            return DeleteProperty(model.PropertyId);
        }



        public List<ServerPropertyModel> GetUserProperties(ServerPlayerModel user)
        {
            if (user == null) throw new ArgumentException("parameter user is null");
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = ("SELECT l.ListPropertyId, l.NameProperty, l.PropertyDescription, l.Threshold, l.Price from dbo.UserProperties p JOIN dbo.Users u on p.UserId = u.UserId JOIN dbo.ListProperties l ON l.ListPropertyId = p.ListPropertyId WHERE p.UserId = @id");
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

        public ServerNotificationMessage BuyProperty(string connectionId, ServerPropertyModel property)
        {
            if (connectionId == null) throw new ArgumentException("parameter user is null");
            if (property == null) throw new ArgumentException("parameter property is null");

            ServerNotificationMessage message = new ServerNotificationMessage() { Content = "error", MessageType = EMessageType.error };

            int count = 0, threshold = -1;
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = string.Format("SELECT COUNT (u.ListPropertyId) as count, l.Threshold FROM dbo.ListProperties l JOIN UserProperties u ON u.ListPropertyId = l.ListPropertyId WHERE u.ListPropertyId = @PropertyId GROUP BY Threshold");
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    cmd.Parameters.AddWithValue("@PropertyId", property.PropertyId);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        count = Convert.ToInt32(reader["count"]);
                        threshold = Convert.ToInt32(reader["Threshold"]);
                    }

                    conn.Close();
                }
            }

            if (count < threshold || (count == 0 && threshold == -1) || threshold == 0)
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

                            cmd.Parameters.AddWithValue("@UserId", id);
                            cmd.Parameters.AddWithValue("@ListPropertyId", property.PropertyId);

                            cmd.ExecuteNonQuery();

                            conn.Close();
                        }
                    }
                    int remainingMoney = money - property.Price;
                    ServiceProvider.GetUserService().SetUserBankAccount(connectionId, remainingMoney);
                    message.Content = "Success";
                    message.MessageType = EMessageType.success;
                }
                else
                {
                    if (alreadyHas)
                    {
                        message.Content = "You already have this property";
                    }
                    else
                    {
                        message.Content = "You dont have enough money";
                    }

                }
            }
            else
            {
                message.Content = "Error, this propery reached its limit";
            }
            return message;


        }

        public ServerNotificationMessage MakePropertyInSell(string connectionId, ServerPropertyModel property, int price)
        {
            if (connectionId == null) throw new ArgumentException("parameter user is null");
            if (property == null) throw new ArgumentException("parameter property is null");
            if (price <= 0) throw new ArgumentException("price must be greater than 0");

            ServerNotificationMessage message = new ServerNotificationMessage() { Content = "error", MessageType = EMessageType.error };

            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                var properties = GetUserProperties(connectionId);
                bool alreadyHas = properties.FirstOrDefault(x => x.PropertyId == property.PropertyId) != null;
                bool alreadyInSell = GetPropertiesInSell().FirstOrDefault(x => x.PropertyId == property.PropertyId) !=
                                     null;
                int id = ServiceProvider.GetPlayerService().GetPlayer(connectionId).UserId;

                if (alreadyInSell)
                {
                    message.Content = "This property is already in sell";
                    message.MessageType = EMessageType.error;
                    return message;
                }

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
                        message.Content = "Success ! ";
                        message.MessageType = EMessageType.success;
                    }
                }
                else
                {
                    message.Content = "You already own this property !";
                }
            }

            return message;

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
                        + "AND UserId = @UserId1";
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

        public int GetOwnersCount(int propertyId)
        {
            int count = -1;
            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = "SELECT COUNT(p.UserPropertyId) FROM dbo.UserProperties p JOIN dbo.Users u ON u.UserId = p.UserId WHERE p.ListPropertyId = @Id; ";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Id", propertyId);
                    var res = cmd.ExecuteScalar();
                    if (res != null)
                    {
                        try
                        {
                            count = (int)res;
                        }
                        catch
                        {
                            count = -1;
                        }
                    }

                    conn.Close();
                }
            }
            return count;
        }

        public ServerPropertyModel GetProperty(int propertyId)
        {

            using (SqlConnection conn = SqlConnectionService.GetConnection())
            {
                string query = string.Format("SELECT * from dbo.ListProperties WHERE ListPropertyId = @Id");
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    ServerPropertyModel property = new ServerPropertyModel();

                    conn.Open();
                    cmd.Parameters.AddWithValue("@Id", propertyId);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        property.PropertyId = Convert.ToInt32(reader["ListPropertyId"]);
                        property.PropertyName = (reader["NameProperty"]).ToString();
                        property.PropertyDescription = (reader["PropertyDescription"]).ToString();
                        property.Threshold = Convert.ToInt32(reader["Threshold"]);
                        property.Price = Convert.ToInt32(reader["Price"]);
                        break;
                    }

                    conn.Close();

                    return property;
                }
            }
        }
    }
}
