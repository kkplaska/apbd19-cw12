using apbd19_cw12.Exceptions;
using apbd19_cw12.Services;
using Microsoft.AspNetCore.Mvc;

namespace apbd19_cw12.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly IDbService _dbService;

    public ClientsController(IDbService dbService)
    {
        _dbService = dbService;
    }

    
    [HttpDelete("{idClient}")]
    public async Task<IActionResult> DeleteClientAsync(int idClient)
    {
        try
        {
            await _dbService.DeleteClientAsync(idClient);
            return NoContent();
        }
        catch (AssignedTripsException)
        {
            return BadRequest($"Klient o id {idClient} jest przypisany do co najmniej jednej wycieczki");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
}