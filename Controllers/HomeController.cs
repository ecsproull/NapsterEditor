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
        public ViewResult Index(string id = "")
        {
            IndexViewModel model = new IndexViewModel
            {
                ShowNavButton = false
            };

            return View(model);
        }

        public async Task<ActionResult> GetGenres()
        {
            GenresViewModel model = new GenresViewModel
            {
                Genres = await NapsterApiHelper.GetGenresAsync()
            };

            return View(model);
        }

        private static GenresViewModel genresModel;
        public async Task<ViewResult> GetStationsForGenre()
        {
            GenrePickerViewModel pickerModel = new GenrePickerViewModel
            {
                Genres = await NapsterApiHelper.GetGenresAsync(),
                SubmitUrl = "/Home/DisplayStations",
            };

            GenresViewModel model = new GenresViewModel
            {
                Genres = await NapsterApiHelper.GetGenresAsync(),
                GenrePickerModel = pickerModel
            };

            genresModel = model;
            return View(model);
        }

        [HttpPost]
        public async Task<ViewResult> DisplayStations(string SelectedGenreId)
        {
            genresModel.Stations = await NapsterApiHelper.GetStationsForGenresAsync(SelectedGenreId);
            return View("GetStationsForGenre", genresModel);
        }

        [HttpGet]
        public async Task<string> UpdateAccessAsync(string token, string refresh, string expire)
        {
            if (token != "undefined" && token != null && token != AccessProperties.Token)
            {
                DateTime expirationTime = DateTime.ParseExact(expire, "O", CultureInfo.InvariantCulture);
                if (AccessProperties.Token == null || DateTime.Compare(expirationTime, DateTime.Now) != 1)
                {
                    await NapsterApiHelper.RefreshTokenAsync(refresh);
                }
            }

            if (!string.IsNullOrEmpty(AccessProperties.Token))
            {
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
                LoginViewModel loginModel = new LoginViewModel();
                loginModel.BackUrl = "GetTracksForMe";
                return View("Login", loginModel);
            }

            TracksViewModel model = new TracksViewModel
            {
                TrackList = await NapsterApiHelper.GetNewTracksForMeAsync(),
                OnNavigateReturnUrl = "/home/GetTracksForMe",
                ShowArtistName = true
            };
            
            return View("ShowTracks", model);
        }

        public ActionResult Login(string routeValue, string navText)
        {
            LoginViewModel model = new LoginViewModel();

            if (!string.IsNullOrEmpty(routeValue))
            {
                model.BackUrl = routeValue;
                model.NavText = navText;
            }

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

            return View(new LoginViewModel());
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