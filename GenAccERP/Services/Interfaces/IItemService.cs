using GenAccERP.Dto;

namespace GenAccERP.Services.Interfaces
{
    public interface IItemService
    {
        Task<ServiceResult<IEnumerable<ItemDto>>> GetAllAsync();
        Task<ServiceResult<ItemDto>> GetAsync(Guid id);
        Task<ServiceResult<StockDto>> GetStockAsync(Guid id);
        Task<ServiceResult<ItemDto>> CreateAsync(CreateItemRequest request);
        Task<ServiceResult<ItemDto>> UpdateAsync(Guid id, UpdateItemRequest request);
        Task<ServiceResult<bool>> DeleteAsync(Guid id);
    }
}
