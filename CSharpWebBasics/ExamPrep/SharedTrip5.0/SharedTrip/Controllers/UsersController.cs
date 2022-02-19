using System.Linq;
using MyWebServer.Controllers;
using MyWebServer.Http;
using SharedTrip.Data;
using SharedTrip.Data.Models;
using SharedTrip.Services;
using SharedTrip.ViewModels;

namespace SharedTrip.Controllers
{
    public class UsersController : Controller
    {
        private readonly IValidator validator;

        private readonly IPasswordHasher passwordHasher;

        private readonly ApplicationDbContext data;

        public UsersController(IValidator _validator, IPasswordHasher _passwordHasher, ApplicationDbContext _data)
        {
            this.validator = _validator;
            this.passwordHasher = _passwordHasher;
            this.data = _data;
        }

        public HttpResponse Login() => this.View();

        [HttpPost]
        public HttpResponse Login(UserLoginForm model)
        {
            var userId = data.Users
                .Where(u=> u.UserName == model.UserName && passwordHasher.GeneratePassword(model.Password) == u.Password)
                .Select(u=> u.Id)
                .FirstOrDefault();

            var errors = validator
                .IsValidLogin(userId != null);

            if (errors.Any())
            {
                return View("/Error", errors);
            }

            this.SignIn(userId);

            return Redirect("/Trips/All");
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
                UserName = model.UserName,
                Password = this.passwordHasher.GeneratePassword(model.Password),
                Email = model.Email
            };

            data.Users.Add(user);

            data.SaveChanges();

            return Redirect("/Users/Login");
        }

        public HttpResponse LogOut()
        {
            this.SignOut();

            return Redirect("/");
        }
    }
}
