using Adventure.API.DataAccess.DomainModel;
using Adventure.API.DataAccess.Repositories;
using Adventure.API.Provider;
using Adventure.API.System;
using Adventure.DataAccessLayer.DBContexts;
using Adventure.DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Adventure.Test
{
    public class AdventureProviderTest
    {

        [Fact]
        public async Task Validate_Create_Adventure_Save()
        {
               Mock<IQuestionRouteRepository> mockQueRepo = new Mock<IQuestionRouteRepository>();
            Mock<IResponseRepository> mockIuQueRepo = new Mock<IResponseRepository>();
            Mock<IAdventureRepository> mockAdvRep = new Mock<IAdventureRepository>();

            var jsonString = File.ReadAllText(System.AppContext.BaseDirectory + "\\sample.json");
            var jsonData = JsonConvert.DeserializeObject<AdventureGame>(jsonString);

            var expected = Guid.NewGuid().ToString();
            mockAdvRep.Setup(o => o.AddAdventure(It.IsAny<Adventures>())).ReturnsAsync(expected);
            mockQueRepo.Setup(o => o.AddQuestionRoute(It.IsAny<QuestionRoute>())).ReturnsAsync(Guid.NewGuid().ToString());
            var questionRoute = new QuestionRouteProvider(mockQueRepo.Object, mockIuQueRepo.Object, mockAdvRep.Object);

            var actual = await questionRoute.AddQuestionRoute(jsonData);

            Assert.NotNull(actual);
            Assert.Equal(actual, expected);
        }

        [Fact]
        public async Task Validate_Exist_Throws_Exception_Adventure_Save()
        {

            Mock<IQuestionRouteRepository> mockQueRepo = new Mock<IQuestionRouteRepository>();
            Mock<IResponseRepository> mockIuQueRepo = new Mock<IResponseRepository>();
            Mock<IAdventureRepository> mockAdvRep = new Mock<IAdventureRepository>();

            var jsonString = File.ReadAllText(System.AppContext.BaseDirectory + "\\sample.json");
            var jsonData = JsonConvert.DeserializeObject<AdventureGame>(jsonString);

            var expected = Guid.NewGuid().ToString();
            mockAdvRep.Setup(o => o.AdventureExists(It.IsAny<string>())).ReturnsAsync(expected);

            var questionRoute = new QuestionRouteProvider(mockQueRepo.Object, mockIuQueRepo.Object, mockAdvRep.Object);

            var ex = await Assert.ThrowsAsync<InvalidOrEmptyException>
                (async () => await questionRoute.AddQuestionRoute(jsonData));

            Assert.NotNull(ex);
            Assert.Equal("Same game type already exists, Do you want delete and create new ! ? ", ex.Message);
        }

        [Fact]
        public async Task Validate_Overwrite_Adventure_Save()
        {

            Mock<IQuestionRouteRepository> mockQueRepo = new Mock<IQuestionRouteRepository>();
            Mock<IResponseRepository> mockIuQueRepo = new Mock<IResponseRepository>();
            Mock<IAdventureRepository> mockAdvRep = new Mock<IAdventureRepository>();

            var jsonString = File.ReadAllText(System.AppContext.BaseDirectory + "\\sample.json");
            var jsonData = JsonConvert.DeserializeObject<AdventureGame>(jsonString);

            var expectedExist = Guid.NewGuid().ToString();
            mockAdvRep.Setup(o => o.AdventureExists(It.IsAny<string>())).ReturnsAsync(expectedExist);

            var questionRouteExist = new QuestionRouteProvider(mockQueRepo.Object, mockIuQueRepo.Object, mockAdvRep.Object);

            var ex = await Assert.ThrowsAsync<InvalidOrEmptyException>
                (async () => await questionRouteExist.AddQuestionRoute(jsonData));

            var expected = Guid.NewGuid().ToString();
            mockAdvRep.Setup(o => o.AdventureExists(It.IsAny<string>())).ReturnsAsync(string.Empty);
            mockAdvRep.Setup(o => o.AddAdventure(It.IsAny<Adventures>())).ReturnsAsync(expected);
            mockQueRepo.Setup(o => o.AddQuestionRoute(It.IsAny<QuestionRoute>())).ReturnsAsync(Guid.NewGuid().ToString());
            var questionRoute = new QuestionRouteProvider(mockQueRepo.Object, mockIuQueRepo.Object, mockAdvRep.Object);

            var actual = await questionRoute.AddQuestionRoute(jsonData, true);
            Assert.NotNull(actual);
            Assert.Equal(actual, expected);

        }
        [Fact]
        public async Task Validate_Questions_Text_And_InOrder_Adventure_Save()
        {

            DbContextOptions<AdventureContext> options = RepositoryHelperTest.DbContextOptionsInMemory();
            RepositoryHelperTest.CreateInMemoryDBData(options);

            using (var context = new AdventureContext(options))
            {
                var mockQuesRouteRepo = new QuestionRouteRepository(context);
                var mockRespRepo = new ResponseRepository(context);
                var mockAdvRepo = new AdventureRepository(context);

                var jsonString = File.ReadAllText(System.AppContext.BaseDirectory + "\\sample.json");
                var jsonData = JsonConvert.DeserializeObject<AdventureGame>(jsonString);

                var questionRoute = new QuestionRouteProvider(mockQuesRouteRepo, mockRespRepo, mockAdvRepo);


                var actual = await questionRoute.AddQuestionRoute(jsonData, true);

                var actualQuestions = context.QuestionRoutes.Include(o => o.Questions).Where(o => o.Order == 1).FirstOrDefault();

                Assert.NotNull(actualQuestions);
                Assert.Equal(actualQuestions?.Questions.Text, jsonData?.node.question);
            }
        }


        [Fact]
        public async Task Validate_Start_Game_On_By_Adventure()
        {

            DbContextOptions<AdventureContext> options = RepositoryHelperTest.DbContextOptionsInMemory();
            RepositoryHelperTest.CreateInMemoryDBData(options);

            using (var context = new AdventureContext(options))
            {
                var mockQuesRouteRepo = new QuestionRouteRepository(context);
                var mockQueRouRepo = new UserQuestionRoutesRepository(context);
                var mockAdvRepo = new AdventureRepository(context);
                var mockRespRepo = new ResponseRepository(context);
                var mockUserRepo = new UserRepository(context);

                var jsonString = File.ReadAllText(System.AppContext.BaseDirectory + "\\sample.json");
                var jsonData = JsonConvert.DeserializeObject<AdventureGame>(jsonString);


                User user = new User { FirstName = "test_User", Email = "test_user@email.com" };
                await mockUserRepo.AddUser(user);
                var testUsr = await mockUserRepo.GetUserByEmailId(user.Email);

                var advProvider = new AdventureProvider(mockQuesRouteRepo, mockQueRouRepo, mockAdvRepo);
                var questionRoute = new QuestionRouteProvider(mockQuesRouteRepo, mockRespRepo, mockAdvRepo);

                var adventureId = await questionRoute.AddQuestionRoute(jsonData, true);

                var nextQueRoutes = await advProvider.StartGame(adventureId, testUsr.Id);
                Assert.NotNull(nextQueRoutes);
                Assert.Equal(nextQueRoutes.currentQuestionText, jsonData?.node.question);
            }
        }

        [Fact]
        public async Task Validate_Move_To_Next_Level_In_Adventure()
        {

            DbContextOptions<AdventureContext> options = RepositoryHelperTest.DbContextOptionsInMemory();
            RepositoryHelperTest.CreateInMemoryDBData(options);

            using (var context = new AdventureContext(options))
            {
                var mockQuesRouteRepo = new QuestionRouteRepository(context);
                var mockQueRouRepo = new UserQuestionRoutesRepository(context);
                var mockAdvRepo = new AdventureRepository(context);
                var mockRespRepo = new ResponseRepository(context);
                var mockUserRepo = new UserRepository(context);
                var jsonString = File.ReadAllText(System.AppContext.BaseDirectory + "\\sample.json");
                var jsonData = JsonConvert.DeserializeObject<AdventureGame>(jsonString);

                User user = new User { FirstName = "test_User", Email = "test_user@email.com" };
                await mockUserRepo.AddUser(user);
                var testUsr = await mockUserRepo.GetUserByEmailId(user.Email);

                var advProvider = new AdventureProvider(mockQuesRouteRepo, mockQueRouRepo, mockAdvRepo);
                var questionRoute = new QuestionRouteProvider(mockQuesRouteRepo, mockRespRepo, mockAdvRepo);
                var adventureId = await questionRoute.AddQuestionRoute(jsonData, true);
                var nextQueRoutes = await advProvider.StartGame(adventureId, testUsr.Id);

                //After adventure begin, Choose answer as yes and pass route Id
                var chooseYesAnswerId = nextQueRoutes.nextQuestions.Where(o => o.responseText == "Yes").FirstOrDefault()?.questionRouteId;

                //Get child nodes for choosing yes as answer
                var actual = jsonData?.node.children.FirstOrDefault(o => o.label == "Yes")?.children.Count();

                var nextLevelRoutes = await advProvider.MoveToNextLevel(
                    chooseYesAnswerId, testUsr.Id);

                Assert.NotNull(nextLevelRoutes);
                Assert.Equal(actual, nextLevelRoutes.nextQuestions.Count);
            }
        }

        [Fact]
        public async Task Validate_User_Flow_In_Decisions_In_Adventure()
        {

            DbContextOptions<AdventureContext> options = RepositoryHelperTest.DbContextOptionsInMemory();
            RepositoryHelperTest.CreateInMemoryDBData(options);

            using (var context = new AdventureContext(options))
            {
                var mockQuesRouteRepo = new QuestionRouteRepository(context);
                var mockQueRouRepo = new UserQuestionRoutesRepository(context);
                var mockAdvRepo = new AdventureRepository(context);
                var mockRespRepo = new ResponseRepository(context);
                var mockUserRepo = new UserRepository(context);
                var jsonString = File.ReadAllText(System.AppContext.BaseDirectory + "\\sample.json");
                var jsonData = JsonConvert.DeserializeObject<AdventureGame>(jsonString);
 
                var testUsr = context.Users.FirstOrDefault()?.Id;

                var advProvider = new AdventureProvider(mockQuesRouteRepo, mockQueRouRepo, mockAdvRepo);
                var questionRoute = new QuestionRouteProvider(mockQuesRouteRepo, mockRespRepo, mockAdvRepo);
                var adventureId = await questionRoute.AddQuestionRoute(jsonData, true);
                var nextQueRoutes = await advProvider.StartGame(adventureId, testUsr);

                //After adventure begin, Choose answer as yes and pass route Id
                var chooseYesAnswerId = nextQueRoutes.nextQuestions.Where(o => o.responseText == "Yes").FirstOrDefault()?.questionRouteId;

                var nextLevelRoutes = await advProvider.MoveToNextLevel(
                    chooseYesAnswerId, testUsr);

                //Move to next level, By Choose answer as yes and pass route Id
                chooseYesAnswerId = nextLevelRoutes.nextQuestions.Where(o => o.responseText == "Yes").FirstOrDefault()?.questionRouteId;

                nextLevelRoutes = await advProvider.MoveToNextLevel(
                    chooseYesAnswerId, testUsr);

                //Move to next level, By Choose answer as yes and pass route Id
                chooseYesAnswerId = nextLevelRoutes.nextQuestions.Where(o => o.responseText == "Yes").FirstOrDefault()?.questionRouteId;

                 var ex = await Assert.ThrowsAsync<InvalidOrEmptyException>
                    (async () => await advProvider.MoveToNextLevel(chooseYesAnswerId, testUsr));

                Assert.NotNull(ex);
                Assert.Equal("Adventure ends, Wise Decisions !!!", ex.Message);

            }
        }

    }

}