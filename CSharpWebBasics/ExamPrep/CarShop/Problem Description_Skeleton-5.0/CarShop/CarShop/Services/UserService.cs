using System.Linq;
using CarShop.Data;

namespace CarShop.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext data;

        public UserService(ApplicationDbContext _data) 
            => data = _data;

        public bool IsMechanic(string userId) 
            => this.data.Users.Any(u => u.Id == userId && u.IsMechanic);

        public bool OwnsCar(string userId, string carId) 
            => this.data.Cars.Any(c => c.Id == carId && c.OwnerId == userId);

    }
}
