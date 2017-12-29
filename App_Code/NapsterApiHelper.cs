using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace CsharpSample.App_Code
{
    public static class NapsterApiHelper
    {
        public static async Task<IEnumerable<Genre>> GetGenres()
        {
            string url = "https://api.napster.com/v2.2/genres";
            GenresRootItem root = await GetObjectAsync<GenresRootItem>(url);
            return root.GenreList;
        }


        public static async Task<IEnumerable<Station>> GetStationsForGenres(string genre)
        {
            string url = $"https://api.napster.com/v2.2/genres/{genre}/stations";
            StationsRootobject root = await GetObjectAsync<StationsRootobject>(url);
            return root.Stations;
        }

        public static async Task<IEnumerable<Track>> GetNewTracksForMe()
        {
            string url = $"https://api.napster.com/v2.2/me/personalized/tracks/new?limit=100";
            TrackRootobject root = await GetObjectAsync<TrackRootobject>(url, true);
            return root.Tracks;
        }

        public static async Task<IEnumerable<Station>> GetStationsDetail(string url)
        {
            StationsRootobject root = await GetObjectAsync<StationsRootobject>(url);
            return root.Stations;
        }

        public static async Task<IEnumerable<Album>> GetAlbumsAsync(string genre)
        {
            string url = $"https://api.napster.com/v2.2/genres/{genre}/albums/new ";
            AlbumRootobject root = await GetObjectAsync<AlbumRootobject>(url);
            return root.Albums;
        }

        public static async Task<IEnumerable<Track>> GetTracksAsync(string albumId)
        {
            string url = $"https://api.napster.com/v2.2/albums/{albumId}/tracks";
            TrackRootobject root = await GetObjectAsync<TrackRootobject>(url);
            return root.Tracks;
        }

        public static async Task<List<Playlist>> GetPlayListsAsync()
        {
            string url = "https://api.napster.com/v2.2/me/library/playlists";
            PlayListsRootObject root = await GetObjectAsync<PlayListsRootObject>(url, true);
            return root.Playlists;
        }

        public static async Task<List<Track>> GetPlayListTracksAsync(string playListId, int trackCount)
        {
            List<Track> tracks = new List<Track>();
            string url = $"https://api.napster.com/v2.2/me/library/playlists/{playListId}/tracks";
            IEnumerable<WebResponse> responses = await GetResponsesAsync(url, trackCount, true);

            foreach (WebResponse response in responses)
            {
                using (Stream stream = response.GetResponseStream())
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(TrackRootobject));
                    TrackRootobject obj = (TrackRootobject)serializer.ReadObject(stream);
                    tracks.AddRange(obj.Tracks);
                }
            }

            return tracks;
        }

        public static bool SavePlayList(string playListId, string tracksJson)
        {
            bool retVal = true;
            try
            {
                string url = $"https://api.napster.com/v2.2/me/library/playlists/{playListId}/tracks";
                WebRequest request = WebRequest.Create(url);
                request.Method = "PUT";
                request.Headers.Add("Authorization: Bearer " + AccessProperties.Token);
                request.Headers.Add("Content-Type: application/json");

                Stream requestStream = request.GetRequestStream();
                Byte[] bytes = Encoding.ASCII.GetBytes(tracksJson);
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                request.GetResponse();
            }
            catch (Exception)
            {
                retVal = false;
            }

            return retVal;
        }

        public static async Task<Data> SearchAsync(string query, QueryType type, bool verbose = true)
        {
            string url;
            if (verbose || query.Contains(" "))
            {
                url = $"https://api.napster.com/v2.2/search/verbose?query={query}";
            }
            else
            {
                url = $"https://api.napster.com/v2.2/search?query={query}";
            }

            if (type != QueryType.all)
            {
                url += "&type=" + Enum.GetName(type.GetType(), type);
            }

            SearchRootobject root = await GetObjectAsync<SearchRootobject>(url);
            return root.Search.Data;
        }

        public static async Task<bool> GetAccessPropertiesAsync(string code)
        {
            string url = "https://api.napster.com/oauth/access_token?" + $"client_id={AccessProperties.ClientId}&client_secret={AccessProperties.ClientSecret}&response_type=code&grant_type=authorization_code&code=" + code;
            NapsterData root = await GetObjectAsync<NapsterData>(url, requestMethod: "POST");
            AccessProperties.Token = root.AccessToken;
            AccessProperties.RefreshToken = root.RefreshToken;
            AccessProperties.ExpirationTime = DateTime.Now.AddSeconds(Convert.ToDouble(root.ExpiresIn));
            return true;
        }

        // This is called before AccessProperties.RefreshToken is initialized.
        public static async Task<bool> RefreshTokenAsync(string refreshToken)
        {
            string url = "https://api.napster.com/oauth/access_token?" + $"client_id={AccessProperties.ClientId}&client_secret={AccessProperties.ClientSecret}&response_type=code&grant_type=refresh_token&refresh_token={refreshToken}";
            NapsterData root = await GetObjectAsync<NapsterData>(url, requestMethod: "POST");
            AccessProperties.Token = root.AccessToken;
            AccessProperties.RefreshToken = root.RefreshToken;
            AccessProperties.ExpirationTime = DateTime.Now.AddSeconds(Convert.ToDouble(root.ExpiresIn));
            return true;
        }

        private static async Task<T> GetObjectAsync<T>(string url, bool useToken = false, string requestMethod = "GET")
        {
            WebRequest request = WebRequest.Create(url);
            request.Method = requestMethod;

            await AddHeader(useToken, request);

            WebResponse response = await request.GetResponseAsync();
            using (Stream stream = response.GetResponseStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                return (T)serializer.ReadObject(stream);
            }
        }

        private static async Task<IEnumerable<WebResponse>> GetResponsesAsync (string url, int count, bool useToken = false)
        {
            const int limit = 200;
            int offset = 0;
            List<Task<WebResponse>> tasks = new List<Task<WebResponse>>();
            while (offset < count)
            {
                WebRequest request = WebRequest.Create(url + $"?offset={offset}&limit={ limit}");
                request.Method = "GET";
                await AddHeader(useToken, request);
                tasks.Add(request.GetResponseAsync());
                offset += limit;
            }

            return await Task.WhenAll(tasks);
        }

        private static async Task AddHeader(bool useToken, WebRequest request)
        {
            if (useToken)
            {
                request.Headers.Add("Authorization: Bearer " + await AccessProperties.GetToken());
            }
            else
            {
                request.Headers.Add("apikey: " + AccessProperties.ClientId);
            }
        }
    }
}
