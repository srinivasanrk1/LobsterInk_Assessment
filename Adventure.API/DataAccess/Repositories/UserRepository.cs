using Adventure.API.DataAccess.DomainModel;
using Adventure.DataAccessLayer;
using Adventure.DataAccessLayer.DBContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adventure.DataAccessLayer.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(string userId);
        Task<User> GetUserByEmailId(string emailId);
        Task AddUser(User user);
    }
    public class UserRepository : IUserRepository, IDisposable
    {
        private readonly AdventureContext _context;

        public UserRepository(AdventureContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUser(string userId)
        {
            return await _context.Users.SingleOrDefaultAsync(a => a.Id == userId);
        }

        public async Task<User> GetUserByEmailId(string emailId)
        {
            return await _context.Users.SingleOrDefaultAsync(a => a.Email == emailId);
        }



        public async Task AddUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose resources when needed
            }
        }


    }
}
