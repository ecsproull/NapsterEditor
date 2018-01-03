using CsharpSample.App_Code;
using CsharpSample.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CsharpSample.Controllers
{
    public class SearchController : Controller
    {
        private static SearchViewModel lastSearchModel {get;set;}

        public ViewResult Index()
        {
            return View("Search", new SearchViewModel());
        }

        public async Task<ViewResult> ShowAlbumDetails (string albumId, string returnUrl)
        {
            TracksViewModel tracks = new TracksViewModel
            {
                TrackList = await NapsterApiHelper.GetTracksAsync(albumId),
                ShowAlbumName = false,
                ShowArtistName = true
            };

            AlbumViewModel model = new AlbumViewModel
            {
                AlbumDetails = await NapsterApiHelper.GetAlbumAsync(albumId),
                Tracks = tracks,
                NavText = "Back",
                BackUrl = returnUrl,
            };

            return View("Album", model);
        }


        [HttpPost]
        public async Task<ViewResult> DoSearch(SearchViewModel m)
        {
            lastSearchModel = m;
            SearchViewModel model = new SearchViewModel
            {
                Data = await NapsterApiHelper.SearchAsync(m.SearchString, QueryType.all)
            };

            //Todo: create partial views for each data set and then remove Data from the model.
            model.Tracks = new TracksViewModel
            {
                TrackList = model.Data.Tracks,
                OnNavigateReturnUrl = "/Search/ReturnToSearch"
            };

            return View("Search", model);
        }

        public async Task<ViewResult> ReturnToSearch()
        {
           if (lastSearchModel != null)
            {
                return await DoSearch(lastSearchModel);
            }

            return Index();
        }
    }
}
