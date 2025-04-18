using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopServer.Services.Infrastuce;
using ShopSharedLibrary.DBContextOperation.Context;
using ShopSharedLibrary.DTO_Operation.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
        /// Kullanıcı giriş işlemini ve JWT token üretimini gerçekleştirir
        /// </summary>
        /// <param name="EMail">Kullanıcı e-posta adresi</param>
        /// <param name="Passwrd">Kullanıcı şifresi (şifrelenmemiş)</param>
        /// <returns>Token ve kullanıcı bilgilerini içeren DTO</returns>
        /// <exception cref="Exception">Giriş işlemi sırasında oluşan hatalar</exception>
        public async Task<UserLoginResponseDTO> Login(string EMail, string Passwrd)
        {
            try
            {
                // Şifreleme yapılacaksa (şu an yorum satırında)
                // var encryptPassword = PasswordEncrypter.Encrypt(Passwrd);

                // Veritabanında kullanıcı kontrolü - email ve şifre eşleşmesi
                var userControl = await _context.Users.FirstOrDefaultAsync(x => x.EMailAddress == EMail && x.Password == Passwrd);

                // Kullanıcı bulunamazsa hata fırlat
                if (userControl == null)
                    throw new Exception("Kullanıcı bulunamadı!");

                // Kullanıcı aktif değilse hata fırlat
                if (!userControl.IsActive)
                    throw new Exception("Kullanıcı kayıtlı fakat aktif değil!");

                // Dönüş nesnesi oluştur
                UserLoginResponseDTO userLoginResponseDTO = new UserLoginResponseDTO();

                // JWT TOKEN ÜRETİM İŞLEMLERİ //

                // 1. Güvenlik anahtarı oluştur (appsettings'den alınan key ile)
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));

                // 2. İmzalama kimlik bilgilerini oluştur (HMAC-SHA256 algoritması ile)
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                // 3. Token geçerlilik süresini ayarla (appsettings'den alınan gün sayısı kadar)
                var expiry = DateTime.UtcNow.AddDays(int.Parse(_configuration["JwtExpiryInDays"]));

                // 4. Token içine eklenecek claim'leri (talep bilgileri) tanımla
                var claims = new[]
                {
            new Claim(ClaimTypes.Email, userControl.EMailAddress), // Kullanıcı email
            new Claim(ClaimTypes.NameIdentifier, userControl.Id.ToString()), // Kullanıcı ID
            new Claim(ClaimTypes.Name, userControl.FirstName + " " + userControl.LastName ?? "") // Tam ad
        };

                // 5. JWT token nesnesini oluştur
                var token = new JwtSecurityToken(
                    issuer: _configuration["JwtIssuer"], // Token yayıncı
                    audience: _configuration["JwtAudience"], // Hedef kitle
                    claims: claims, // Tanımlanan claim'ler
                    expires: expiry, // Geçerlilik süresi
                    signingCredentials: credentials); // İmzalama bilgileri

                // 6. Oluşturulan token'ı string'e çevir
                userLoginResponseDTO.ApiToken = new JwtSecurityTokenHandler().WriteToken(token);

                // 7. Kullanıcı bilgilerini DTO'ya map'le
                userLoginResponseDTO.User = _mapper.Map<UserDTO>(userControl);

                // 8. Sonucu döndür
                return userLoginResponseDTO;
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama yapılabilir
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
