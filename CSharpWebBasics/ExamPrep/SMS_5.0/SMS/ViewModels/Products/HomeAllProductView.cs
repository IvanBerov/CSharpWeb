using System.Collections.Generic;

namespace SMS.ViewModels.Products
{
    public class HomeAllProductView
    {
        public string UserName { get; set; }

        public List<HomeViewProduct> Products { get; set; } = new List<HomeViewProduct>();
    }
}
