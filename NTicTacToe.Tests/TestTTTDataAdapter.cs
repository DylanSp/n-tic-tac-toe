using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Types;

namespace NTicTacToe.Tests
{
    [TestClass]
    public class TestTTTDataAdapter
    {
        [TestMethod]
        public void SerializeThenDeserialize_ShouldRoundTrip()
        {
            var data = new TicTacToeData();
            var roundTripped = TTTDataAdapter.DeserializeGameData(TTTDataAdapter.SerializeGameData(data));
            Assert.AreEqual(data, roundTripped);
        }

        // note - requires database connection!
        [TestMethod, TestCategory("IntegrationTest")]
        public void TempTests()
        {
            var adapter = new TTTDataAdapter();
            var gameData0 = new TicTacToeData();
            adapter.Save(gameData0);
            var gameDataShouldEqual0 = adapter.Read(gameData0.Id);
            Assert.AreEqual(gameData0, gameDataShouldEqual0);

            gameData0.CurrentPlayer = Player.O;
            adapter.Save(gameData0);

            var gameData1 = new TicTacToeData();
            adapter.Save(gameData1);
            adapter.Delete(gameData1.Id);
        }
    }
}
