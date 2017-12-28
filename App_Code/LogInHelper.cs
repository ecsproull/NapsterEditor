using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace CsharpSample.App_Code
{
    public static class LogInHelper
    {
        public static async Task<bool> GetAccessPropertiesAsync(string code)
        {
            string data = $"client_id={AccessProperties.ClientId}&client_secret={AccessProperties.ClientSecret}&response_type=code&grant_type=authorization_code&code=" + code;
            return await QueryForTokenAsync(data);
        }

        public static async Task<bool> RefreshTokenAsync(string refreshToken)
        {
            string data = $"client_id={AccessProperties.ClientId}&client_secret={AccessProperties.ClientSecret}&response_type=code&grant_type=refresh_token&refresh_token={refreshToken}";
            return await QueryForTokenAsync(data);
        }

        private static async Task<bool> QueryForTokenAsync(string data)
        {
            bool retVal = true;
            try
            {
                byte[] dataStream = Encoding.UTF8.GetBytes(data);
                string encodedData = Encoding.UTF8.GetString(dataStream);
                WebRequest request = WebRequest.Create("https://api.napster.com/oauth/access_token");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = dataStream.Length;
                Stream newStream = request.GetRequestStream();
                newStream.Write(dataStream, 0, dataStream.Length);
                newStream.Close();
                WebResponse response = await request.GetResponseAsync();

                using (Stream stream = response.GetResponseStream())
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(NapsterData));
                    NapsterData obj = (NapsterData)serializer.ReadObject(stream);
                    AccessProperties.Token = obj.AccessProperties;
                    AccessProperties.RefreshToken = obj.RefreshToken;
                    AccessProperties.ExpirationTime = DateTime.Now.AddSeconds(Convert.ToDouble(obj.ExpiresIn));
                }
            }
            catch (Exception)
            {
                retVal = false;
            }

            return retVal;
        }
    }
}
