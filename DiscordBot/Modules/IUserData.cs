using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public interface IUserData
    {
        Dictionary<string, string> getUserData(string server, string user);

        void registerToProfile();
    }
}
