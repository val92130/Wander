using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Wander.Server.Model;

namespace Wander.Server.Services
{
    public class Helper
    {
        /// <summary>
        /// Checks whether an email is valid or not
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Return True if the email is valid, otherwise returns false</returns>
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static ClientMessageModel CreateNotificationMessage(string content, EMessageType type)
        {
            return new ClientMessageModel() { Content = content, MessageType = type.ToString() };
        }

        public static ChatMessageModel CreateChatMessage(string username, string content, int sex, string hour)
        {
            return new ChatMessageModel() { UserName = username, Content = content, Sex = sex, Hour = hour };
        }

    }
}