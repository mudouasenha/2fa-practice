using Doodle.Services.Security.Models;

namespace Doodle.Services.Security.Abstractions
{
    public interface ICryptoService
    {
        DataIntegritySummaryResultDTO GenerateExecutionSummary(DataIntegrityInputDTO input);
    }
}