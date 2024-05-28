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

                    if (userEntity != null && userEntity.Email != null)
                    {
                        GetProfileResult gpResult = new GetProfileResult
                        {
                            UserId = userEntity.Id,
                            ProfileImg = userEntity.ProfileImg,
                            FirstName = userEntity.FirstName,
                            LastName = userEntity.LastName,
                            Email = userEntity.Email,
                            Phone = userEntity.Phone,
                            Bio = userEntity.Bio
                        };

                        if (userEntity.Address != null)
                        {
                            gpResult.Address1 = userEntity.Address.Address1;
                            gpResult.Address2 = userEntity.Address.Address2;
                            gpResult.PostalCode = userEntity.Address.PostalCode;
                            gpResult.City = userEntity.Address.City;
                        }

                        //var json = JsonConvert.SerializeObject(gpResult);

                        return new OkObjectResult(gpResult);
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
