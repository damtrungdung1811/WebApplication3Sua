using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuDungLinhKienController : ControllerBase
    {
        private readonly WebApplication3Context _context;

        public SuDungLinhKienController(WebApplication3Context context)
        {
            _context = context;
        }

        // GET: api/SuDungLinhKien/get-all
        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<SuDungLinhKien>>> GetAll()
        {
            return await _context.SuDungLinhKiens
                .Include(s => s.PhieuCongViec)
                .Include(s => s.LinhKien)
                .ToListAsync();
        }

        // GET: api/SuDungLinhKien/get/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<SuDungLinhKien>> GetById(int id)
        {
            var obj = await _context.SuDungLinhKiens
                .Include(s => s.PhieuCongViec)
                .Include(s => s.LinhKien)
                .FirstOrDefaultAsync(s => s.MaSuDung == id);

            if (obj == null) return NotFound();
            return obj;
        }

        // POST: api/SuDungLinhKien/create
        [HttpPost("create")]
        public async Task<ActionResult<SuDungLinhKien>> Create([FromBody] SuDungLinhKien model)
        {
            _context.SuDungLinhKiens.Add(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = model.MaSuDung }, model);
        }

        // PUT: api/SuDungLinhKien/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SuDungLinhKien model)
        {
            if (id != model.MaSuDung) return BadRequest();

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/SuDungLinhKien/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var obj = await _context.SuDungLinhKiens.FindAsync(id);
            if (obj == null) return NotFound();

            _context.SuDungLinhKiens.Remove(obj);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ========== NGHIỆP VỤ ==========

        // GET: api/SuDungLinhKien/by-pcv/3  (lấy linh kiện theo Phiếu Công Việc)
        [HttpGet("by-pcv/{maPhieuCV}")]
        public async Task<ActionResult<IEnumerable<SuDungLinhKien>>> GetByPhieuCongViec(int maPhieuCV)
        {
            return await _context.SuDungLinhKiens
                .Include(s => s.LinhKien)
                .Where(s => s.MaPhieuCV == maPhieuCV)
                .ToListAsync();
        }
    }
}
