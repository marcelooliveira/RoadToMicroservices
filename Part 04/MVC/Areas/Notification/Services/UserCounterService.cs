﻿namespace MVC.Areas.Notification.Services
{
    public class UserCounterService : IUserCounterService
    {
        public int GetNotificationCount()
        {
            return 7;
        }

        public int GetBasketCount()
        {
            return 9;
        }
    }
}
