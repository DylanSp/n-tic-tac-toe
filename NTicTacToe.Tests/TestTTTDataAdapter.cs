using System;
using Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var gameData0 = adapter.Create();
            var gameData1 = adapter.Create();
            var gameDataShouldEqual0 = adapter.Read(gameData0.Id);
            gameData0.CurrentPlayer = Player.O;
            adapter.Update(gameData0);
            adapter.Delete(gameData1.Id);
        }
    }
}
