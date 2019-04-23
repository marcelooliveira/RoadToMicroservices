using Microsoft.AspNetCore.Mvc;
using MVC.Models.ViewModels;
using MVC.Services;

namespace MVC.ViewComponents
{

    public abstract class UserCounterViewComponent : ViewComponent
    {
        protected enum UserCounterType
        {
            Notification = 1,
            Basket = 2
        }

        protected readonly IUserCounterService userCounterService;

        public UserCounterViewComponent(IUserCounterService userCounterService)
        {
            this.userCounterService = userCounterService;
        }

        protected IViewComponentResult Invoke(string title, string controllerName, string cssClass, string icon, int count, UserCounterType userCounterType)
        {
            var model = new UserCountViewModel(title, controllerName, cssClass, icon, count);
            return View("~/Views/Shared/Components/UserCounter/Default.cshtml", model);
        }
    }

    public class NotificationCounterViewComponent : UserCounterViewComponent
    {
        public NotificationCounterViewComponent(IUserCounterService userCounterService) : base(userCounterService) { }

        public IViewComponentResult Invoke(string title, string controllerName, string cssClass, string icon)
        {
            int count = userCounterService.GetNotificationCount();
            return Invoke(title, controllerName, cssClass, icon, count, UserCounterType.Notification);
        }
    }

    public class BasketCounterViewComponent : UserCounterViewComponent
    {
        public BasketCounterViewComponent(IUserCounterService userCounterService) : base(userCounterService) { }

        public IViewComponentResult Invoke(string title, string controllerName, string cssClass, string icon)
        {
            int count = userCounterService.GetBasketCount();
            return Invoke(title, controllerName, cssClass, icon, count, UserCounterType.Basket);
        }
    }
}
