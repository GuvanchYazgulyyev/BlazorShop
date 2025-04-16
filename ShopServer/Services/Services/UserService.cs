using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ShopServer.Services.Infrastuce;
using ShopSharedLibrary.DBContextOperation.Context;
using ShopSharedLibrary.DTO_Operation.DTO;

namespace ShopServer.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly BlazorShopDbContext _context;

        public UserService(IMapper mapper, BlazorShopDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }


        /// <summary>
        /// Müşteri Ekle.
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<UserDTO> CreateUser(UserDTO User)
        {
            var findData = await _context.Users.Where(f => f.Id == User.Id).FirstOrDefaultAsync();
            if (findData == null)
                throw new Exception("Daha Önce Kaydınız Yapılmış!!!!");

            findData = _mapper.Map<ShopSharedLibrary.DBContextOperation.Models.Users>(User);

            await _context.Users.AddAsync(findData);
            int result = await _context.SaveChangesAsync();
            return _mapper.Map<UserDTO>(findData);
        }

        /// <summary>
        /// Veriyi Id Ye Göre sil.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteUserById(Guid id)
        {
            var findData = await _context.Users.Where(f => f.Id == id).FirstOrDefaultAsync();
            if (findData == null)
                throw new Exception("Kullanıcı Bulunamadı!!!!");
            _context.Users.Remove(findData);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }

        /// <summary>
        ///  Kullanıcıları getir
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<UserDTO>> GetUser()
        {
            return await _context.Users.Where(f => f.IsActive)
                 .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
                 .ToListAsync();
        }

        /// <summary>
        ///  Id Ye Göre Tek Veriyi getir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<UserDTO> GetUserById(Guid id)
        {
            return await _context.Users.Where(f => f.IsActive && f.Id == id)
                .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }


        /// <summary>
        /// Veriyi Güncelle
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<UserDTO> UpdateUser(UserDTO User)
        {
            var findData = await _context.Users.Where(f => f.Id == User.Id).FirstOrDefaultAsync();
            if (findData == null)
                throw new Exception("Kayıt Bulunamadı!!!!");

            _mapper.Map(User, findData);

            int result = await _context.SaveChangesAsync();
            return _mapper.Map<UserDTO>(findData);
        }
    }
}
