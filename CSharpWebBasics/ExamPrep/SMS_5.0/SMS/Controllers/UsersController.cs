using MyWebServer.Controllers;
using MyWebServer.Http;
using SMS.Data;
using SMS.Data.Models;
using SMS.Services;
using System.Linq;
using SMS.ViewModels.Users;

namespace SMS.Controllers
{
    public class UsersController : Controller
    {
        private readonly IValidator validator;
        private readonly SMSDbContext data;
        private readonly IPasswordHasher hasher;

        public UsersController(IValidator _validator, SMSDbContext _data, IPasswordHasher _hasher)
        {
            this.validator = _validator;
            this.data = _data;
            this.hasher = _hasher;
        }

        public HttpResponse Register()
        {
            return View();
        }

        [HttpPost]
        public HttpResponse Register(UserRegisterViewModel model)
        {
            var modelErrors = this.validator.IsRegisterValid(model);

            if (this.data.Users.Any(u=>u.Username == model.Username))
            {
                modelErrors.Add($"Username {model.Username} already exist!");
            }

            if (modelErrors.Any())
            {
                return View("/Error", modelErrors);
            }

            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                Password = this.hasher.Generate(model.Password)
            };

            var cart = new Cart
            {
                UserId = user.Id
            };

            user.Cart = cart;

            this.data.Users.Add(user);

            this.data.Carts.Add(cart);

            this.data.SaveChanges();

            return this.View("/Users/Login");
        }

        public HttpResponse Login()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(UserLoginViewModel model)
        {
            var userId = this.data.Users
                .Where(u => u.Username == model.Username && u.Password == this.hasher.Generate(model.Password))
                .Select(u => u.Id)
                .FirstOrDefault();

            var modelErrors = this.validator.IsLoginValid(userId != null);

            if (modelErrors.Any())
            {
                return this.View("/Error", modelErrors);
            }

            this.SignIn(userId);

            return this.Redirect("/");
        }

        public HttpResponse LogOut()
        {
            this.SignOut();

            return Redirect("/");
        }
    }
}
