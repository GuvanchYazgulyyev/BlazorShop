using ShopSharedLibrary.DTO_Operation.DTO;

namespace ShopServer.Services.Infrastuce
{
    public interface ISupplierService
    {
        public Task<SupplierDTO> GetSupplieById(Guid id);
        public Task<List<SupplierDTO>> GetSupplie();
        public Task<SupplierDTO> CreateSupplie(SupplierDTO Supplie);
        public Task<SupplierDTO> UpdateSupplie(SupplierDTO Supplie);
        public Task<bool> DeleteSupplieById(Guid id);
    }
}
