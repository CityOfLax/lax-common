using System.IO;
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

    }

}