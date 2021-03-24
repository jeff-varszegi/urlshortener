using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

using Npgsql;
using NpgsqlTypes;

using UrlShortener.Common;
using UrlShortener.Common.Utility;

namespace UrlShortener.DataAccess.Repositories
{
    /// <summary>Implements storage and retrieval of URL shortener data in a PostgreSQL database</summary>
    public sealed class PostgresUrlShortenerRepository : IUrlShortenerRepository
    {
        /// <summary>Application settings</summary>
        private readonly AppSettings _settings;
        
        /// <summary>Constructs a new instance</summary>
        /// <param name="settings">Application-wide settings to use</param>
        public PostgresUrlShortenerRepository(IOptionsSnapshot<AppSettings> settings)
        {
            _settings = settings.Value;
            if (string.IsNullOrWhiteSpace(_settings.RepositoryConnectionString)) throw new InvalidOperationException("No database connection info configured");
        }

        /// <summary>Creates (or finds if pre-existing) a shortening for the specified URL, returning its key</summary>
        /// <param name="url">The URL to shorten</param>
        /// <returns>The key for the specified URL</returns>
        public async Task<string> CreateUrlShortening(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentException("Invalid (null or whitespace) URL");
            else if (url.Length > _settings.MaxUrlLength) throw new ArgumentException("Invalid URL (max supported length is " + _settings.MaxUrlLength + ")");

            url = url.ToLower();
            int urlHashCode = url.GetStableHashCode();
            if (_settings.ValidateUrls && !UrlUtility.IsUrlValid(url))
                throw new ArgumentException("Invalid URL");

            object returnValue = null;
            using (var connection = new NpgsqlConnection(_settings.RepositoryConnectionString))
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = $@"SELECT urls.""CreateUrlShortening""(@url, @urlHashCode);";

                    command.Parameters.Add(new NpgsqlParameter("url", NpgsqlDbType.Text) { Value = url });
                    command.Parameters.Add(new NpgsqlParameter("urlHashCode", NpgsqlDbType.Integer) { Value = urlHashCode });

                    returnValue = await command.ExecuteScalarAsync();
                }
            }

            if (returnValue == DBNull.Value)
                return null;

            return UrlUtility.ConvertIdToKey((long)returnValue);
        }

        /// <summary>Gets the URL for the specified key, and increments its redirect count</summary>
        /// <param name="key">The key for which to get the URL</param>
        /// <returns>The URL for the key, or null if not found</returns>
        public async Task<string> GetRedirectUrl(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Invalid (null or whitespace) key");
            
            long urlShorteningId = UrlUtility.ConvertKeyToId(key);
            object returnValue = null;
            using (var connection = new NpgsqlConnection(_settings.RepositoryConnectionString))
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = $@"SELECT urls.""GetRedirectUrl""(@urlShorteningId);";

                    command.Parameters.Add(new NpgsqlParameter("urlShorteningId", NpgsqlDbType.Bigint) { Value = urlShorteningId });

                    returnValue = await command.ExecuteScalarAsync();
                }
            }

            if (returnValue == DBNull.Value)
                return null;
            else
                return (string)returnValue;
        }

        /// <summary>Increments the redirect count for the specified key, for use when the key was found in cache</summary>
        /// <param name="key">The key for which to update the redirect count</param>
        /// <returns>An async task</returns>
        public async Task IncrementRedirectCount(string key) 
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Invalid (null or whitespace) key");

            long urlShorteningId = UrlUtility.ConvertKeyToId(key);
            using (var connection = new NpgsqlConnection(_settings.RepositoryConnectionString))
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = $@"SELECT urls.""IncrementRedirectCount""(@urlShorteningId);";

                    command.Parameters.Add(new NpgsqlParameter("urlShorteningId", NpgsqlDbType.Bigint) { Value = urlShorteningId });

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

    }
}
