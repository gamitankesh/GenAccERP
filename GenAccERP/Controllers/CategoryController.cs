using GenAccERP.Dto;
using GenAccERP.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GenAccERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll() => this.ToActionResult(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> Get(Guid id) => this.ToActionResult(await _service.GetAsync(id));

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> Create([FromBody] CreateCategoryRequest request) =>
            this.ToCreatedResult(await _service.CreateAsync(request), nameof(Get), x => new { id = x.CategoryId });

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDto>> Update(Guid id, [FromBody] CreateCategoryRequest request) =>
            this.ToActionResult(await _service.UpdateAsync(id, request));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) => this.ToNoContentResult(await _service.DeleteAsync(id));
    }
}
