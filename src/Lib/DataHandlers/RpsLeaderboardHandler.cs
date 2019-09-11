using System;
using System.Collections.Generic;
using System.Text;
using Lib.DataHandlers;
using Lib.DataSaving;

namespace Lib.DataHandlers
{
    class RpsLeaderboardHandler : ILeaderboardHandler
    {
        private IDataSaver _saver;
        private const string DATA_FOLDER = "Resources";
        private const string LEADERBOARD_FOLDER = "RpsData";
        private const string LEADERBOARD_DIRECTORY = DATA_FOLDER + "/" + LEADERBOARD_FOLDER;

        private Dictionary<string, Leaderboard> _leaderboard = new Dictionary<string, Leaderboard>();

        public RpsLeaderboardHandler(IDataSaver saver)
        {
            _saver = saver;
        }

        private void SaveLeaderboard(string guildName)
        {
            _saver.SaveData(_leaderboard[guildName], guildName, LEADERBOARD_DIRECTORY);
        }

        private void LoadLeaderboard(string guildName)
        {
            if (!_leaderboard.ContainsKey(guildName))
            {
                _leaderboard[guildName] = _saver.LoadData<Leaderboard>(guildName, LEADERBOARD_DIRECTORY);
                if (_leaderboard[guildName].leaderboardNames == null)
                {
                    Leaderboard data = new Leaderboard();
                    data.leaderboardNames = new Dictionary<string, Player>();
                    data.leaderboardScores = new SortedSet<Player>();
                    _leaderboard[guildName] = data;
                }
            }
        }

        public void Loser(string guildName, string player)
        {
            LoadLeaderboard(guildName);
            if (!_leaderboard[guildName].leaderboardNames.ContainsKey(player))
            {
                AddPlayer(player, guildName);
            }

            Player temp = _leaderboard[guildName].leaderboardNames[player];
            _leaderboard[guildName].leaderboardScores.Remove(temp);
            _leaderboard[guildName].leaderboardNames[player].AddLoss();
            _leaderboard[guildName].leaderboardScores.Add(_leaderboard[guildName].leaderboardNames[player]);
            SaveLeaderboard(guildName);
        }

        public void Tie(string guildName, params string[] players)
        {
            LoadLeaderboard(guildName);

            Player temp;

            foreach (string player in players)
            {
                if (!_leaderboard[guildName].leaderboardNames.ContainsKey(player))
                {
                    AddPlayer(player, guildName);
                }

                temp = _leaderboard[guildName].leaderboardNames[player];
                _leaderboard[guildName].leaderboardScores.Remove(temp);
                _leaderboard[guildName].leaderboardNames[player].AddTie();
                _leaderboard[guildName].leaderboardScores.Add(_leaderboard[guildName].leaderboardNames[player]);
            }
            SaveLeaderboard(guildName);
        }

        public void Winner(string guildName, string player)
        {
            LoadLeaderboard(guildName);
            if (!_leaderboard[guildName].leaderboardNames.ContainsKey(player))
            {
                AddPlayer(player, guildName);
            }

            Player temp = _leaderboard[guildName].leaderboardNames[player];
            _leaderboard[guildName].leaderboardScores.Remove(temp);
            _leaderboard[guildName].leaderboardNames[player].AddWin();
            _leaderboard[guildName].leaderboardScores.Add(_leaderboard[guildName].leaderboardNames[player]);
            SaveLeaderboard(guildName);
        }

        private void AddPlayer(string playerName, string guildName)
        {
            LoadLeaderboard(guildName);
            Player player = new Player(playerName);
            _leaderboard[guildName].leaderboardNames[playerName] = player;
            _leaderboard[guildName].leaderboardScores.Add(player);
            SaveLeaderboard(guildName);
        }

        public Leaderboard Board(string serverName)
        {
            LoadLeaderboard(serverName);
            return _leaderboard[serverName];
        }
    }
}
