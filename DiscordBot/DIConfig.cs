using System;
using DiscordBot.DataSaving;
using DiscordBot.DataSaving.implementations;
using DiscordBot.DataHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot
{
    public class DIConfig
    {
        public static IServiceProvider GetServiceProvider()
        {
            ServiceCollection serviceCollection = new ServiceCollection();

            if (Config.bot.dataType == "json")
            {
                serviceCollection.AddSingleton<IDataSaver, JsonDataSaver>();
            }
            serviceCollection.AddSingleton<QuoteHandler>();
            serviceCollection.AddSingleton<RpsHandler>();
            serviceCollection.AddSingleton<ProfileHandler>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
