using Discord.Commands;
using DiscordBot.DataSaving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class Quotes : ModuleBase<SocketCommandContext>
    {
        public IDataSaver _saver { get; set; }

        private const string dataFolder = "Resources";
        private const string quoteFolder = "quoteData";
        private const string quoteDirectory = dataFolder + "/" + quoteFolder;

        private Dictionary<string, QuoteData> quoteData = new Dictionary<string, QuoteData>();

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

        private void AddQuote(string quote, string serverName)
        {
            LoadQuotes(serverName);
            quoteData[serverName].quotes.Add(quote);
            SaveQuotes(serverName);
        }

        private bool RemoveQuote(int quoteNumber, string serverName)
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

        private string getQuote(int quoteNumber, string serverName)
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

        private string GetRandQuote(string serverName)
        {
            LoadQuotes(serverName);
            if (GetQuoteAmt(serverName) == 0)
            {
                return "";
            }
            Random random = new Random();
            return quoteData[serverName].quotes[random.Next(quoteData[serverName].quotes.Count)];
        }

        private int GetQuoteAmt(string serverName)
        {
            LoadQuotes(serverName);
            return quoteData[serverName].quotes.Count;
        }

        private string[] GetAllQuotes(string serverName)
        {
            LoadQuotes(serverName);
            return quoteData[serverName].quotes.ToArray();
        }

        [Command("addQuote")]
        public async Task AddQuote([Remainder]string message)
        {
            AddQuote(message, Context.Guild.Name);
            await Context.Channel.SendMessageAsync("Quote added...probably");
        }

        [Command("remQuote")]
        public async Task RemoveQuote([Remainder]string message)
        {
            int index;

            if (Int32.TryParse(message, out index))
            {
                if (RemoveQuote(index, Context.Guild.Name))
                {
                    await Context.Channel.SendMessageAsync("Quote removed successfully!");
                }
                else
                {
                    await Context.Channel.SendMessageAsync("Quote does not exist...");
                }
            }
            else
            {
                await Context.Channel.SendMessageAsync("Invalid Quote");
            }
        }

        [Command("getQuote")]
        public async Task GetQuote([Remainder]string message)
        {
            int index;

            if (Int32.TryParse(message, out index))
            {
                string quote = getQuote(index, Context.Guild.Name);
                if (quote == "")
                {
                    await Context.Channel.SendMessageAsync("Invalid quote number...");
                }
                else
                {
                    await Context.Channel.SendMessageAsync(quote);
                }
            }
            else
            {
                await Context.Channel.SendMessageAsync("Invalid quote number...");
            }
        }

        [Command("randQuote")]
        public async Task RandQuote()
        {
            string quote = GetRandQuote(Context.Guild.Name);
            if (quote == "")
            {
                await Context.Channel.SendMessageAsync("There are no quotes yet...");
            }
            else
            {
                await Context.Channel.SendMessageAsync(GetRandQuote(Context.Guild.Name));
            }
        }

        [Command("quoteAmt")]
        public async Task quoteAmt()
        {
            await Context.Channel.SendMessageAsync(GetQuoteAmt(Context.Guild.Name).ToString());
        }

        [Command("allQuotes")]
        [RequireUserPermission(Discord.GuildPermission.MuteMembers)]
        public async Task all()
        {
            string[] quotes = GetAllQuotes(Context.Guild.Name);
            await Context.Channel.SendMessageAsync("Printing " + GetQuoteAmt(Context.Guild.Name).ToString() + " quotes:");
            foreach (string quote in quotes)
            {
                await Context.Channel.SendMessageAsync(quote);
            }
        }

        [Command("qUndo")]
        public async Task undoQuote()
        {
            int last = GetQuoteAmt(Context.Guild.Name);

            if (last == 0)
            {
                await Context.Channel.SendMessageAsync("There are no quotes...");
            }
            else
            {
                RemoveQuote(last, Context.Guild.Name);
                await Context.Channel.SendMessageAsync("Quote " + last.ToString() + " removed...");
            }

        }
    }

    public struct QuoteData
    {
        public List<string> quotes;
    }
}
