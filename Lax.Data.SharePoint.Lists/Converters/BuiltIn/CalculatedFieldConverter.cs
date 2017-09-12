using System;
using System.Collections.Generic;
using Lax.Data.SharePoint.Lists.MetaModels;
using Lax.Data.SharePoint.Lists.Utils;
using Microsoft.SharePoint.Client;

namespace Lax.Data.SharePoint.Lists.Converters.BuiltIn {

    [SpFieldConverter("Calculated")]
    public partial class CalculatedFieldConverter : MultiTypeFieldConverter {

        private static readonly IReadOnlyDictionary<string, Func<IFieldConverter>> ValueConverters =
            new Dictionary<string, Func<IFieldConverter>> {
                {"Text", () => new TextValueConverter()},
                {"Number", () => new NumberValueConverter()},
                {"Currency", () => new NumberValueConverter()},
                {"DateTime", () => new DateTimeValueConverter()},
                {"Boolean", () => new BoolValueConverter()}
            };

        public override void Initialize(MetaField field) {
            base.Initialize(field);

            if (!field.IsCalculated) {
                throw new ArgumentException("This fields is not a calculated.");
            }

            if (!ValueConverters.ContainsKey(field.OutputType)) {
                throw new ArgumentException($"Output type '{field.OutputType}' is invalid.");
            }

            Internal = ValueConverters[field.OutputType]();
        }

        private static T GetValue<T>(object value) {
            if (value == null) {
                return default(T);
            }

            var errValue = value as FieldCalculatedErrorValue;
            if (errValue != null) {
                throw new ArgumentException("Calculated field value is an error: " + errValue.ErrorMessage);
            }

            Guard.CheckIsObjectAssignableTo<T>(nameof(value), value);
            return (T) value;
        }

    }

}