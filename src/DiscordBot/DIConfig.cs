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
            serviceCollection.AddSingleton<IQuoteHandler, QuoteHandler>();
            serviceCollection.AddSingleton<RpsHandlerBase, RpsHandler>();
            serviceCollection.AddSingleton<ProfileHandlerBase, ProfileHandler>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
