using GenAccERP.Dto;
using GenAccERP.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GenAccERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _service;

        public ItemsController(IItemService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAll() => this.ToActionResult(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> Get(Guid id) => this.ToActionResult(await _service.GetAsync(id));

        [HttpPost]
        public async Task<ActionResult<ItemDto>> Create([FromBody] CreateItemRequest request) =>
            this.ToCreatedResult(await _service.CreateAsync(request), nameof(Get), x => new { id = x.ItemId });

        [HttpPut("{id}")]
        public async Task<ActionResult<ItemDto>> Update(Guid id, [FromBody] UpdateItemRequest request) =>
            this.ToActionResult(await _service.UpdateAsync(id, request));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) => this.ToNoContentResult(await _service.DeleteAsync(id));

        [HttpGet("{id}/stock")]
        public async Task<ActionResult<StockDto>> GetStock(Guid id) => this.ToActionResult(await _service.GetStockAsync(id));
    }
}
