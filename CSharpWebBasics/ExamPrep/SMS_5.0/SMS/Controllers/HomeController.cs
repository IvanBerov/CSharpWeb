using System.Linq;
using SMS.Data;
using SMS.ViewModels.Products;

namespace SMS.Controllers
{
    using MyWebServer.Controllers;
    using MyWebServer.Http;

    public class HomeController : Controller
    {
        private readonly SMSDbContext data;

        public HomeController(SMSDbContext _data)
        {
            this.data = _data;
        }

        public HttpResponse Index()
        {
            if (!this.User.IsAuthenticated)
            {
                return this.View("Index");
            }
            else
            {
                var user = data.Users.FirstOrDefault(x => x.Id == this.User.Id);

                var products = data.Products
                    .Select(x => new HomeViewProduct
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Price = x.Price
                    })
                    .ToList();

                var model = new HomeAllProductView
                {
                    UserName = user.Username,
                    Products = products
                };

                return this.View("IndexLoggedIn", model);
            }
        }
    }
}