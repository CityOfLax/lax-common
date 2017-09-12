using System.Linq;

namespace Lax.Helpers.Strings {

    public static class StringHelper {

        public static string GetTrailingNumbers(string input) => new string(input.Where(c => char.IsDigit(c)).ToArray());

    }

}