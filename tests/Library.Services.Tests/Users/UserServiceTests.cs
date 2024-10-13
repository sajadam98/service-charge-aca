using FluentAssertions;
using Library.Entities.Users;
using Library.Persistence.Ef.UnitOfWorks;
using Library.Persistence.Ef.Users;
using Library.Services.Users;
using Library.Services.Users.Contracts;
using Library.Services.Users.Contracts.Dtos;
using Library.Services.Users.Exceptions;
using
    SalesSystem.TestTools.Infrastructure.
    DataBaseConfig.Integration;

namespace Library.Services.Tests.Users;

public class
    UserServiceTests :
    BusinessIntegrationTest
{
    private readonly UserService _sut;

    public UserServiceTests()
    {
        var unitOfWork =
            new EfUnitOfWork(Context);
        var repository =
            new EfUserRepository(Context);
        _sut = new UserAppService(
            unitOfWork,
            repository);
    }

    [Fact]
    public async Task Create_create_a_user_properly()
    {
        //arrange
        var dto = new CreateUserDto()
        {
            Name = "dummy"
        };

        //act
        var result =
            await _sut.CreateAsync(dto);

        //assert
        var actual = ReadContext.Set<User>().Single();
        actual.Id.Should().Be(result);
        actual.Name.Should().Be(dto.Name);
        actual.Name.Should().Be("dummy");
        actual.Lends.Should().HaveCount(0);
    }

    [Fact]
    public async Task
        Create_throw_exception_when_user_duplicated()
    {
        //arrange
        var user = new User()
        {
            Name = "ali",
        };
        Save(user);
        var dto = new CreateUserDto()
        {
            Name = "ali"
        };
        
        //act
        var actual = () => _sut.CreateAsync(dto);

        //assert
       await actual.Should()
            .ThrowExactlyAsync<UserDuplicateException>();
    }

    [Fact]
    public async Task Get()
    {
        var user = new User()
        {
            Name = "ali",
        };
        Save(user);
        var user2 = new User()
        {
            Name = "dummy",
        };
        Save(user2);

        var actual =
            await _sut.GetById(user2.Id);

        actual!.Name.Should().Be(user2.Name);
        actual.JoinDate.Should().Be(user2.JoinDate);
    }
    
    [Fact]
    public async Task GetAll()
    {
        var user = new User()
        {
            Name = "ali",
        };
        Save(user);
        var user2 = new User()
        {
            Name = "dummy",
        };
        Save(user2);

        var actual =
            await _sut.GetAll();

        actual.Should().Contain(_=> _.Name == user.Name && _.JoinDate == user.JoinDate);
        actual.Should().Contain(_=> _.Name == user2.Name && _.JoinDate == user2.JoinDate);
    }
}