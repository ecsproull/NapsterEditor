using CsharpSample.App_Code;
using CsharpSample.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CsharpSample.Controllers
{
    public class PlaylistsController : Controller
    {
        private static Tracks currentModel;
        public async Task<ActionResult> ShowPlaylists()
        {
            if (string.IsNullOrWhiteSpace(AccessProperties.Token))
            {
                Login loginModel = new Login();
                loginModel.ContinueUrl = "/Playlists/ShowPlaylists";
                return RedirectToAction("Login", "Home");
            }
            else
            {
                PlayLists model = new PlayLists();
                model.Playlists = await NapsterApiHelper.GetPlayListsAsync();
                return View(model);
            }
        }

        public async Task<ViewResult> ShowTracks(string playlistId, int trackCount, string playlistName)
        {
            currentModel = new Tracks
            {
                TrackList = await NapsterApiHelper.GetPlayListTracksAsync(playlistId, trackCount),
                PlayListName = playlistName,
                PlayListId = playlistId
            };

            return View(currentModel);
        }

        public ViewResult Sort(string sortItem = "Title")
        {
            SortTracks(sortItem);

            return View("ShowTracks", currentModel);
        }

        private static void SortTracks(string sortItem = "Title")
        {
            List<Track> tracks = new List<Track>(currentModel.TrackList);
            switch (sortItem)
            {
                case "Title":
                    {
                        tracks.Sort(delegate (Track a, Track b)
                            {
                                return a.Name.CompareTo(b.Name);
                            });
                        break;
                    }

                case "Artist":
                    {
                        tracks.Sort(delegate (Track a, Track b)
                        {
                            return a.ArtistName.CompareTo(b.ArtistName);
                        });
                        break;
                    }

                case "Album":
                    {
                        tracks.Sort(delegate (Track a, Track b)
                        {
                            return a.AlbumName.CompareTo(b.AlbumName);
                        });
                        break;
                    }
            }

            currentModel.TrackList = tracks;
        }

        public ViewResult Shuffle()
        {
            List<Track> tracks = new List<Track>(currentModel.TrackList);
            Random rng = new Random(DateTime.Now.Millisecond + DateTime.Now.Second);
            int n = tracks.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Track value = tracks[k];
                tracks[k] = tracks[n];
                tracks[n] = value;
            }

            currentModel.TrackList = tracks;
            return View("ShowTracks", currentModel);
        }

        public ViewResult MarkDups()
        {
            List<Track> playlistTracks = currentModel.TrackList.ToList();

             playlistTracks.Sort(delegate (Track a, Track b)
             {
                 return a.Name.CompareTo(b.Name);
             });

            List<Track> dups = new List<Track>();
            Track previousTrack = new Track();

            foreach (Track td in playlistTracks)
            {
                if (previousTrack.Name.ToUpperInvariant() == td.Name.ToUpperInvariant() &&
                    previousTrack.ArtistName.ToUpperInvariant() == td.ArtistName.ToUpperInvariant())
                {
                    if (!dups.Contains(td))
                    {
                        dups.Add(td);
                    }

                    if (!dups.Contains(previousTrack))
                    {
                        dups.Add(previousTrack);
                    }
                }

                previousTrack = td;
            }

            if (dups.Count > 0)
            {
                Tracks model = new Tracks
                {
                    PlayListId = currentModel.PlayListId,
                    TrackList = dups,
                    PlayListName = currentModel.PlayListName
                };

                foreach (Track tdat in model.TrackList)
                {
                    tdat.IsSelected = true;
                }

                return View("DeDup", model);
            }

            return View("ShowTracks", currentModel);
        }

        public ViewResult Save()
        {
            string tracksJson = "{\"tracks\":[";
            bool first = true;
            foreach (Track track in currentModel.TrackList)
            {
                if (first)
                {
                    tracksJson += "{\"id\":\"" + track.Id + "\"}";
                    first = false;
                }
                else
                {
                    tracksJson += ",{\"id\":\"" + track.Id + "\"}";
                }
            }

            tracksJson += "]}";

            bool success = NapsterApiHelper.SavePlayList(currentModel.PlayListId, tracksJson);


            return View("ShowTracks", currentModel);
        }

        [HttpPost]
        public ActionResult RemoveTracks(Tracks model)
        {
            List<Track> tracks = new List<Track>(currentModel.TrackList);
            foreach (Track td in model.TrackList)
            {
                if (!td.IsSelected)
                {
                    Track itemToRemove = tracks.Find((item) => { return item.Id == td.Id; });
                    bool wasRemoved = tracks.Remove(itemToRemove);
                }
            }

            currentModel.TrackList = tracks;
            return View("ShowTracks", currentModel);
        }

        [HttpGet]
        public string GetNextTrack(string currentTrack)
        {
            bool foundTrack = false;
            string nextTrack = string.Empty;
            foreach (Track t in currentModel.TrackList)
            {
                if (t.Id == currentTrack)
                {
                    foundTrack = true;
                    continue;
                }

                if (foundTrack)
                {
                    nextTrack = t.Id;
                    break;
                }
            }

            if (string.IsNullOrEmpty(nextTrack))
            {
                nextTrack = currentModel.TrackList.ElementAt(0).Id;
            }

            return nextTrack;
        }
    }
}