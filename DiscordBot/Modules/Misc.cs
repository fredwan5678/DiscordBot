using Discord.Commands;
using DiscordBot.Handlers;
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
            QuoteHandler.GetInstance().AddQuote(message, Context.Guild.Name);
            await Context.Channel.SendMessageAsync("Quote added...probably");
        }

        [Command("remQuote")]
        public async Task RemoveQuote([Remainder]string message)
        {
            int index;

            if (Int32.TryParse(message, out index))
            {
                if (QuoteHandler.GetInstance().RemoveQuote(index, Context.Guild.Name))
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
                string quote = QuoteHandler.GetInstance().getQuote(index, Context.Guild.Name);
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
            string quote = QuoteHandler.GetInstance().GetRandQuote(Context.Guild.Name);
            if (quote == "")
            {
                await Context.Channel.SendMessageAsync("There are no quotes yet...");
            }
            else
            {
                await Context.Channel.SendMessageAsync(QuoteHandler.GetInstance().GetRandQuote(Context.Guild.Name));
            }
        }

        [Command("quoteAmt")]
        public async Task quoteAmt()
        {
            await Context.Channel.SendMessageAsync(QuoteHandler.GetInstance().GetQuoteAmt(Context.Guild.Name).ToString());
        }

        [Command("allQuotes")]
        [RequireUserPermission(Discord.GuildPermission.MuteMembers)]
        public async Task all()
        {
            string[] quotes = QuoteHandler.GetInstance().GetAllQuotes(Context.Guild.Name);
            await Context.Channel.SendMessageAsync("Printing " + QuoteHandler.GetInstance().GetQuoteAmt(Context.Guild.Name).ToString() + " quotes:");
            foreach (string quote in quotes)
            {
                await Context.Channel.SendMessageAsync(quote);
            }
        }

        [Command("qUndo")]
        public async Task undoQuote()
        {
            int last = QuoteHandler.GetInstance().GetQuoteAmt(Context.Guild.Name);

            if (last == 0)
            {
                await Context.Channel.SendMessageAsync("There are no quotes...");
            }
            else
            {
                QuoteHandler.GetInstance().RemoveQuote(last, Context.Guild.Name);
                await Context.Channel.SendMessageAsync("Quote " + last.ToString() + " removed...");
            }
            
        }

        [Command("die")]
        public async Task die()
        {
            await Context.Channel.SendMessageAsync(":skull_crossbones: You will be immediately killed by this action. Proceed?");
        }

        [Command("yes")]
        public async Task yes()
        {
            await Context.Guild.CurrentUser.KickAsync();
            await Context.Channel.SendMessageAsync(Context.User.Username + "has died! :skull_crossbones:");
        }
    }
}
