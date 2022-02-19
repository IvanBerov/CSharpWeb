namespace SharedTrip.Services
{
    public interface IPasswordHasher
    {
        public string GeneratePassword(string password);
    }
}
