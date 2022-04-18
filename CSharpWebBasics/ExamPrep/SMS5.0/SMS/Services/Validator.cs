using System.Collections.Generic;
using System.Text.RegularExpressions;
using SMS.ViewModels.Products;
using SMS.ViewModels.Users;

using static SMS.Data.DataConstants;

namespace SMS.Services
{
    public class Validator : IValidator
    {
        public ICollection<string> IsRegisterValid(UserRegisterViewModel model)
        {
            var errors = new List<string>();

            if (model.Username.Length < MinUsernameLength || model.Username.Length > MaxUserLength)
            {
                errors.Add($"Username must be between {MinUsernameLength} and {MaxUserLength} characters lang!");
            }

            if (Regex.IsMatch(model.Email, "@\"^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$\""))
            {
                errors.Add("Email field is not a valid e-mail address!");
            }

            if (model.Password.Length < MinUserPasswordLength || model.Password.Length > MaxUserLength )
            {
                errors.Add($"Password must be between {MinUserPasswordLength} and {MaxUserLength} characters long!");
            }

            if (model.Password != model.ConfirmPassword)
            {
                errors.Add("Passwords must be the same!");
            }

            return errors;
        }

        public ICollection<string> IsLoginValid(bool isUserInDatabase)
        {
            var errors = new List<string>();

            if (!isUserInDatabase)
            {
                errors.Add("Wrong Password or Username!");
                errors.Add("User does not exist!");
            }

            return errors;
        }

        public ICollection<string> IsValidFormProduct(CreateProductForm model)
        {
            var errors = new List<string>();

            if (model.Name.Length < MinNameLength || model.Name.Length > MaxUserLength)
            {
                errors.Add($"Product {model.Name} is not correct product name!");

                errors.Add($"Must be between {MinNameLength} and {MaxUserLength}.");
            }
            if (model.Price < MinPrice || model.Price > MaxPrice)
            {
                errors.Add($"Price must be bewtween {MinPrice} and {MaxPrice}.");
            }

            return errors;
        }
    }
}
