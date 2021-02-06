using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using IC3000.Models;

namespace IC3000.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClaimController : ControllerBase
    {
        private readonly ILogger<ClaimController> _logger;

        public ClaimController(ILogger<ClaimController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Claim> Get()
        {
            return new List<Claim>{ new Claim() };
        }
    }
}
