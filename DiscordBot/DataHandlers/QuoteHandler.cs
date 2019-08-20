using DiscordBot.DataSaving;
using DiscordBot.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.DataHandlers
{
    public class QuoteHandler : IServerData
    {
        private IDataSaver _saver;

        private const string dataFolder = "Resources";
        private const string quoteFolder = "quoteData";
        private const string quoteDirectory = dataFolder + "/" + quoteFolder;

        private Dictionary<string, QuoteData> quoteData = new Dictionary<string, QuoteData>();

        private ProfileHandler _dataHub;

        public QuoteHandler(IDataSaver saver, ProfileHandler dataHub)
        {
            _saver = saver;
            _dataHub = dataHub;

            registerToServerProfile();
        }

        private void SaveQuotes(string guildName)
        {
            _saver.SaveData(quoteData[guildName], guildName, quoteDirectory);
        }

        private void LoadQuotes(string guildName)
        {
            if (!quoteData.ContainsKey(guildName))
            {
                quoteData[guildName] = _saver.LoadData<QuoteData>(guildName, quoteDirectory);
                if (quoteData[guildName].quotes == null)
                {
                    QuoteData data = new QuoteData();
                    data.quotes = new List<string>();
                    quoteData[guildName] = data;
                }
            }
        }

        public void AddQuote(string quote, string serverName)
        {
            LoadQuotes(serverName);
            quoteData[serverName].quotes.Add(quote);
            SaveQuotes(serverName);
        }

        public bool RemoveQuote(int quoteNumber, string serverName)
        {
            LoadQuotes(serverName);
            if (quoteNumber > quoteData[serverName].quotes.Count || quoteNumber <= 0)
            {
                return false;
            }
            else
            {
                quoteData[serverName].quotes.RemoveAt(quoteNumber - 1);
                SaveQuotes(serverName);
                return true;
            }
        }

        public string getQuote(int quoteNumber, string serverName)
        {
            LoadQuotes(serverName);
            if (quoteNumber > quoteData[serverName].quotes.Count || quoteNumber < 1)
            {
                return "";
            }
            else
            {
                return quoteData[serverName].quotes[quoteNumber - 1];
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
            return quoteData[serverName].quotes[random.Next(quoteData[serverName].quotes.Count)];
        }

        public int GetQuoteAmt(string serverName)
        {
            LoadQuotes(serverName);
            return quoteData[serverName].quotes.Count;
        }

        public string[] GetAllQuotes(string serverName)
        {
            LoadQuotes(serverName);
            return quoteData[serverName].quotes.ToArray();
        }

        public Dictionary<string, string> getServerData(string server)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            data["Amount of Quotes: "] = GetQuoteAmt(server).ToString();

            return data;
        }

        public void registerToServerProfile()
        {
            _dataHub.serverData.Add(this);
        }
    }

    struct QuoteData
    {
        public List<string> quotes;
    }
}
