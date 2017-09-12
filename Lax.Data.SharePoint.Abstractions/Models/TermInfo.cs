using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Lax.Data.SharePoint.Abstractions.Models {
    
    [DataContract]
    public class TermInfo : IEquatable<TermInfo> {

        [DataMember]
        [JsonProperty("termGuid")]
        public string TermGuid { get; set; }

        [DataMember]
        [JsonProperty("termSetId")]
        public string TermSetId { get; set; }

        [DataMember]
        [JsonProperty("label")]
        public string Label { get; set; }

        [DataMember]
        [JsonProperty("wssId")]
        public int WssId { get; set; }


        public bool Equals(TermInfo other) =>
            TermGuid.Equals(other.TermGuid);

        public static bool operator ==(TermInfo left, TermInfo right) {
            if (ReferenceEquals(left, right)) {
                return true;
            }

            if (ReferenceEquals(null, left) || ReferenceEquals(null, right)) {
                return false;
            }

            return left.Equals(right);
        }

        public static bool operator !=(TermInfo left, TermInfo right) => !(left == right);

    }

}