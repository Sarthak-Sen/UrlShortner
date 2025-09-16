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

        [HttpPost("createShortUrl")]
        public async Task<IActionResult> CreateShortUrl([FromBody] string originalUrl)
        {

            var shortCode = await _createHandler.Handle(originalUrl);
            string shortUrl = $"{Request.Scheme}://{Request.Host}/Url/{shortCode}";
            return Ok(new { originalUrl, shortUrl });
        }

        [HttpGet("{shortCode}")]
        public async Task<IActionResult> RedirectOriginal([FromRoute] string shortCode)
        {
            var originalUrl = await _redirectHandler.Handle(shortCode);
            if (originalUrl == null) return NotFound();

            return Redirect(originalUrl);
        }
    }
}
