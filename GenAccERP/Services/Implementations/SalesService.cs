using GenAccERP.DataAccess.Models;
using GenAccERP.DataAccess.Repositories;
using GenAccERP.DataAccess.Repositories.Interfaces;
using GenAccERP.Dto;
using GenAccERP.Services.Interfaces;

namespace GenAccERP.Services.Implementations
{
    public class SalesService : ISalesService
    {
        private readonly ISalesRepository _sales;
        private readonly IItemRepository _items;
        private readonly IUserService _users;

        public SalesService(ISalesRepository sales, IItemRepository items, IUserService users)
        {
            _sales = sales;
            _items = items;
            _users = users;
        }

        private static SaleDto ToDto(SalesMaster p) => new(
            p.SaleId, p.SaleDate, p.CustomerName, p.TotalAmount, p.CreatedDate,
            p.SalesDetails.Select(d => new SaleLineDto(d.SalesDetailId, d.ItemId, d.Quantity, d.UnitPrice, d.LineTotal)).ToList());

        public async Task<ServiceResult<IEnumerable<SaleDto>>> GetAllAsync()
        {
            var list = await _sales.GetAllAsync();
            return ServiceResult<IEnumerable<SaleDto>>.Success(list.Select(ToDto).ToList());
        }

        public async Task<ServiceResult<SaleDto>> GetAsync(Guid id)
        {
            var p = await _sales.GetByIdAsync(id);
            return p == null ? ServiceResult<SaleDto>.NotFound() : ServiceResult<SaleDto>.Success(ToDto(p));
        }

        public async Task<ServiceResult<SaleDto>> CreateAsync(SaleRequest request)
        {
            if (request == null || request.ItemId == null || request.Quantity <= 0)
                return ServiceResult<SaleDto>.BadRequest("ItemId and a positive Quantity are required.");
            var item = await _items.GetByIdAsync(request.ItemId.Value);
            if (item == null) return ServiceResult<SaleDto>.BadRequest("Item not found.");

            var user = await _users.ResolveCreatorAsync(request.CreatedBy);
            var unitPrice = request.UnitPrice ?? item.SalesPrice;

            var master = new SalesMaster
            {
                SaleId = Guid.NewGuid(),
                SaleDate = request.SaleDate ?? DateTime.UtcNow,
                CustomerName = request.CustomerName,
                TotalAmount = unitPrice * request.Quantity,
                CreatedBy = user.UserId,
                CreatedDate = DateTime.UtcNow
            };
            master.SalesDetails.Add(new SalesDetail
            {
                SalesDetailId = Guid.NewGuid(),
                SaleId = master.SaleId,
                ItemId = item.ItemId,
                Quantity = request.Quantity,
                UnitPrice = unitPrice
            });
            await _sales.AddAsync(master);
            try { await _sales.SaveChangesAsync(); }
            catch (DataAccessException ex) { return ServiceResult<SaleDto>.BadRequest(ex.Message); }

            return await GetAsync(master.SaleId);
        }

        public async Task<ServiceResult<SaleDto>> UpdateAsync(Guid id, SaleRequest request)
        {
            var master = await _sales.GetForUpdateAsync(id);
            if (master == null) return ServiceResult<SaleDto>.NotFound();

            if (request.SaleDate != null) master.SaleDate = request.SaleDate.Value;
            if (request.CustomerName != null) master.CustomerName = request.CustomerName;

            var line = master.SalesDetails.FirstOrDefault();
            if (line != null && request.Quantity > 0)
            {
                if (request.ItemId != null && request.ItemId != line.ItemId)
                {
                    var item = await _items.GetByIdAsync(request.ItemId.Value);
                    if (item == null) return ServiceResult<SaleDto>.BadRequest("Item not found.");
                    line.ItemId = item.ItemId;
                    line.UnitPrice = request.UnitPrice ?? item.SalesPrice;
                }
                else if (request.UnitPrice != null) line.UnitPrice = request.UnitPrice.Value;
                line.Quantity = request.Quantity;
            }
            master.TotalAmount = master.SalesDetails.Sum(d => d.Quantity * d.UnitPrice);
            master.ModifiedDate = DateTime.UtcNow;
            master.ModifiedBy = request.ModifiedBy;
            try { await _sales.SaveChangesAsync(); }
            catch (DataAccessException ex) { return ServiceResult<SaleDto>.BadRequest(ex.Message); }

            return await GetAsync(id);
        }

        public async Task<ServiceResult<bool>> DeleteAsync(Guid id)
        {
            var master = await _sales.GetForUpdateAsync(id);
            if (master == null) return ServiceResult<bool>.NotFound();
            _sales.Remove(master);
            try { await _sales.SaveChangesAsync(); }
            catch (DataAccessException ex) { return ServiceResult<bool>.Conflict(ex.Message); }
            return ServiceResult<bool>.Success(true);
        }
    }
}
