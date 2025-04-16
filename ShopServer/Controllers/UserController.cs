using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopServer.Services.Infrastuce;
using ShopSharedLibrary.DataObject.ResponseModels;
using ShopSharedLibrary.DTO_Operation.DTO;

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

        /// <summary>
        ///  Kullancıları Getir.
        /// </summary>
        /// <returns></returns>

        [HttpGet(Name = "Users")]
        public async Task<ServiceResponse<List<UserDTO>>> GetUser()
        {
            return new ServiceResponse<List<UserDTO>>()
            {
                Value = await _userService.GetUser()
            };
        }
    }
}
