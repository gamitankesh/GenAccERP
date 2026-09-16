using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GenAccERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        // Simple test endpoint returning sample items without any DB dependency
        // GET: api/items
        [HttpGet]
        public ActionResult<IEnumerable<object>> Get()
        {
            var items = new[]
            {
                new { Id = 1, ItemNo = "ITM-001", Name = "Sample Item A", Price = 10.00M, Stock = 100 },
                new { Id = 2, ItemNo = "ITM-002", Name = "Sample Item B", Price = 25.50M, Stock = 50 }
            };

            return Ok(items);
        }
    }
}
