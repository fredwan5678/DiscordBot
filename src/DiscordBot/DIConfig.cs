using System;
using System.Reflection;
using Lib.DataSaving;
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
                        string name;
                        foreach (Type inter in type.GetInterfaces())
                        {
                            name = inter.Name.Substring(1);
                            if (type.Name.Contains(name))
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
