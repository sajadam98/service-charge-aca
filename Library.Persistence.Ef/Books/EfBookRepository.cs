using Library.Entities.Books;
using Library.Services.Books.Contracts;
using Library.Services.Books.Contracts.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistence.Ef.Books;

public class EfBookRepository(EfDataContext dbContext) : BookRepository
{
    public async Task<GetBookById?> GetByIdAsync(int id)
    {
        return await dbContext.Books
            .Where(_=>_.Id == id)
            .Select(_=> new GetBookById(_.Title))
            .FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Book book)
    {
        await dbContext.Books.AddAsync(book);
    }

    public async Task<IEnumerable<ShowAllBooksDto>> GetAllAsync()
    {
        // return await dbContext.Books.Select(_ => new ShowBookDto
        // {
        //     Id = _.Id,
        //     Title = _.Title
        // }).ToListAsync();
        return await dbContext.Books
            .Include(b => b.Lends)
            .Include(_ => _.Rates)
            .Select(b => new ShowAllBooksDto
            {
                Id = b.Id,
                Title = b.Title,
                LendsCount = b.Lends.Count,
                AverageScore = b.Rates.Any()
                    ? b.Rates.Average(_ => _.Score)
                    : 0
            })
            .OrderByDescending(b => b.AverageScore)
            .ToListAsync();

        // return await dbContext.Books.Include(_ => _.Lends)
        //     .ThenInclude(_ => _.Rate).Select(_ => new ShowAllBooksDto
        //     {
        //         Id = _.Id,
        //         Title = _.Title,
        //         LendsCount = _.Lends.Count,
        //         AverageScore = _.Lends.Average(l => l.Rate.Score)
        //     }).OrderByDescending(_ => _.AverageScore).ToListAsync();
    }

    public async Task<bool> CheckIfExistsByIdAsync(int lendDtoBookId)
    {
        return await dbContext.Books.AnyAsync(_ => _.Id == lendDtoBookId);
    }

    public async Task<Book?> FindById(int id)
    {
        return await dbContext.Books.FindAsync(id);
    }

    public void Delete(Book book)
    {
        dbContext.Books.Remove(book);
    }
}