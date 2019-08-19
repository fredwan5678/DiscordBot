﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordBot.DataHandlers.Utilities
{
    class RpsPlayer : IComparable<RpsPlayer>, IEquatable<RpsPlayer>
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

        public RpsPlayer(string name)
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

        public int CompareTo(RpsPlayer other)
        {
            if (_score > other._score) return 1;
            else if (_score < other._score) return -1;
            else if (_total > other._total) return 1;
            else if (_total < other._total) return -1;
            else return 0;
        }

        public bool Equals(RpsPlayer other)
        {
            if (_name == other._name && _score == other._score && _total == other._total)
            {
                return true;
            }
            return false;
        }
    }
}
