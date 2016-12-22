using System.Diagnostics;

namespace Archbishop2.Classes.JSONModels
{
    public class Credentials
    {
        public string Token { get; set; } = "MjU0NTIyMjIzNDAwOTc2Mzg0.CyQYWw.GEFgUzE9ZO4YziCbnJSz3-nj8hE";
        public string ClientId { get; set; } = "254522223400976384";
        public ulong BotId { get; set; } = 254522223400976384;
        public ulong[] OwnerIds { get; set; } = { 127188004216373248 };
        public string GoogleAPIKey { get; set; } = "AIzaSyBp3qCSSd6qH1E-hnxuaY7vjeJhPM76ftA";
        public string OsuAPIKey { get; set; } = "80d6f911d63ce74ff69c65965bd2ac8aae2f75ab";
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
