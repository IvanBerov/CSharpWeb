namespace CarShop.Services
{
    public interface IPasswordHasher
    {
        string GeneratePassword(string password);
    }
}
