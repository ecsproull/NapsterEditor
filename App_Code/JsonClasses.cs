using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CsharpSample.App_Code
{
    public enum QueryType { all, album, artist, track, tag, playlist }

    /// <summary>
    /// All of the classes here are decorated with DataContract and DataMember.
    /// For re-use feel free to delete any properties that you don't care about.
    /// With the decorations in place the classes do not need to be complete.
    /// </summary>

    #region base classes

    [DataContract]
    public class HRef
    {
        [DataMember(Name = "href")]
        public string Href { get; set; }
    }

    [DataContract]
    public class BaseItem : HRef
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "links")]
        public AlbumLinks Links { get; set; }
    }

    [DataContract]
    public class LinkedItems
    {
        [DataMember(Name = "ids")]
        public List<string> Ids { get; set; } = new List<string>();

        [DataMember(Name = "href")]
        public string Href { get; set; }
    }

#endregion
    #region Login
    [DataContract]
    public sealed class NapsterData
    {
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }

        [DataMember(Name = "refresh_token")]
        public string RefreshToken { get; set; }

        [DataMember(Name = "expires_in")]
        public string ExpiresIn { get; set; }
    }

    #endregion //login

    #region Genre

    [DataContract]
    public sealed class GenreLinkItems : LinkedItems
    {
    }

    [DataContract]
    public sealed class GenreLink
    {
        [DataMember(Name = "childGenres")]
        public GenreLinkItems ChildGenres { get; set; }

        [DataMember(Name = "parentGenres")]
        public GenreLinkItems parentGenres { get; set; }
    }

    [DataContract]
    public sealed class Genre : BaseItem
    {
        [DataMember(Name = "shortcut")]
        public string Shortcut { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }
    }

    [DataContract]
    public sealed class Meta
    {
        [DataMember(Name = "totalCount")]
        public object TotalCount { get; set; }

        [DataMember(Name = "returnedCount")]
        public int ReturnedCount { get; set; }
    }

    [DataContract]
    public sealed class GenresRootItem
    {
        [DataMember(Name = "genres")]
        public List<Genre> GenreList { get; set; } = new List<Genre>();

        [DataMember(Name = "meta")]
        public Meta Meta { get; set; }
    }

    #endregion // Genre

    #region Artists

    [DataContract]
    public sealed class ArtistRootobject
    {
        [DataMember(Name = "artists")]
        public Artist[] Artists { get; set; }

        [DataMember(Name = "meta")]
        public Meta Meta { get; set; }
    }

    #endregion

    #region Album

    [DataContract]
    public sealed class AlbumRootobject
    {
        [DataMember(Name = "albums")]
        public Album[] Albums { get; set; }

        [DataMember(Name = "meta")]
        public Meta Meta { get; set; }
    }

    [DataContract]
    public sealed class Album : BaseItem
    {
        [DataMember(Name = "upc")]
        public string Upc { get; set; }

        [DataMember(Name = "shortcut")]
        public string Shortcut { get; set; }

        [DataMember(Name = "isStreamable")]
        public bool IsStreamable { get; set; }

        [DataMember(Name = "released")]
        public string Released { get; set; }

        [DataMember(Name = "originallyReleased")]
        public string OriginallyReleased { get; set; }

        [DataMember(Name = "label")]
        public string Label { get; set; }

        [DataMember(Name = "copyright")]
        public string Copyright { get; set; }

        [DataMember(Name = "tags")]
        public List<string> Tags { get; set; }

        [DataMember(Name = "discCount")]
        public int DiscCount { get; set; }

        [DataMember(Name = "trackCount")]
        public int TrackCount { get; set; }

        [DataMember(Name = "isExplicit")]
        public bool IsExplicit { get; set; }

        [DataMember(Name = "isSingle")]
        public bool IsSingle { get; set; }

        [DataMember(Name = "accountPartner")]
        public string AccountPartner { get; set; }

        [DataMember(Name = "artistName")]
        public string ArtistName { get; set; }

        [DataMember(Name = "contributingArtists")]
        public Contributingartists ContributingArtists { get; set; }

        [DataMember(Name = "discographies")]
        public Discographies Discographies { get; set; }

    }

    [DataContract]
    public sealed class Contributingartists
    {
        [DataMember(Name = "primaryArtist")]
        public string PrimaryArtist { get; set; }
    }

    [DataContract]
    public sealed class Discographies
    {
        [DataMember(Name = "main")]
        public string[] Main { get; set; }

        [DataMember(Name = "others")]
        public string[] Others { get; set; }

        [DataMember(Name = "singlesAndEPs")]
        public string[] SinglesAndEPs { get; set; }
    }

    [DataContract]
    public sealed class AlbumLinks
    {
        [DataMember(Name = "images")]
        public Images Images { get; set; }

        [DataMember(Name = "tracks")]
        public AlbumLinksTrackLinks Tracks { get; set; }

        [DataMember(Name = "posts")]
        public Posts Posts { get; set; }

        [DataMember(Name = "artists")]
        public Artists Artists { get; set; }

        [DataMember(Name = "genres")]
        public GenreLinkItems Genres { get; set; }

        [DataMember(Name = "reviews")]
        public Reviews Reviews { get; set; }
    }

    [DataContract]
    public sealed class Images : HRef
    {
    }

    [DataContract]
    public sealed class AlbumLinksTrackLinks : HRef
    {
    }

    [DataContract]
    public sealed class Posts : HRef
    {
    }

    [DataContract]
    public sealed class Artists : LinkedItems
    {
    }

    [DataContract]
    public sealed class Reviews : LinkedItems
    {
    }

    #endregion //Album

    #region Tracks

    [DataContract]
    public sealed class TrackRootobject
    {
        [DataMember(Name = "meta")]
        public Meta Meta { get; set; }

        [DataMember(Name = "tracks")]
        public Track[] Tracks { get; set; }
    }

    [DataContract]
    public sealed class Track : BaseItem
    {
        [DataMember(Name = "index")]
        public int Index { get; set; }

        [DataMember(Name = "disc")]
        public int Disc { get; set; }

        [DataMember(Name = "playbackSeconds")]
        public int PlaybackSeconds { get; set; }

        [DataMember(Name = "isExplicit")]
        public bool IsExplicit { get; set; }

        [DataMember(Name = "isStreamable")]
        public bool IsStreamable { get; set; }

        [DataMember(Name = "isrc")]
        public string Isrc { get; set; }

        [DataMember(Name = "shortcut")]
        public string Shortcut { get; set; }

        [DataMember(Name = "blurbs")]
        public object[] Blurbs { get; set; }

        [DataMember(Name = "artistId")]
        public string ArtistId { get; set; }

        [DataMember(Name = "artistName")]
        public string ArtistName { get; set; }

        [DataMember(Name = "albumName")]
        public string AlbumName { get; set; }

        [DataMember(Name = "formats")]
        public Format[] Formats { get; set; }

        [DataMember(Name = "albumId")]
        public string AlbumId { get; set; }

        [DataMember(Name = "contributors")]
        public Contributors Contributors { get; set; }

        [DataMember(Name = "previewURL")]
        public string PreviewURL { get; set; }

        // Used for deduping tracks in the playlist editor.
        public bool IsSelected { get; set; } = true;
    }

    [DataContract]
    public sealed class Contributors
    {
        [DataMember(Name = "primaryArtist")]
        public string PrimaryArtist { get; set; }
    }

    [DataContract]
    public sealed class TrackLinks
    {
        [DataMember(Name = "artists")]
        public Artists Artists { get; set; }

        [DataMember(Name = "albums")]
        public Albums Albums { get; set; }

        [DataMember(Name = "genres")]
        public Genres Genres { get; set; }

        [DataMember(Name = "tags")]
        public Tags Tags { get; set; }
    }

    [DataContract]
    public sealed class Albums : LinkedItems
    {
    }

    [DataContract]
    public sealed class Genres : LinkedItems
    {
    }

    [DataContract]
    public sealed class Tags : LinkedItems
    {
    }

    [DataContract]
    public sealed class Format
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "bitrate")]
        public int Bitrate { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }

    #endregion

    #region search

    [DataContract]
    public sealed class SearchRootobject
    {
        [DataMember(Name = "meta")]
        public Meta Meta { get; set; }

        [DataMember(Name = "search")]
        public Search Search { get; set; }
    }

    [DataContract]
    public sealed class Search
    {
        [DataMember(Name = "query")]
        public string Query { get; set; }

        [DataMember(Name = "data")]
        public Data Data { get; set; }

        [DataMember(Name = "order")]
        public string[] Order { get; set; }
    }

    [DataContract]
    public sealed class Data
    {
        [DataMember(Name = "tracks")]
        public Track[] Tracks { get; set; }

        [DataMember(Name = "albums")]
        public Album[] Albums { get; set; }

        [DataMember(Name = "artists")]
        public Artist[] Artists { get; set; }

        [DataMember(Name = "playlists")]
        public Playlist[] Playlists { get; set; }

        [DataMember(Name = "tags")]
        public Tags[] Taglist { get; set; }
    }

    [DataContract]
    public sealed class Artist : BaseItem
    {
        [DataMember(Name = "shortcut")]
        public string Shortcut { get; set; }

        [DataMember(Name = "blurbs")]
        public string[] Blurbs { get; set; }

        [DataMember(Name = "albumGroups")]
        public Albumgroups AlbumGroups { get; set; }

        [DataMember(Name = "amg")]
        public string Amg { get; set; }

        [DataMember(Name = "bios")]
        public Bio[] Bios { get; set; }
    }

    [DataContract]
    public sealed class Albumgroups
    {
        [DataMember(Name = "singlesAndEPs")]
        public string[] SinglesAndEPs { get; set; }

        [DataMember(Name = "compilations")]
        public string[] Compilations { get; set; }

        [DataMember(Name = "others")]
        public string[] Others { get; set; }

        [DataMember(Name = "main")]
        public string[] Main { get; set; }
    }

    [DataContract]
    public sealed class Links
    {
        [DataMember(Name = "albums")]
        public ArtistAlbums Albums { get; set; }

        [DataMember(Name = "images")]
        public Images Images { get; set; }

        [DataMember(Name = "posts")]
        public Posts Posts { get; set; }

        [DataMember(Name = "topTracks")]
        public Toptracks TopTracks { get; set; }

        [DataMember(Name = "genres")]
        public Genres Genres { get; set; }

        [DataMember(Name = "stations")]
        public Stations Stations { get; set; }

        [DataMember(Name = "contemporaries")]
        public Contemporaries Contemporaries { get; set; }

        [DataMember(Name = "followers")]
        public Followers Followers { get; set; }

        [DataMember(Name = "influences")]
        public Influences Influences { get; set; }

        [DataMember(Name = "relatedProjects")]
        public Relatedprojects RelatedProjects { get; set; }
    }

    [DataContract]
    public sealed class ArtistAlbums : HRef
    {
    }

    [DataContract]
    public sealed class Toptracks : HRef
    {
    }

    [DataContract]
    public sealed class Stations : LinkedItems
    {
    }

    [DataContract]
    public sealed class Contemporaries : LinkedItems
    {
    }

    [DataContract]
    public sealed class Followers : LinkedItems
    {
    }

    [DataContract]
    public sealed class Influences : LinkedItems
    {
    }

    [DataContract]
    public sealed class Relatedprojects : LinkedItems
    {
    }

    [DataContract]
    public sealed class Bio
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "author")]
        public string Author { get; set; }

        [DataMember(Name = "publishDate")]
        public string PublishDate { get; set; }

        [DataMember(Name = "bio")]
        public string Biography { get; set; }
    }

    [DataContract]
    public sealed class Playlist : BaseItem
    {
        [DataMember(Name = "modified")]
        public string Modified { get; set; }

        [DataMember(Name = "trackCount")]
        public int TrackCount { get; set; }

        [DataMember(Name = "privacy")]
        public string Privacy { get; set; }

        [DataMember(Name = "images")]
        public Image[] Images { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "favoriteCount")]
        public int FavoriteCount { get; set; }

        [DataMember(Name = "freePlayCompliant")]
        public bool FreePlayCompliant { get; set; }
    }

    [DataContract]
    public sealed class PlayListArtistLinks
    {
        [DataMember(Name = "members")]
        public Members Members { get; set; }

        [DataMember(Name = "tracks")]
        public LinkedTracks Tracks { get; set; }

        [DataMember(Name = "tags")]
        public Tags Tags { get; set; }

        [DataMember(Name = "sampleArtists")]
        public Sampleartists SampleArtists { get; set; }
    }

    [DataContract]
    public sealed class Members : LinkedItems
    {
    }

    [DataContract]
    public sealed class LinkedTracks : HRef
    {
    }

    [DataContract]
    public sealed class Sampleartists : LinkedItems
    {
    }

    [DataContract]
    public sealed class Image
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "contentId")]
        public string ContentId { get; set; }

        [DataMember(Name = "width")]
        public int? Width { get; set; }

        [DataMember(Name = "height")]
        public int? Height { get; set; }

        [DataMember(Name = "isDefault")]
        public bool IsDefault { get; set; }

        [DataMember(Name = "version")]
        public long Version { get; set; }

        [DataMember(Name = "imageType")]
        public string ImageType { get; set; }
    }

    #endregion //search

    #region stations

    [DataContract]
    public sealed class StationsRootobject
    {
        [DataMember(Name = "meta")]
        public Meta Meta { get; set; }

        [DataMember(Name = "stations")]
        public Station[] Stations { get; set; }
    }

    [DataContract]
    public sealed class Station : BaseItem
    {
        [DataMember(Name = "subType")]
        public string SubType { get; set; }

        [DataMember(Name = "author")]
        public string Author { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "summary")]
        public string Summary { get; set; }

        [DataMember(Name = "artists")]
        public string Artists { get; set; }
    }

    [DataContract]
    public sealed class StationLinks
    {
        [DataMember(Name = "genres")]
        public Genres Genres { get; set; }

        [DataMember(Name = "mediumImage")]
        public Mediumimage MediumImage { get; set; }

        [DataMember(Name = "largeImage")]
        public Largeimage LargeImage { get; set; }
    }

    [DataContract]
    public sealed class Mediumimage : HRef
    {
    }

    [DataContract]
    public sealed class Largeimage : HRef
    {
    }

    #endregion

    #region Playlists

    [DataContract]
    public sealed class PlayListsRootObject
    {
        [DataMember(Name = "playlists")]
        public List<Playlist> Playlists { get; set; } = new List<Playlist>();
    }

    #endregion
}
