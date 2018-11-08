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
            // Assert.AreEqual(data, roundTripped);
            Assert.IsTrue(data.Equals(roundTripped));
        }

        // note - requires database connection!
        [TestMethod]
        public void TempTests()
        {
            var adapter = new TTTDataAdapter();
            int createdId0 = adapter.Create();
            int createdId1 = adapter.Create();
            var gameData0 = adapter.Read(createdId0);
            gameData0.CurrentPlayer = Player.O;
            adapter.Update(gameData0);
            adapter.Delete(createdId1);
        }
    }
}
