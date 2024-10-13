using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HPBarcodeTest.Services;
using System.Threading.Tasks;
using HPBarcodeTest.Interfaces;

namespace HPBarcodeTest.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserModel model)
        {
            var user = await _userService.UpdateUser(model.Id, model.Email, model.Password);
            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserModel model)
        {
            var result = await _userService.DeleteUser(model.Id);
            if (!result)
                return NotFound("User not found");

            return NoContent();
        }
    }

    public class UpdateUserModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class DeleteUserModel
    {
        public string Id { get; set; }
    }
}