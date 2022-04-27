using Adventure.API.DataAccess.DomainModel;
using Adventure.API.System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Adventure.API.Provider.Contracts
{
    public interface IAdventureProvider
    {
        public Task<AdventureStepResult> StartGame(string adventureId, string userId);
        public Task<AdventureStepResult> MoveToNextLevel(string quoteRouteId, string userId);
        public Task<AdventureGame> UserChoosenGamePath(string userId, string adventureId);
        public Task<List<AdventureResult>> GetAdventuresList();
    }
}
