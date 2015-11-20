using System.Collections.Generic;
using Wander.Server.Model;
using Wander.Server.Model.Players;

namespace Wander.Server.Services
{
    public interface IPropertyService
    {
        List<ServerPropertyModel> GetProperties();
        List<ServerPropertyUserModel> GetPropertiesInSell();
        int AddProperty(ServerPropertyModel model);
        bool DeleteProperty(int id);
        bool DeleteProperty(ServerPropertyModel model);
        List<ServerPropertyModel> GetUserProperties( ServerPlayerModel user);
        ServerNotificationMessage BuyProperty(string connectionId, ServerPropertyModel property);
        ServerNotificationMessage MakePropertyInSell(string connectionId, ServerPropertyModel property, int price);
        void BuyPropertyFromUser(string connectionId, string connectionId2, ServerPropertyModel property);
        List<ServerPropertyModel> GetUserProperties(string ConnectionId);
    }
}