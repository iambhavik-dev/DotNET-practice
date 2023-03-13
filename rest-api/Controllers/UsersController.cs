using Microsoft.AspNetCore.Mvc;
using rest_api.Models;
using rest_api.Repositories.Interfaces;

namespace rest_api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UsersController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<Users>> GetUsers()
        {
            var result = await userRepository.GetUsers();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Users>> AddUsers(Users user)
        {
            await userRepository.AddUsers(user);
            return Ok();
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<ActionResult<Users>> GetUsers(string userId)
        {
            var result = await userRepository.GetUserById(userId);
            return Ok(result);
        }

        [HttpPost]
        [Route("{userId}")]
        public async Task<ActionResult<Users>> UpdateUser(string userId, Users user)
        {
            await userRepository.UpdateUser(userId, user);
            return Ok();
        }

        [HttpDelete]
        [Route("{userId}")]
        public async Task<ActionResult<Users>> DeleteUser(string userId)
        {
            await userRepository.DeleteUser(userId);
            return Ok();
        }

    }
}
