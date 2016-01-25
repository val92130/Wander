using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Players;

namespace Wander.Server.ClassLibrary.Services
{
    public class Helper
    {
        /// <summary>
        ///     Checks whether an email is valid or not
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Return True if the email is valid, otherwise returns false</returns>
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static ClientNotificationMessageModel CreateNotificationMessage(string content, EMessageType type)
        {
            return new ClientNotificationMessageModel {Content = content, MessageType = type.ToString()};
        }

        public static ChatMessageModel CreateChatMessage(string username, int userId, string content, int sex,
            string hour)
        {
            return new ChatMessageModel
            {
                UserName = username,
                Content = content,
                Sex = sex,
                Hour = hour,
                UserId = userId
            };
        }

        public static ClientPlayerModel CreateClientPlayerModel(string username, int sex, Vector2 position)
        {
            return new ClientPlayerModel {Position = position, Sex = sex, UserName = username};
        }

        public static string Sha1Encode(string input)
        {
            var data = Encoding.ASCII.GetBytes(input);
            data = new SHA256Managed().ComputeHash(data);
            return Encoding.ASCII.GetString(data);
        }

        public static CommandModel ParseCommand(ChatMessageModel chatMessage)
        {
            if (chatMessage == null) return null;
            var command = new CommandModel();
            var messageContent = chatMessage.Content;
            if (!messageContent.StartsWith("/")) return null;
            if (messageContent.Length <= 1) return null;

            messageContent = messageContent.Replace("&quot;", @"""");
            var data = Regex.Matches(messageContent, @"[\""].+?[\""]|[^ ]+")
                .Cast<Match>()
                .Select(m => m.Value)
                .ToList();

            if (data.Count <= 0) return null;
            var cmd = data[0].Substring(1);
            data.RemoveAt(0);
            var args = data.ToArray<string>();

            command.Args = args;
            command.Command = cmd;
            return command;
        }
    }
}