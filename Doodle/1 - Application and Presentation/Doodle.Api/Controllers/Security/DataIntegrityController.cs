using Doodle.Api.Controllers.Models;
using Doodle.Api.Controllers.Security.Models;
using Doodle.Services.Common;
using Microsoft.AspNetCore.Mvc;

namespace Doodle.Api.Controllers.Security
{
    [ApiController]
    [Route("[controller]")]
    public class DataIntegrityController : ControllerBase
    {
        private readonly ILogger<DataIntegrityController> _logger;

        public DataIntegrityController(ILogger<DataIntegrityController> logger)
        {
            _logger = logger;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("test-integrity")]
        public async Task<Result<DataIntegritySummaryResultModel>> TestIntegrity(UserRegisterInputModel inputModel)
        {
            throw new NotImplementedException("not yet implemented");
        }
    }
}