﻿using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Lax.Data.SharePoint.Abstractions.Models {

    /// <summary>
    /// Represents url info.
    /// </summary>
    [DataContract]
    public class UrlInfo {

        /// <summary>
        /// Gets or sets url.
        /// </summary>
        [DataMember]
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets url description.
        /// </summary>
        [DataMember]
        [JsonProperty("description")]
        public string Description { get; set; }

    }

}