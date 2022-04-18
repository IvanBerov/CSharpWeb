namespace FootballManager.Services
{
    public interface IPasswordHasher
    {
        string GeneratePassword(string password);
    }
}
