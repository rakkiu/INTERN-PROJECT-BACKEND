namespace Application.Interfaces
{
    /// <summary>
    /// Password hashing service interface
    /// </summary>
    public interface IPasswordHashService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}
