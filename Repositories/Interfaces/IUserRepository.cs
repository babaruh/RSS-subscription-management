using TestTask.Models;

namespace TestTask.Repository.Interfaces;

public interface IUserRepository
{
    User FindByUsername(string username);
    User FindByUsernameAndPassword(string username, string password);
    void Add(User user);
}
