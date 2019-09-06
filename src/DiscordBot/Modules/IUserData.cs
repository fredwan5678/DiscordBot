using System.Collections.Generic;

namespace DiscordBot.Modules
{
    public interface IUserData
    {
        Dictionary<string, string> getUserData(string server, string user);

        void RegisterToProfile();
    }
}
