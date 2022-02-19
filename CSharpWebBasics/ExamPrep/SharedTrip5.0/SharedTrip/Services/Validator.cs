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

        public ICollection<string> IsValidTripFormModel(TripsAddFormModel model)
        {
            var errors = new List<string>();

            if (model.Seats < MinSeatsCount || model.Seats > MaxSeatsCount)
            {
                errors.Add($"Seats must be between {MinSeatsCount} and {MaxSeatsCount}");
            }

            if (model.Description.Length > MaxDescriptionLength)
            {
                errors.Add($"Max description length is {MaxDescriptionLength}");
            }

            if (model.StartPoint == null)
            {
                errors.Add($"StartPoint is required");
            }

            if (model.EndPoint == null)
            {
                errors.Add($"EndPoint is required");
            }

            if (model.ImagePath == null)
            {
                errors.Add($"Image is required");
            }

            return errors;
        }
    }
}
