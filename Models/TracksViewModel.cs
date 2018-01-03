using CsharpSample.App_Code;
using System.Collections.Generic;

namespace CsharpSample.Models
{
    public class TracksViewModel : BaseViewModel
    {
        public IEnumerable<Track> TrackList { get; set; }
        public bool ShowArtistName { get; set; } = false;
        public bool ShowAlbumName { get; set; } = true;
        public string PlayListName { get; set; }
        public string PlayListId { get; set; }
        public string Token { get { return AccessProperties.Token; } set { } }
        public string RefreshToken { get { return AccessProperties.RefreshToken; } set { } }
        public string ClientId { get { return AccessProperties.ClientId; } set { } }
    }

    // Only used for DeDup-ing tracks. The Tracklist has to be a real list
    public class TracksViewModel2 : BaseViewModel
    {
        public List<Track> TrackList { get; set; }
        public bool ShowArtistName { get; set; } = false;
        public bool ShowAlbumName { get; set; } = true;
        public string PlayListName { get; set; }
        public string PlayListId { get; set; }
        public string Token { get { return AccessProperties.Token; } set { } }
        public string RefreshToken { get { return AccessProperties.RefreshToken; } set { } }
        public string ClientId { get { return AccessProperties.ClientId; } set { } }
    }
} 
