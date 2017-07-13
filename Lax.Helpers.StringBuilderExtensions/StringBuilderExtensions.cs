using System.Linq;
using System.Text;

namespace Lax.Helpers.StringBuilderExtensions {

    public static class StringBuilderExtensions {

        public static StringBuilder AppendIfNotEmptyOrNull(this StringBuilder stringBuilder, params string[] values) {

            if (values.All(v => !string.IsNullOrEmpty(v))) {
                stringBuilder.Append(values.Aggregate("", (x, y) => $"{x}{y}"));
            }

            return stringBuilder;

        }

    }

}