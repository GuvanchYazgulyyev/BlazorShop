using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopServer.Helpers.HashOperation.PassEncrypter;
using ShopServer.Services.Infrastuce;
using ShopSharedLibrary.DBContextOperation.Context;
using ShopSharedLibrary.DTO_Operation.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ShopServer.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly BlazorShopDbContext _context;
        private readonly IConfiguration _configuration;

        public UserService(IMapper mapper, BlazorShopDbContext context, IConfiguration configuration)
        {
            _mapper = mapper;
            _context = context;
            _configuration = configuration;
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
        ///  Login And JWT Operation
        /// </summary>
        /// <param name="EMail"></param>
        /// <param name="Passwrd"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<UserLoginResponseDTO> Login(string EMail, string Passwrd)
        {
            try
            {
                // Şifreyi şifrele
                // var encryptPassword = PasswordEncrypter.Encrypt(Passwrd);

                // Kullanıcı doğrulama
                var userControl = await _context.Users.FirstOrDefaultAsync(x => x.EMailAddress == EMail && x.Password == Passwrd);

                if (userControl == null)
                    throw new Exception("Kullanıcı bulunamadı!");

                if (!userControl.IsActive)
                    throw new Exception("Kullanıcı kayıtlı fakat aktif değil!");

                UserLoginResponseDTO userLoginResponseDTO = new UserLoginResponseDTO();

                // Token üretimi
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expiry = DateTime.UtcNow.AddDays(int.Parse(_configuration["JwtExpiryInDays"]));

                var claims = new[]
                {
            new Claim(ClaimTypes.Email, userControl.EMailAddress),
            new Claim(ClaimTypes.NameIdentifier, userControl.Id.ToString()), // ekstra claim önerisi
            new Claim(ClaimTypes.Name, userControl.FirstName + " " + userControl.LastName ?? "")
                };

                var token = new JwtSecurityToken(
                    issuer: _configuration["JwtIssuer"],
                    audience: _configuration["JwtAudience"],
                    claims: claims,
                    expires: expiry,
                    signingCredentials: credentials);
                userLoginResponseDTO.ApiToken = new JwtSecurityTokenHandler().WriteToken(token);
                userLoginResponseDTO.User = _mapper.Map<UserDTO>(userControl);
                return userLoginResponseDTO;


            }
            catch (Exception ex)
            {
                // Loglanabilir
                throw new Exception("Login sırasında hata oluştu: " + ex.Message);
            }
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
