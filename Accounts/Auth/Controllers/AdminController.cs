using Microsoft.AspNetCore.Mvc;
using RentMaster.Accounts.Models;
using RentMaster.Accounts.Services;

namespace RentMaster.Controllers
{
    [ApiController]
    [Route("[controller]/api")]
    public class AdminController : ControllerBase
    {
        private readonly AdminService _service;

        public AdminController(AdminService service)
        {
            _service = service;
        }

        // GET: api/admin
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var admins = await _service.GetAllAsync();
            return Ok(admins);
        }

        // GET: api/admin/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var admin = await _service.GetByIdAsync(id);
            if (admin == null)
                return NotFound();

            return Ok(admin);
        }

        // POST: api/admin
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Admin model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateAsync(model);
            if (created == null)
                return Conflict(new { message = "Gmail already exists." });

            return CreatedAtAction(nameof(GetById), new { id = created.Uid }, created);
        }

        // PUT: api/admin/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Admin model)
        {
            if (id != model.Uid)
                return BadRequest("Mismatched admin ID");

            await _service.UpdateAsync(id, model);
            return NoContent();
        }

        // DELETE: api/admin/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}