using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using DiscordBot.Handlers.DataSaving;

namespace DiscordBot.Handlers
{
    public class QuoteHandler : DataHandler
    {
        private const string quoteFolder = "quoteData";
        private const string quoteDirectory = dataFolder + "/" + quoteFolder;

        private Dictionary<string, QuoteData> quoteData = new Dictionary<string, QuoteData>();

        public static QuoteHandler instance;

        private QuoteHandler() { }
        
        public static QuoteHandler GetInstance()
        {
            if (instance == null) instance = new QuoteHandler();
            return instance;
        }

        private void SaveQuotes(string guildName)
        {
            saver.SaveData(quoteData[guildName], guildName, quoteDirectory);
        }

        private void LoadQuotes(string guildName)
        {
            if (!quoteData.ContainsKey(guildName))
            {
                quoteData[guildName] = saver.LoadData<QuoteData>(guildName, quoteDirectory);
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
                quoteData[serverName].quotes.RemoveAt(quoteNumber-1);
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
                return quoteData[serverName].quotes[quoteNumber-1];
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
    }

    public struct QuoteData
    {
        public List<string> quotes;
    }
}
