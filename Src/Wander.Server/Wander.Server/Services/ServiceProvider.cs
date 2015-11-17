using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            return null;
        }

        public static PlayerService GetPlayerService()
        {
            return null;
        }

        public static UserService GetUserService()
        {
            return null;
        }
        public static PropertyService GetPropertiesService()
        {
            return null;
        }

        public static MessageService GetMessageService()
        {
            return null;
        }

        public static JobService GetJobService()
        {
            return null;
        }

        public static GameManager GetGameManager()
        {
            return null;
        }



    }
}