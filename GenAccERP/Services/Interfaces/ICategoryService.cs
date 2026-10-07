using GenAccERP.Dto;

namespace GenAccERP.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<ServiceResult<IEnumerable<CategoryDto>>> GetAllAsync();
        Task<ServiceResult<CategoryDto>> GetAsync(Guid id);
        Task<ServiceResult<CategoryDto>> CreateAsync(CreateCategoryRequest request);
        Task<ServiceResult<CategoryDto>> UpdateAsync(Guid id, CreateCategoryRequest request);
        Task<ServiceResult<bool>> DeleteAsync(Guid id);
    }
}
