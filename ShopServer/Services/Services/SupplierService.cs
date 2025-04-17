using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ShopServer.Services.Infrastuce;
using ShopSharedLibrary.DBContextOperation.Context;
using ShopSharedLibrary.DBContextOperation.Models;
using ShopSharedLibrary.DTO_Operation.DTO;

namespace ShopServer.Services.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly IMapper _mapper;
        private readonly BlazorShopDbContext _dbContext;

        public SupplierService(IMapper mapper, BlazorShopDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        /// <summary>
        ///  Veri Ekle
        /// </summary>
        /// <param name="Supplie"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<SupplierDTO> CreateSupplie(SupplierDTO supplie)
        {
            //var findData = await _dbContext.Suppliers.FirstOrDefaultAsync(f => f.Id == supplie.Id);
            //if (findData == null)
            //    throw new Exception("Daha Önce Kaydınız Bulunmakta!");
            //findData = _mapper.Map<Suppliers>(supplie);
            var dbSuplier= _mapper.Map<Suppliers>(supplie);

            await _dbContext.AddAsync(dbSuplier);
            int result = await _dbContext.SaveChangesAsync();
            return _mapper.Map<SupplierDTO>(supplie);
        }

        /// <summary>
        ///  Veriyi sil. Id Yı Görei
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteSupplieById(Guid id)
        {
            var fd = await _dbContext.Suppliers.FirstOrDefaultAsync(f => f.Id == id);
            if (fd == null)
                throw new Exception("Data Bulunmadı!!!");
            _dbContext.Suppliers.Remove(fd);
            int result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        /// <summary>
        ///  Verileri Getir
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<SupplierDTO>> GetSupplie()
        {
            return await _dbContext.Suppliers.ProjectTo<SupplierDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }

        /// <summary>
        ///  Sadece Id Ye ait verileri Getir.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<SupplierDTO> GetSupplieById(Guid id)
        {
            return await _dbContext.Suppliers.Where(k => k.Id == id).ProjectTo<SupplierDTO>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }

        /// <summary>
        ///  Verileri Günvelle.
        /// </summary>
        /// <param name="Supplie"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<SupplierDTO> UpdateSupplie(SupplierDTO supplie)
        {
            var dataFind = await _dbContext.Suppliers.FirstOrDefaultAsync(f => f.Id == supplie.Id);
            if (dataFind == null)
                throw new Exception("DAta Bulunamadı!!");
            _mapper.Map(supplie, dataFind);
            int result = await _dbContext.SaveChangesAsync();
            return _mapper.Map<SupplierDTO>(dataFind);
        }
    }
}
