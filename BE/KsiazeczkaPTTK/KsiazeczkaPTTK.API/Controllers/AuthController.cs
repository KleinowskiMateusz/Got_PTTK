using KsiazeczkaPttk.Domain.Models;
using KsiazeczkaPTTK.API.ViewModels;
using KsiazeczkaPTTK.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace KsiazeczkaPTTK.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private string AdminToken;
        private string AppId;
        private string AppName;

        private readonly IUserRepository _userRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthController(IUserRepository userRepository, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _httpClientFactory = httpClientFactory;
            AdminToken = configuration["FbAdminToken"];
            AppId = configuration["FbAppId"];
            AppName = configuration["FbAppName"];
        }

        [HttpPost("facebook")]
        public async Task<ActionResult> AuthorizeFacebook([FromBody] FacebookUserViewModel viewModel)
        {
            try
            {
                var isValid = await VeriyToken(viewModel.AccessToken, viewModel.UserID);
                if (!isValid)
                {
                    return Ok("unauthorized");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

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

        private async Task<bool> VeriyToken(string token, string userId)
        {
            var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            $"https://graph.facebook.com/debug_token?input_token={AdminToken}&access_token={token}")
            {};

            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var contentString = await httpResponseMessage.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<FacebookVerificationResponse>(contentString);

                if (result?.data != null
                    && result.data.app_id == AppId
                    && result.data.application == AppName
                    && result.data.is_valid
                    && result.data.user_id == userId)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
