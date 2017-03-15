namespace Lax.Helpers.StringComparison {

    public static class StringComparisonExtensions {

        public static bool EqualsWithNull(this string left, string right) =>
            (left == null && right == null) || (left != null && right != null && left.Equals(right));

    }

}