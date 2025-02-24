using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductPriceNegotiationApi.Models;
using ProductPriceNegotiationApi.Services;
using System;
using System.Threading.Tasks;

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
        public async Task<IActionResult> StartNegotiation(int productId, [FromBody] CreateNegotiationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var negotiationId = await _negotiationService.StartNegotiation(productId, request.ProposedPrice);
                return CreatedAtAction(nameof(GetNegotiation), new { negotiationId }, new { message = "Negotiation submitted successfully.", negotiationId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{negotiationId}/update")]
        public async Task<IActionResult> UpdateNegotiation(int negotiationId, [FromBody] CreateNegotiationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _negotiationService.UpdateNegotiation(negotiationId, request.ProposedPrice);
                return Ok(new { message = "Negotiation updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("{negotiationId}/response")]
        public async Task<IActionResult> RespondToNegotiation(int negotiationId, [FromQuery] bool accept)
        {
            try
            {
                await _negotiationService.RespondToNegotiation(negotiationId, accept);
                return Ok(new { message = accept ? "Negotiation accepted." : "Negotiation rejected." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{negotiationId}")]
        public async Task<IActionResult> GetNegotiation(int negotiationId)
        {
            var negotiation = await _negotiationService.GetNegotiationById(negotiationId);
            if (negotiation == null)
            {
                return NotFound(new { message = "Negotiation not found." });
            }

            return Ok(negotiation);
        }
    }
}
