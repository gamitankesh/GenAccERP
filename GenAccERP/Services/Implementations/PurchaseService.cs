using GenAccERP.DataAccess.Models;
using GenAccERP.DataAccess.Repositories;
using GenAccERP.DataAccess.Repositories.Interfaces;
using GenAccERP.Dto;
using GenAccERP.Services.Interfaces;

namespace GenAccERP.Services.Implementations
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _purchases;
        private readonly IItemRepository _items;
        private readonly IUserService _users;

        public PurchaseService(IPurchaseRepository purchases, IItemRepository items, IUserService users)
        {
            _purchases = purchases;
            _items = items;
            _users = users;
        }

        private static PurchaseDto ToDto(PurchaseMaster p) => new(
            p.PurchaseId, p.PurchaseDate, p.SupplierName, p.TotalAmount, p.CreatedDate,
            p.PurchaseDetails.Select(d => new PurchaseLineDto(d.PurchaseDetailId, d.ItemId, d.Quantity, d.UnitPrice, d.LineTotal)).ToList());

        public async Task<ServiceResult<IEnumerable<PurchaseDto>>> GetAllAsync()
        {
            var list = await _purchases.GetAllAsync();
            return ServiceResult<IEnumerable<PurchaseDto>>.Success(list.Select(ToDto).ToList());
        }

        public async Task<ServiceResult<PurchaseDto>> GetAsync(Guid id)
        {
            var p = await _purchases.GetByIdAsync(id);
            return p == null ? ServiceResult<PurchaseDto>.NotFound() : ServiceResult<PurchaseDto>.Success(ToDto(p));
        }

        public async Task<ServiceResult<PurchaseDto>> CreateAsync(PurchaseRequest request)
        {
            if (request == null || request.ItemId == null || request.Quantity <= 0)
                return ServiceResult<PurchaseDto>.BadRequest("ItemId and a positive Quantity are required.");
            var item = await _items.GetByIdAsync(request.ItemId.Value);
            if (item == null) return ServiceResult<PurchaseDto>.BadRequest("Item not found.");

            var user = await _users.ResolveCreatorAsync(request.CreatedBy);
            var unitPrice = request.UnitPrice ?? item.PurchasePrice;

            var master = new PurchaseMaster
            {
                PurchaseId = Guid.NewGuid(),
                PurchaseDate = request.PurchaseDate ?? DateTime.UtcNow,
                SupplierName = request.SupplierName,
                TotalAmount = unitPrice * request.Quantity,
                CreatedBy = user.UserId,
                CreatedDate = DateTime.UtcNow
            };
            master.PurchaseDetails.Add(new PurchaseDetail
            {
                PurchaseDetailId = Guid.NewGuid(),
                PurchaseId = master.PurchaseId,
                ItemId = item.ItemId,
                Quantity = request.Quantity,
                UnitPrice = unitPrice
            });
            await _purchases.AddAsync(master);
            try { await _purchases.SaveChangesAsync(); }
            catch (DataAccessException ex) { return ServiceResult<PurchaseDto>.BadRequest(ex.Message); }

            return await GetAsync(master.PurchaseId);
        }

        public async Task<ServiceResult<PurchaseDto>> UpdateAsync(Guid id, PurchaseRequest request)
        {
            var master = await _purchases.GetForUpdateAsync(id);
            if (master == null) return ServiceResult<PurchaseDto>.NotFound();

            if (request.PurchaseDate != null) master.PurchaseDate = request.PurchaseDate.Value;
            if (request.SupplierName != null) master.SupplierName = request.SupplierName;

            var line = master.PurchaseDetails.FirstOrDefault();
            if (line != null && request.Quantity > 0)
            {
                if (request.ItemId != null && request.ItemId != line.ItemId)
                {
                    var item = await _items.GetByIdAsync(request.ItemId.Value);
                    if (item == null) return ServiceResult<PurchaseDto>.BadRequest("Item not found.");
                    line.ItemId = item.ItemId;
                    line.UnitPrice = request.UnitPrice ?? item.PurchasePrice;
                }
                else if (request.UnitPrice != null) line.UnitPrice = request.UnitPrice.Value;
                line.Quantity = request.Quantity;
            }
            master.TotalAmount = master.PurchaseDetails.Sum(d => d.Quantity * d.UnitPrice);
            master.ModifiedDate = DateTime.UtcNow;
            master.ModifiedBy = request.ModifiedBy;
            try { await _purchases.SaveChangesAsync(); }
            catch (DataAccessException ex) { return ServiceResult<PurchaseDto>.BadRequest(ex.Message); }

            return await GetAsync(id);
        }

        public async Task<ServiceResult<bool>> DeleteAsync(Guid id)
        {
            var master = await _purchases.GetForUpdateAsync(id);
            if (master == null) return ServiceResult<bool>.NotFound();
            _purchases.Remove(master);
            try { await _purchases.SaveChangesAsync(); }
            catch (DataAccessException ex) { return ServiceResult<bool>.Conflict(ex.Message); }
            return ServiceResult<bool>.Success(true);
        }
    }
}
