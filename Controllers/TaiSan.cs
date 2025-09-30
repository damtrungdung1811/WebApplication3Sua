using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaiSanController : ControllerBase
    {
        private readonly WebApplication3Context _context;

        public TaiSanController(WebApplication3Context context)
        {
            _context = context;
        }

        // GET: api/TaiSan/get-all
        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<TaiSan>>> GetAll()
        {
            return await _context.TaiSans
                .Include(t => t.BaoHanh)
                .Include(t => t.KhachHang)
                .ToListAsync();
        }

        // GET: api/TaiSan/get/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<TaiSan>> GetById(int id)
        {
            var ts = await _context.TaiSans
                .Include(t => t.BaoHanh)
                .Include(t => t.KhachHang)
                .FirstOrDefaultAsync(t => t.MaTaiSan == id);

            if (ts == null) return NotFound();
            return ts;
        }

        // POST: api/TaiSan/create
        [HttpPost("create")]
        public async Task<ActionResult<TaiSan>> Create([FromBody] TaiSan ts)
        {
            _context.TaiSans.Add(ts);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = ts.MaTaiSan }, ts);
        }

        // PUT: api/TaiSan/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TaiSan ts)
        {
            if (id != ts.MaTaiSan) return BadRequest();

            _context.Entry(ts).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/TaiSan/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ts = await _context.TaiSans.FindAsync(id);
            if (ts == null) return NotFound();

            _context.TaiSans.Remove(ts);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
