using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using silicon_profileProvider.Models;

namespace silicon_profileProvider.Functions;

public class GetProfile
{
    private readonly ILogger<GetProfile> _logger;
    private readonly UserManager<UserEntity> _userManager;

    public GetProfile(ILogger<GetProfile> logger, UserManager<UserEntity> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    [Function("GetProfile")]
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
            GetProfileRequest gpr = null!;

            try
            {
                gpr = JsonConvert.DeserializeObject<GetProfileRequest>(body)!;
            }
            catch (Exception ex)
            {
                _logger.LogError($"JsonConvert.DeserializeObject<UserProfileRequest>(body) :: {ex.Message}");
            }

            if (gpr != null)
            {
                try
                {
                    var userEntity = await _userManager.FindByIdAsync(gpr.UserId);

                    if (userEntity != null)
                    {
                        UserProfileResult upResult = new UserProfileResult
                        {
                            ProfileImg = userEntity.ProfileImg,
                            FirstName = userEntity.FirstName,
                            LastName = userEntity.LastName,
                            Email = userEntity.Email!,
                            Phone = userEntity.Phone,
                            Bio = userEntity.Bio,
                        };

                        return new OkObjectResult(upResult);
                    }

                    return new NotFoundObjectResult("User for Token not found");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"_userManager.FindByIdAsync :: {ex.Message}");
                }
            }
        }
        return new BadRequestResult();
    }
}
