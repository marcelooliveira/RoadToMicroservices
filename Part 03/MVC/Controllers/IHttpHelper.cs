using MVC.Models.ViewModels;

namespace MVC.Controllers
{
    public interface IHttpHelper
    {
        RegistrationViewModel GetRegistration(string clientId, string email);
        void SetRegistration(string clientId, RegistrationViewModel registrationViewModel);
    }
}