using System.Collections.Generic;

namespace Lib.DataHandlers
{
    public interface IServerData
    {
        Dictionary<string, string> getServerData(string server);

        void RegisterToServerProfile();
    }
}
