using Adventure.API.DataAccess.DomainModel;
using Adventure.DataAccessLayer.Repositories;
using Adventure.Provider.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Adventure.Provider
{
    public class UserProvider : IUserProvider
    {
        private readonly IUserRepository _userRepository;

        public UserProvider(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task AddUser(User user)
        {
            await _userRepository.AddUser(user);
        }

        public async Task<User> GetUser(string userId)
        {
            return await _userRepository.GetUser(userId);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _userRepository.GetUsers();
        }
    }
}
