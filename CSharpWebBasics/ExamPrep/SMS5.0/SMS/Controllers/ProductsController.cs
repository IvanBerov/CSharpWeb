using MyWebServer.Controllers;
using MyWebServer.Http;
using SMS.Data;
using SMS.Data.Models;
using SMS.Services;
using SMS.ViewModels.Products;
using System.Linq;

namespace SMS.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IValidator validator;

        private readonly SMSDbContext data;

        public ProductsController(IValidator _validator, SMSDbContext _data)
        {
            this.validator = _validator;
            this.data = _data;
        }

        public HttpResponse Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]
        public HttpResponse Create(CreateProductForm model)
        {
            var errors = this.validator.IsValidFormProduct(model);

            if (errors.Any())
            {
                return View("/Error", errors);
            }

            var product = new Product
            {
                Name = model.Name,
                Price = model.Price
            };

            data.Products.Add(product);

            data.SaveChanges();

            return Redirect("/");
        }
    }
}
