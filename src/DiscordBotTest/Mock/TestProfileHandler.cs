using System;
using System.Collections.Generic;
using System.Text;
using Lib.DataHandlers;

namespace DiscordBotTest.Mock
{
    class TestProfileHandler : ProfileHandlerBase
    {
        public override int AddWarn(string guildName, string user)
        {
            return default;
        }

        public override Dictionary<string, string> GenerateServerProfile(string guildName)
        {
            return default;
        }

        public override Dictionary<string, string> GenerateUserProfile(string guildName, string user)
        {
            return default;
        }

        public override int RemoveWarn(string guildName, string user)
        {
            return default;
        }

        public override void ResetWarn(string guildName, string user)
        {
            return;
        }
    }
}
