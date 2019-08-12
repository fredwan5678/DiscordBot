using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {
        [Command("echo")]
        public async Task Echo([Remainder]string message)
        {
            await Context.Channel.SendMessageAsync(message);
        }

        [Command("help")]
        public async Task Help()
        {
            await Context.Channel.SendMessageAsync("Command in construction :)");
        }

        [Command("addQuote")]
        public async Task AddQuote([Remainder]string message)
        {
            QuoteHandler.GetInstance().AddQuote(message);
            await Context.Channel.SendMessageAsync("Quote added...probably");
        }

        [Command("removeQuote")]
        public async Task RemoveQuote([Remainder]string message)
        {
            int index;

            if (Int32.TryParse(message, out index))
            {
                if (QuoteHandler.GetInstance().RemoveQuote(index))
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
                string quote = QuoteHandler.GetInstance().getQuote(index);
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
                await Context.Channel.SendMessageAsync("Invalid Quote");
            }
        }

        [Command("randQuote")]
        public async Task RandQuote()
        {
            string quote = QuoteHandler.GetInstance().GetRandQuote();
            if (quote == "")
            {
                await Context.Channel.SendMessageAsync("There are no quotes yet...");
            }
            else
            {
                await Context.Channel.SendMessageAsync(QuoteHandler.GetInstance().GetRandQuote());
            }
        }

        [Command("quoteAmt")]
        public async Task quoteAmt()
        {
            await Context.Channel.SendMessageAsync(QuoteHandler.GetInstance().GetQuoteAmt().ToString());
        }
    }
}
