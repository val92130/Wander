using System.Collections.Generic;
using Wander.Server.Model;

namespace Wander.Server.Services
{
    public interface IMessageService
    {
        List<ChatMessageModel> GetMessagesLimit(int limit);
        bool LogMessage(ChatMessageModel message);
    }
}