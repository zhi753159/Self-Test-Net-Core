using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace api_server.Models
{
    public class Customer
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Salt { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public ICollection<Order> Orders { get; set; } = null!;

        public static string HashPassword(string password, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8
            ));
        }
    }
}
