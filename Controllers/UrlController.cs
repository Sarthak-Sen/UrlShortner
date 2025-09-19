using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UrlShortner.Features;

namespace UrlShortner.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UrlController : ControllerBase
    {
        //private readonly ILogger<UrlController> _logger;
        private readonly CreateShortUrlHandler _createHandler;
        private readonly RedirectUrlHandler _redirectHandler;

        public UrlController(CreateShortUrlHandler createHandler, RedirectUrlHandler redirectHandler)
        {
            _createHandler = createHandler;
            _redirectHandler = redirectHandler;
        }

        [HttpPost("CreateShortUrl")]
        public IActionResult CreateShortUrl([FromBody] string originalUrl)
        {

            var shortCode = _createHandler.Handle(originalUrl);
            string shortUrl = $"{Request.Scheme}://{Request.Host}/Url/{shortCode}";
            return Ok(new { originalUrl, shortUrl });
        }

        [HttpGet("{shortCode}")]
        public IActionResult RedirectOriginal([FromRoute] string shortCode)
        {
            var originalUrl = _redirectHandler.Handle(shortCode);
            if (originalUrl == null) return NotFound();

            return Redirect(originalUrl);
        }
    }
}
