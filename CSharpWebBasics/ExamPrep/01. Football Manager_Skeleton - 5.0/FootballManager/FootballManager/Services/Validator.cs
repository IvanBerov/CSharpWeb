using System.Collections.Generic;
using System.Text.RegularExpressions;
using FootballManager.ViewModels.Players;
using FootballManager.ViewModels.Users;

using static FootballManager.Data.DataConstants;

namespace FootballManager.Services
{
    public class Validator : IValidator
    {
        public ICollection<string> IsValidRegister(UserRegisterForm model)
        {
            var errors = new List<string>();

            if (model.Username.Length < MinUsernameLength || model.Username.Length > MaxUsernameLength)
            {
                errors.Add($"Username must be between {MinUsernameLength} and {MaxUsernameLength} characters long!");
            }

            if (!Regex.IsMatch(model.Email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
            {
                errors.Add($"Email is not a valid email address!");
            }

            if (model.Password.Length < MinPassLength || model.Password.Length > MaxPassLength)
            {
                errors.Add($"Password must be between {MinPassLength} and {MaxPassLength} characters long!");
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

        public ICollection<string> IsValidPlayerFormModel(AddPlayerModel model)
        {
            var errors = new List<string>();

            if (model.FullName.Length < MinPlayerFullName || model.FullName.Length > MaxPlayerNameLength)
            {
                errors.Add($"Player name must be between {MinPlayerFullName} and {MaxPlayerNameLength}!");
            }

            if (model.Description.Length > MaxDescriptionLength)
            {
                errors.Add($"Max description length is {MaxDescriptionLength}!");
            }

            if (model.Position.Length < PositionMinLength || model.Position.Length > PositionMaxLength)
            {
                errors.Add($"Position name must be between {PositionMinLength} and {PositionMaxLength}!");
            }

            if (model.Speed <= 0 || model.Speed > SpeedEnduranceMaxLength)
            {
                errors.Add($"Speed can not be negative or bigger than {SpeedEnduranceMaxLength}");
            }

            if (model.Endurance <= 0 || model.Endurance > SpeedEnduranceMaxLength)
            {
                errors.Add($"Speed can not be negative or bigger than {SpeedEnduranceMaxLength}");
            }

            if (model.ImageUrl == null)
            {
                errors.Add($"Image is required");
            }

            return errors;
        }
    }
}
