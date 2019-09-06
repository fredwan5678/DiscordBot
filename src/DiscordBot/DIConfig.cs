using System;
using System.Reflection;
using DiscordBot.DataSaving;
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

            /*var assembly = Assembly.Load()
            
            foreach (Type type in assembly.GetTypes())
            {
                if (!type.IsAbstract && !type.IsInterface)
                {
                    serviceCollection.AddSingleton(type.BaseType, type);
                }
            }*/

            return serviceCollection.BuildServiceProvider();
        }
    }
}
