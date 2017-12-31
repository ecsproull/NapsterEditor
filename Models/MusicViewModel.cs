using Microsoft.AspNetCore.Mvc.Rendering;
using CsharpSample.App_Code;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CsharpSample.Models
{
    public class MusicViewModel
    {
        private IEnumerable<Genre> genres;

        [Display(Name = "Genres")]
        public string SelectedGenreId { get; set; }
        public IEnumerable<Album> Albums { get; set; } = new List<Album>();
        public Dictionary<string, IEnumerable<Track>> AlbumTracks = new Dictionary<string, IEnumerable<Track>>();
        public async Task<IEnumerable<SelectListItem>> GetGenresListItems()
        {
            List<SelectListItem> GenreListItems = new List<SelectListItem>();
            genres = await NapsterApiHelper.GetGenresAsync();
            foreach (Genre g in genres)
            {
                GenreListItems.Add(new SelectListItem
                {
                    Value = g.Id,
                    Text = g.Name
                });
            }

            return GenreListItems;
        }
    }
}
