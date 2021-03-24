namespace UrlShortener.Common
{
    /// <summary>Application-wide configuration settings</summary>
    public class AppSettings
    {
        /// <summary>A functional limit to what the system will handle; the HTTP specification is unclear, but this should provide some headroom</summary>
        public int MaxUrlLength { get; set; }

        /// <summary>The number of seconds to hold things in the in-memory cache</summary>
        public int MemoryCacheExpirationSeconds { get; set; }

        /// <summary>The base to use in constructing new redirect URLs</summary>
        public string RedirectUrlBase { get; set; }

        /// <summary>The connection string to the data store</summary>
        public string RepositoryConnectionString { get; set; }

        /// <summary>If true, the application should validate URLs</summary>
        public bool ValidateUrls { get; set; }
    }
}
