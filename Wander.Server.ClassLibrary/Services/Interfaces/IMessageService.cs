using System.Collections.Generic;
using Wander.Server.ClassLibrary.Model;

namespace Wander.Server.ClassLibrary.Services
{
    public interface IMessageService
    {
        List<ChatMessageModel> GetMessagesLimit(int limit);
        bool LogMessage(ChatMessageModel message);
    }
}