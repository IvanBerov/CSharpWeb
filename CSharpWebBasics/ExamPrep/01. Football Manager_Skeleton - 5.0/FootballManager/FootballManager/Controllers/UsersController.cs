using System.Linq;
using FootballManager.Data;
using FootballManager.Data.Models;
using FootballManager.Services;
using FootballManager.ViewModels.Users;
using MyWebServer.Controllers;
using MyWebServer.Http;

namespace FootballManager.Controllers
{
    public class UsersController : Controller
    {
        private readonly IValidator validator;

        private readonly IPasswordHasher passwordHasher;

        private readonly FootballManagerDbContext data;

        public UsersController(IValidator _validator, IPasswordHasher _passwordHasher, FootballManagerDbContext _data)
        {
            this.validator = _validator;
            this.passwordHasher = _passwordHasher;
            this.data = _data;
        }

        public HttpResponse Register() => this.View();

        [HttpPost]
        public HttpResponse Register(UserRegisterForm model)
        {
            var errors = this.validator.IsValidRegister(model);

            var emailExist = this.data
                .Users
                .FirstOrDefault(u => u.Email == model.Email) != null ? true : false;

            if (emailExist)
            {
                errors.Add($"User with the same email address already exist!");
            }

            if (errors.Any())
            {
                return View("/Error", errors);
            }

            var user = new User()
            {
                Username = model.Username,
                Password = this.passwordHasher.GeneratePassword(model.Password),
                Email = model.Email
            };

            data.Users.Add(user);

            data.SaveChanges();

            return Redirect("/Users/Login");
        }

        public HttpResponse Login() => this.View();

        [HttpPost]
        public HttpResponse Login(UserLoginForm model)
        {
            var userId = data.Users
                .Where(u => u.Username == model.Username && passwordHasher.GeneratePassword(model.Password) == u.Password)
                .Select(u => u.Id)
                .FirstOrDefault();

            var errors = validator
                .IsValidLogin(userId != null);

            if (errors.Any())
            {
                return View("/Error", errors);
            }

            this.SignIn(userId);

            return Redirect("/Players/All");
        }

        public HttpResponse LogOut()
        {
            this.SignOut();

            return Redirect("/");
        }
    }
}
