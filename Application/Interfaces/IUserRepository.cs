using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        Task CreateUserAsync(User user);
        Task<bool> CheckUsernameExistAsync(string username);
    }
}