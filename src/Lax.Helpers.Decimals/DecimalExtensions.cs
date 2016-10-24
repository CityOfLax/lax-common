namespace Lax.Helpers.Decimals {

    public static class DecimalExtensions {

        public static decimal Abs(this decimal value) {
            if (value > 0m) {
                return value;
            }
            return decimal.Negate(value);
        }

    }

}
