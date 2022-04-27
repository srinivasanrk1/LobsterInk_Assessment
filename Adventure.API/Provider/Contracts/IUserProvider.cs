using Adventure.API.DataAccess.DomainModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Adventure.Provider.Contracts
{
    public interface IUserProvider
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(string userId);
        Task AddUser(User user);
    }
}
