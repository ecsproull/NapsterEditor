using CsharpSample.App_Code;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CsharpSample.Models
{
    public class GenrePickerViewModel : BaseViewModel
    {
        [Display(Name = "Genres")]
        public string SelectedGenreId { get; set; }
        public string SubmitUrl { get; set; }
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
