using Adventure.API.DataAccess.DomainModel;
using Adventure.DataAccessLayer.DBContexts;
using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Adventure.API.DataAccess.Repositories
{
    public interface IUserQuestionRoutesRepository
    {
        Task<bool> AddUserQuestionRoutes(string userId, string questionRouteId);
        Task<List<UserQuestionRoutes>> GetUserQuestionRoutes(string userId);
    }
    public class UserQuestionRoutesRepository : IUserQuestionRoutesRepository, IDisposable
    {
        private readonly AdventureContext _context;
        public UserQuestionRoutesRepository(AdventureContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<bool> AddUserQuestionRoutes(string userId, string questionRouteId)
        {
            UserQuestionRoutes userQuestionRoutes = new UserQuestionRoutes();
            userQuestionRoutes.UserId = userId;
            userQuestionRoutes.QuestionRouteId = questionRouteId;

            await _context.UserQuestionRoutes.AddAsync(userQuestionRoutes);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<UserQuestionRoutes>> GetUserQuestionRoutes(string userId)
        {
            return await _context.UserQuestionRoutes
                            .Include(k => k.QuestionRoute)
                            .ThenInclude(l => l.Questions)
                            .Include(j => j.QuestionRoute)
                            .ThenInclude(l => l.Responses)
                            .Include(j => j.QuestionRoute)
                            .ThenInclude(l => l.Adventure)
                            .Where(o => o.UserId == userId).ToListAsync();
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
