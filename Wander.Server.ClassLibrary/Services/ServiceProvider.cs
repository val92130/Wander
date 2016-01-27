using System;
using Wander.Server.ClassLibrary.Services.Interfaces;

namespace Wander.Server.ClassLibrary.Services
{
    public class ServiceProvider
    {
        private static IUserRegistrationService RegistrationService;
        private static IPlayerService PlayerService;
        private static IUserService UserService;
        private static IMessageService MessageService;
        private static IJobService JobService;
        private static IPropertyService PropertyService;
        private static IQuestionService QuestionService;
        private static GameManager GameManager;
        private static IAdminService AdminService;
        private static IMapService MapService;
        private static IHookService HookService;

        public static IUserRegistrationService GetUserRegistrationService()
        {
            if (RegistrationService == null)
                RegistrationService = new DbUserRegistrationService();

            return RegistrationService;
        }

        public static IPlayerService GetPlayerService()
        {
            if (PlayerService == null)
                PlayerService = new PlayerService();

            return PlayerService;
        }

        public static IUserService GetUserService()
        {
            if (UserService == null)
                UserService = new UserService();

            return UserService;
        }

        public static IPropertyService GetPropertiesService()
        {
            if (PropertyService == null)
                PropertyService = new PropertyService();

            return PropertyService;
        }

        public static IMessageService GetMessageService()
        {
            if (MessageService == null)
                MessageService = new MessageService();

            return MessageService;
        }

        public static IJobService GetJobService()
        {
            if (JobService == null)
                JobService = new JobService();

            return JobService;
        }

        public static IQuestionService GetQuestionService()
        {
            if (QuestionService == null)
                QuestionService = new QuestionService();

            return QuestionService;
        }

        public static IAdminService GetAdminService()
        {
            if (AdminService == null)
                AdminService = new AdminService();

            return AdminService;
        }

        public static IHookService GetHookService()
        {
            if (HookService == null)
                HookService = new HookService();

            return HookService;

        }

        public static GameManager GetGameManager()
        {
            if (GameManager == null)
                GameManager = new GameManager();

            return GameManager;
        }

        public static IMapService GetMapService()
        {
            if (MapService == null)
                MapService = new MapService();

            return MapService;
        }
    }
}