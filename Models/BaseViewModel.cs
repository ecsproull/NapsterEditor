namespace CsharpSample.Models
{
    public class BaseViewModel
    {
        public string BackUrl { get; set; } = "/Home/Index";
        public string OnNavigateReturnUrl { get; set; }
        public string NavText { get; set; } = "Menu";
        public bool ShowNavButton { get; set; } = true;
    }
}
