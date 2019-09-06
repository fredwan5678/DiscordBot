using System.IO;
using Newtonsoft.Json;

namespace DiscordBot.DataSaving
{
    class JsonDataSaver : IDataSaver
    {
        public T LoadData<T>(string fileName, string folderName)
        {
            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);

            if (!File.Exists(folderName + "/" + fileName + ".json"))
            {
                return default;
            }
            else
            {
                string json = File.ReadAllText(folderName + "/" + fileName + ".json");
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        public void SaveData<T>(T data, string fileName, string folderName)
        {
            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);

            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(folderName + "/" + fileName + ".json", json);
        }
    }
}
