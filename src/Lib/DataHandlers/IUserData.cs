using System.Collections.Generic;

namespace Lib.DataHandlers
{
    public interface IUserData
    {
        Dictionary<string, string> getUserData(string server, string user);

        void RegisterToProfile();
    }
}
