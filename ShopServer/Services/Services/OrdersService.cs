using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ShopServer.Services.Infrastuce;
using ShopSharedLibrary.DBContextOperation.Context;
using ShopSharedLibrary.DBContextOperation.Models;
using ShopSharedLibrary.DTO_Operation.DTO;

namespace ShopServer.Services.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IMapper _mapper;
        private readonly BlazorShopDbContext _dbContext;

        public OrdersService(IMapper mapper, BlazorShopDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Sipariş Yarat
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<OrderDTO> CreateOrder(OrderDTO order)
        {
            var findData = await _dbContext.Orders.FirstOrDefaultAsync(f => f.Id == order.Id);
            if (findData == null)
                throw new Exception("Daha Önce Kaydınız Bulunmakta!");
            findData = _mapper.Map<Orders>(order);

            await _dbContext.AddAsync(findData);
            int result = await _dbContext.SaveChangesAsync();
            return _mapper.Map<OrderDTO>(order);
        }

        /// <summary>
        /// Silme işlemi Yap. Id Ye Göre
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> DeleteOrderById(Guid id)
        {
            var fd = await _dbContext.Orders.FirstOrDefaultAsync(f => f.Id == id);
            if (fd == null)
                throw new Exception("Data Bulunmadı!!!");
            _dbContext.Orders.Remove(fd);
            int result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        /// <summary>
        ///  Ürünleri Getir Listele.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<OrderDTO>> GetOrder()
        {
            return await _dbContext.Orders.ProjectTo<OrderDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }

        /// <summary>
        ///  Id Ye Göre veri Getir. !!!
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<OrderDTO> GetOrderById(Guid id)
        {
            return await _dbContext.Orders.Where(k => k.Id == id).ProjectTo<OrderDTO>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }

        /// <summary>
        ///  Veri Güncelleme Kısmı
        /// </summary>
        /// <param name="Order"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>

        public async Task<OrderDTO> UpdateOrder(OrderDTO Order)
        {
            var dataFind = await _dbContext.Orders.FirstOrDefaultAsync(f => f.Id == Order.Id);
            if (dataFind == null)
                throw new Exception("DAta Bulunamadı!!");
            _mapper.Map(Order, dataFind);
            int result = await _dbContext.SaveChangesAsync();
            return _mapper.Map<OrderDTO>(dataFind);
        }

        // Ürün Ekle

    }
}
