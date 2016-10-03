using System.Linq;

namespace Lax.Helpers.StringManipulation {

    public static class StringManipulationExtensions {

        public static string TakeFirst(this string value, int numberOfCharactersToTake)
            => value.Take(numberOfCharactersToTake).Aggregate("", (a, b) => a + b.ToString());

    }

}