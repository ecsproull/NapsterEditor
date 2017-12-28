using CsharpSample.App_Code;
using CsharpSample.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace CsharpSample.Controllers
{
    // only ued locally.
    public class AccessProps
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string ExpirationTime { get; set; }
        public string ClientId { get; set; }
    }

    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            return View(new Index());
        }

        public async Task<ActionResult> GetGenres()
        {
            GenresModel model = new GenresModel
            {
                Genres = await NapsterApiHelper.GetGenres()
            };

            return View(model);
        }

        private static GenresModel genresModel;
        public async Task<ViewResult> GetStationsForGenre()
        {
            GenresModel model = new GenresModel
            {
                Genres = await NapsterApiHelper.GetGenres()
            };

            genresModel = model;
            return View(model);
        }

        [HttpPost]
        public async Task<ViewResult> DisplayStations(string SelectedGenreId)
        {
            genresModel.Stations = await NapsterApiHelper.GetStationsForGenres(SelectedGenreId);
            return View("GetStationsForGenre", genresModel);
        }

        [HttpPost]
        public async Task SetAccessAsync([FromBody] AccessProps props)
        {
            if (props.Token != null && props.Token != "undefined")
            {
                DateTime expirationTime = DateTime.ParseExact(props.ExpirationTime, "O", CultureInfo.InvariantCulture);
                if (AccessProperties.Token == null || DateTime.Compare(expirationTime, DateTime.Now) != 1)
                {
                    await NapsterApiHelper.RefreshTokenAsync(props.RefreshToken);
                }
            }
        }

        [HttpGet]
        public async Task<string> UpdateAccessAsync(string token, string refresh, string expire)
        {
            if (token != "undefined")
            {
                DateTime expirationTime = DateTime.ParseExact(expire, "O", CultureInfo.InvariantCulture);
                if (AccessProperties.Token == null || DateTime.Compare(expirationTime, DateTime.Now) != 1)
                {
                    await NapsterApiHelper.RefreshTokenAsync(refresh);
                }

                AccessProps props = new AccessProps
                {
                    Token = AccessProperties.Token,
                    RefreshToken = AccessProperties.RefreshToken,
                    ExpirationTime = AccessProperties.ExpirationTime.ToString("O"),
                    ClientId = AccessProperties.ClientId
                };

                return JsonConvert.SerializeObject(props);
            }

            return string.Empty;
        }

        public async Task<ViewResult> GetTracksForMe()
        {
            if (string.IsNullOrWhiteSpace(AccessProperties.Token))
            {
                Login loginModel = new Login();
                loginModel.ContinueUrl = "GetTracksForMe";
                return View("Login", loginModel);
            }

            Tracks model = new Tracks
            {
                TrackList = await NapsterApiHelper.GetNewTracksForMe()
            };
            
            return View("ShowTracks", model);
        }

        public ActionResult Login()
        {
            Login model = new Login();
            model.ContinueUrl = "Index";
            return View(model);
        }

        public async Task<ViewResult> LoginCallback(string code)
        {
            //Anything returned from this function will show up in the IFrame.
            // TODO: Figure a way to navigate to another page outside the IFrame if possible.
            if (await NapsterApiHelper.GetAccessPropertiesAsync(code))
            {
                return View("Success");
            }

            return View(new Login());
        }

        public ViewResult Success()
        {
            return View("Success");
        }

        public ViewResult MainMenu()
        {
            return View();
        }
    }
}