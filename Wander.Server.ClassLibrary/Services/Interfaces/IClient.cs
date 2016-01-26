using System.Collections.Generic;
using Wander.Server.ClassLibrary.Model;

namespace Wander.Server.ClassLibrary.Services.Interfaces
{
    public interface IClient
    {
        void notify(ClientNotificationMessageModel message);
        void LoadMessages(List<ChatMessageModel> messages);
        void forceDisconnect();
        void onConnected(object user);
        void playerConnected(object user);
        void playerDisconnected(object user);
        void MessageReceived(ChatMessageModel message);
        void PrivateMessageReceived(ChatMessageModel message);
        void playerMoved(object player);
        void updateTime(bool isDay);
        void playerEnterMap(object player);
        void playerExitMap(object player);
    }
}