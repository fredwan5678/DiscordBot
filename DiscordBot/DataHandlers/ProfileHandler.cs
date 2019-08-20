using DiscordBot.DataSaving;
using DiscordBot.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.DataHandlers
{
    public class ProfileHandler
    {
        private IDataSaver _saver;

        private const string dataFolder = "Resources";
        private const string warnFolder = "warnData";
        private const string warnDirectory = dataFolder + "/" + warnFolder;

        private Dictionary<string, WarnData> warnData = new Dictionary<string, WarnData>();

        public List<IUserData> userData = new List<IUserData>();
        public List<IServerData> serverData = new List<IServerData>();

        public ProfileHandler(IDataSaver saver)
        {
            _saver = saver;
        }

        private void SaveWarnings(string guildName)
        {
            _saver.SaveData(warnData[guildName], guildName, warnDirectory);
        }

        private void LoadWarnings(string guildName)
        {
            if (!warnData.ContainsKey(guildName))
            {
                warnData[guildName] = _saver.LoadData<WarnData>(guildName, warnDirectory);
                if (warnData[guildName].warnings == null)
                {
                    WarnData data = new WarnData();
                    data.warnings = new Dictionary<string, int>();
                    warnData[guildName] = data;
                }
            }
        }

        public int AddWarn(string guildName, string user)
        {
            LoadWarnings(guildName);
            warnData[guildName].warnings[user]++;
            SaveWarnings(guildName);

            return warnData[guildName].warnings[user];
        }

        public int RemoveWarn(string guildName, string user)
        {
            LoadWarnings(guildName);
            warnData[guildName].warnings[user]--;
            SaveWarnings(guildName);

            if (warnData[guildName].warnings[user] < 0)
            {
                warnData[guildName].warnings[user] = 0;
            }
            return warnData[guildName].warnings[user];
        }

        public void ResetWarn(string guildName, string user)
        {
            LoadWarnings(guildName);
            warnData[guildName].warnings[user] = 0;
            SaveWarnings(guildName);
        }

        public Dictionary<string, string> GenerateUserProfile(string guildName, string user)
        {
            LoadWarnings(guildName);
            Dictionary<string, string> userProperties = new Dictionary<string, string>();

            Dictionary<string, string> temp = new Dictionary<string, string>();

            foreach (IUserData dataset in userData)
            {
                temp = dataset.getUserData(guildName, user);
                foreach (var entry in temp)
                {
                    userProperties.Add(entry.Key, entry.Value);
                }
            }

            userProperties["Warnings: "] = "0";
            if (warnData[guildName].warnings.ContainsKey(user))
            {
                userProperties["Warnings: "] = warnData[guildName].warnings[user].ToString();
            }

            return userProperties;
        }

        public Dictionary<string, string> GenerateServerProfile(string guildName)
        {
            Dictionary<string, string> serverProperties = new Dictionary<string, string>();

            Dictionary<string, string> temp = new Dictionary<string, string>();

            foreach (IServerData dataset in serverData)
            {
                temp = dataset.getServerData(guildName);
                foreach (var entry in temp)
                {
                    serverProperties.Add(entry.Key, entry.Value);
                }
            }

            return serverProperties;
        }
    }

    struct WarnData
    {
        public Dictionary<string, int> warnings;
    }
}
