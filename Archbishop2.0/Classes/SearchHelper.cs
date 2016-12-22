using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Discord.Rest;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Archbishop2.Classes.JSONModels;
using System.Collections.Specialized;


namespace Archbishop2.Classes
{
    public enum RequestHttpMethod
    {
        Get,
        Post
    }
    public static class SearchHelper
    {
        private static DateTime lastRefreshed = DateTime.MinValue;
        private static readonly HttpClient httpClient = new HttpClient();
        private static string token { get; set; } = "";
        public static async Task<UserResult> GetUserData(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentNullException(nameof(query));

            await RefreshAnilistToken().ConfigureAwait(false);
            ABNet abNet = new ABNet("https://anilist.co/api/users/" + query);
            abNet.AddQuery("access_token", token);
            return await Task.Run(async() => JsonConvert.DeserializeObject<UserResult>(await abNet.GetStringAsync())).ConfigureAwait(false);
        }
        public static async Task<Stream> GetResponseStreamAsync(string url,
            IEnumerable<KeyValuePair<string, string>> headers = null, RequestHttpMethod method = RequestHttpMethod.Get)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url));
            //if its a post or there are no headers, use static httpclient
            // if there are headers and it's get, it's not threadsafe
            var cl = headers == null || method == RequestHttpMethod.Post ? httpClient : new HttpClient();
            cl.DefaultRequestHeaders.Clear();
            switch (method)
            {
                case RequestHttpMethod.Get:
                    if (headers != null)
                    {
                        foreach (var header in headers)
                        {
                            cl.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                        }
                    }
                    return await cl.GetStreamAsync(url).ConfigureAwait(false);
                case RequestHttpMethod.Post:
                    FormUrlEncodedContent formContent = null;
                    if (headers != null)
                    {
                        formContent = new FormUrlEncodedContent(headers);
                    }
                    var message = await cl.PostAsync(url, formContent).ConfigureAwait(false);
                    return await message.Content.ReadAsStreamAsync().ConfigureAwait(false);
                default:
                    throw new NotImplementedException("That type of request is unsupported.");
            }
        }
        public static async Task<string> GetResponseStringAsync(string url,
            IEnumerable<KeyValuePair<string, string>> headers = null,
            RequestHttpMethod method = RequestHttpMethod.Get)
        {

            using (var streamReader = new StreamReader(await GetResponseStreamAsync(url, headers, method).ConfigureAwait(false)))
            {
                return await streamReader.ReadToEndAsync().ConfigureAwait(false);
            }
        }
        private static async Task RefreshAnilistToken()
        {
            if (DateTime.Now - lastRefreshed > TimeSpan.FromMinutes(29))
                lastRefreshed = DateTime.Now;
            else
            {
                return;
            }
            
            var headers = new Dictionary<string, string> {
                {"grant_type", "client_credentials"},
                {"client_id", "vorgunthebeta-ep4h8"},
                {"client_secret", "qLzmwqIji2oXUd8HjLZR"},
            };
            var content = await GetResponseStringAsync(
                            "http://anilist.co/api/auth/access_token",
                            headers,
                            RequestHttpMethod.Post).ConfigureAwait(false);

            token = JObject.Parse(content)["access_token"].ToString();
        }
    }
    public class ABNet
    {
        public Dictionary<string, string> Query = new Dictionary<string, string>();
        public string BaseUrl { get; set; }

        public HttpClient _client = new HttpClient();
        private string completeUrl = "";

        public ABNet(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        public void AddQuery(string key, string value)
        {
            Query[key] = value;
        }

        public async Task<string> GetStringAsync()
        {
            string url = GetUrl();
            return await _client.GetStringAsync(GetUrl());
        }


        public async Task<byte[]> GetByteArrayAsync()
        {
            return await _client.GetByteArrayAsync(GetUrl());
        }


        public async Task<HttpResponseMessage> PostAsync()
        {
            var content = new FormUrlEncodedContent(Query);
            var response = await _client.PostAsync(BaseUrl, content);
            return response;
        }

        public string GetUrl()
        {

            if (Query.Count == 0)
                return BaseUrl;

            string querys = "?";

            foreach (var query in Query)
            {
                querys += Uri.EscapeUriString(query.Key) + "=" + Uri.EscapeUriString(query.Value) + "&";
            }

            querys = querys.Remove(querys.Length - 1);

            return BaseUrl + querys;
        }
    }
}
