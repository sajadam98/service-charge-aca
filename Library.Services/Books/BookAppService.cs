using Library.Entities.Books;
using Library.Services.Books.Contracts;
using Library.Services.Books.Contracts.Dtos;
using Library.Services.UnitOfWorks;

namespace Library.Services.Books;

public class BookAppService(
    UnitOfWork unitOfWork,
    BookRepository bookRepository) : BookService
{
    public async Task<GetBookById?> GetByIdAsync(int id)
    {
        return 
            await bookRepository
                .GetByIdAsync(id);
    }

    public async Task<int> CreateAsync(
        CreateBookDto bookDto)
    {
        var newBook = new Book
        {
            Title = bookDto.Title
        };
        await bookRepository.CreateAsync(newBook);
        await unitOfWork.SaveAsync();
        return newBook.Id;
    }

    public async Task<IEnumerable<ShowAllBooksDto>> GetAllAsync()
    {
        return await bookRepository.GetAllAsync();
    }

    public async Task Delete(int id)
    {
        var book =
            await bookRepository.FindById(id);

        bookRepository.Delete(book);

        await unitOfWork.SaveAsync();
    }
}