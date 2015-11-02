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

       
    }
    
}