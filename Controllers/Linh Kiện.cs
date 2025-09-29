using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinhKienController : ControllerBase
    {
        private readonly WebApplication3Context _context;

        public LinhKienController(WebApplication3Context context)
        {
            _context = context;
        }

        // GET: api/LinhKien/get-all
        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<LinhKien>>> GetAll()
        {
            return await _context.LinhKiens.ToListAsync();
        }

        // GET: api/LinhKien/get/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<LinhKien>> GetById(int id)
        {
            var lk = await _context.LinhKiens.FindAsync(id);
            if (lk == null) return NotFound();
            return lk;
        }

        // POST: api/LinhKien/create
        [HttpPost("create")]
        public async Task<ActionResult<LinhKien>> Create([FromBody] LinhKien lk)
        {
            _context.LinhKiens.Add(lk);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = lk.MaLinhKien }, lk);
        }

        // PUT: api/LinhKien/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LinhKien lk)
        {
            if (id != lk.MaLinhKien) return BadRequest();

            _context.Entry(lk).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/LinhKien/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var lk = await _context.LinhKiens.FindAsync(id);
            if (lk == null) return NotFound();

            _context.LinhKiens.Remove(lk);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
