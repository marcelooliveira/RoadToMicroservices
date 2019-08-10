using Microsoft.AspNetCore.Mvc;
using MVC.Areas.Notification.Services;
using MVC.Models.ViewModels;

namespace MVC.ViewComponents
{
    public abstract class UserCounterViewComponent : ViewComponent
    {
        protected readonly IUserCounterService userCounterService;

        public UserCounterViewComponent(IUserCounterService userCounterService)
        {
            this.userCounterService = userCounterService;
        }

        protected IViewComponentResult Invoke(string title, string controllerName, string cssClass, string icon, int count)
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
            return Invoke(title, controllerName, cssClass, icon, count);
        }
    }

    public class BasketCounterViewComponent : UserCounterViewComponent
    {
        public BasketCounterViewComponent(IUserCounterService userCounterService) : base(userCounterService) { }

        public IViewComponentResult Invoke(string title, string controllerName, string cssClass, string icon)
        {
            int count = userCounterService.GetBasketCount();
            return Invoke(title, controllerName, cssClass, icon, count);
        }
    }
}
