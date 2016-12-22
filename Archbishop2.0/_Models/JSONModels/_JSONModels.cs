using System.Diagnostics;

namespace Archbishop2.Classes.JSONModels
{
    public class Credentials
    {
        public string Token { get; set; }
        public string ClientId { get; set; } 
        public ulong BotId { get; set; }
        public ulong[] OwnerIds { get; set; }
        public string GoogleAPIKey { get; set; }
        public string OsuAPIKey { get; set; }
    }
    //public class WikipediaApiModel
    //{
    //    public WikipediaQuery Query { get; set; }
    //}

    //public class WikipediaQuery
    //{
    //    public WikipediaPage[] Pages { get; set; }
    //}

    //public class WikipediaPage
    //{
    //    public bool Missing { get; set; } = false;
    //    public string FullUrl { get; set; }
    //}
    //[DebuggerDisplay("{items[0].id.playlistId}")]
    //public class YoutubePlaylistSearch
    //{
    //    public YtPlaylistItem[] items { get; set; }
    //}
    //public class YtPlaylistItem
    //{
    //    public YtPlaylistId id { get; set; }
    //}
    //public class YtPlaylistId
    //{
    //    public string kind { get; set; }
    //    public string playlistId { get; set; }
    //}
    //[DebuggerDisplay("{items[0].id.videoId}")]
    //public class YoutubeVideoSearch
    //{
    //    public YtVideoItem[] items { get; set; }
    //}
    //public class YtVideoItem
    //{
    //    public YtVideoId id { get; set; }
    //}
    //public class YtVideoId
    //{
    //    public string kind { get; set; }
    //    public string videoId { get; set; }
    //}
    //public class PlaylistItemsSearch
    //{
    //    public string nextPageToken { get; set; }
    //    public PlaylistItem[] items { get; set; }
    //}
    //public class PlaylistItem
    //{
    //    public YtVideoId contentDetails { get; set; }
    //}
}
