using CsharpSample.App_Code;

namespace CsharpSample.Models
{
    public class SearchViewModel : BaseViewModel
    {
        public string SearchString { get; set; } = "Enter search string.";
        public SearchData Data { get; set; }
        public TracksViewModel Tracks { get; set; }
    }
}
