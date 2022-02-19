using System.Collections.Generic;
using SharedTrip.ViewModels;

namespace SharedTrip.Services
{
    public interface IValidator
    {
        ICollection<string> IsValidRegister(UserRegisterForm model);

        ICollection<string> IsValidLogin(bool isUserInDatabase);

        ICollection<string> IsValidTripFormModel(TripsAddFormModel model);
    }
}
