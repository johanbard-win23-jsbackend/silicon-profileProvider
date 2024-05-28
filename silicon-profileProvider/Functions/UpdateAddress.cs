using Data.Contexts;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using silicon_profileProvider.Models;

namespace silicon_profileProvider.Functions;

public class UpdateAddress
{
    private readonly ILogger<UpdateAddress> _logger;
    private readonly DataContext _context;
    private readonly UserManager<UserEntity> _userManager;

    public UpdateAddress(ILogger<UpdateAddress> logger, UserManager<UserEntity> userManager, DataContext context)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }

    [Function("UpdateAddress")]
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
            UpdateProfileAddressRequest upar = null!;

            try
            {
                upar = JsonConvert.DeserializeObject<UpdateProfileAddressRequest>(body)!;
            }
            catch (Exception ex)
            {
                _logger.LogError($"JsonConvert.DeserializeObject<UpdateProfileAddressRequest>(body) :: {ex.Message}");
            }

            if (upar != null)
            {
                try
                {
                    var userEntity = await _userManager.FindByNameAsync(upar.Email);

                    if (userEntity != null)
                    {
                        userEntity.Address = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == userEntity.AddressId);

                        if (userEntity.Address == null)
                        {
                            userEntity.Address = new AddressEntity
                            {
                                Address1 = upar.Address1,
                                Address2 = upar.Address2,
                                PostalCode = upar.PostalCode,
                                City = upar.City
                            };
                        }
                        else
                        {
                            userEntity.Address.Address1 = upar.Address1;
                            userEntity.Address.Address2 = upar.Address2;
                            userEntity.Address.PostalCode = upar.PostalCode;
                            userEntity.Address.City = upar.City;
                        }

                        var res = await _userManager.UpdateAsync(userEntity);

                        if (res != null && res.Succeeded)
                            return new OkResult();

                        _logger.LogError($"UserEntity.Address update failed");
                    }

                    return new NotFoundObjectResult(new { StatusCode = 404, Message = "User for Email not found" });
                }
                catch (Exception ex)
                {
                    _logger.LogError($"_userManager.FindByNameAsync or _userManager.UpdateAsync :: {ex.Message}");
                }
            }
        }
        return new ObjectResult(new { Error = $"Function UpdateAddress failed" }) { StatusCode = 500 };
    }
}
