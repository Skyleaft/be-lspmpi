using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

namespace be_lspmpi.Services
{
    public class EncryptionService : IEncryptionService
    {
        public string GenerateSalt()
        {
            var salt = RandomNumberGenerator.GetBytes(16);
            return Convert.ToBase64String(salt);
        }

        public string HashPassword(string password, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = saltBytes,
                DegreeOfParallelism = 1,
                Iterations = 2,
                MemorySize = 1024 * 64
            };
            var hash = argon2.GetBytes(16);
            return Convert.ToBase64String(saltBytes.Concat(hash).ToArray());
        }

        public bool VerifyPassword(string password, string hash)
        {
            var hashBytes = Convert.FromBase64String(hash);
            var salt = hashBytes.Take(16).ToArray();
            var storedHash = hashBytes.Skip(16).ToArray();

            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = 1,
                Iterations = 2,
                MemorySize = 1024 * 64
            };
            var computedHash = argon2.GetBytes(16);
            return storedHash.SequenceEqual(computedHash);
        }
    }
}