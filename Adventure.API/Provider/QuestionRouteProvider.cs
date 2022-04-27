using Adventure.API.DataAccess.DomainModel;
using Adventure.API.DataAccess.Repositories;
using Adventure.API.Provider.Contracts;
using Adventure.API.System;
using Adventure.DataAccessLayer.DBContexts;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Adventure.API.Provider
{
    public class QuestionRouteProvider : IQuestionRouteProvider
    {
        private readonly IQuestionRouteRepository _questionRouteRepository;
        private readonly IResponseRepository _responseRepository;
        private readonly IAdventureRepository _adventureRepository;

        public QuestionRouteProvider(IQuestionRouteRepository questionRoutRepository,
            IResponseRepository responseRepository,
            IAdventureRepository adventureRepository)
        {
            _questionRouteRepository = questionRoutRepository;
            _adventureRepository = adventureRepository;
            _responseRepository = responseRepository;

        }
        ///<summary>
        /// This function do multiple operation, Since it needs to save mutiple table
        /// 1. Save adventure name, It is primary game name.
        /// 2. Save all details for question route tables like 
        /// questions, responses, question routes
        ///</summary>
        public async Task<string> AddQuestionRoute(AdventureGame adventureGame, bool existsOverWrite = false)
        {

            //Save adventure if not exists, Else throw ex already exists.

            var result = await AdventureNameSave(adventureGame.adventureName);

            //Already exists throw exception, same game key exists.
            if (result.exists && !existsOverWrite)
                throw new InvalidOrEmptyException("Same game type already exists, Do you want delete and create new ! ? ");
            //Already exists and user wants to overwrite.
            else if (result.exists && existsOverWrite)
            {
                await ClearAllOldAdventureQuestionRoutes(result.adventureId);
                result = await AdventureNameSave(adventureGame.adventureName);
            }

            string prevQuestionID = await ProcessChild(adventureGame.node, result.adventureId, null);

            foreach (var children in adventureGame.node.children ?? Enumerable.Empty<Node>())
            {
               await  RunThroughChildren(children, result.adventureId, prevQuestionID);

            }

            return result.adventureId;
         }

        private async Task<(bool exists, string adventureId)> AdventureNameSave(string adventureName)
        {
            var adventureId = await _adventureRepository.AdventureExists(adventureName);
            if (string.IsNullOrEmpty(adventureId))
            {
                return (false, await _adventureRepository.AddAdventure(new Adventures { Text = adventureName }));
            }
            return (true, adventureId);
        }

        private async Task ClearAllOldAdventureQuestionRoutes(string adventureId)
        {
            var queRoutes = await _questionRouteRepository.GetQuestionRoutesByAdventureId(adventureId);
            await _questionRouteRepository.DeleteQuestionRoutesRange(queRoutes?.ToList());
        }
        private async Task RunThroughChildren(Node child, string adventureId, string prevQuestionID)
        {

            string prevQueID = await ProcessChild(child, adventureId, prevQuestionID);

            foreach (var childItem in child.children ?? Enumerable.Empty<Node>())
            {
                await RunThroughChildren(childItem, adventureId, prevQueID);
            }
        }

        private async Task<string> ProcessChild(Node chilld, string adventureId, string previousQuestionId)
        {
            Questions questions = new Questions();
            questions.Text = chilld.question;

            Responses responseObj;
            responseObj = await _responseRepository.GetResponses(chilld.label);
            if (responseObj == null)
            {
                responseObj = new Responses();
                responseObj.Text = chilld.label;
            }

            QuestionRoute questionRoute = new QuestionRoute();
            questionRoute.Questions = questions;
            questionRoute.Responses = responseObj;
            questionRoute.PreviousQuestionRouteId = previousQuestionId;
            questionRoute.Order = chilld.level;
            questionRoute.AdventureId = adventureId;

            await _questionRouteRepository.AddQuestionRoute(questionRoute);

            return questionRoute.Id;
        }
    }
}
