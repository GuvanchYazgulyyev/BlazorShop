using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopServer.Services.Infrastuce;
using ShopSharedLibrary.DataObject.ResponseModels;
using ShopSharedLibrary.DTO_Operation.DTO;
using System.Threading.Tasks;

namespace ShopServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        // Login Operation
        [HttpPost("Login")]
        public async Task<ServiceResponse<UserLoginResponseDTO>> Login(UserLoginRequestDTO userLogin)
        {
            return new ServiceResponse<UserLoginResponseDTO>()
            {
                Value = await _userService.Login(userLogin.Email, userLogin.Password)
            };
        }

        /// <summary>
        ///  Kullancıları Getir.
        /// </summary>
        /// <returns></returns>

        [HttpGet("Users")]
        public async Task<ServiceResponse<List<UserDTO>>> GetUser()
        {
            return new ServiceResponse<List<UserDTO>>()
            {
                Value = await _userService.GetUser()
            };
        }
        ///
        // Create User
        [HttpPost("Create")]
        public async Task<ServiceResponse<UserDTO>> CreateUser([FromBody] UserDTO user)
        {
            return new ServiceResponse<UserDTO>()
            {
                Value = await _userService.CreateUser(user)
            };
        }
        ///User Update
        ///
        [HttpPut("Update")]
        public async Task<ServiceResponse<UserDTO>> UpdateUser([FromBody] UserDTO user)
        {
            return new ServiceResponse<UserDTO>()
            {
                Value = await _userService.UpdateUser(user)
            };
        }

        /// Id Ye Göre Getir 
        [HttpPost("UserById/{Id}")]
        public async Task<ServiceResponse<UserDTO>> GetUserById(Guid Id)
        {
            return new ServiceResponse<UserDTO>()
            {
                Value = await _userService.GetUserById(Id)
            };
        }
    }
}
