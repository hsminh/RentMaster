using Microsoft.AspNetCore.Mvc;
using RentMaster.Accounts.Models;
using RentMaster.Accounts.Services;

namespace RentMaster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsumerController : ControllerBase
    {
        private readonly ConsumerService _service;

        public ConsumerController(ConsumerService service)
        {
            _service = service;
        }

        // GET: api/consumer
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var consumers = await _service.GetAllAsync();
            return Ok(consumers);
        }

        // GET: api/consumer/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var consumer = await _service.GetByIdAsync(id);
            if (consumer == null)
                return NotFound();

            return Ok(consumer);
        }

        // POST: api/consumer
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Consumer model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateAsync(model);
            if (created == null)
                return Conflict(new { message = "Gmail already exists." });

            return CreatedAtAction(nameof(GetById), new { id = created.Uid }, created);
        }

        // PUT: api/consumer/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Consumer model)
        {
            if (id != model.Uid)
                return BadRequest("Mismatched consumer ID");

            await _service.UpdateAsync(id, model);
            return NoContent();
        }

        // DELETE: api/consumer/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}