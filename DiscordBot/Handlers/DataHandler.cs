using DiscordBot.Handlers.DataSaving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DiscordBot.Handlers
{
    public abstract class DataHandler
    {
        public static IDataSaver saver { get; set; }
        protected const string dataFolder = "Resources";
    }
}
