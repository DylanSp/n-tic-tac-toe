using Interfaces;
using System.IO;
using System.Xml.Serialization;

namespace Data
{
    public class Serializer
    {
        private readonly XmlSerializer InnerSerializer;

        public Serializer()
        {
            InnerSerializer = new XmlSerializer(typeof(TicTacToeData));
        }

        public string SerializeGameData(ITicTacToeData data)
        {
            using (var stringWriter = new StringWriter())
            {
                InnerSerializer.Serialize(stringWriter, data);
                return stringWriter.ToString();
            }
        }

        public ITicTacToeData DeserializeGameData(string data)
        {
            using (var reader = new StringReader(data))
            {
                return InnerSerializer.Deserialize(reader) as TicTacToeData;
            }
        }
    }
}
