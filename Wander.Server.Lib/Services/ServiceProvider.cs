namespace Wander.Server.Services
{

    public class ServiceProvider
    {
        private static IUserRegistrationService RegistrationService;
        private static PlayerService PlayerService;
        private static UserService UserService;
        private static MessageService MessageService;
        private static JobService JobService;
        private static PropertyService PropertyService;
        private static GameManager GameManager;

        public static IUserRegistrationService GetUserRegistrationService()
        {
            if(RegistrationService == null)
                RegistrationService = new DbUserRegistrationService();

            return RegistrationService;
        }

        public static PlayerService GetPlayerService()
        {
            if(PlayerService == null)
                PlayerService = new PlayerService();

            return PlayerService;
        }

        public static UserService GetUserService()
        {
            if (UserService == null)
                UserService = new UserService();

            return UserService;
        }
        public static PropertyService GetPropertiesService()
        {
            if (PropertyService == null)
                PropertyService = new PropertyService();

            return PropertyService;
        }

        public static MessageService GetMessageService()
        {
            if (MessageService == null)
                MessageService = new MessageService();

            return MessageService;
        }

        public static JobService GetJobService()
        {
            if (JobService == null)
                JobService = new JobService();

            return JobService;
        }

        public static GameManager GetGameManager()
        {
            if (GameManager == null)
                GameManager = new GameManager();

            return GameManager;
        }



    }
}