using Adventure.API.DataAccess.DomainModel;
using Adventure.API.DataAccess.Repositories;
using Adventure.API.Provider.Contracts;
using Adventure.API.System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adventure.API.Provider
{
    public class AdventureProvider : IAdventureProvider
    {
        private readonly IQuestionRouteRepository _questionRouteRepository;

        private readonly IUserQuestionRoutesRepository _userQuestionRoutesRepository;

        private readonly IAdventureRepository _adventureRepository;

        public AdventureProvider(IQuestionRouteRepository questionRoutRepository,
            IUserQuestionRoutesRepository userQuestionRoutesRepository,
            IAdventureRepository adventureRepository)
        {
            _questionRouteRepository = questionRoutRepository;
            _userQuestionRoutesRepository = userQuestionRoutesRepository;
            _adventureRepository = adventureRepository;
        }

        public async Task<List<AdventureResult>> GetAdventuresList()
        {
            var result = await _adventureRepository.GetAllAdventures();
            return result.Select(o => new AdventureResult
            {
                adventureId = o.Id,
                adventureName = o.Text
            }
            ).ToList();
        }

        public async Task<AdventureStepResult> MoveToNextLevel(string quoteRouteId, string userId)
        {

            var queRoutesExist = await _questionRouteRepository.GetQuestionRoutesById(quoteRouteId);
            if (queRoutesExist == null)
                throw new InvalidOrEmptyException("Question route not exists, Please enter route Id in list!");

            await _userQuestionRoutesRepository.AddUserQuestionRoutes(userId, quoteRouteId);
            var nextQueRoutes = await _questionRouteRepository.GetQuestionRoutesByNextRouteId(quoteRouteId);

            if (nextQueRoutes == null || nextQueRoutes?.Count() == 0)
                throw new InvalidOrEmptyException("Adventure ends, Wise Decisions !!!");
            return new AdventureStepResult
            {
                currentquestionRouteId = quoteRouteId,
                nextQuestions = nextQueRoutes.Select(o => new NextQuestions()
                {
                    questionRouteId = o.Id,
                    questionText = o.Questions.Text,
                    responseId = o.ResponseId,
                    responseText = o.Responses.Text
                }).ToList()
            };
        }

        public async Task<AdventureStepResult> StartGame(string adventureId, string userId)
        {
            var adventure = await _adventureRepository.GetAdventureById(adventureId);
            if (adventure == null)
                throw new InvalidOrEmptyException("Adventure not exists, Please check with exact Id!!");

            var queRoutes = await _questionRouteRepository.GetQuestionRoutesByOrder(1, adventureId);
            var nextQueRoutes = await _questionRouteRepository.GetQuestionRoutesByNextRouteId(queRoutes.First().Id);
            await _userQuestionRoutesRepository.AddUserQuestionRoutes(userId, queRoutes.First().Id);


            if (nextQueRoutes == null || nextQueRoutes?.Count() == 0)
                throw new InvalidOrEmptyException("Adventure ends, No decisions is been added !!");
            return new AdventureStepResult
            {
                currentquestionRouteId = queRoutes.First().Id,
                currentQuestionText = queRoutes.First().Questions.Text,
                nextQuestions = nextQueRoutes.Select(o => new NextQuestions()
                {
                    questionRouteId = o.Id,
                    questionText = o.Questions.Text,
                    responseId = o.ResponseId,
                    responseText = o.Responses.Text
                }).ToList()
            };
        }

        public async Task<AdventureGame> UserChoosenGamePath(string userId, string adventureId)
        {
            try
            {


                var userRoutes = await _userQuestionRoutesRepository.GetUserQuestionRoutes(userId);
                if (userRoutes == null) throw new InvalidOrEmptyException($"No details exists for user Id{userId}");




                AdventureGame adventureGame = new AdventureGame();


                var routeItem = userRoutes.OrderByDescending(i => i.QuestionRoute.Order.HasValue)
                                          .ThenBy(i => i.QuestionRoute.Order).ToList();

                adventureGame.adventureName = routeItem.First().QuestionRoute.Adventure.Text;
                adventureGame.node.label = routeItem.First().QuestionRoute.Responses.Text;
                adventureGame.node.question = routeItem.First().QuestionRoute.Questions.Text;
                adventureGame.node.level = routeItem.First().QuestionRoute.Order;
                adventureGame.node.children = ChildrenOf(userRoutes, 1);

                return adventureGame;
            }
            catch (global::System.Exception ex)
            {

                throw ex;
            }
        }
        private List<Node> ChildrenOf(List<UserQuestionRoutes> flatItems, int? order)
        {
            var childrenFlatItems = flatItems.Where(i => i.QuestionRoute.Order == order);
            order = (order.HasValue) ? order + 1 : null;

            return childrenFlatItems.Select(i => new Node
            {
                label = i.QuestionRoute.Responses.Text,
                level = i.QuestionRoute.Order,
                question = i.QuestionRoute.Questions.Text,
                children = ChildrenOf(flatItems, order)
            })
            .ToList();
        }
    }
}
