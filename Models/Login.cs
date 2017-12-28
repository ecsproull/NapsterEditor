using CsharpSample.App_Code;

namespace CsharpSample.Models
{
    public class Login
    {
        public string LoginSrc { get { return $"https://api.napster.com/oauth/authorize?client_id={AccessProperties.ClientId}&response_type=code&redirect_uri={AccessProperties.CallbackUri}"; } }
        public string ContinueUrl { get; set; }
    }
}
