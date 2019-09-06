using System.Collections.Generic;

namespace DiscordBot.Modules
{
    public interface IServerData
    {
        Dictionary<string, string> getServerData(string server);

        void registerToServerProfile();
    }
}
