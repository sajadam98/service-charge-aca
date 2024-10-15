using Library.Entities.Users;
using Library.Services.Users.Contracts;
using Library.Services.Users.Contracts.Dtos;
using Library.Services.Users.Users.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistence.Ef.Users;

public class EfUserRepository(
    EfDataContext dbContext) : UserRepository
{
    public async Task CreateAsync(
        User newUser)
    {
        await dbContext.Users.AddAsync(
            newUser);
    }

    public async Task<ShowUserDto?>
        GetByIdAsync(int id)
    {
        return await dbContext.Set<User>()
            .Where(_ => _.Id == id)
            .Select(_ => new ShowUserDto()
            {
                JoinDate = _.JoinDate,
                Name = _.Name
            }).FirstOrDefaultAsync();
    }

    public async Task<bool>
        CheckIfExistsAsync(string userName)
    {
        return await
            dbContext.Users.AnyAsync(_ =>
                _.Name == userName);
    }

    public async Task<bool> IsExistsNameWithoutUser(
        int id,
        string name)
    {
        return await
            dbContext.Users.AnyAsync(_ =>
                _.Name == name &&
                _.Id != id);
    }

    public async Task<bool>
        CheckIfExistsByIdAsync(int id)
    {
        return await
            dbContext.Users.AnyAsync(_ =>
                _.Id == id);
    }

    public async Task<int>
        CalculateYearsHistoryAsync(
            int id)
    {
        var temp = await dbContext.Users
            .Where(_ => _.Id == id)
            .Select(_ =>
                new
                {
                    History =
                        DateTime.Now.Year -
                        _.JoinDate.Year
                }).FirstAsync();
        return temp.History;
    }

    public async Task<int>
        CurrentLendsCountAsync(int userId)
    {
        return await dbContext.Lends
            .Where(_ =>
                _.UserId == userId &&
                _.IsReturned == false)
            .CountAsync();
    }

    public async
        Task<IEnumerable<ShowAllUsersDto>>
        GetAllAsync()
    {
        return await dbContext.Users.Select(
            _ => new ShowAllUsersDto
            {
                Id = _.Id,
                JoinDate = _.JoinDate,
                Name = _.Name,
                Penalty =
                    (from user in dbContext
                            .Users
                        join lend in
                            dbContext.Lends
                            on user.Id equals
                            lend.UserId
                        where
                            user.Id ==
                            _.Id &&
                            lend
                                .IsReturned ==
                            false &&
                            lend.ReturnDate <
                            DateOnly
                                .FromDateTime(
                                    DateTime
                                        .Now)
                        group lend by user.Id
                        into g
                        select new
                        {
                            Days = g.Sum(l =>
                                DateTime.Now
                                    .Day -
                                l.ReturnDate
                                    .Day)
                        }).Select(
                        _ => _.Days)
                    .FirstOrDefault()
            }).ToListAsync();
    }

    public async Task<User?> FindById(int id)
    {
        return await dbContext.Set<User>()
            .FindAsync(id);
    }

    public void Update(User user)
    {
        dbContext.Users.Update(user);
    }

    public void Delete(User user)
    {
        dbContext.Users.Remove(user);
    }
}