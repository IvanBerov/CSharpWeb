namespace SMS.Services
{
    public interface IPasswordHasher
    {
        string Generate(string password);
    }
}
