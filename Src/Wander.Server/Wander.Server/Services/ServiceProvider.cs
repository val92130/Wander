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
        private static LogService LogService;

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

        public static LogService GetLogService()
        {
            if (LogService == null)
                LogService = new LogService();

            return LogService;
        }



    }
}