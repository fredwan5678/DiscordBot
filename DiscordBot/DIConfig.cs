using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            return serviceCollection.BuildServiceProvider();
        }
    }
}
