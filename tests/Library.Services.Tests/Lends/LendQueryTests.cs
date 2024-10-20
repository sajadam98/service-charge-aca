using FluentAssertions;
using Library.Entities.Books;
using Library.Entities.Lends;
using Library.Entities.Users;
using Library.Persistence.Ef.Lends;
using Library.Services.Lends.Contracts;
using Library.Services.Lends.Contracts.Dtos;
using SalesSystem.TestTools.Infrastructure.DataBaseConfig.Integration;

namespace Library.Services.Tests.Lends;

public class LendQueryTests :  BusinessIntegrationTest
{
    private readonly LendQuery _sut;

    public LendQueryTests()
    {
        _sut = new EFLendQuery(Context);
    }

    [Fact]
    public async Task GetAll()
    {
        var book = new Book()
        {
            Title = "title"
        };
        Save(book);
        var user = new User()
        {
            Name = "name"
        };
        Save(user);
        var lend = new Lend()
        {
            BookId = book.Id,
            UserId = user.Id,
            ReturnDate = new DateOnly(2021,1,1),
            LendDate = new DateOnly(2022,2,2),
            IsReturned = false
        };
        Save(lend);
        var result =
            await _sut.GetAllAsync();

        result.Should().HaveCount(1);
        result.First().Should()
            .BeEquivalentTo(new ShowLendDto
            {
                BookId = book.Id,
                UserId = user.Id,
                IsReturned = false,
                Id = lend.Id,
                ReturnDate = lend.ReturnDate,
                LendDate = lend.LendDate
            });
    }
    
    [Fact]
    public async Task GetAll_filter_book()
    {
        var book = new Book()
        {
            Title = "title"
        };
        Save(book);
        var book2 = new Book()
        {
            Title = "title2"
        };
        Save(book2);
        var user = new User()
        {
            Name = "name"
        };
        Save(user);
        var lend = new Lend()
        {
            BookId = book.Id,
            UserId = user.Id,
            ReturnDate = new DateOnly(2021,1,1),
            LendDate = new DateOnly(2022,2,2),
            IsReturned = false
        };
        Save(lend);
        var lend2 = new Lend()
        {
            BookId = book2.Id,
            UserId = user.Id,
            ReturnDate = new DateOnly(2021,1,1),
            LendDate = new DateOnly(2022,2,2),
            IsReturned = false
        };
        Save(lend2);
        var result =
            await _sut.GetAllAsync(book2.Id);

        result.Single().Id.Should()
            .Be(lend2.Id);
    }
}