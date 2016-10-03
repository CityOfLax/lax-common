using System.Text;

namespace Lax.Helpers.Serialization.ByteArrays {

    public static class ByteArraySerializationUtility {

        public static byte[] SerializeDefaultStringToByteArray(string value) =>
            Encoding.Default.GetBytes(value);

        public static string SerializeByteArrayToDefaultString(byte[] value) =>
            Encoding.Default.GetString(value);

    }

}