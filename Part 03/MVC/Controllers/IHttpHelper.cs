using MVC.Models.ViewModels;

namespace MVC.Controllers
{
    public interface IHttpHelper
    {
        RegistrationViewModel GetRegistration(string clientId);
        void SetRegistration(string clientId, RegistrationViewModel registrationViewModel);
    }
}