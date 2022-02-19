using System.Linq;
using MyWebServer.Controllers;
using MyWebServer.Http;
using SMS.Data;
using SMS.ViewModels.Carts;

namespace SMS.Controllers
{
    public class CartsController : Controller
    {
        private readonly SMSDbContext data;

        public CartsController(SMSDbContext _data)
        {
            this.data = _data;
        }

        public HttpResponse Details()
        {
            var user = this.data
                .Users
                .FirstOrDefault(u => u.Id == User.Id);

            var products = this.data
                .Products
                .Where(c => c.CartId == user.CartId)
                .Select(p => new CartDetailsProduct
                {
                    Name = p.Name,
                    Price = p.Price,
                })
                .ToList();

            return this.View(products);
        }

        public HttpResponse AddProduct(string productId)
        {
            var product = this.data.Products
                .FirstOrDefault(p => p.Id == productId);

            var cart = data.Carts
                .FirstOrDefault(c => c.UserId == User.Id);

            product.CartId = cart.Id;

            this.data.SaveChanges();

            return Redirect("/Carts/Details");
        }

        public HttpResponse Buy()
        {
            var cart = this.data.Carts
                .FirstOrDefault(c => c.UserId == User.Id);

            var products = this.data.Products
                .Where(p => p.CartId == cart.Id)
                .ToList();

            foreach (var product in products)
            {
                product.CartId = null;
            }

            this.data.SaveChanges();
            
            return Redirect("/");
        }
    }
}
