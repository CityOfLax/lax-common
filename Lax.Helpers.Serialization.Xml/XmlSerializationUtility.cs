using System.IO;
using System.Text;
using System.Xml.Serialization;
using Lax.Helpers.Serialization.ByteArrays;

namespace Lax.Helpers.Serialization.Xml {

    public static class XmlSerializationUtility {

        public static byte[] SerializeToByteArray<T>(T obj) =>
            ByteArraySerializationUtility.SerializeDefaultStringToByteArray(SerializeToString(obj));

        public static string SerializeToString<T>(T obj) {
            var xmlSerializer = new XmlSerializer(typeof(T));
            var stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, obj);
            return stringWriter.ToString();
        }

        private static byte[] SerializeToByteArrayWithEncoding<T>(T obj, Encoding encoding) {
            var memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream, encoding);
            var xmlSerializer = new XmlSerializer(typeof(T));

            xmlSerializer.Serialize(streamWriter, obj);

            return memoryStream.ToArray();
        }

        public static byte[] SerializeToUTF8ByteArray<T>(T obj) =>
            SerializeToByteArrayWithEncoding(obj, Encoding.UTF8);

        public static byte[] SerializeToUTF16ByteArray<T>(T obj) =>
            SerializeToByteArrayWithEncoding(obj, Encoding.Unicode);

        public static string SerializeToUTF8String<T>(T obj) =>
            ByteArraySerializationUtility.SerializeByteArrayToUTF8String(SerializeToUTF8ByteArray(obj));

        public static string SerializeToUTF16String<T>(T obj) =>
            ByteArraySerializationUtility.SerializeByteArrayToUTF16String(SerializeToUTF16ByteArray(obj));

    }

}