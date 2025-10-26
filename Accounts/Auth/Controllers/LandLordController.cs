using Microsoft.AspNetCore.Mvc;
using RentMaster.Accounts.Models;
using RentMaster.Accounts.Services;

namespace RentMaster.Controllers
{
    [ApiController]
    [Route("[controller]/api")]
    public class LandLordController : ControllerBase
    {
        private readonly LandLordService _service;

        public LandLordController(LandLordService service)
        {
            _service = service;
        }

        // GET: api/landlord
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var consumers = await _service.GetAllAsync();
            return Ok(consumers);
        }

        // GET: api/landlord/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var consumer = await _service.GetByIdAsync(id);
            if (consumer == null)
                return NotFound();

            return Ok(consumer);
        }

        // POST: api/landlord
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LandLord model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateAsync(model);
            if (created == null)
                return Conflict(new { message = "Gmail already exists." });

            return CreatedAtAction(nameof(GetById), new { id = created.Uid }, created);
        }

        // PUT: api/landlord/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] LandLord model)
        {
            if (id != model.Uid)
                return BadRequest("Mismatched consumer ID");

            await _service.UpdateAsync(id, model);
            return NoContent();
        }

        // DELETE: api/landlord/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}