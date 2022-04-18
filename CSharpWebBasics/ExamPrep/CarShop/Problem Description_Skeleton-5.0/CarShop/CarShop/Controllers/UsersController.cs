using System.Linq;
using CarShop.Data;
using CarShop.Data.Models;
using CarShop.Models.Users;
using CarShop.Services;
using MyWebServer.Controllers;
using MyWebServer.Http;

using static CarShop.Data.DataConstants;

namespace CarShop.Controllers
{
    public class UsersController : Controller
    {
        private readonly IValidator validator;
        private readonly ApplicationDbContext data;
        private readonly IPasswordHasher passwordHasher;

        public UsersController(IValidator _validator, IPasswordHasher _passwordHasher, ApplicationDbContext _data)
        {
            this.validator = _validator;
            this.data = _data;
            this.passwordHasher = _passwordHasher;
        }

        public HttpResponse Register() => View();

        [HttpPost]
        public HttpResponse Register(RegisterUserFormModel model)
        {
            var modelError = this.validator.ValidateUser(model);

            if (this.data.Users.Any(u=> u.Username == model.Username))
            {
                modelError.Add($"User with '{model.Username}' username already exist.");
            }

            if (this.data.Users.Any(u=> u.Email == model.Email))
            {
                modelError.Add($"User with '{model.Email}' e-mail already exist.");
            }

            if (modelError.Any())
            {
                return Error(modelError);
            }

            var user = new User
            {
                Username = model.Username,
                Password = this.passwordHasher.GeneratePassword(model.Password),
                Email = model.Email,
                IsMechanic = model.UserType == UserTypeMechanic
            };

            data.Users.Add(user);

            data.SaveChanges();

            return Redirect("/Users/Login");
        }

        public HttpResponse Login() => View();

        [HttpPost]
        public HttpResponse Login(LoginUserFormModel model)
        {
            var hashedPassword = this.passwordHasher.GeneratePassword(model.Password);

            var userId = this.data
                .Users
                .Where(u => u.Username == model.Username && u.Password == hashedPassword)
                .Select(u => u.Id)
                .FirstOrDefault();

            if (userId == null)
            {
                return Error("Username and password combination ius not valid.");
            }

            this.SignIn(userId);

            return Redirect("/Cars/All");
        }

        [Authorize]
        public HttpResponse Logout()
        {
            this.SignOut();

            return Redirect("/");
        }
    }
}
