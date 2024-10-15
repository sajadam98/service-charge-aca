using FluentAssertions;
using Library.Entities.Books;
using Library.Persistence.Ef.Books;
using Library.Persistence.Ef.UnitOfWorks;
using Library.Services.Books;
using Library.Services.Books.Contracts;
using Library.Services.Books.Contracts.Dtos;
using
    SalesSystem.TestTools.Infrastructure.
    DataBaseConfig.Integration;

namespace Library.Services.Tests.Books;

public class
    BookServiceTests :
    BusinessIntegrationTest
{
    private readonly BookService _sut;

    public BookServiceTests()
    {
        var unitOfWork =
            new EfUnitOfWork(Context);
        var repository =
            new EfBookRepository(Context);
        _sut = new BookAppService(
            unitOfWork, repository);
    }

    [Fact]
    public async Task
        Create_create_a_book_properly()
    {
        //arrange
        var dto = new CreateBookDto()
        {
            Title = "dummy_title"
        };
        
        //act
        var result =await _sut.CreateAsync(dto);

        //assert
        var actual = ReadContext.Books.Single();
        actual.Id.Should().Be(result);
        actual.Title.Should()
            .Be("dummy_title");
    }

    [Fact]
    public async Task
        GetById_get_book_by_id()
    {
        var book = new Book()
        {
            Title = "dummy"
        };
        Save(book);
        var book2 = new Book()
        {
            Title = "dummy2"
        };
        Save(book2);

        var actual =
           await _sut.GetByIdAsync(book2.Id);

        actual.Should()
            .BeEquivalentTo(
                new GetBookById("dummy2"));
    }

    [Fact]
    public async Task
        Delete_delete_a_book_properly()
    {
        var book = new Book()
        {
            Title = "dummy"
        };
        Save(book);
        var book2 = new Book()
        {
            Title = "dummy2"
        };
        Save(book2);

        await _sut.Delete(book2.Id);

        var actual =
            ReadContext.Books
                .SingleOrDefault(_ =>
                    _.Id == book2.Id);
        actual.Should().BeNull();

        ReadContext.Books
            .Any(_ => _.Id == book2.Id)
            .Should()
            .BeFalse();

        var actual2 =
            ReadContext.Books.ToList();
        actual2.Should().HaveCount(1);

        actual2.First().Id.Should()
            .Be(book.Id);
        
        actual2.Single().Id.Should()
            .Be(book.Id);

        actual2.Should()
            .Contain(_ => _.Id == book.Id);
    }
}