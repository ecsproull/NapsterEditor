using CsharpSample.App_Code;
using System.Collections.Generic;

namespace CsharpSample.Models
{
    public class Action
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class IndexViewModel
    {
        public List<Action> Actions { get; set; } = new List<Action>();

        public IndexViewModel ()
        {
            Actions.Add(new Action
            {
                Name = "Genres",
                Url = "/home/GetGenres"
            });

            Actions.Add(new Action
            {
                Name = "Stations for Genre",
                Url = "/home/GetStationsForGenre"
            });

            Actions.Add(new Action
            {
                Name = "Top Artists",
                Url = "/artists/GetTopArtists"
            });

            Actions.Add(new Action
            {
                Name = "Top Artists by Genre",
                Url = "/artists/GetArtistsForGenre"
            });

            Actions.Add(new Action
            {
                Name = "Login via OAuth",
                Url = "/home/Login"
            });

            Actions.Add(new Action
            {
                Name = "Tracks for Me",
                Url = "/home/GetTracksForMe"
            });

            Actions.Add(new Action
            {
                Name = "Playlist Editor",
                Url = "/Playlists/ShowPlaylists"
            });
        }
    }
}
