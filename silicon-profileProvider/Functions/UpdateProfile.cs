using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace silicon_profileProvider.Functions
{
    public class UpdateProfile
    {
        private readonly ILogger<UpdateProfile> _logger;

        public UpdateProfile(ILogger<UpdateProfile> logger)
        {
            _logger = logger;
        }

        [Function("UpdateProfile")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
