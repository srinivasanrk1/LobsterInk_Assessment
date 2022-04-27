using Adventure.API.Provider.Contracts;
using Adventure.API.System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Adventure.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdventureController : ControllerBase
    {
        private readonly IQuestionRouteProvider _questionRouteProvider;
        private readonly IAdventureProvider _adventureProvider;

        public AdventureController(IQuestionRouteProvider questionRouteProvider, IAdventureProvider adventureProvider)
        {
            _questionRouteProvider = questionRouteProvider;
            _adventureProvider = adventureProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetListOfAdventure()
        {
            try
            {
                var response = await _adventureProvider.GetAdventuresList();
                return Ok(response);
            }
            catch (InvalidOrEmptyException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (global::System.Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateAdventure([FromBody] AdventureGame adventureGame, [FromQuery] bool existsOverWrite = false)
        {
            try
            {
                if (adventureGame?.adventureName == null)
                    return BadRequest("Adventure data invalid");
                if (adventureGame?.node?.question == null)
                    return BadRequest("Adventure data invalid or there should be atleast one node");

                var response = await _questionRouteProvider.AddQuestionRoute(adventureGame, existsOverWrite);
                return Ok(response);
            }
            catch (InvalidOrEmptyException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (global::System.Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("StartGame")]
        public async Task<IActionResult> StartGame([FromQuery] string adventureId, [FromHeader] string userId)
        {
            try
            {
                var response = await _adventureProvider.StartGame(adventureId, userId);
                return Ok(response);
            }
            catch (InvalidOrEmptyException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (global::System.Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Route("MoveToNextLevel")]
        public async Task<IActionResult> MoveToNextLevel([FromQuery] string questionRouteId, [FromHeader] string userId)
        {
            try
            {
                var response = await _adventureProvider.MoveToNextLevel(questionRouteId, userId);
                return Ok(response);
            }
            catch (InvalidOrEmptyException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (global::System.Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [Route("UserDecisions")]
        public async Task<IActionResult> UserDecisions( [FromHeader] string userId, [FromQuery] string adventureID)
        {
            try
            {
                var response = await _adventureProvider.UserChoosenGamePath(userId, adventureID);
                return Ok(response);
            }
            catch (InvalidOrEmptyException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (global::System.Exception)
            {

                throw;
            }
        }
    }
}
