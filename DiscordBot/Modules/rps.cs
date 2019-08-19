using Discord.Commands;
using DiscordBot.DataHandlers;
using DiscordBot.DataHandlers.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class rps : ModuleBase<SocketCommandContext>
    {
        public RpsHandler _handler { get; set; }

        [Command("rps")]
        public async Task Game([Remainder]string message)
        {
            if (Context.User.Username == _handler.lastUser)
            {
                await Context.Channel.SendMessageAsync("You cannot play against yourself!");
            } else if (message.StartsWith("||") && message.EndsWith("||"))
            {
                string temp = message.Trim('|');
                temp = temp.Trim(' ');
                if (temp == "r" || temp == "p" || temp == "s")
                {
                    if (_handler.lastUserChoice == "")
                    {
                        _handler.lastUser = Context.User.Username;
                        _handler.lastUserChoice = temp;
                        await Context.Channel.SendMessageAsync("It is you opponent's turn " + _handler.lastUser + "!!");
                    } else
                    {
                        if (_handler.lastUserChoice == temp)
                        {
                            _handler.AddGame(Context.Guild.Name, _handler.lastUser, Context.User.Username, outcome.TIE);
                            await Context.Channel.SendMessageAsync("TIE!");
                        }
                        else if (_handler.lastUserChoice == "r" && temp == "p")
                        {
                            _handler.AddGame(Context.Guild.Name, _handler.lastUser, Context.User.Username, outcome.P2);
                            await Context.Channel.SendMessageAsync(Context.User.Username + " is the winner!");
                        }
                        else if (_handler.lastUserChoice == "r" && temp == "s")
                        {
                            _handler.AddGame(Context.Guild.Name, _handler.lastUser, Context.User.Username, outcome.P1);
                            await Context.Channel.SendMessageAsync(_handler.lastUser + " is the winner!");
                        }
                        else if (_handler.lastUserChoice == "p" && temp == "r")
                        {
                            _handler.AddGame(Context.Guild.Name, _handler.lastUser, Context.User.Username, outcome.P1);
                            await Context.Channel.SendMessageAsync(_handler.lastUser + " is the winner!");
                        }
                        else if (_handler.lastUserChoice == "p" && temp == "s")
                        {
                            _handler.AddGame(Context.Guild.Name, _handler.lastUser, Context.User.Username, outcome.P2);
                            await Context.Channel.SendMessageAsync(Context.User.Username + " is the winner!");
                        }
                        else if (_handler.lastUserChoice == "s" && temp == "r")
                        {
                            _handler.AddGame(Context.Guild.Name, _handler.lastUser, Context.User.Username, outcome.P2);
                            await Context.Channel.SendMessageAsync(Context.User.Username + " is the winner!");
                        }
                        else if (_handler.lastUserChoice == "s" && temp == "p")
                        {
                            _handler.AddGame(Context.Guild.Name, _handler.lastUser, Context.User.Username, outcome.P1);
                            await Context.Channel.SendMessageAsync(_handler.lastUser + " is the winner!");
                        }
                        _handler.lastUser = "";
                        _handler.lastUserChoice = "";
                    }
                } else
                {
                    await Context.Channel.SendMessageAsync("You must choose r, p or s");
                }
            } else
            {
                await Context.Channel.SendMessageAsync("You must put spoiler tags around your choice!");
            }
        }

        [Command("cancelRps")]
        public async Task Cancel()
        {
            if (_handler.lastUserChoice == "")
            {
                await Context.Channel.SendMessageAsync("There is no choice to cancel!");
            } else if (Context.User.Username == _handler.lastUser)
            {
                _handler.lastUser = "";
                _handler.lastUserChoice = "";
                await Context.Channel.SendMessageAsync("Rps choice cancelled...");
            } else
            {
                await Context.Channel.SendMessageAsync("You cannot cancel someone else's rps!");
            }
        }
    }
}
