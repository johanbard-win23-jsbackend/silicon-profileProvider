using Data.Contexts;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using silicon_profileProvider.Models;

namespace silicon_profileProvider.Functions;

public class DeleteAddress(ILogger<DeleteAddress> logger, DataContext context)
{
    private readonly ILogger<DeleteAddress> _logger = logger;
    private readonly DataContext _context = context;

    [Function("DeleteAddress")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
    {
        try
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var daReq = JsonConvert.DeserializeObject<DeleteAddressReq>(body);

            if (daReq == null)
                return new BadRequestObjectResult(new { Error = "Please provide a valid request" });

            var entity = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == daReq.Id);

            if (entity != null)
            {
                var res = _context.Addresses.Remove(entity);
                _context.SaveChanges();

                return new OkResult();
            }

        }
        catch (Exception ex)
        {
            return new ObjectResult(new { Error = $"Function DeleteAddress failed :: {ex.Message}" }) { StatusCode = 500 };
        }

        return new ObjectResult(new { Error = $"Function DeleteAddress failed :: Unknown" }) { StatusCode = 500 };
    }
}
