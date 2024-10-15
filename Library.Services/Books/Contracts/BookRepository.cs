using Library.Entities.Books;
using Library.Services.Books.Contracts.Dtos;

namespace Library.Services.Books.Contracts;

public interface BookRepository
{
    Task<GetBookById?> GetByIdAsync(int id);
    Task CreateAsync(Book book);
    Task<IEnumerable<ShowAllBooksDto>> GetAllAsync();
    Task<bool> CheckIfExistsByIdAsync(int userId);
    Task<Book?> FindById(int id);
    void Delete(Book book);
}