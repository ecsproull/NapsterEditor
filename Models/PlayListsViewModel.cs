using CsharpSample.App_Code;
using System.Collections.Generic;

namespace CsharpSample.Models
{
    public class PlayListsViewModel : BaseViewModel
    {
        public IEnumerable<Playlist> Playlists { get; set; }
    }
}
