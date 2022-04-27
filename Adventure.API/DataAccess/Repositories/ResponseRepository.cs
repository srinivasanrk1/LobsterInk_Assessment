using Adventure.API.DataAccess.DomainModel;
using Adventure.DataAccessLayer.DBContexts;
using System;
using System.Threading.Tasks; 
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Adventure.API.DataAccess.Repositories
{
    public interface IResponseRepository
    {
        Task<Responses> GetResponses(string text);
    }
    public class ResponseRepository : IResponseRepository, IDisposable
    {
        private readonly AdventureContext _context;

        public ResponseRepository(AdventureContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Responses> GetResponses(string text)
        {
            return await _context.Responses.FirstOrDefaultAsync(a => a.Text.ToLower() == text.ToLower());
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
