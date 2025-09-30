using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaoHanhController : ControllerBase
    {
        private readonly WebApplication3Context _context;

        public BaoHanhController(WebApplication3Context context)
        {
            _context = context;
        }

        // GET: api/BaoHanh/get-all
        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<BaoHanh>>> GetAll()
        {
            return await _context.BaoHanhs.Include(b => b.NhanVien).ToListAsync();
        }

        // GET: api/BaoHanh/get/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<BaoHanh>> GetById(int id)
        {
            var bh = await _context.BaoHanhs.Include(b => b.NhanVien)
                                            .FirstOrDefaultAsync(x => x.MaBaoHanh == id);
            if (bh == null) return NotFound();
            return bh;
        }

        // POST: api/BaoHanh/create
        [HttpPost("create")]
        public async Task<ActionResult<BaoHanh>> Create([FromBody] BaoHanh bh)
        {
            _context.BaoHanhs.Add(bh);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = bh.MaBaoHanh }, bh);
        }

        // PUT: api/BaoHanh/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BaoHanh bh)
        {
            if (id != bh.MaBaoHanh) return BadRequest();

            _context.Entry(bh).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/BaoHanh/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var bh = await _context.BaoHanhs.FindAsync(id);
            if (bh == null) return NotFound();

            _context.BaoHanhs.Remove(bh);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
