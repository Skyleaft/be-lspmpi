namespace be_lspmpi.Services
{
    public interface IEncryptionService
    {
        string GenerateSalt();
        string HashPassword(string password, string salt);
        bool VerifyPassword(string password, string hash);
    }
}