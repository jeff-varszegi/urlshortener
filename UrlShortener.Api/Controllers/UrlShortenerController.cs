using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UrlShortener.Api.Models;
using UrlShortener.Common;
using UrlShortener.Common.Caching;
using UrlShortener.DataAccess.Repositories;

namespace UrlShortener.Controllers
{
    /// <summary>Provides lookup of URL shortenings</summary>
    [ApiController]
    [Route("")]
    public class UrlShortenerController : ControllerBase
    {
        /// <summary>The data store for the application</summary>
        private readonly IUrlShortenerRepository _repository;

        /// <summary>Manages caching of redirectable URLs</summary>
        private readonly ICacheManager<string> _cacheManager;

        /// <summary>Settings to use in constructing new URLs</summary>
        private readonly AppSettings _settings;

        /// <summary>Constructs a new instance</summary>
        /// <param name="repository">The repository in which to store and retrieve data</param>
        /// <param name="cacheManager">A caching service provider</param>
        /// <param name="settings">Settings to use</param>
        public UrlShortenerController(IUrlShortenerRepository repository, ICacheManager<string> cacheManager, IOptionsSnapshot<AppSettings> settings)
        {
            _repository = repository ?? throw new ArgumentNullException("repository");
            _cacheManager = cacheManager ?? throw new ArgumentNullException("cacheManager");
            _settings = settings.Value;
        }

        [HttpGet]
        [Route("{key}")]
        public async Task<ActionResult> Get([FromRoute] string key)
        {
            string url = await _cacheManager.Get(key);

            if (!string.IsNullOrEmpty(url)) 
            {
                _repository.IncrementRedirectCount(key);        // It is fine for this to be fire-and-forget
            }
            else {
                url = await _repository.GetRedirectUrl(key);
                if (!string.IsNullOrWhiteSpace(url))
                    _cacheManager.Set(key, url);                // ... as well as this
            }

            if (!string.IsNullOrEmpty(url))
                return new RedirectResult(url, true);
            else
                return NotFound();
        }

        [HttpPost]
        [Route("[controller]/redirects")]
        public async Task<CreateRedirectResponse> CreateRedirect([FromBody] CreateRedrectRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Url)) throw new ArgumentException("Invalid request");

            string key = await _repository.CreateUrlShortening(request.Url);
            string redirectUrl = _settings.RedirectUrlBase.TrimEnd('/') + "/" + key;

            return new CreateRedirectResponse() { Url = redirectUrl };
        }
    }
}
