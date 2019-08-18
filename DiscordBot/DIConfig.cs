using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBot.DataSaving;
using DiscordBot.DataSaving.implementations;
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

            return serviceCollection.BuildServiceProvider();
        }
    }
}
