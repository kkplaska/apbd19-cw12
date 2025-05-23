using System.Globalization;
using System.Transactions;
using apbd19_cw12.DTOs;
using apbd19_cw12.Exceptions;
using apbd19_cw12.Models;
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

    [HttpPost]
    [Route("{idTrip}/clients")]
    public async Task<IActionResult> AddClientToTrip(AddClientDto newTripsClient, int idTrip)
    {
        try
        {
            await _dbService.CheckClient(newTripsClient.Pesel);
            await _dbService.CheckIsClientAssignedToTrip(newTripsClient.Pesel);
            await _dbService.CheckTripExists(idTrip);
            await _dbService.CheckTripDateFrom(idTrip);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        DateTime? paymentDate = null;
        if (!string.IsNullOrWhiteSpace(newTripsClient.PaymentDate))
        {
            string[] formats =
            {
                "yyyy-MM-dd",
                "MM/dd/yyyy",
                "dd/MM/yyyy",
                "M/d/yyyy",
                "MM/d/yyyy",
                "d/M/yyyy",
                "dd/M/yyyy",
                "yyyy/MM/dd",
                "dd.MM.yyyy",
                "M.d.yyyy",
                "yyyy.MM.dd",
                "yyyyMMdd",
                "yyyy-MM-ddTHH:mm:ss",
                "yyyy-MM-ddTHH:mm:ssZ"
            };

            if (!DateTime.TryParseExact(
                    newTripsClient.PaymentDate,
                    formats,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime parsedDate))
            {
                return BadRequest("Invalid date format for PaymentDate.");
            }

            paymentDate = parsedDate;
        }
        
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var clientId = await _dbService.AddClient(new Client()
            {
                FirstName = newTripsClient.FirstName,
                LastName = newTripsClient.LastName,
                Email = newTripsClient.Email,
                Telephone = newTripsClient.Telephone,
                Pesel = newTripsClient.Pesel
            });
            await _dbService.AddClientToTrip(clientId, idTrip, paymentDate);
            
            scope.Complete();
        }
        return Created();
    }
}