using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using silicon_profileProvider.Models;
using System;

namespace silicon_profileProvider.Functions
{
    public class GetUserSubscriberId(ILogger<GetUserSubscriberId> logger, UserManager<UserEntity> userManager)
    {
        private readonly ILogger<GetUserSubscriberId> _logger = logger;
        private readonly UserManager<UserEntity> _userManager = userManager;

        [Function("GetUserSubscriberId")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogWarning("Started");
            string body = null!;

            try
            {
                body = await new StreamReader(req.Body).ReadToEndAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"StreamReader :: {ex.Message}");
            }

            if (body != null)
            {
                GetUserSubscriberIdRequest gusidReq = null!;

                try
                {
                    gusidReq = JsonConvert.DeserializeObject<GetUserSubscriberIdRequest>(body)!;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"JsonConvert.DeserializeObject<GetUserSubscriberIdRequest>(body) :: {ex.Message}");
                }

                if (gusidReq.Id != null)
                {
                    try
                    {
                        var res = await _userManager.FindByIdAsync(gusidReq.Id);

                        if (res != null)
                            return new OkObjectResult( new { Id = res.SubscriberId });
                    }
                    catch (Exception ex) { _logger.LogError($"JsonConvert.DeserializeObject<GetUserSubscriberIdRequest>(body) :: {ex.Message}"); }
                }
                    
            }

            return new BadRequestResult();
            
        }
    }
}
