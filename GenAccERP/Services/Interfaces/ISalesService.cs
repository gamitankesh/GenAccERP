using GenAccERP.Dto;

namespace GenAccERP.Services.Interfaces
{
    public interface ISalesService
    {
        Task<ServiceResult<IEnumerable<SaleDto>>> GetAllAsync();
        Task<ServiceResult<SaleDto>> GetAsync(Guid id);
        Task<ServiceResult<SaleDto>> CreateAsync(SaleRequest request);
        Task<ServiceResult<SaleDto>> UpdateAsync(Guid id, SaleRequest request);
        Task<ServiceResult<bool>> DeleteAsync(Guid id);
    }
}
