namespace UrlShortener.Api.Models
{
    /// <summary>A request to create a URL redirect</summary>
    public class CreateRedrectRequest
    {
        /// <summary>The URL for which to create the redirect</summary>
        public string Url { get; set; }
    }
}
