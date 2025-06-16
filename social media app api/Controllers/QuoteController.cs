using Microsoft.AspNetCore.Mvc;
using social_media_app_api.Services;

namespace social_media_app_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuoteController : ControllerBase
    {
        private readonly QuoteService _quoteService;

        public QuoteController(QuoteService quoteService)
        {
            _quoteService = quoteService;
        }

        [HttpGet("random")]
        public async Task<IActionResult> GetRandomQuote()
        {
            var quote = await _quoteService.GetRandomQuoteAsync();

            if (quote == null)
                return StatusCode(500, "Kunde inte hämta citat");

            return Ok(quote);
        }
    }
}
