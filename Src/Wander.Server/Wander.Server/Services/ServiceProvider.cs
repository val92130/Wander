using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wander.Server.Services
{
    
    public class ServiceProvider
    {
        private static IUserRegistrationService RegistrationService;

        public static IUserRegistrationService GetUserRegistrationService()
        {
            if(RegistrationService == null)
                RegistrationService = new DbUserRegistrationService();

            return RegistrationService;
        }

    }
}