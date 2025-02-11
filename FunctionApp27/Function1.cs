//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Azure.Functions.Worker;
//using Microsoft.Extensions.Logging;

//namespace FunctionApp27
//{
//    public class Function1
//    {
//        private readonly ILogger<Function1> _logger;

//        public Function1(ILogger<Function1> logger)
//        {
//            _logger = logger;
//        }

//        [Function("Function1")]
//        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
//        {
//            _logger.LogInformation("C# HTTP trigger function processed a request.");
//            return new OkObjectResult("Welcome to Azure Functions!");
//        }
//    }
//}













using FunctionApp27.Helpers;
using FunctionApp27.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionApp27
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;
        private readonly DatabaseService _databaseService;
        private readonly CsvHelperService _csvHelperService;

        public Function1(ILogger<Function1> logger, DatabaseService databaseService, CsvHelperService csvHelperService)
        {
            _logger = logger;
            _databaseService = databaseService;
            _csvHelperService = csvHelperService;
        }

        [Function("ExportToCsv")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            try
            {
                _logger.LogInformation("Starting CSV Export process...");

                var variations = await _databaseService.GetVariationsAsync();
                var commentaries = await _databaseService.GetCommentariesAsync();
                string filePath = GetFilePath();
                _csvHelperService.WriteToCsv(variations, commentaries, filePath);
                return new OkObjectResult($"CSV file generated at: {filePath}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing request: {ex.Message}");
                return new BadRequestObjectResult("Failed to process CSV export.");
            }
        }

        private string GetFilePath()
        {
            if (Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT") == "Development")
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Temp", "exported_data.csv");
            }
            else
            {
                return Path.Combine("C:\\home\\site", "exported_data.csv");
            }
        }
    }
}
