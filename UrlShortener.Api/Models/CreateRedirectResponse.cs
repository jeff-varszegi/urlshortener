namespace UrlShortener.Api.Models
{
    /// <summary>A response to a request to create a URL redirect</summary>
    public class CreateRedirectResponse
    {
        /// <summary>The shortened URL</summary>
        public string Url { get; set; }
    }
}
