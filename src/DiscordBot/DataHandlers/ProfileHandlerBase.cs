using DiscordBot.Modules;
using System.Collections.Generic;

namespace DiscordBot.DataHandlers
{
    public abstract class ProfileHandlerBase
    {
        public List<IUserData> UserData = new List<IUserData>();
        public List<IServerData> ServerData = new List<IServerData>();

        public abstract int AddWarn(string guildName, string user);
        public abstract Dictionary<string, string> GenerateServerProfile(string guildName);
        public abstract Dictionary<string, string> GenerateUserProfile(string guildName, string user);
        public abstract int RemoveWarn(string guildName, string user);
        public abstract void ResetWarn(string guildName, string user);
    }
}