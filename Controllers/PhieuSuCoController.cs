using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhieuSuCoController : ControllerBase
    {
        private readonly WebApplication3Context _context;

        public PhieuSuCoController(WebApplication3Context context)
        {
            _context = context;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<PhieuSuCo>>> GetAll()
        {
            return await _context.PhieuSuCos
                .Include(p => p.TaiSan)
                .Include(p => p.KhachHang)
                .Include(p => p.NhanVien)
                .Include(p => p.PhieuCongViec)
                .ToListAsync();
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<PhieuSuCo>> GetById(int id)
        {
            var obj = await _context.PhieuSuCos
                .Include(p => p.TaiSan)
                .Include(p => p.KhachHang)
                .Include(p => p.NhanVien)
                .Include(p => p.PhieuCongViec)
                .FirstOrDefaultAsync(p => p.MaSuCo == id);

            if (obj == null) return NotFound();
            return obj;
        }

        [HttpPost("create")]
        public async Task<ActionResult<PhieuSuCo>> Create([FromBody] PhieuSuCo model)
        {
            _context.PhieuSuCos.Add(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = model.MaSuCo }, model);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PhieuSuCo model)
        {
            if (id != model.MaSuCo) return BadRequest();

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var obj = await _context.PhieuSuCos.FindAsync(id);
            if (obj == null) return NotFound();

            _context.PhieuSuCos.Remove(obj);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
