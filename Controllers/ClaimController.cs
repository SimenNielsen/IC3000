using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using IC3000.Models;
using IC3000.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using IC3000.Service;

namespace IC3000.Controllers
{
    [ApiController]
    [Route("claim")]
    public class ClaimController : ControllerBase
    {
        private readonly ILogger<ClaimController> _logger;
        private readonly ClaimContext _claimContext;
        private readonly QueueService _queueService;

        public ClaimController(ILogger<ClaimController> logger, ClaimContext claimContext, QueueService queueService)
        {
            _logger = logger;
            _claimContext = claimContext;
            _queueService = queueService;
        }
        /// <summary>
        /// List all claim objects.
        /// </summary>
        /// <returns>List of all claim objects</returns>
        [HttpGet]   // GET /claim
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Claim>))]
        public Task<List<Claim>> ListProducts()
        {
            return _claimContext.Claims.ToListAsync();
        }
        /// <summary>
        /// Get single claim object if id exists.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Claim object or 404 Not found</returns>
        [HttpGet("{id}")]   // GET /claim/1
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Claim))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task<Claim> GetProduct(int id)
        {
            return _claimContext.Claims.FirstOrDefaultAsync(claim => claim.Id == id);
        }
        /// <summary>
        /// Insert new claim object if valid
        /// </summary>
        /// <param name="claim"></param>
        /// <returns>200 OK or 400 Bad request</returns>
        [HttpPost]   // POST /claim
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post(Claim claim)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model");
            }
            await this._claimContext.AddAsync(claim);
            this._claimContext.SaveChanges();
            await this._queueService.SendMessageAsync(string.Format("Claim with ID:{0}, was created", claim.Id));
            return Ok();
        }
        /// <summary>
        /// Delete claim object id id exists.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>200 OK or 404 Not found</returns>
        [HttpDelete]   // DELETE /claim
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var claim = this._claimContext.Claims.FirstOrDefaultAsync(claim => claim.Id == id);
            if(claim != null)
            {
                this._claimContext.Remove(claim);
                this._claimContext.SaveChanges();
                await this._queueService.SendMessageAsync(string.Format("Claim with ID:{0}, was deleted", claim.Id));
                return Ok();
            }
            return NotFound();
        }
    }
}
