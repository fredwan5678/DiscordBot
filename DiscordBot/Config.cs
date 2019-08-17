using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordBot
{
    class Config
    {
        private const string configFolder = "Resources";
        private const string configFile = "config.json";

        public static BotConfig bot;

        static Config()
        {
            if (!Directory.Exists(configFolder))
                Directory.CreateDirectory(configFolder);
             
            if(!File.Exists(configFolder + "/" + configFile))
            {
                bot = new BotConfig();
                string json = JsonConvert.SerializeObject(bot, Formatting.Indented);
                File.WriteAllText(configFolder + "/" + configFile, json);

                Handlers.DataHandler.saver = new Handlers.DataSaving.implementations.JsonDataSaver();
            }
            else
            {
                string json = File.ReadAllText(configFolder + "/" + configFile);
                bot = JsonConvert.DeserializeObject<BotConfig>(json);

                if (bot.dataType == "json") //fix
                {
                    Handlers.DataHandler.saver = new Handlers.DataSaving.implementations.JsonDataSaver();
                }
            }
        }
    }

    public struct BotConfig
    {
        public string token;
        public string cmdPrefix;
        public string dataType;
    }
}
