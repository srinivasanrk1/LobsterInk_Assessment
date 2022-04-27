using Adventure.API.DataAccess.DomainModel;
using Adventure.API.System;
using Adventure.DataAccessLayer.DBContexts;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Adventure.API.DataAccess.Repositories
{
    public interface IQuestionRouteRepository
    {
        Task<IEnumerable<QuestionRoute>> GetQuestionRoutesByOrder(int order, string adventureId);
        Task<IEnumerable<QuestionRoute>> GetQuestionRoutesByNextRouteId(string queRouteId);
        Task<IEnumerable<QuestionRoute>> GetQuestionRoutesByAdventureId(string adventureId);
        Task<QuestionRoute> GetQuestionRoutesById(string queRouteId);
        Task DeleteQuestionRoutesRange(List<QuestionRoute> questionRoutes);
        Task<string> AddQuestionRoute(QuestionRoute questionRoute);
    }

    public class QuestionRouteRepository : IQuestionRouteRepository, IDisposable
    {
        private readonly AdventureContext _context;

        public QuestionRouteRepository(AdventureContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<QuestionRoute>> GetQuestionRoutesByOrder(int order, string adventureId)
        {
            return await _context.QuestionRoutes
                                    .Include(k => k.Questions)
                                    .Include(j => j.Responses)
                                    .Where(o => o.AdventureId == adventureId && o.Order == order).ToListAsync();
        }
        public async Task<IEnumerable<QuestionRoute>> GetQuestionRoutesByAdventureId(string adventureId)
        {
            return await _context.QuestionRoutes
                                    .Include(k => k.Questions)
                                    .Include(j => j.Responses)
                                    .Where(o => o.AdventureId == adventureId).ToListAsync();
        }
        public async Task<IEnumerable<QuestionRoute>> GetQuestionRoutesByNextRouteId(string queRouteId)
        {
            return await _context.QuestionRoutes
                                    .Include(k => k.Questions)
                                    .Include(j => j.Responses)
                                    .Where(o => o.PreviousQuestionRouteId == queRouteId).ToListAsync();
        }
        public async Task<IEnumerable<QuestionRoute>> GetNextRoutesById(string queRouteId)
        {
            return await _context.QuestionRoutes
                                  .Include(k => k.Questions)
                                  .Include(j => j.Responses)
                                  .Where(o => o.Id == queRouteId).ToListAsync();
        }

        public async Task DeleteQuestionRoutesRange(List<QuestionRoute> quesRoutes)
        {
            var userRoutes = await _context.UserQuestionRoutes
                            .Include(o => o.QuestionRoute)
                            .Where(p => p.QuestionRoute.AdventureId == quesRoutes[0].AdventureId).ToListAsync();

            if (userRoutes != null)
                _context.UserQuestionRoutes.RemoveRange(userRoutes.ToList());
            _context.QuestionRoutes.RemoveRange(quesRoutes);
            await _context.SaveChangesAsync();
        }
        public async Task<string> AddQuestionRoute(QuestionRoute questionRoute)
        {
            if (questionRoute == null)
            {
                throw new ArgumentNullException(nameof(questionRoute));
            }

            await _context.QuestionRoutes.AddAsync(questionRoute);
            await _context.SaveChangesAsync();
            return questionRoute.Id;
        }

        public async Task<QuestionRoute> GetQuestionRoutesById(string queRouteId)
        {
           return await _context.QuestionRoutes.FirstOrDefaultAsync(o => o.Id == queRouteId);
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
