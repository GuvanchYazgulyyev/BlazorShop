using ShopSharedLibrary.DTO_Operation.DTO;

namespace ShopServer.Services.Infrastuce
{
    public interface IOrdersService
    {
        public Task<OrderDTO> GetOrderById(Guid id);
        public Task<List<OrderDTO>> GetOrder();
        public Task<OrderDTO> CreateOrder(OrderDTO Order);
        public Task<OrderDTO> UpdateOrder(OrderDTO Order);
        public Task<bool> DeleteOrderById(Guid id);
    }
}
