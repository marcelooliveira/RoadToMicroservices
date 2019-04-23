namespace MVC.Models.ViewModels
{
    public class UserCountViewModel
    {   
        public UserCountViewModel(string title, string controllerName, string cssClass, string icon, string count)
        {
            Title = title;
            ControllerName = controllerName;
            CssClass = cssClass;
            Icon = icon;
            Count = count;
        }

        public string ControllerName { get; set; }
        public string Title { get; set; }
        public string CssClass { get; set; }
        public string Icon { get; set; }
        public string Count { get; set; }
    }
}
