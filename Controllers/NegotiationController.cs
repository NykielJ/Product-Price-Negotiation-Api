using Microsoft.AspNetCore.Mvc;
using ProductPriceNegotiationApi.Models;
using ProductPriceNegotiationApi.Services;

namespace ProductPriceNegotiationApi.Controllers
{
    [Route("api/negotiations")]
    [ApiController]
    public class NegotiationController : ControllerBase
    {
        private readonly NegotiationService _negotiationService;

        public NegotiationController(NegotiationService negotiationService)
        {
            _negotiationService = negotiationService;
        }

        [HttpPost("{productId}")]
        public async Task<IActionResult> StartNegotiation(int productId, [FromBody] Negotiation negotiation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _negotiationService.StartNegotiation(productId, negotiation.ProposedPrice);
                return Ok(new { message = "Negotiation started successfully." });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("{productId}")]
        public async Task<IActionResult> RespondToNegotiation(int productId, [FromQuery] bool accept)
        {
            try
            {
                await _negotiationService.RespondToNegotiation(productId, accept);
                return Ok(new { message = accept ? "Negotiation accepted." : "Negotiation rejected." });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}
