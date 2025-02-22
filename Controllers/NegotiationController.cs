using Microsoft.AspNetCore.Mvc;
using ProductPriceNegotiationApi.DTOs;
using ProductPriceNegotiationApi.Services;
using ProductPriceNegotiationApi.Utilities;
using ProductPriceNegotiationApi.Models;

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
        public IActionResult StartNegotiation(int productId, [FromBody] NegotiationDto negotiationDto)
        {
            try
            {
                ValidationHelper.ValidateProposedPrice(negotiationDto.ProposedPrice);
                
                _negotiationService.StartNegotiation(productId, negotiationDto.ProposedPrice);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{productId}")]
        public IActionResult RespondToNegotiation(int productId, [FromQuery] bool accept)
        {
            try
            {
                _negotiationService.RespondToNegotiation(productId, accept);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
