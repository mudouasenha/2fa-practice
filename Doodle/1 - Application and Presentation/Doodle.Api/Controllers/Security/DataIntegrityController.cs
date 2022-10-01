using Doodle.Api.Controllers.Security.Models;
using Doodle.Services.Common;
using Doodle.Services.Security.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Doodle.Api.Controllers.Security
{
    [ApiController]
    [Route("[controller]")]
    public class DataIntegrityController : ControllerBase
    {
        private readonly ILogger<DataIntegrityController> _logger;
        private readonly ICryptoService _cryptoService;

        public DataIntegrityController(ILogger<DataIntegrityController> logger, ICryptoService cryptoService)
        {
            _logger = logger;
            _cryptoService = cryptoService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("test-integrity")]
        public async Task<Result<DataIntegritySummaryResultModel>> TestIntegrity(DataIntegrityInputModel inputModel)
        {
            var result = _cryptoService.GenerateExecutionSummary(DataIntegrityInputModel.ToDto(inputModel));

            return new Result<DataIntegritySummaryResultModel>(DataIntegritySummaryResultModel.FromDto(result), "Comparação realizada", true);
        }
    }
}