using ShopSharedLibrary.DTO_Operation.DTO;

namespace ShopServer.Services.Infrastuce
{
    public interface IUserService
    {
        public Task<UserDTO> GetUserById(Guid id);
        public Task<List<UserDTO>> GetUser();
        public Task<UserDTO> CreateUser(UserDTO User);
        public Task<UserDTO> UpdateUser(UserDTO User);
        public Task<bool> DeleteUserById(Guid id);
    }
}
