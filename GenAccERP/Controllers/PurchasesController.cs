using GenAccERP.Dto;
using GenAccERP.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GenAccERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly IPurchaseService _service;

        public PurchasesController(IPurchaseService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseDto>>> GetAll() => this.ToActionResult(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseDto>> Get(Guid id) => this.ToActionResult(await _service.GetAsync(id));

        [HttpPost]
        public async Task<ActionResult<PurchaseDto>> Create([FromBody] PurchaseRequest request) =>
            this.ToCreatedResult(await _service.CreateAsync(request), nameof(Get), x => new { id = x.PurchaseId });

        [HttpPut("{id}")]
        public async Task<ActionResult<PurchaseDto>> Update(Guid id, [FromBody] PurchaseRequest request) =>
            this.ToActionResult(await _service.UpdateAsync(id, request));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) => this.ToNoContentResult(await _service.DeleteAsync(id));
    }
}
