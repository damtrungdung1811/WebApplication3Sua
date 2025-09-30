using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhieuKhoController : ControllerBase
    {
        private readonly WebApplication3Context _context;

        public PhieuKhoController(WebApplication3Context context)
        {
            _context = context;
        }

        // GET: api/PhieuKho/get-all
        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<PhieuKho>>> GetAll()
        {
            return await _context.PhieuKhos.ToListAsync();
        }

        // GET: api/PhieuKho/get/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<PhieuKho>> GetById(int id)
        {
            var phieu = await _context.PhieuKhos.FindAsync(id);
            if (phieu == null) return NotFound();
            return phieu;
        }

        // POST: api/PhieuKho/create
        [HttpPost("create")]
        public async Task<ActionResult<PhieuKho>> Create([FromBody] PhieuKho model)
        {
            _context.PhieuKhos.Add(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = model.MaPhieuKho }, model);
        }

        // PUT: api/PhieuKho/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PhieuKho model)
        {
            if (id != model.MaPhieuKho) return BadRequest();

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/PhieuKho/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var phieu = await _context.PhieuKhos.FindAsync(id);
            if (phieu == null) return NotFound();

            _context.PhieuKhos.Remove(phieu);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ================= NGHIỆP VỤ BỔ SUNG =================
        // GET: api/PhieuKho/by-type/Nhap
        [HttpGet("by-type/{loai}")]
        public async Task<ActionResult<IEnumerable<PhieuKho>>> GetByType(string loai)
        {
            return await _context.PhieuKhos
                .Where(x => x.Loai == loai)
                .ToListAsync();
        }
    }
}
