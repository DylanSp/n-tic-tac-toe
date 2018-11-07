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
    }
}
