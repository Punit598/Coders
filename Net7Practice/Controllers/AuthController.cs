using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net7Practice.Dtos.User;

namespace Net7Practice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private static IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost]
        [Route("registeruser")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto userRegisterDto)
        {
            var response = await _authRepository.Register(new Users { UserName = userRegisterDto.UserName }, userRegisterDto.Password);
            if (!response.Issuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(string UserName, string Password)
        {
            var login = await _authRepository.Login(UserName, Password);
            if (login.Issuccess == false)
                return BadRequest(login);

            return login;
        }
    }
}
