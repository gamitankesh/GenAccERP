using GenAccERP.DataAccess.Models;
using GenAccERP.DataAccess.Repositories;
using GenAccERP.DataAccess.Repositories.Interfaces;
using GenAccERP.Dto;
using GenAccERP.Services.Interfaces;

namespace GenAccERP.Services.Implementations
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _items;
        private readonly IUserService _users;

        public ItemService(IItemRepository items, IUserService users)
        {
            _items = items;
            _users = users;
        }

        private static ItemDto ToDto(ItemMaster i) => new(
            i.ItemId, i.CategoryId, i.ItemNo, i.ItemName, i.ItemDescription, i.PurchasePrice, i.SalesPrice, i.CurrentStock,
            i.Category?.CategoryName,
            i.CreatedByNavigation != null ? new UserDto(i.CreatedByNavigation.UserId, i.CreatedByNavigation.Username, i.CreatedByNavigation.Email, i.CreatedByNavigation.Role) : null);

        public async Task<ServiceResult<IEnumerable<ItemDto>>> GetAllAsync()
        {
            var items = await _items.GetAllWithDetailsAsync();
            return ServiceResult<IEnumerable<ItemDto>>.Success(items.Select(ToDto).ToList());
        }

        public async Task<ServiceResult<ItemDto>> GetAsync(Guid id)
        {
            var item = await _items.GetWithDetailsAsync(id);
            return item == null ? ServiceResult<ItemDto>.NotFound() : ServiceResult<ItemDto>.Success(ToDto(item));
        }

        public async Task<ServiceResult<StockDto>> GetStockAsync(Guid id)
        {
            if (!await _items.ExistsAsync(id)) return ServiceResult<StockDto>.NotFound();
            var purchased = await _items.GetPurchasedQuantityAsync(id);
            var sold = await _items.GetSoldQuantityAsync(id);
            return ServiceResult<StockDto>.Success(new StockDto(id, purchased, sold, purchased - sold));
        }

        public async Task<ServiceResult<ItemDto>> CreateAsync(CreateItemRequest request)
        {
            if (request == null) return ServiceResult<ItemDto>.BadRequest("Request body is required.");
            if (request.CategoryId == null) return ServiceResult<ItemDto>.BadRequest("Category not found. Provide a valid CategoryId.");

            var category = await _items.GetCategoryAsync(request.CategoryId.Value);
            if (category == null) return ServiceResult<ItemDto>.BadRequest("Category not found. Provide a valid CategoryId.");

            var createdBy = await _users.ResolveCreatorAsync(request.CreatedBy);
            var item = new ItemMaster
            {
                ItemId = Guid.NewGuid(),
                ItemNo = request.ItemNo ?? Guid.NewGuid().ToString(),
                ItemName = request.ItemName ?? string.Empty,
                ItemDescription = request.ItemDescription,
                PurchasePrice = request.PurchasePrice,
                SalesPrice = request.SalesPrice,
                CurrentStock = request.CurrentStock,
                CategoryId = category.CategoryId,
                CreatedBy = createdBy.UserId,
                CreatedDate = DateTime.UtcNow
            };
            await _items.AddAsync(item);
            try { await _items.SaveChangesAsync(); }
            catch (DataAccessException ex) { return ServiceResult<ItemDto>.BadRequest(ex.Message); }

            return await GetAsync(item.ItemId);
        }

        public async Task<ServiceResult<ItemDto>> UpdateAsync(Guid id, UpdateItemRequest request)
        {
            var item = await _items.GetByIdAsync(id);
            if (item == null) return ServiceResult<ItemDto>.NotFound();

            if (request.CategoryId != null && request.CategoryId != Guid.Empty)
            {
                var category = await _items.GetCategoryAsync(request.CategoryId.Value);
                if (category == null) return ServiceResult<ItemDto>.BadRequest("Category not found. Provide a valid CategoryId.");
                item.CategoryId = category.CategoryId;
            }
            if (request.ItemNo != null) item.ItemNo = request.ItemNo;
            if (request.ItemName != null) item.ItemName = request.ItemName;
            if (request.ItemDescription != null) item.ItemDescription = request.ItemDescription;
            if (request.PurchasePrice != null) item.PurchasePrice = request.PurchasePrice.Value;
            if (request.SalesPrice != null) item.SalesPrice = request.SalesPrice.Value;
            item.ModifiedBy = request.ModifiedBy;
            item.ModifiedDate = DateTime.UtcNow;
            try { await _items.SaveChangesAsync(); }
            catch (DataAccessException ex) { return ServiceResult<ItemDto>.BadRequest(ex.Message); }

            return await GetAsync(id);
        }

        public async Task<ServiceResult<bool>> DeleteAsync(Guid id)
        {
            var item = await _items.GetByIdAsync(id);
            if (item == null) return ServiceResult<bool>.NotFound();
            _items.Remove(item);
            try { await _items.SaveChangesAsync(); }
            catch (DataAccessException) { return ServiceResult<bool>.Conflict("Item is used in purchases or sales and cannot be deleted."); }
            return ServiceResult<bool>.Success(true);
        }
    }
}
