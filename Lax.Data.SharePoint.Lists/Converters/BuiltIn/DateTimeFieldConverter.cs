using System;
using System.Collections.Generic;
using System.Text;
using Lax.Data.SharePoint.Lists.CodeAnnotations;
using Lax.Data.SharePoint.Lists.MetaModels;

namespace Lax.Data.SharePoint.Lists.Converters.BuiltIn {

    [SpFieldConverter("DateTime")]
    [UsedImplicitly]
    internal partial class DateTimeFieldConverter : MultiTypeFieldConverter {

        private static readonly IReadOnlyDictionary<Type, Func<IFieldConverter>> TypeConverters =
            new Dictionary<Type, Func<IFieldConverter>> {
                {typeof(DateTime), () => new DateTimeTypeConverter()},
                {typeof(DateTime?), () => new NullableDateTimeTypeConverter()}
            };

        public override void Initialize(MetaField field) {
            base.Initialize(field);
            if (TypeConverters.ContainsKey(field.MemberType)) {
                Internal = TypeConverters[field.MemberType]();
            } else {
                throw new ArgumentException("Member type should be DateTime or Syste.Nullable<DateTime>.");
            }
        }

        /// <summary>
        /// Converts a system DateTime value to ISO8601 DateTime format (yyyy-mm-ddThh:mm:ssZ).
        /// </summary>
        /// <returns>
        /// A string that contains the date and time in ISO8601 DateTime format.
        /// </returns>
        /// <param name="dtValue">A System.DateTime object that represents the system DateTime value in the form mm/dd/yyyy hh:mm:ss AM or PM.</param>
        private static string CreateIsoDate(DateTime dtValue) {
            var sb = new StringBuilder();
            sb.Append(dtValue.Year.ToString("0000"));
            sb.Append("-");
            sb.Append(dtValue.Month.ToString("00"));
            sb.Append("-");
            sb.Append(dtValue.Day.ToString("00"));
            sb.Append("T");
            sb.Append(dtValue.Hour.ToString("00"));
            sb.Append(":");
            sb.Append(dtValue.Minute.ToString("00"));
            sb.Append(":");
            sb.Append(dtValue.Second.ToString("00"));
            sb.Append("Z");
            return sb.ToString();
        }

        private static DateTime ToLocalTime(DateTime dateTime) => dateTime.Kind == DateTimeKind.Unspecified
            ? new DateTime(dateTime.Ticks, DateTimeKind.Local)
            : dateTime.ToLocalTime();

        private static DateTime? ToLocalTime(DateTime? dateTime) => dateTime.HasValue
            ? dateTime.Value.Kind == DateTimeKind.Unspecified
                ? new DateTime(dateTime.Value.Ticks, DateTimeKind.Local)
                : dateTime.Value.ToLocalTime()
            : (DateTime?) null;

    }

}