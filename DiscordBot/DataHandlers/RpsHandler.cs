using DiscordBot.DataHandlers.Utilities;
using DiscordBot.DataSaving;
using DiscordBot.Modules;
using System;
using System.Collections.Generic;

namespace DiscordBot.DataHandlers
{
    public class RpsHandler : IServerData, IUserData
    {
        private IDataSaver _saver;

        private const string DATA_FOLDER = "Resources";
        private const string LEADERBOARD_FOLDER = "rpsData";
        private const string LEADERBOARD_DIRECTORY = DATA_FOLDER + "/" + LEADERBOARD_FOLDER;

        private Dictionary<string, RpsLeaderboard> _leaderboard = new Dictionary<string, RpsLeaderboard>();

        private ProfileHandler _dataHub;

        public string LastUser = "";
        public string LastUserChoice = "";

        public RpsHandler(IDataSaver saver, ProfileHandler dataHub)
        {
            _saver = saver;
            _dataHub = dataHub;

            registerToProfile();
            registerToServerProfile();
        }

        private void SaveLeaderboard(string guildName)
        {
            _saver.SaveData(_leaderboard[guildName], guildName, LEADERBOARD_DIRECTORY);
        }

        private void LoadLeaderboard(string guildName)
        {
            if (!_leaderboard.ContainsKey(guildName))
            {
                _leaderboard[guildName] = _saver.LoadData<RpsLeaderboard>(guildName, LEADERBOARD_DIRECTORY);
                if (_leaderboard[guildName].leaderboardNames == null)
                {
                    RpsLeaderboard data = new RpsLeaderboard();
                    data.leaderboardNames = new Dictionary<string, RpsPlayer>();
                    data.leaderboardScores = new SortedSet<RpsPlayer>();
                    _leaderboard[guildName] = data;
                }
            }
        }

        private void Winner(string guildName, string player)
        {
            RpsPlayer temp = _leaderboard[guildName].leaderboardNames[player];
            _leaderboard[guildName].leaderboardScores.Remove(temp);
            _leaderboard[guildName].leaderboardNames[player].AddWin();
            _leaderboard[guildName].leaderboardScores.Add(_leaderboard[guildName].leaderboardNames[player]);
        }

        private void Loser(string guildName, string player)
        {
            RpsPlayer temp = _leaderboard[guildName].leaderboardNames[player];
            _leaderboard[guildName].leaderboardScores.Remove(temp);
            _leaderboard[guildName].leaderboardNames[player].AddLoss();
            _leaderboard[guildName].leaderboardScores.Add(_leaderboard[guildName].leaderboardNames[player]);
        }

        private void Tie(string guildName, string player1, string player2)
        {
            RpsPlayer temp = _leaderboard[guildName].leaderboardNames[player1];
            _leaderboard[guildName].leaderboardScores.Remove(temp);
            _leaderboard[guildName].leaderboardNames[player1].AddTie();
            _leaderboard[guildName].leaderboardScores.Add(_leaderboard[guildName].leaderboardNames[player1]);

            temp = _leaderboard[guildName].leaderboardNames[player2];
            _leaderboard[guildName].leaderboardScores.Remove(temp);
            _leaderboard[guildName].leaderboardNames[player2].AddTie();
            _leaderboard[guildName].leaderboardScores.Add(_leaderboard[guildName].leaderboardNames[player2]);
        }

        private void AddPlayer(string playerName, string guildName)
        {
            LoadLeaderboard(guildName);
            RpsPlayer player = new RpsPlayer(playerName);
            _leaderboard[guildName].leaderboardNames[playerName] = player;
            _leaderboard[guildName].leaderboardScores.Add(player);
            SaveLeaderboard(guildName);
        }

        public void AddGame(string guildName, string player1, string player2, outcome winner)
        {
            LoadLeaderboard(guildName);
            if (!_leaderboard[guildName].leaderboardNames.ContainsKey(player1))
            {
                AddPlayer(player1, guildName);
            }
            if (!_leaderboard[guildName].leaderboardNames.ContainsKey(player2))
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

            amt = Math.Min(amt, _leaderboard[server].leaderboardNames.Count);
            var iter = _leaderboard[server].leaderboardScores.Reverse().GetEnumerator();

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

            var iter = _leaderboard[server].leaderboardScores.Reverse().GetEnumerator();

            int amt = Math.Min(3, _leaderboard[server].leaderboardNames.Count);

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

            if (_leaderboard[server].leaderboardNames.ContainsKey(user))
            {
                data["RPS score: "] = _leaderboard[server].leaderboardNames[user].getScore().ToString();
                data["RPS wins: "] = _leaderboard[server].leaderboardNames[user].getWins().ToString();
                data["RPS losses: "] = _leaderboard[server].leaderboardNames[user].getLosses().ToString();
                data["Total RPS games: "] = _leaderboard[server].leaderboardNames[user].getTotal().ToString();
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
            _dataHub.UserData.Add(this);
        }

        public void registerToServerProfile()
        {
            _dataHub.ServerData.Add(this);
        }
    }

    struct RpsLeaderboard
    {
        public SortedSet<RpsPlayer> leaderboardScores;
        public Dictionary<string, RpsPlayer> leaderboardNames;
    }
}
