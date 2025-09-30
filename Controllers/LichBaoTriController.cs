using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LichBaoTriController : ControllerBase
    {
        private readonly WebApplication3Context _context;

        public LichBaoTriController(WebApplication3Context context)
        {
            _context = context;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<LichBaoTri>>> GetAll()
        {
            return await _context.LichBaoTris
                .Include(l => l.TaiSan)
                .Include(l => l.NhanVien)
                .ToListAsync();
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<LichBaoTri>> GetById(int id)
        {
            var obj = await _context.LichBaoTris
                .Include(l => l.TaiSan)
                .Include(l => l.NhanVien)
                .FirstOrDefaultAsync(l => l.MaLich == id);

            if (obj == null) return NotFound();
            return obj;
        }

        [HttpPost("create")]
        public async Task<ActionResult<LichBaoTri>> Create([FromBody] LichBaoTri model)
        {
            _context.LichBaoTris.Add(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = model.MaLich }, model);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LichBaoTri model)
        {
            if (id != model.MaLich) return BadRequest();

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var obj = await _context.LichBaoTris.FindAsync(id);
            if (obj == null) return NotFound();

            _context.LichBaoTris.Remove(obj);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
