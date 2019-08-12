using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules.DataSaving
{
    public interface IDataSaver
    {
        void SaveData(object data, string fileName, string folderName);

        T LoadData<T>(string fileName, string folderName);
    }
}
