using Adventure.API.System;
using System.Threading.Tasks;
namespace Adventure.API.Provider.Contracts
{
    public interface IQuestionRouteProvider
    {
        Task<string> AddQuestionRoute(AdventureGame adventureGame, bool existsOverWrite = false);
    }
}
