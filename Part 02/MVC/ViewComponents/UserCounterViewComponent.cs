using Microsoft.AspNetCore.Mvc;
using MVC.Models.ViewModels;
using MVC.Services;

namespace MVC.ViewComponents
{
    public enum UserCounterType
    {
        Notification = 1,
        Basket = 2
    }

    public class UserCounterViewComponent : ViewComponent
    {
        protected readonly IUserCounterService userCounterService;

        public UserCounterViewComponent(IUserCounterService userCounterService)
        {
            this.userCounterService = userCounterService;
        }

        public IViewComponentResult Invoke(string title, string controllerName, string cssClass, string icon, UserCounterType userCounterType)
        {
            int count = 0;

            if (userCounterType == UserCounterType.Notification)
            {
                count = userCounterService.GetNotificationCount();
            }
            else if (userCounterType == UserCounterType.Basket)
            {
                count = userCounterService.GetBasketCount();
            }

            var model = new UserCountViewModel(title, controllerName, cssClass, icon, count);
            return View("Default", model);
        }
    }
}
