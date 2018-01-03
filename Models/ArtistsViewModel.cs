using CsharpSample.App_Code;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace CsharpSample.Models
{
    public class ArtistsViewModel : BaseViewModel
    {
        public IEnumerable<Artist> ArtistsList { get; set; } = new List<Artist>();
        public GenrePickerViewModel GenrePickerModel { get; set; }

        public string GetArtistBio(string artistId)
        {
            IEnumerable<IEnumerable<Bio>> bioList = from a in ArtistsList
                            where a.Id == artistId
                            select a.Bios;
            if (bioList.ElementAt(0) != null && bioList.ElementAt(0).ElementAt(0) != null)
            {
                return bioList.ElementAt(0).ElementAt(0).Biography;
            }
            else
            {
                return "No biography available.";
            }
        }
    }
}
