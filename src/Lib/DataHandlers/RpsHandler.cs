using Lib.Util;
using System;
using System.Collections.Generic;

namespace Lib.DataHandlers
{
    public class RpsHandler : RpsHandlerBase, IServerData, IUserData
    {
        private ILeaderboardHandler _leaderboard;

        private ProfileHandlerBase _dataHub;

        public RpsHandler(ServiceResolver serviceResolver, ProfileHandlerBase dataHub)
        {
            _leaderboard = serviceResolver("RpsHandler");
            _dataHub = dataHub;

            RegisterToProfile();
            RegisterToServerProfile();
        }

        public override void AddGame(string guildName, string player1, string player2, outcome winner)
        {
            if (winner == outcome.P1)
            {
                _leaderboard.Winner(guildName, player1);
                _leaderboard.Loser(guildName, player2);
            }
            else if (winner == outcome.P2)
            {
                _leaderboard.Winner(guildName, player2);
                _leaderboard.Loser(guildName, player1);
            }
            else if (winner == outcome.TIE)
            {
                _leaderboard.Tie(guildName, player1, player2);
            }
        }

        public override Dictionary<string, int> GenerateLeaderboard(string server, int amt)
        {
            Dictionary<string, int> board = new Dictionary<string, int>();

            amt = Math.Min(amt, _leaderboard.Board(server).leaderboardNames.Count);
            var iter = _leaderboard.Board(server).leaderboardScores.Reverse().GetEnumerator();

            for (int i = 0; i < amt; i++)
            {
                iter.MoveNext();
                board[iter.Current.getName()] = iter.Current.getScore();
            }

            return board;
        }

        public Dictionary<string, string> getServerData(string server)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            var iter = _leaderboard.Board(server).leaderboardScores.Reverse().GetEnumerator();

            int amt = Math.Min(3, _leaderboard.Board(server).leaderboardNames.Count);

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
            Dictionary<string, string> data = new Dictionary<string, string>();

            if (_leaderboard.Board(server).leaderboardNames.ContainsKey(user))
            {
                data["RPS score: "] = _leaderboard.Board(server).leaderboardNames[user].getScore().ToString();
                data["RPS wins: "] = _leaderboard.Board(server).leaderboardNames[user].getWins().ToString();
                data["RPS losses: "] = _leaderboard.Board(server).leaderboardNames[user].getLosses().ToString();
                data["Total RPS games: "] = _leaderboard.Board(server).leaderboardNames[user].getTotal().ToString();
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

        public void RegisterToProfile()
        {
            _dataHub.UserData.Add(this);
        }

        public void RegisterToServerProfile()
        {
            _dataHub.ServerData.Add(this);
        }
    }
}
