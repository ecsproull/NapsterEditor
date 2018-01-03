using CsharpSample.App_Code;

namespace CsharpSample.Models
{
    public class AlbumViewModel : BaseViewModel
    {
        public Album AlbumDetails { get; set; }
        public TracksViewModel Tracks { get; set; }
    }
}
