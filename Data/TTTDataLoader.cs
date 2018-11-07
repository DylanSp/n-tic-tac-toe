using System.IO;
using System.Xml.Serialization;

namespace Data
{
    public class TTTDataLoader
    {
        public byte[] SerializeGameData(TicTacToeData data)
        {
            var serializer = new XmlSerializer(typeof(TicTacToeData));
            using (var memStream = new MemoryStream())
            {
                serializer.Serialize(memStream, data);
                return memStream.ToArray();
            }
        }
    }
}
