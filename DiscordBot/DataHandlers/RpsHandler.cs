using DiscordBot.DataHandlers.Utilities;
using DiscordBot.DataSaving;
using DiscordBot.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.DataHandlers
{
    public class RpsHandler : IServerData, IUserData
    {
        private IDataSaver _saver;

        private const string dataFolder = "Resources";
        private const string leaderboardFolder = "rpsData";
        private const string leaderboardDirectory = dataFolder + "/" + leaderboardFolder;

        private Dictionary<string, RpsLeaderboard> leaderboard = new Dictionary<string, RpsLeaderboard>();

        public string lastUser = "";
        public string lastUserChoice = "";

        public RpsHandler(IDataSaver saver)
        {
            _saver = saver;
        }

        private void SaveLeaderboard(string guildName)
        {
            _saver.SaveData<RpsLeaderboard>(leaderboard[guildName], guildName, leaderboardDirectory);
        }

        private void LoadLeaderboard(string guildName)
        {
            if (!leaderboard.ContainsKey(guildName))
            {
                leaderboard[guildName] = _saver.LoadData<RpsLeaderboard>(guildName, leaderboardDirectory);
                if (leaderboard[guildName].leaderboardNames == null)
                {
                    RpsLeaderboard data = new RpsLeaderboard();
                    data.leaderboardNames = new Dictionary<string, RpsPlayer>();
                    data.leaderboardScores = new SortedSet<RpsPlayer>();
                    leaderboard[guildName] = data;
                }
            }
        }

        private void Winner(string guildName, string player)
        {
            RpsPlayer temp = leaderboard[guildName].leaderboardNames[player];
            leaderboard[guildName].leaderboardScores.Remove(temp);
            leaderboard[guildName].leaderboardNames[player].AddWin();
            leaderboard[guildName].leaderboardScores.Add(leaderboard[guildName].leaderboardNames[player]);
        }

        private void Loser(string guildName, string player)
        {
            RpsPlayer temp = leaderboard[guildName].leaderboardNames[player];
            leaderboard[guildName].leaderboardScores.Remove(temp);
            leaderboard[guildName].leaderboardNames[player].AddLoss();
            leaderboard[guildName].leaderboardScores.Add(leaderboard[guildName].leaderboardNames[player]);
        }

        private void Tie(string guildName, string player1, string player2)
        {
            RpsPlayer temp = leaderboard[guildName].leaderboardNames[player1];
            leaderboard[guildName].leaderboardScores.Remove(temp);
            leaderboard[guildName].leaderboardNames[player1].AddTie();
            leaderboard[guildName].leaderboardScores.Add(leaderboard[guildName].leaderboardNames[player1]);

            temp = leaderboard[guildName].leaderboardNames[player2];
            leaderboard[guildName].leaderboardScores.Remove(temp);
            leaderboard[guildName].leaderboardNames[player2].AddTie();
            leaderboard[guildName].leaderboardScores.Add(leaderboard[guildName].leaderboardNames[player2]);
        }

        private void AddPlayer(string playerName, string guildName)
        {
            LoadLeaderboard(guildName);
            RpsPlayer player = new RpsPlayer(playerName);
            leaderboard[guildName].leaderboardNames[playerName] = player;
            leaderboard[guildName].leaderboardScores.Add(player);
            SaveLeaderboard(guildName);
        }

        public void AddGame(string guildName, string player1, string player2, outcome winner)
        {
            LoadLeaderboard(guildName);
            if (!leaderboard[guildName].leaderboardNames.ContainsKey(player1))
            {
                AddPlayer(player1, guildName);
            }
            if (!leaderboard[guildName].leaderboardNames.ContainsKey(player2))
            {
                AddPlayer(player2, guildName);
            }

            if (winner == outcome.P1)
            {
                Winner(guildName, player1);
                Loser(guildName, player2);
            }
            else if (winner == outcome.P2)
            {
                Winner(guildName, player2);
                Loser(guildName, player1);
            }
            else if (winner == outcome.TIE)
            {
                Tie(guildName, player1, player2);
            }
            SaveLeaderboard(guildName);
        }

        public Dictionary<string, string> getServerData(string server)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> getUserData(string user)
        {
            throw new NotImplementedException();
        }

        public void registerToProfile()
        {
            throw new NotImplementedException();
        }

        public void registerToServerProfile()
        {
            throw new NotImplementedException();
        }
    }

    struct RpsLeaderboard
    {
        public SortedSet<RpsPlayer> leaderboardScores;
        public Dictionary<string, RpsPlayer> leaderboardNames;
    }
}
