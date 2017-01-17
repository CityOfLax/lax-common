using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Lax.Helpers.Serialization.Xml {

    public static class XmlDeserializationUtility {

        private static T DeserializeFromByteArrayWithEncoding<T>(byte[] byteArray, Encoding encoding) {
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var memoryStream = new MemoryStream(byteArray)) {
                var streamReader = new StreamReader(memoryStream, encoding);

                return (T) xmlSerializer.Deserialize(streamReader);
            }
        }

        public static T DeserializeFromUTF8ByteArray<T>(byte[] byteArray) =>
            DeserializeFromByteArrayWithEncoding<T>(byteArray, Encoding.UTF8);

        public static T DeserializeFromUTF16ByteArray<T>(byte[] byteArray) =>
            DeserializeFromByteArrayWithEncoding<T>(byteArray, Encoding.Unicode);

    }

}