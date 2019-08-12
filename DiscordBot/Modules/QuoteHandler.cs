using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using DiscordBot.Modules.DataSaving;

namespace DiscordBot.Modules
{
    public class QuoteHandler
    {
        private const string quoteFolder = "Resources";
        private const string quoteFile = "quotes.json";

        private static IDataSaver dataSaver;

        private QuoteData quoteData;

        private readonly Random random = new Random();

        public static QuoteHandler instance;

        private QuoteHandler()
        {
            quoteData = dataSaver.LoadData<QuoteData>(quoteFile, quoteFolder);
        }

        ~QuoteHandler()
        {
            string json = JsonConvert.SerializeObject(quoteData, Formatting.Indented);
            File.WriteAllText(quoteFolder + "/" + quoteFile, json);
        }

        public static QuoteHandler GetInstance(params IDataSaver[] saver)
        {
            if (saver.Length > 0) dataSaver = saver[0];
            else if (dataSaver == null) dataSaver = new DataSaving.implementations.JsonDataSaver();

            if (instance == null) instance = new QuoteHandler();
            return instance;
        }

        private void SaveQuote()
        {
            dataSaver.SaveData(quoteData, quoteFile, quoteFolder);
        }

        public void AddQuote(string quote)
        {
            quoteData.quotes.Add(quote);
            SaveQuote();
        }

        public bool RemoveQuote(int quoteNumber)
        {
            if (quoteNumber > quoteData.quotes.Count || quoteNumber <= 0)
            {
                return false;
            }
            else
            {
                quoteData.quotes.RemoveAt(quoteNumber-1);
                SaveQuote();
                return true;
            }
        }

        public string getQuote(int quoteNumber)
        {
            if (quoteNumber > quoteData.quotes.Count)
            {
                return "";
            }
            else
            {
                return quoteData.quotes[quoteNumber-1];
            }
        }

        public string GetRandQuote()
        {
            if (GetQuoteAmt() == 0)
            {
                return "";
            }
            return quoteData.quotes[random.Next(quoteData.quotes.Count)];
        }

        public int GetQuoteAmt()
        {
            return quoteData.quotes.Count;
        }
    }

    public struct QuoteData
    {
        public List<string> quotes;
    }
}
