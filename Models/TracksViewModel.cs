using CsharpSample.App_Code;
using System.Collections.Generic;

namespace CsharpSample.Models
{
    public class TracksViewModel
    {
        public IEnumerable<Track> TrackList { get; set; }
        public string PlayListName { get; set; }
        public string PlayListId { get; set; }
        public string Token { get { return AccessProperties.Token; } set { } }
        public string RefreshToken { get { return AccessProperties.RefreshToken; } set { } }
        public string ClientId { get { return AccessProperties.ClientId; } set { } }
    }
} 
