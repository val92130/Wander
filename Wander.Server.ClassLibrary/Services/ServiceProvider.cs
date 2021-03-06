﻿namespace Wander.Server.ClassLibrary.Services
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
        public static GameManager GetGameManager()
        {
            if (GameManager == null)
                GameManager = new GameManager();

            return GameManager;
        }



    }
}
