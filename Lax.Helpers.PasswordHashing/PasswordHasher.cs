namespace Lax.Helpers.PasswordHashing {

    public static class PasswordHasher {

        public static string HashPassword(string password) =>
            BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));

        public static bool ValidatePassword(string password, string hash) =>
            BCrypt.Net.BCrypt.Verify(password, hash);

    }

}
