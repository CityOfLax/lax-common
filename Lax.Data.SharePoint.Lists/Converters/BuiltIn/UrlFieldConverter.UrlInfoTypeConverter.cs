using Lax.Data.SharePoint.Abstractions.Models;
using Lax.Data.SharePoint.Lists.MetaModels;
using Microsoft.SharePoint.Client;

namespace Lax.Data.SharePoint.Lists.Converters.BuiltIn {
    internal partial class UrlFieldConverter : MultiTypeFieldConverter {
        private class UrlInfoTypeConverter : IFieldConverter {

            public void Initialize(MetaField field) { }

            public object FromSpValue(object value) {
                if (value == null) {
                    return null;
                }

                var spValue = (FieldUrlValue) value;

                return new UrlInfo {
                    Url = spValue.Url,
                    Description = spValue.Description
                };
            }

            public object ToSpValue(object value) {
                if (value == null) {
                    return null;
                }

                var urlInfo = (UrlInfo) value;

                return new FieldUrlValue {Url = urlInfo.Url, Description = urlInfo.Description};
            }

            public string ToCamlValue(object value) {
                if (value == null) {
                    return "";
                }

                var urlInfo = (UrlInfo) value;
                return string.IsNullOrEmpty(urlInfo.Description)
                    ? urlInfo.Url
                    : $"{urlInfo.Url.Replace(",", ",,")}, {urlInfo.Description}";
            }

        }

    }

}