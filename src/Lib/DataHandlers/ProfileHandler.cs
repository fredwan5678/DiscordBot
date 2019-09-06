using System.Collections.Generic;
using Lib.DataSaving;

namespace Lib.DataHandlers
{
    public class ProfileHandler : ProfileHandlerBase
    {
        private IDataSaver _saver;

        private const string DATA_FOLDER = "Resources";
        private const string WARN_FOLDER = "WarnData";
        private const string WARN_DIRECTORY = DATA_FOLDER + "/" + WARN_FOLDER;

        private Dictionary<string, WarnData> _warnData = new Dictionary<string, WarnData>();

        public ProfileHandler(IDataSaver saver)
        {
            _saver = saver;
        }

        private void SaveWarnings(string guildName)
        {
            _saver.SaveData(_warnData[guildName], guildName, WARN_DIRECTORY);
        }

        private void LoadWarnings(string guildName)
        {
            if (!_warnData.ContainsKey(guildName))
            {
                _warnData[guildName] = _saver.LoadData<WarnData>(guildName, WARN_DIRECTORY);
                if (_warnData[guildName].warnings == null)
                {
                    WarnData data = new WarnData();
                    data.warnings = new Dictionary<string, int>();
                    _warnData[guildName] = data;
                }
            }
        }

        public override int AddWarn(string guildName, string user)
        {
            LoadWarnings(guildName);
            _warnData[guildName].warnings[user]++;
            SaveWarnings(guildName);

            return _warnData[guildName].warnings[user];
        }

        public override int RemoveWarn(string guildName, string user)
        {
            LoadWarnings(guildName);
            _warnData[guildName].warnings[user]--;
            SaveWarnings(guildName);

            if (_warnData[guildName].warnings[user] < 0)
            {
                _warnData[guildName].warnings[user] = 0;
            }
            return _warnData[guildName].warnings[user];
        }

        public override void ResetWarn(string guildName, string user)
        {
            LoadWarnings(guildName);
            _warnData[guildName].warnings[user] = 0;
            SaveWarnings(guildName);
        }

        public override Dictionary<string, string> GenerateUserProfile(string guildName, string user)
        {
            LoadWarnings(guildName);
            Dictionary<string, string> userProperties = new Dictionary<string, string>();

            Dictionary<string, string> temp = new Dictionary<string, string>();

            foreach (IUserData dataset in UserData)
            {
                temp = dataset.getUserData(guildName, user);
                foreach (var entry in temp)
                {
                    userProperties.Add(entry.Key, entry.Value);
                }
            }

            userProperties["Warnings: "] = "0";
            if (_warnData[guildName].warnings.ContainsKey(user))
            {
                userProperties["Warnings: "] = _warnData[guildName].warnings[user].ToString();
            }

            return userProperties;
        }

        public override Dictionary<string, string> GenerateServerProfile(string guildName)
        {
            Dictionary<string, string> serverProperties = new Dictionary<string, string>();

            Dictionary<string, string> temp = new Dictionary<string, string>();

            foreach (IServerData dataset in ServerData)
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
