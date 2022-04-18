using SMS.ViewModels.Products;
using SMS.ViewModels.Users;
using System.Collections.Generic;

namespace SMS.Services
{
    public interface IValidator
    {
        ICollection<string> IsRegisterValid(UserRegisterViewModel model);

        ICollection<string> IsLoginValid(bool isUserInDatabase);

        ICollection<string> IsValidFormProduct(CreateProductForm model);
    }
}
