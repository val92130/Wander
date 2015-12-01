using System.Collections.Generic;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Players;

namespace Wander.Server.ClassLibrary.Services
{
    public interface IPropertyService
    {
        int AddProperty(ServerPropertyModel model);
        ServerNotificationMessage BuyProperty(string connectionId, ServerPropertyModel property);
        void BuyPropertyFromUser(string connectionId, string connectionId2, ServerPropertyModel property);
        bool DeleteProperty(ServerPropertyModel model);
        bool DeleteProperty(int id);
        int GetOwnersCount(int propertyId);
        List<ServerPropertyModel> GetProperties();
        List<ServerPropertyUserModel> GetPropertiesInSell();
        ServerPropertyModel GetProperty(int propertyId);
        List<ServerPropertyModel> GetUserProperties(string ConnectionId);
        List<ServerPropertyModel> GetUserProperties(ServerPlayerModel user);
        ServerNotificationMessage MakePropertyInSell(string connectionId, ServerPropertyModel property, int price);
    }
}