using GenAccERP.Dto;
using GenAccERP.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GenAccERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll() => this.ToActionResult(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> Get(Guid id) => this.ToActionResult(await _service.GetAsync(id));

        [HttpPost]
        public async Task<ActionResult<UserDto>> Create([FromBody] UserRequest request) =>
            this.ToCreatedResult(await _service.CreateAsync(request), nameof(Get), x => new { id = x.UserId });

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> Update(Guid id, [FromBody] UserRequest request) =>
            this.ToActionResult(await _service.UpdateAsync(id, request));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id) => this.ToNoContentResult(await _service.DeleteAsync(id));
    }
}
