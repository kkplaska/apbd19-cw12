using apbd19_cw12.DTOs;
using apbd19_cw12.Models;

namespace apbd19_cw12.Services;

public interface IDbService
{
    Task<TripsPageDto> GetTripsAsync(int pageNum, int pageSize = 10);
    Task DeleteClientAsync(int id);
    Task CheckClient(String pesel);
    Task CheckIsClientAssignedToTrip(String pesel);
    Task CheckTripExists(int id);
    Task CheckTripDateFrom(int id);
    Task<int> AddClient (Client client);
    Task AddClientToTrip(int idClient, int idTrip, DateTime? paymentDate);
}