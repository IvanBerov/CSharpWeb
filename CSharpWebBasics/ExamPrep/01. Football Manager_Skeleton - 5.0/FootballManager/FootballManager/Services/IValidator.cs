using System.Collections.Generic;
using FootballManager.ViewModels.Players;
using FootballManager.ViewModels.Users;

namespace FootballManager.Services
{
    public interface IValidator
    {
        ICollection<string> IsValidRegister(UserRegisterForm model);

        ICollection<string> IsValidLogin(bool isUserInDatabase);

        ICollection<string> IsValidPlayerFormModel(AddPlayerModel model);
    }
}
