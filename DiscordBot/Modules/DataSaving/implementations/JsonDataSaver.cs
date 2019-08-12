using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DiscordBot.Modules.DataSaving.implementations
{
    class JsonDataSaver : IDataSaver
    {
        public T LoadData<T>(string fileName, string folderName)
        {
            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);

            if (!File.Exists(folderName + "/" + fileName))
            {
                return default;
            }
            else
            {
                string json = File.ReadAllText(folderName + "/" + fileName);
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        public void SaveData(object data, string fileName, string folderName)
        {
            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(fileName);

            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(folderName + "/" + fileName, json);
        }
    }
}
