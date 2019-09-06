using Discord.Commands;
using DiscordBot.DataHandlers;
using DiscordBot.DataHandlers.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class rps : ModuleBase<SocketCommandContext>
    {
        public RpsHandler _handler { get; set; }

        [Command("rps")]
        public async Task Game([Remainder]string message)
        {
            if (Context.User.Username == _handler.LastUser)
            {
                await Context.Channel.SendMessageAsync("You cannot play against yourself!");
            } else if (message.StartsWith("||") && message.EndsWith("||"))
            {
                string temp = message.Trim('|');
                temp = temp.Trim(' ');
                if (temp == "r" || temp == "p" || temp == "s")
                {
                    if (_handler.LastUserChoice == "")
                    {
                        _handler.LastUser = Context.User.Username;
                        _handler.LastUserChoice = temp;
                        await Context.Channel.SendMessageAsync("It is you opponent's turn " + _handler.LastUser + "!!");
                    } else
                    {
                        if (_handler.LastUserChoice == temp)
                        {
                            _handler.AddGame(Context.Guild.Name, _handler.LastUser, Context.User.Username, outcome.TIE);
                            await Context.Channel.SendMessageAsync("TIE!");
                        }
                        else if (_handler.LastUserChoice == "r" && temp == "p")
                        {
                            _handler.AddGame(Context.Guild.Name, _handler.LastUser, Context.User.Username, outcome.P2);
                            await Context.Channel.SendMessageAsync(Context.User.Username + " is the winner!");
                        }
                        else if (_handler.LastUserChoice == "r" && temp == "s")
                        {
                            _handler.AddGame(Context.Guild.Name, _handler.LastUser, Context.User.Username, outcome.P1);
                            await Context.Channel.SendMessageAsync(_handler.LastUser + " is the winner!");
                        }
                        else if (_handler.LastUserChoice == "p" && temp == "r")
                        {
                            _handler.AddGame(Context.Guild.Name, _handler.LastUser, Context.User.Username, outcome.P1);
                            await Context.Channel.SendMessageAsync(_handler.LastUser + " is the winner!");
                        }
                        else if (_handler.LastUserChoice == "p" && temp == "s")
                        {
                            _handler.AddGame(Context.Guild.Name, _handler.LastUser, Context.User.Username, outcome.P2);
                            await Context.Channel.SendMessageAsync(Context.User.Username + " is the winner!");
                        }
                        else if (_handler.LastUserChoice == "s" && temp == "r")
                        {
                            _handler.AddGame(Context.Guild.Name, _handler.LastUser, Context.User.Username, outcome.P2);
                            await Context.Channel.SendMessageAsync(Context.User.Username + " is the winner!");
                        }
                        else if (_handler.LastUserChoice == "s" && temp == "p")
                        {
                            _handler.AddGame(Context.Guild.Name, _handler.LastUser, Context.User.Username, outcome.P1);
                            await Context.Channel.SendMessageAsync(_handler.LastUser + " is the winner!");
                        }
                        _handler.LastUser = "";
                        _handler.LastUserChoice = "";
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
            if (_handler.LastUserChoice == "")
            {
                await Context.Channel.SendMessageAsync("There is no choice to cancel!");
            } else if (Context.User.Username == _handler.LastUser)
            {
                _handler.LastUser = "";
                _handler.LastUserChoice = "";
                await Context.Channel.SendMessageAsync("Rps choice cancelled...");
            } else
            {
                await Context.Channel.SendMessageAsync("You cannot cancel someone else's rps!");
            }
        }

        [Command("rpsLeaderboard")]
        public async Task Board([Remainder]string message)
        {
            int amt;

            if (Int32.TryParse(message, out amt) && amt > 0)
            {
                await Context.Channel.SendMessageAsync(Context.Guild.Name + "'s RPS leaders:");
                Dictionary<string, int> scores = _handler.GenerateLeaderboard(Context.Guild.Name, amt);
                if (scores.Count > 0)
                {
                    foreach (var i in scores)
                    {
                        await Context.Channel.SendMessageAsync(i.Key + ": " + i.Value);
                    }
                }
                else
                {
                    await Context.Channel.SendMessageAsync("No games of rps have been played in this server...");
                }
            }
            else
            {
                await Context.Channel.SendMessageAsync("Invalid number...");
            }
        }

        [Command("rpsLeaderboard")]
        public async Task Board()
        {
            await Context.Channel.SendMessageAsync(Context.Guild.Name + "'s RPS leaders:");
            Dictionary<string, int> scores = _handler.GenerateLeaderboard(Context.Guild.Name, 9);
            if (scores.Count > 0)
            {
                foreach (var i in scores)
                {
                    await Context.Channel.SendMessageAsync(i.Key + ": " + i.Value);
                }
            }
            else
            {
                await Context.Channel.SendMessageAsync("No games of rps have been played in this server...");
            }
        }
    }
}
