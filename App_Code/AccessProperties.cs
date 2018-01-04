using System;
using System.Threading.Tasks;

namespace CsharpSample.App_Code
{
    public static class AccessProperties
    {
        //TODO: got to Napster Developer site and register your app.
        // Then fill in you ID and Secret
        // DON"T foget to adjust the CallbackUri also.
        public static string Token { get; set; } = null;
        public static string RefreshToken { get; set; } = null;
        public static DateTime ExpirationTime { get; set; }
        public static string ClientId { get { return Your client ID goes here.; } }
        public static string ClientSecret { get { return Your Secret goes here; } }
        public static string CallbackUri { get { return "http://localhost:56182/home/LoginCallback"; } }
        //public static string CallbackUri { get { return "http://edsview.azurewebsites.net/home/LoginCallback"; } }

        public static async Task<string> GetToken()
        {
            if (DateTime.Compare(ExpirationTime, DateTime.Now) != 1)
            {
                await NapsterApiHelper.RefreshTokenAsync(RefreshToken);
            }

            return Token;
        }
    }
}
