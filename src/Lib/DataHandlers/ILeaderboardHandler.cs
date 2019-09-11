using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DataHandlers
{
    public interface ILeaderboardHandler
    {
        void Winner(string guildName, string player);

        void Loser(string guildName, string player);

        void Tie(string guildName, params string[] players);

        Leaderboard Board(string serverName);
    }

    public struct Leaderboard
    {
        public SortedSet<Player> leaderboardScores;
        public Dictionary<string, Player> leaderboardNames;
    }

    public class Player : IComparable<Player>, IEquatable<Player>
    {
        [JsonProperty]
        private string _name;
        [JsonProperty]
        private int _wins = 0;
        [JsonProperty]
        private int _losses = 0;
        [JsonProperty]
        private int _total = 0;
        [JsonProperty]
        private int _score = 0;

        public Player(string name)
        {
            _name = name;
        }

        public void AddWin()
        {
            _wins++;
            _total++;
            _score++;
        }

        public void AddLoss()
        {
            _losses++;
            _total++;
            _score--;
        }

        public void AddTie()
        {
            _total++;
        }

        public int CompareTo(Player other)
        {
            if (_score > other._score) return 1;
            else if (_score < other._score) return -1;
            else if (_total > other._total) return 1;
            else if (_total < other._total) return -1;
            else return 0;
        }

        public string getName()
        {
            return _name;
        }

        public int getScore()
        {
            return _score;
        }
        public int getWins()
        {
            return _wins;
        }
        public int getLosses()
        {
            return _losses;
        }
        public int getTotal()
        {
            return _total;
        }

        public bool Equals(Player other)
        {
            if (_name == other._name && _score == other._score && _total == other._total)
            {
                return true;
            }
            return false;
        }
    }
}
