using Microsoft.AspNetCore.Http;
using MVC.Models.ViewModels;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    public class HttpHelper : IHttpHelper
    {
        private readonly IHttpContextAccessor contextAccessor;

        public HttpHelper(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public void SetRegistration(string clientId, RegistrationViewModel registrationViewModel)
        {
            string json = JsonConvert.SerializeObject(registrationViewModel.GetClone());
            contextAccessor.HttpContext.Session.SetString($"registration_{clientId}", json);
        }

        public RegistrationViewModel GetRegistration(string clientId, string email)
        {
            string json = contextAccessor.HttpContext.Session.GetString($"registration_{clientId}");
            if (string.IsNullOrWhiteSpace(json))
                return new RegistrationViewModel(clientId, email);

            return JsonConvert.DeserializeObject<RegistrationViewModel>(json);
        }
    }
}
