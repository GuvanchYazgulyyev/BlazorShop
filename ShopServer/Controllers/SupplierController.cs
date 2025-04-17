using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopServer.Services.Infrastuce;
using ShopServer.Services.Services;
using ShopSharedLibrary.DataObject.ResponseModels;
using ShopSharedLibrary.DTO_Operation.DTO;

namespace ShopServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        /// <summary>
        ///  Kullancıları Getir.
        /// </summary>
        /// <returns></returns>

        [HttpGet("Supplies")]
        public async Task<ServiceResponse<List<SupplierDTO>>> GetSupplie()
        {
            return new ServiceResponse<List<SupplierDTO>>()
            {
                Value = await _supplierService.GetSupplie()
            };
        }
        ///
        // Create Supplie
        [HttpPost("Create")]
        public async Task<ServiceResponse<SupplierDTO>> CreateSupplie([FromBody] SupplierDTO Supplie)
        {
            return new ServiceResponse<SupplierDTO>()
            {
                Value = await _supplierService.CreateSupplie(Supplie)
            };
        }
        ///Supplie Update
        ///
        [HttpPut("Update")]
        public async Task<ServiceResponse<SupplierDTO>> UpdateSupplie([FromBody] SupplierDTO Supplie)
        {
            return new ServiceResponse<SupplierDTO>()
            {
                Value = await _supplierService.UpdateSupplie(Supplie)
            };
        }

        /// Id Ye Göre Getir 
        [HttpPost("SupplieById/{Id}")]
        public async Task<ServiceResponse<SupplierDTO>> GetSupplieById(Guid Id)
        {
            return new ServiceResponse<SupplierDTO>()
            {
                Value = await _supplierService.GetSupplieById(Id)
            };
        }
    }
}
