using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wander.Server.Services
{
    using Wander.Server.Model;
    using Wander.Server.Model.Players;

    public interface IPropertyService
    {
        List<ServerPropertyModel> GetProperties();
        List<ServerPropertyUserModel> GetPropertiesInSell();

        List<ServerPropertyModel> GetUserProperties(ServerPlayerModel user);
        List<ServerPropertyModel> GetUserProperties(string connectionId);
        int AddProperty(ServerPropertyModel model);
        bool DeleteProperty(int id);
        bool DeleteProperty(ServerPropertyModel model);
        void BuyProperty(string connectionId, ServerPropertyModel property);
        void MakePropertyInSell(string connectionId, ServerPropertyModel property, int price);
        void BuyPropertyFromUser(string connectionId, string connectionId2, ServerPropertyModel property);

    }
}
