using KsiazeczkaPttk.Domain.Models;
using KsiazeczkaPTTK.API.ViewModels;
using KsiazeczkaPTTK.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KsiazeczkaPTTK.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("facebook")]
        public async Task<ActionResult> AuthorizeFacebook([FromBody] FacebookUserViewModel viewModel)
        {
            var user = await _userRepository.GetUserByEmail(viewModel.Email);
            if (user is null)
            {
                var role = await _userRepository.GetRole("Tourist");
                user = await _userRepository.AddUser(new User
                {
                    Email = viewModel.Email,
                    FirstName = viewModel.Name,
                    LastName = viewModel.Name,
                    Login = viewModel.Email,
                    Password = Guid.NewGuid().ToString(),
                    UserRole = role,
                    UserRoleName = "Tourist"
                });
            }
            return Ok(user.UserRoleName.ToLower());
        }
    }
}
