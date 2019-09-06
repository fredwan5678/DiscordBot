using Discord.Commands;
using DiscordBot.DataHandlers;
using System;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class Quotes : ModuleBase<SocketCommandContext>
    {
        public IQuoteHandler _handler { get; set; }

        [Command("addQuote")]
        public async Task AddQuote([Remainder]string message)
        {
            _handler.AddQuote(message, Context.Guild.Name);
            await Context.Channel.SendMessageAsync("Quote added...probably");
        }

        [Command("remQuote")]
        public async Task RemoveQuote([Remainder]string message)
        {
            int index;

            if (Int32.TryParse(message, out index))
            {
                if (_handler.RemoveQuote(index, Context.Guild.Name))
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
                string quote = _handler.getQuote(index, Context.Guild.Name);
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
            string quote = _handler.GetRandQuote(Context.Guild.Name);
            if (quote == "")
            {
                await Context.Channel.SendMessageAsync("There are no quotes yet...");
            }
            else
            {
                await Context.Channel.SendMessageAsync(_handler.GetRandQuote(Context.Guild.Name));
            }
        }

        [Command("quoteAmt")]
        public async Task quoteAmt()
        {
            await Context.Channel.SendMessageAsync(_handler.GetQuoteAmt(Context.Guild.Name).ToString());
        }

        [Command("allQuotes")]
        [RequireUserPermission(Discord.GuildPermission.MuteMembers)]
        public async Task all()
        {
            string[] quotes = _handler.GetAllQuotes(Context.Guild.Name);
            await Context.Channel.SendMessageAsync("Printing " + _handler.GetQuoteAmt(Context.Guild.Name).ToString() + " quotes:");
            foreach (string quote in quotes)
            {
                await Context.Channel.SendMessageAsync(quote);
            }
        }

        [Command("qUndo")]
        public async Task undoQuote()
        {
            int last = _handler.GetQuoteAmt(Context.Guild.Name);

            if (last == 0)
            {
                await Context.Channel.SendMessageAsync("There are no quotes...");
            }
            else
            {
                _handler.RemoveQuote(last, Context.Guild.Name);
                await Context.Channel.SendMessageAsync("Quote " + last.ToString() + " removed...");
            }

        }
    }
}
