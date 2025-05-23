using apbd19_cw12.Services;
using Microsoft.AspNetCore.Mvc;

namespace apbd19_cw12.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TripsController : ControllerBase
{
    private readonly IDbService _dbService;

    public TripsController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTripsAsync([FromQuery] int pageNum = 1, [FromQuery] int pageSize = 10)
    {
        var tripPage = await _dbService.GetTripsAsync(pageNum, pageSize);
        return Ok(tripPage);
    }
}