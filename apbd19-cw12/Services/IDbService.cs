using apbd19_cw12.DTOs;

namespace apbd19_cw12.Services;

public interface IDbService
{
    Task<TripsPageDto> GetTripsAsync(int pageNum, int pageSize = 10);
    Task DeleteClientAsync(int id);
}