using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using DiscordBotTest.Mock;
using Lib.Util;
using Lib.DataSaving;
using Lib.DataHandlers;

namespace DiscordBotTest
{
    public class RpsHandlerTest
    {
        private const string SERVER = "Test_Server";
        private const string TEST_PLAYER_1 = "plr1";
        private const string TEST_PLAYER_2 = "plr2";

        private readonly ProfileHandlerBase TEST_PROFILE_HANDLER = new TestProfileHandler();

        private readonly IDataSaver saver = new JsonDataSaver();

        [Theory]
        [InlineData(outcome.P1)]
        [InlineData(outcome.P2)]
        [InlineData(outcome.TIE)]
        public void AddGame_ShouldAddCorrectOutcomeToData(outcome outcome)
        {
            IDataSaver TEST_SAVER = new RpsTestDataSaver();
            RpsHandler rpsHandler = new RpsHandler(TEST_SAVER, TEST_PROFILE_HANDLER);

            //Assert.
        }
    }
}
