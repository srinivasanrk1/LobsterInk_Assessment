using Adventure.API.DataAccess.DomainModel;
using Adventure.DataAccessLayer;
using Adventure.Provider.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adventure.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserProvider _userProvider;

        public UserController(IUserProvider userProvider)
        {
           
            _userProvider = userProvider;


        }

        [HttpGet()]
        public async Task<IActionResult> GetUsersAsync()
        {
            var result = await _userProvider.GetUsers();
            return Ok(result?.ToList());
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserAsync(string userId)
        {
            var result = await _userProvider.GetUser(userId);
            return Ok(result);
        }

        [HttpPost]
        public void AddUser(User user)
        {
            _userProvider.AddUser(user);
        }
    }
}
