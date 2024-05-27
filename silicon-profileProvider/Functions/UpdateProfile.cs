using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using silicon_profileProvider.Models;

namespace silicon_profileProvider.Functions;

public class UpdateProfile
{
    private readonly ILogger<UpdateProfile> _logger;
    private readonly UserManager<UserEntity> _userManager;

    public UpdateProfile(ILogger<UpdateProfile> logger, UserManager<UserEntity> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    [Function("UpdateProfile")]
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
            UpdateProfileDetailsRequest updr = null!;

            try
            {
                updr = JsonConvert.DeserializeObject<UpdateProfileDetailsRequest>(body)!;
            }
            catch (Exception ex)
            {
                _logger.LogError($"JsonConvert.DeserializeObject<UserProfileRequest>(body) :: {ex.Message}");
            }

            if (updr != null)
            {
                try
                {
                    var userEntity = await _userManager.FindByNameAsync(updr.Email);

                    if (userEntity != null)
                    {
                        userEntity.FirstName = updr.FirstName;
                        userEntity.LastName = updr.LastName;
                        userEntity.PhoneNumber = updr.Phone;
                        userEntity.Phone = updr.Phone;
                        userEntity.Bio = updr.Bio;

                        var res = await _userManager.UpdateAsync(userEntity);

                        if (res != null && res.Succeeded)
                            return new OkResult();

                        _logger.LogError($"UserEntity update failed");
                    }

                    return new NotFoundObjectResult(new { StatusCode = 400, Message = "User for Token not found" });
                }
                catch (Exception ex)
                {
                    _logger.LogError($"_userManager.FindByIdAsync or _userManager.UpdateAsync :: {ex.Message}");
                }
            }
        }
        return new ObjectResult(new { Error = $"Function UpdateProfile failed" }) { StatusCode = 500 };
    }
}