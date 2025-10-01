using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TepDinhKemController : ControllerBase
    {
        private readonly WebApplication3Context _context;
        public TepDinhKemController(WebApplication3Context context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TepDinhKem>>> GetAll()
            => await _context.TepDinhKems.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<TepDinhKem>> GetById(int id)
        {
            var obj = await _context.TepDinhKems.FindAsync(id);
            return obj == null ? NotFound() : obj;
        }

        [HttpPost]
        public async Task<ActionResult<TepDinhKem>> Create(TepDinhKem file)
        {
            _context.TepDinhKems.Add(file);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = file.MaTep }, file);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TepDinhKem file)
        {
            if (id != file.MaTep) return BadRequest();
            _context.Entry(file).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var obj = await _context.TepDinhKems.FindAsync(id);
            if (obj == null) return NotFound();
            _context.TepDinhKems.Remove(obj);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
