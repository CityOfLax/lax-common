using System.Text;

namespace Lax.Helpers.Serialization.ByteArrays {

    public static class ByteArraySerializationUtility {

        public static byte[] SerializeDefaultStringToByteArray(string value) =>
            SerializeUTF8StringToByteArray(value);

        public static string SerializeByteArrayToDefaultString(byte[] value) =>
            SerializeByteArrayToUTF8String(value);

        public static byte[] SerializeUTF8StringToByteArray(string value) =>
            Encoding.UTF8.GetBytes(value);

        public static string SerializeByteArrayToUTF8String(byte[] value) =>
            Encoding.UTF8.GetString(value);

        public static byte[] SerializeUTF16StringToByteArray(string value) =>
            Encoding.Unicode.GetBytes(value);

        public static string SerializeByteArrayToUTF16String(byte[] value) =>
            Encoding.Unicode.GetString(value);

    }

}