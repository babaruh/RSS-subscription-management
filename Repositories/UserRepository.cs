using TestTask.Data;
using TestTask.Models;
using TestTask.Repository.Interfaces;

namespace TestTask.Repository;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public User FindByUsername(string username)
    {
        return _dbContext.Users.SingleOrDefault(u => u.Username == username);
    }

    public void Add(User user)
    {
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
    }
    
    public User FindByUsernameAndPassword(string username, string password)
    {
        return _dbContext.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
    }

}
