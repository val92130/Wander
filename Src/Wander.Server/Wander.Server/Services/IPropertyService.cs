using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wander.Server.Services
{
    using Wander.Server.Model;
    using Wander.Server.Model.Players;

    public interface IPropertyService
    {
        List<ServerPropertyModel> GetProperties();

        List<ServerPropertyModel> GetUserProperties(ServerPlayerModel user);

    }
}
