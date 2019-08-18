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
