using DiscordBot.DataSaving;
using DiscordBot.Modules;
using System;
using System.Collections.Generic;

namespace DiscordBot.DataHandlers
{
    public class QuoteHandler : IServerData
    {
        private IDataSaver _saver;

        private const string DATA_FOLDER = "Resources";
        private const string QUOTE_FOLDER = "quoteData";
        private const string QUOTE_DIRECTORY = DATA_FOLDER + "/" + QUOTE_FOLDER;

        private Dictionary<string, QuoteData> _quoteData = new Dictionary<string, QuoteData>();

        private ProfileHandler _dataHub;

        public QuoteHandler(IDataSaver saver, ProfileHandler dataHub)
        {
            _saver = saver;
            _dataHub = dataHub;

            registerToServerProfile();
        }

        private void SaveQuotes(string guildName)
        {
            _saver.SaveData(_quoteData[guildName], guildName, QUOTE_DIRECTORY);
        }

        private void LoadQuotes(string guildName)
        {
            if (!_quoteData.ContainsKey(guildName))
            {
                _quoteData[guildName] = _saver.LoadData<QuoteData>(guildName, QUOTE_DIRECTORY);
                if (_quoteData[guildName].quotes == null)
                {
                    QuoteData data = new QuoteData();
                    data.quotes = new List<string>();
                    _quoteData[guildName] = data;
                }
            }
        }

        public void AddQuote(string quote, string serverName)
        {
            LoadQuotes(serverName);
            _quoteData[serverName].quotes.Add(quote);
            SaveQuotes(serverName);
        }

        public bool RemoveQuote(int quoteNumber, string serverName)
        {
            LoadQuotes(serverName);
            if (quoteNumber > _quoteData[serverName].quotes.Count || quoteNumber <= 0)
            {
                return false;
            }
            else
            {
                _quoteData[serverName].quotes.RemoveAt(quoteNumber - 1);
                SaveQuotes(serverName);
                return true;
            }
        }

        public string getQuote(int quoteNumber, string serverName)
        {
            LoadQuotes(serverName);
            if (quoteNumber > _quoteData[serverName].quotes.Count || quoteNumber < 1)
            {
                return "";
            }
            else
            {
                return _quoteData[serverName].quotes[quoteNumber - 1];
            }
        }

        public string GetRandQuote(string serverName)
        {
            LoadQuotes(serverName);
            if (GetQuoteAmt(serverName) == 0)
            {
                return "";
            }
            Random random = new Random();
            return _quoteData[serverName].quotes[random.Next(_quoteData[serverName].quotes.Count)];
        }

        public int GetQuoteAmt(string serverName)
        {
            LoadQuotes(serverName);
            return _quoteData[serverName].quotes.Count;
        }

        public string[] GetAllQuotes(string serverName)
        {
            LoadQuotes(serverName);
            return _quoteData[serverName].quotes.ToArray();
        }

        public Dictionary<string, string> getServerData(string server)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            data["Amount of Quotes: "] = GetQuoteAmt(server).ToString();

            return data;
        }

        public void registerToServerProfile()
        {
            _dataHub.ServerData.Add(this);
        }
    }

    struct QuoteData
    {
        public List<string> quotes;
    }
}
