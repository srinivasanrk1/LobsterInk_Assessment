using Adventure.API.DataAccess.DomainModel;
using Adventure.DataAccessLayer.DBContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Adventure.API.DataAccess.Repositories
{
    public interface IAdventureRepository
    {
        Task<IEnumerable<Adventures>> GetAllAdventures();
        Task<string> AdventureExists(string adventureName);
        Task<string> AddAdventure(Adventures adventures);
        Task<Adventures> GetAdventureById(string queRouteId);

    }

    public class AdventureRepository : IAdventureRepository, IDisposable
    {
        private readonly AdventureContext _context;

        public AdventureRepository(AdventureContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<string> AdventureExists(string adventureName)
        {
            var result = await _context.Adventures.FirstOrDefaultAsync(o => o.Text.ToLower() == adventureName);
            return result?.Id;
        }
        public async Task<string> AddAdventure(Adventures adventures)
        {
            if (adventures == null)
            {
                throw new ArgumentNullException(nameof(adventures));
            }

            await _context.Adventures.AddAsync(adventures);
            await _context.SaveChangesAsync();
            return adventures.Id;
        }

        public async Task<IEnumerable<Adventures>> GetAllAdventures()
        {
            return await _context.Adventures.ToListAsync();
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
        public async Task<Adventures> GetAdventureById(string adventureId)
        {
            return await _context.Adventures.FirstOrDefaultAsync(o => o.Id == adventureId);
        }
    }




}
