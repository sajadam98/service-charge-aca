using Library.Services.Books.Contracts.Dtos;

namespace Library.Services.Books.Contracts;

public interface BookService
{
    Task<GetBookById?> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateBookDto bookDto);
    Task<IEnumerable<ShowAllBooksDto>> GetAllAsync();
    Task Delete(int id);
}