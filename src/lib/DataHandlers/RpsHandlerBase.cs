using System.Collections.Generic;
using DiscordBot.Util;

namespace DiscordBot.DataHandlers
{
    public abstract class RpsHandlerBase
    {
        public string LastUser = "";
        public string LastUserChoice = "";

        public abstract void AddGame(string guildName, string player1, string player2, outcome winner);
        public abstract Dictionary<string, int> GenerateLeaderboard(string server, int amt);
    }
}