using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhieuKhoChiTietController : ControllerBase
    {
        private readonly WebApplication3Context _context;

        public PhieuKhoChiTietController(WebApplication3Context context)
        {
            _context = context;
        }

        // GET: api/PhieuKhoChiTiet/get-all
        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<PhieuKho_ChiTiet>>> GetAll()
        {
            return await _context.PhieuKho_ChiTiets
                .Include(c => c.PhieuKho)
                .Include(c => c.LinhKien)
                .ToListAsync();
        }

        // GET: api/PhieuKhoChiTiet/get/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<PhieuKho_ChiTiet>> GetById(int id)
        {
            var ct = await _context.PhieuKho_ChiTiets
                .Include(c => c.PhieuKho)
                .Include(c => c.LinhKien)
                .FirstOrDefaultAsync(c => c.MaCT == id);

            if (ct == null) return NotFound();
            return ct;
        }

        // POST: api/PhieuKhoChiTiet/create
        [HttpPost("create")]
        public async Task<ActionResult<PhieuKho_ChiTiet>> Create([FromBody] PhieuKho_ChiTiet model)
        {
            _context.PhieuKho_ChiTiets.Add(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = model.MaCT }, model);
        }

        // PUT: api/PhieuKhoChiTiet/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PhieuKho_ChiTiet model)
        {
            if (id != model.MaCT) return BadRequest();

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/PhieuKhoChiTiet/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ct = await _context.PhieuKho_ChiTiets.FindAsync(id);
            if (ct == null) return NotFound();

            _context.PhieuKho_ChiTiets.Remove(ct);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // NGHIỆP VỤ: Lấy chi tiết theo phiếu kho
        // GET: api/PhieuKhoChiTiet/by-phieu/1
        [HttpGet("by-phieu/{phieuId}")]
        public async Task<ActionResult<IEnumerable<PhieuKho_ChiTiet>>> GetByPhieu(int phieuId)
        {
            return await _context.PhieuKho_ChiTiets
                .Include(c => c.LinhKien)
                .Where(c => c.MaPhieuKho == phieuId)
                .ToListAsync();
        }
    }
}
