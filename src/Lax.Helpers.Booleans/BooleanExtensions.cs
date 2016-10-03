namespace Lax.Helpers.Booleans {

    public static class BooleanExtensions {

        public static string ToStringIfTrue(this bool boolean, string value) => boolean ? value : "";

        public static string ToStringIfFalse(this bool boolean, string value) => (!boolean).ToStringIfTrue(value);

    }

}