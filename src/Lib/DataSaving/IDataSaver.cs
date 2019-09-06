namespace Lib.DataSaving
{
    public interface IDataSaver
    {
        void SaveData<T>(T data, string fileName, string folderName);

        T LoadData<T>(string fileName, string folderName);
    }
}
