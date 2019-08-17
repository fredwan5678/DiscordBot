namespace DiscordBot.Handlers.DataSaving
{
    public interface IDataSaver
    {
        void SaveData(object data, string fileName, string folderName);

        T LoadData<T>(string fileName, string folderName);
    }
}
