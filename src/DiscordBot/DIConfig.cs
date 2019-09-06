using System;
using System.Reflection;
using Lib.DataSaving;
using Microsoft.Extensions.DependencyInjection;
using Lib.DataHandlers;

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

            var assembly = Assembly.Load(nameof(Lib));
            
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsClass && 
                    !type.IsAbstract && 
                    !type.IsInterface && 
                    type.Namespace.Contains("DataHandlers") &&
                    (type.BaseType.Name != "Object" || type.GetInterfaces().Length != 0))
                {
                    if (type.BaseType.Name != "Object")
                    {
                        serviceCollection.AddSingleton(type.BaseType, type);
                    }
                    else
                    {
                        foreach (Type inter in type.GetInterfaces())
                        {
                            if (inter.Name.Contains(type.Name))
                            {
                                serviceCollection.AddSingleton(inter, type);
                                break;
                            }
                        }
                    }
                }
            }

            return serviceCollection.BuildServiceProvider();
        }
    }
}
