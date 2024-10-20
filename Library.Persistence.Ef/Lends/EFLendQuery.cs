using Library.Services.Lends.Contracts;
using Library.Services.Lends.Contracts.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistence.Ef.Lends;

public class EFLendQuery(
    EfDataContext dbContext) : LendQuery
{
    public async Task<List<ShowLendDto>> GetAllAsync(int? bookId,
        int? userId)
    {
        var baseQuery = dbContext.Lends.AsQueryable();

        if (bookId != null)
            baseQuery = baseQuery.Where(_ => _.BookId == bookId);
        if (userId != null)
            baseQuery = baseQuery.Where(_ => _.UserId == userId);

        return await baseQuery.Select(_ => new ShowLendDto
        {
            BookId = _.BookId,
            Id = _.Id,
            UserId = _.UserId,
            LendDate = _.LendDate,
            ReturnDate = _.ReturnDate,
            IsReturned = _.IsReturned
        }).ToListAsync();
    }
}