using GenAccERP.Dto;
using GenAccERP.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GenAccERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ISalesService _service;

        public SalesController(ISalesService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleDto>>> GetAll() => this.ToActionResult(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<SaleDto>> Get(Guid id) => this.ToActionResult(await _service.GetAsync(id));

        [HttpPost]
        public async Task<ActionResult<SaleDto>> Create([FromBody] SaleRequest request) =>
            this.ToCreatedResult(await _service.CreateAsync(request), nameof(Get), x => new { id = x.SaleId });

        [HttpPut("{id}")]
        public async Task<ActionResult<SaleDto>> Update(Guid id, [FromBody] SaleRequest request) =>
            this.ToActionResult(await _service.UpdateAsync(id, request));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) => this.ToNoContentResult(await _service.DeleteAsync(id));
    }
}
