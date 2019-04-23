using Microsoft.AspNetCore.Mvc;
using MVC.Models.ViewModels;

namespace MVC.ViewComponents
{
    public class UserCounterViewComponent : ViewComponent
    {
        public UserCounterViewComponent()
        {

        }

        public IViewComponentResult Invoke(string title, string controllerName, string cssClass, string icon, string count)
        {
            var model = new UserCountViewModel(title, controllerName, cssClass, icon, count);
            return View("Default", model);
        }
    }
}
