using System.Collections.Generic;
using System.Text.RegularExpressions;
using SharedTrip.ViewModels;

using static SharedTrip.Data.DataConstants;

namespace SharedTrip.Services
{
    public class Validator : IValidator
    {
        public ICollection<string> IsValidRegister(UserRegisterForm model)
        {
            var errors = new List<string>();

            if (model.UserName.Length < MinUsernameLength || model.UserName.Length > MaxLengthModels)
            {
                errors.Add($"Username must be between {MinUsernameLength} and {MaxLengthModels} characters long!");
            }

            if (!Regex.IsMatch(model.Email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
            {
                errors.Add($"Email is not a valid email address!");
            }

            if (model.Password.Length < MinUserPasswordLength || model.Password.Length > MaxLengthModels)
            {
                errors.Add($"Password must be between {MinUserPasswordLength} and {MaxLengthModels} characters long!");
            }

            if (model.Password != model.ConfirmPassword)
            {
                errors.Add($"Passwords must be equal!");
            }

            return errors;
        }

        public ICollection<string> IsValidLogin(bool isUserInDatabase)
        {
            var errors = new List<string>();

            if (!isUserInDatabase)
            {
                errors.Add("Wrong Password or Username!");

                errors.Add("User does not exist!");
            }

            return errors;
        }
    }
}
