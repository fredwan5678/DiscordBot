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

        private ProfileHandler _dataHub;

        public string lastUser = "";
        public string lastUserChoice = "";

        public RpsHandler(IDataSaver saver, ProfileHandler dataHub)
        {
            _saver = saver;
            _dataHub = dataHub;

            registerToProfile();
            registerToServerProfile();
        }

        private void SaveLeaderboard(string guildName)
        {
            _saver.SaveData(leaderboard[guildName], guildName, leaderboardDirectory);
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

        public Dictionary<string, int> GenerateLeaderboard(string server, int amt)
        {
            LoadLeaderboard(server);
            Dictionary<string, int> board = new Dictionary<string, int>();

            amt = Math.Min(amt, leaderboard[server].leaderboardNames.Count);
            var iter = leaderboard[server].leaderboardScores.Reverse().GetEnumerator();

            for (int i = 0; i < amt; i++ )
            {
                iter.MoveNext();
                board[iter.Current.getName()] = iter.Current.getScore();
            }

            return board;
        }

        public Dictionary<string, string> getServerData(string server)
        {
            LoadLeaderboard(server);

            Dictionary<string, string> data = new Dictionary<string, string>();

            var iter = leaderboard[server].leaderboardScores.Reverse().GetEnumerator();

            int amt = Math.Min(3, leaderboard[server].leaderboardNames.Count);

            if (amt == 0) data["Top RPS Players: "] = "No rps games played yet...";
            else
            {
                string topPlayers = "";

                for (int i = 0; i < amt - 1; i++)
                {
                    iter.MoveNext();
                    topPlayers += iter.Current.getName();
                    topPlayers += ", ";
                }
                iter.MoveNext();
                topPlayers += iter.Current.getName();
                data["Top RPS Players: "] = topPlayers;
            }

            return data;
        }

        public Dictionary<string, string> getUserData(string server, string user)
        {
            LoadLeaderboard(server);

            Dictionary<string, string> data = new Dictionary<string, string>();

            if (leaderboard[server].leaderboardNames.ContainsKey(user))
            {
                data["RPS score: "] = leaderboard[server].leaderboardNames[user].getScore().ToString();
                data["RPS wins: "] = leaderboard[server].leaderboardNames[user].getWins().ToString();
                data["RPS losses: "] = leaderboard[server].leaderboardNames[user].getLosses().ToString();
                data["Total RPS games: "] = leaderboard[server].leaderboardNames[user].getTotal().ToString();
            }
            else
            {
                data["RPS score: "] = "0";
                data["RPS wins: "] = "0";
                data["RPS losses: "] = "0";
                data["Total RPS games: "] = "0";
            }
            return data;
        }

        public void registerToProfile()
        {
            _dataHub.userData.Add(this);
        }

        public void registerToServerProfile()
        {
            _dataHub.serverData.Add(this);
        }
    }

    struct RpsLeaderboard
    {
        public SortedSet<RpsPlayer> leaderboardScores;
        public Dictionary<string, RpsPlayer> leaderboardNames;
    }
}
