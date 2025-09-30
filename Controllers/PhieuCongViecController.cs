using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhieuCongViecController : ControllerBase
    {
        private readonly WebApplication3Context _context;

        public PhieuCongViecController(WebApplication3Context context)
        {
            _context = context;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<PhieuCongViec>>> GetAll()
        {
            return await _context.PhieuCongViecs
                .Include(p => p.TaiSan)
                .Include(p => p.LichBaoTri)
                .Include(p => p.NhanVien)
                .ToListAsync();
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<PhieuCongViec>> GetById(int id)
        {
            var obj = await _context.PhieuCongViecs
                .Include(p => p.TaiSan)
                .Include(p => p.LichBaoTri)
                .Include(p => p.NhanVien)
                .FirstOrDefaultAsync(p => p.MaPhieuCV == id);

            if (obj == null) return NotFound();
            return obj;
        }

        [HttpPost("create")]
        public async Task<ActionResult<PhieuCongViec>> Create([FromBody] PhieuCongViec model)
        {
            _context.PhieuCongViecs.Add(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = model.MaPhieuCV }, model);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PhieuCongViec model)
        {
            if (id != model.MaPhieuCV) return BadRequest();

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var obj = await _context.PhieuCongViecs.FindAsync(id);
            if (obj == null) return NotFound();

            _context.PhieuCongViecs.Remove(obj);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
