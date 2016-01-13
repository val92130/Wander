using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Wander.Server.ClassLibrary.Hooks;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Model.Players;
using Wander.Server.ClassLibrary.Services;

namespace Wander.Server.ClassLibrary.Plugins
{
    [PluginInfo("RandomQuestions", "Send random questions players","Wander", "1.0")]
    public class RandomQuestionsPlugin : GameHook
    {
        private int _intervalMinutes = 25;
        protected override void Init()
        {
            Utilities.Timer.RepeatAfter(() =>
            {
                List<ServerPlayerModel> connectedPlayers = ServiceProvider.GetPlayerService().GetAllPlayersServer();

                for (int i = 0; i < connectedPlayers.Count; i++)
                {
                    var question = ServiceProvider.GetQuestionService().GetRandomQuestion(connectedPlayers[i].SignalRId);
                    if (question == null) continue;
                    Context.Clients.Client(connectedPlayers[i].SignalRId)
        .notify(Helper.CreateNotificationMessage("We have to test your Competence ", EMessageType.info));
                    Context.Clients.Client(connectedPlayers[i].SignalRId).sendQuestionToClient(new { Question = question.Question, QuestionId = question.QuestionId });
                }
            },60 * _intervalMinutes, 60 * _intervalMinutes );
            base.Init();
        }
    }
}
