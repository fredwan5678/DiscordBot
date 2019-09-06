using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Lib.DataHandlers;

namespace DiscordBot.Modules
{
    public class profiles : ModuleBase<SocketCommandContext>
    {
        public ProfileHandlerBase _handler { get; set; }

        [Command("userProfile")]
        public async Task User(IGuildUser user)
        {
            Dictionary<string, string> data = _handler.GenerateUserProfile(Context.Guild.Name, user.Username);
            await Context.Channel.SendMessageAsync(user.Username + "'s user profile:");

            foreach (var entry in data)
            {
                await Context.Channel.SendMessageAsync(entry.Key + entry.Value);
            }
        }

        [Command("serverProfile")]
        public async Task Server()
        {
            Dictionary<string, string> data = _handler.GenerateServerProfile(Context.Guild.Name);
            await Context.Channel.SendMessageAsync(Context.Guild.Name + "'s server profile:");

            foreach (var entry in data)
            {
                await Context.Channel.SendMessageAsync(entry.Key + entry.Value);
            }
        }

        [Command("warn")]
        [RequireBotPermission(Discord.GuildPermission.KickMembers)]
        [RequireUserPermission(Discord.GuildPermission.KickMembers)]
        public async Task Warn(IGuildUser user)
        {
            int warns = _handler.AddWarn(Context.Guild.Name, user.Username);
            await Context.Channel.SendMessageAsync(user.Username + " was warned and now has " + warns.ToString() + " warnings!");
            if (warns >= 5)
            {
                await user.KickAsync("The user has reached 5 warnings!!");
            }
        }

        [Command("unwarn")]
        [RequireBotPermission(Discord.GuildPermission.KickMembers)]
        [RequireUserPermission(Discord.GuildPermission.KickMembers)]
        public async Task Unwarn(IGuildUser user)
        {
            int warns = _handler.RemoveWarn(Context.Guild.Name, user.Username);
            await Context.Channel.SendMessageAsync(user.Username + " was unwarned and now has " + warns.ToString() + " warnings!");
        }
    }
}
