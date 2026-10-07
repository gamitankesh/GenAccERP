using GenAccERP.DataAccess.Models;
using GenAccERP.DataAccess.Repositories;
using GenAccERP.DataAccess.Repositories.Interfaces;
using GenAccERP.Dto;
using GenAccERP.Services.Interfaces;

namespace GenAccERP.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categories;
        private readonly IUserService _users;

        public CategoryService(ICategoryRepository categories, IUserService users)
        {
            _categories = categories;
            _users = users;
        }

        private static UserDto ToUserDto(AppUser u) => new(u.UserId, u.Username, u.Email, u.Role);

        public async Task<ServiceResult<IEnumerable<CategoryDto>>> GetAllAsync()
        {
            var list = await _categories.GetAllAsync();
            return ServiceResult<IEnumerable<CategoryDto>>.Success(
                list.Select(c => new CategoryDto(c.CategoryId, c.CategoryName, c.CreatedBy, c.CreatedDate, null)).ToList());
        }

        public async Task<ServiceResult<CategoryDto>> GetAsync(Guid id)
        {
            var c = await _categories.GetWithCreatorAsync(id);
            if (c == null) return ServiceResult<CategoryDto>.NotFound();
            return ServiceResult<CategoryDto>.Success(new CategoryDto(c.CategoryId, c.CategoryName, c.CreatedBy, c.CreatedDate,
                c.CreatedByNavigation != null ? ToUserDto(c.CreatedByNavigation) : null));
        }

        public async Task<ServiceResult<CategoryDto>> CreateAsync(CreateCategoryRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.CategoryName))
                return ServiceResult<CategoryDto>.BadRequest("CategoryName is required.");

            var createdBy = await _users.ResolveCreatorAsync(request.CreatedBy);
            var category = new ItemCategory
            {
                CategoryId = Guid.NewGuid(),
                CategoryName = request.CategoryName,
                CreatedBy = createdBy.UserId,
                CreatedDate = DateTime.UtcNow
            };
            await _categories.AddAsync(category);
            try { await _categories.SaveChangesAsync(); }
            catch (DataAccessException ex) { return ServiceResult<CategoryDto>.BadRequest(ex.Message); }

            return ServiceResult<CategoryDto>.Success(new CategoryDto(category.CategoryId, category.CategoryName, category.CreatedBy, category.CreatedDate, ToUserDto(createdBy)));
        }

        public async Task<ServiceResult<CategoryDto>> UpdateAsync(Guid id, CreateCategoryRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.CategoryName))
                return ServiceResult<CategoryDto>.BadRequest("CategoryName is required.");
            var category = await _categories.GetByIdAsync(id);
            if (category == null) return ServiceResult<CategoryDto>.NotFound();
            category.CategoryName = request.CategoryName;
            category.ModifiedBy = request.CreatedBy;
            category.ModifiedDate = DateTime.UtcNow;
            try { await _categories.SaveChangesAsync(); }
            catch (DataAccessException ex) { return ServiceResult<CategoryDto>.BadRequest(ex.Message); }
            return ServiceResult<CategoryDto>.Success(new CategoryDto(category.CategoryId, category.CategoryName, category.CreatedBy, category.CreatedDate, null));
        }

        public async Task<ServiceResult<bool>> DeleteAsync(Guid id)
        {
            var category = await _categories.GetByIdAsync(id);
            if (category == null) return ServiceResult<bool>.NotFound();
            _categories.Remove(category);
            try { await _categories.SaveChangesAsync(); }
            catch (DataAccessException) { return ServiceResult<bool>.Conflict("Category is used by items and cannot be deleted."); }
            return ServiceResult<bool>.Success(true);
        }
    }
}
