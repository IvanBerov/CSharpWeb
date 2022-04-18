namespace CarShop.Services
{
    public interface IUserService
    {
        bool IsMechanic(string UserId);

        bool OwnsCar(string userId, string carId);
    }
}
