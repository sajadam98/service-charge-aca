using Library.Services.Lends.Contracts.Dtos;

namespace Library.Services.Lends.Contracts;

public interface LendQuery
{
    Task<List<ShowLendDto>> GetAllAsync(
        int? bookId = null,
        int? userId = null);
}