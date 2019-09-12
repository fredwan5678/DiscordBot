using System;
using System.Collections.Generic;
using System.Text;
using Lib.DataSaving;
using Lib.DataHandlers;
using Lib.Util;
using Newtonsoft.Json;
using System.IO;

namespace DiscordBotTest.Mock
{
    class RpsTestDataSaver : IDataSaver
    {
        private string TEST_FOLDER = "TestResults";
        private string TEST_FILE = "RpsDataSaverTestResult";

        private Dictionary<string, Player> _testNames = new Dictionary<string, Player>();
        private SortedSet<Player> _testScores = new SortedSet<Player>();

        public T LoadData<T>(string fileName, string folderName)
        {
            Leaderboard testData = new Leaderboard();
            testData.leaderboardNames = _testNames;
            testData.leaderboardScores = _testScores;

            try
            {
                return (T)Convert.ChangeType(testData, typeof(T));
            }
            catch (InvalidCastException)
            {
                return default;
            }
        }

        public void SaveData<T>(T data, string fileName, string folderName)
        {
            if (!Directory.Exists(TEST_FOLDER))
                Directory.CreateDirectory(TEST_FOLDER);

            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(TEST_FOLDER + "/" + TEST_FILE + ".json", json);
        }
    }
}
