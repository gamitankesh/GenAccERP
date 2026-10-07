using GenAccERP.Dto;

namespace GenAccERP.Services.Interfaces
{
    public interface IPurchaseService
    {
        Task<ServiceResult<IEnumerable<PurchaseDto>>> GetAllAsync();
        Task<ServiceResult<PurchaseDto>> GetAsync(Guid id);
        Task<ServiceResult<PurchaseDto>> CreateAsync(PurchaseRequest request);
        Task<ServiceResult<PurchaseDto>> UpdateAsync(Guid id, PurchaseRequest request);
        Task<ServiceResult<bool>> DeleteAsync(Guid id);
    }
}
