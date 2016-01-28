using System.Collections.Generic;
using Microsoft.AspNet.SignalR.Hubs;
using Wander.Server.ClassLibrary.Hooks;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Players;
using Wander.Server.ClassLibrary.Services;
using Wander.Server.ClassLibrary.Services.Interfaces;
using Wander.Server.ClassLibrary.Utilities;

namespace Wander.Server.ClassLibrary.Plugins
{
    [PluginInfo("RandomQuestions", "Send random questions players", "Wander", "1.0")]
    public class RandomQuestionsPlugin : GameHook
    {
        private readonly int _intervalMinutes = 25;

        protected override void Init()
        {
            Timer.RepeatAfter(() =>
            {
                var connectedPlayers = ServiceProvider.GetPlayerService().GetAllPlayersServer();
                SendQuestion(connectedPlayers);

            }, 60*_intervalMinutes, 60*_intervalMinutes);
            base.Init();
        }

        private void SendQuestion(List<ServerPlayerModel> players)
        {
            for (var i = 0; i < players.Count; i++)
            {
                var question = ServiceProvider.GetQuestionService().GetRandomQuestion(players[i].SignalRId);
                if (question == null) continue;
                Context.Clients.Client(players[i].SignalRId)
                    .notify(Helper.CreateNotificationMessage("We have to test your Competence ", EMessageType.info));
                Context.Clients.Client(players[i].SignalRId)
                    .sendQuestionToClient(new { question.Question, question.QuestionId });
            }
        }

        [ChatCommand("forceQuestion")]
        public bool ForceQuestion(IHubCallerConnectionContext<IClient> clients, ServerPlayerModel player, CommandModel command)
        {
            if (ServiceProvider.GetAdminService().IsAdmin(player.UserId))
            {
                var connectedPlayers = ServiceProvider.GetPlayerService().GetAllPlayersServer();
                clients.Caller.notify(Helper.CreateNotificationMessage(
                    "Sending questions to every players", EMessageType.info));
                SendQuestion(connectedPlayers);
            }
            else
            {
                clients.Caller.notify(Helper.CreateNotificationMessage(
                    "You have to be an admin to perform this action", EMessageType.error));
            }
            return true;
            
        }
    }
}