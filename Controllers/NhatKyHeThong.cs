using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhatKyHeThongController : ControllerBase
    {
        private readonly WebApplication3Context _context;
        public NhatKyHeThongController(WebApplication3Context context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NhatKyHeThong>>> GetAll()
            => await _context.NhatKyHeThongs.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<NhatKyHeThong>> GetById(long id)
        {
            var obj = await _context.NhatKyHeThongs.FindAsync(id);
            return obj == null ? NotFound() : obj;
        }

        [HttpPost]
        public async Task<ActionResult<NhatKyHeThong>> Create(NhatKyHeThong log)
        {
            _context.NhatKyHeThongs.Add(log);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = log.MaLog }, log);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, NhatKyHeThong log)
        {
            if (id != log.MaLog) return BadRequest();
            _context.Entry(log).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var obj = await _context.NhatKyHeThongs.FindAsync(id);
            if (obj == null) return NotFound();
            _context.NhatKyHeThongs.Remove(obj);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
