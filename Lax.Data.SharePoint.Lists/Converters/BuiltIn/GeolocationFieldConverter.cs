using System.Globalization;
using Lax.Data.SharePoint.Abstractions.Models;
using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.MetaModels;
using Lax.Data.SharePoint.Lists.Utils;
using Microsoft.SharePoint.Client;

namespace Lax.Data.SharePoint.Lists.Converters.BuiltIn {

    [SpFieldConverter("Geolocation")]
    [UsedImplicitly]
    internal class GeolocationFieldConverter : IFieldConverter {

        /// <summary>
        /// Initialzes current instance with the specified <see cref="MetaField"/>
        /// </summary>
        /// <param name="field"></param>
        public void Initialize(MetaField field) {
            Guard.CheckNotNull("field", field);
        }

        /// <summary>
        /// Converts SP field value to <see cref="MetaField.MemberType"/>.
        /// </summary>
        /// <param name="value">SP value to convert.</param>
        /// <returns>Member value.</returns>
        public object FromSpValue(object value) {
            if (value == null) {
                return null;
            }

            var spValue = (FieldGeolocationValue) value;

            return new GeoInfo {
                Altitude = spValue.Altitude,
                Latitude = spValue.Latitude,
                Longitude = spValue.Longitude,
                Measure = spValue.Measure
            };
        }

        /// <summary>
        /// Converts <see cref="MetaField.Member"/> value to SP field value.
        /// </summary>
        /// <param name="value">Member value to convert.</param>
        /// <returns>SP field value.</returns>
        public object ToSpValue(object value) {
            if (value == null) {
                return null;
            }

            var geoInfo = (GeoInfo) value;

            return new FieldGeolocationValue {
                Latitude = geoInfo.Latitude,
                Longitude = geoInfo.Longitude,
                Altitude = geoInfo.Altitude,
                Measure = geoInfo.Measure
            };
        }

        /// <summary>
        /// Converts <see cref="MetaField.Member"/> value to SP Caml value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Caml value.</returns>
        public string ToCamlValue(object value) {
            if (value == null) {
                return "";
            }

            var geoInfo = (GeoInfo) value;

            return string.Format(CultureInfo.InvariantCulture, "Point ({0} {1} {2} {3})", geoInfo.Longitude,
                geoInfo.Latitude,
                geoInfo.Altitude, geoInfo.Measure);
        }

    }

}