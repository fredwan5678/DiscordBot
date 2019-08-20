using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public interface IServerData
    {
        Dictionary<string, string> getServerData(string server);

        void registerToServerProfile();
    }
}
