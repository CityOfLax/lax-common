using System;
using System.IO;
using System.Text;

namespace Lax.Data.SharePoint.Taxonomy {

    public static class JsonHelper {

        public static string Serialize<T>(T obj) {
            var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
            var ms = new MemoryStream();
            serializer.WriteObject(ms, obj);
            var retVal = Encoding.Default.GetString(ms.ToArray());
            ms.Dispose();
            return retVal;
        }

        public static T Deserialize<T>(string json) {
            var obj = Activator.CreateInstance<T>();
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json))) {
                var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
                obj = (T)serializer.ReadObject(ms);
                return obj;
            }
        }
    }

}