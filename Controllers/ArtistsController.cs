using CsharpSample.App_Code;
using CsharpSample.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CsharpSample.Controllers
{
    public class ArtistsController : Controller
    {
        public async Task<ViewResult> GetTopArtists()
        {
            ArtistsViewModel model = new ArtistsViewModel
            {
                ArtistsList = await NapsterApiHelper.GetTopArtistsAsync()
            };

            return View("Artists", model);
        }

        private static ArtistsViewModel artistsModel;
        public async Task<ViewResult> GetArtistsForGenre()
        {
            GenrePickerViewModel pickerModel = new GenrePickerViewModel
            {
                Genres = await NapsterApiHelper.GetGenresAsync(),
                SubmitUrl = "/Artists/DisplayArtistsForGenre",
            };

            artistsModel = new ArtistsViewModel
            {
                GenrePickerModel = pickerModel
            };

            return View("TopArtistsForGenre", artistsModel);
        }

        [HttpPost]
        public async Task<ViewResult> DisplayArtistsForGenre(string SelectedGenreId)
        {
            artistsModel.ArtistsList = await NapsterApiHelper.GetTopArtistsForGenreAsync(SelectedGenreId);
            return View("TopArtistsForGenre", artistsModel);
        }
    }
}
