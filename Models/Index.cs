using CsharpSample.App_Code;
using System.Collections.Generic;

namespace CsharpSample.Models
{
    public class Action
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class Index
    {
        public List<Action> Actions { get; set; } = new List<Action>();

        public Index ()
        {
            Actions.Add(new Action
            {
                Name = "Get Genres",
                Url = "/home/GetGenres"
            });

            Actions.Add(new Action
            {
                Name = "Get Stations for Genre",
                Url = "/home/GetStationsForGenre"
            });

            Actions.Add(new Action
            {
                Name = "Login For More Options",
                Url = "/home/Login"
            });

            Actions.Add(new Action
            {
                Name = "Get Tracks for Me",
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
