using Wander.Server.ClassLibrary.Hooks;
using Wander.Server.ClassLibrary.Model;
using Wander.Server.ClassLibrary.Services;
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

                for (var i = 0; i < connectedPlayers.Count; i++)
                {
                    var question = ServiceProvider.GetQuestionService().GetRandomQuestion(connectedPlayers[i].SignalRId);
                    if (question == null) continue;
                    Context.Clients.Client(connectedPlayers[i].SignalRId)
                        .notify(Helper.CreateNotificationMessage("We have to test your Competence ", EMessageType.info));
                    Context.Clients.Client(connectedPlayers[i].SignalRId)
                        .sendQuestionToClient(new {question.Question, question.QuestionId});
                }
            }, 60*_intervalMinutes, 60*_intervalMinutes);
            base.Init();
        }
    }
}