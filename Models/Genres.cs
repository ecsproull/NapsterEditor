using CsharpSample.App_Code;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CsharpSample.Models
{
    public class GenresModel
    {
        [Display(Name = "Genres")]
        public string SelectedGenreId { get; set; }

        public IEnumerable<Station> Stations { get; set; } = new List<Station>();
        public IEnumerable<Genre> Genres { get; set; }

        public IEnumerable<SelectListItem> GetGenresListItems()
        {
            List<SelectListItem> GenreListItems = new List<SelectListItem>();

            foreach (Genre g in Genres)
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
