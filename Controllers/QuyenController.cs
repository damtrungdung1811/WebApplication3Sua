using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuyenController : ControllerBase
    {
        private readonly WebApplication3Context _context;

        public QuyenController(WebApplication3Context context)
        {
            _context = context;
        }

        // ================== CRUD CƠ BẢN ==================

        // GET: api/Quyen/get-all
        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<Quyen>>> GetAll()
        {
            return await _context.Quyens.ToListAsync();
        }

        // GET: api/Quyen/get/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<Quyen>> GetById(int id)
        {
            var q = await _context.Quyens.FindAsync(id);
            if (q == null) return NotFound();
            return q;
        }

        // POST: api/Quyen/create
        [HttpPost("create")]
        public async Task<ActionResult<Quyen>> Create([FromBody] Quyen model)
        {
            _context.Quyens.Add(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = model.QuyenID }, model);
        }

        // PUT: api/Quyen/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Quyen model)
        {
            if (id != model.QuyenID) return BadRequest();

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Quyen/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var q = await _context.Quyens.FindAsync(id);
            if (q == null) return NotFound();

            _context.Quyens.Remove(q);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ================== NGHIỆP VỤ ==================

        // GET: api/Quyen/get-by-group/Asset
        [HttpGet("get-by-group/{nhom}")]
        public async Task<ActionResult<IEnumerable<Quyen>>> GetByGroup(string nhom)
        {
            return await _context.Quyens
                .Where(x => x.Nhom == nhom)
                .ToListAsync();
        }

        // GET: api/Quyen/search?keyword=view
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Quyen>>> Search([FromQuery] string keyword)
        {
            return await _context.Quyens
                .Where(x => x.TenQuyen.Contains(keyword) || x.MoTa.Contains(keyword))
                .ToListAsync();
        }
    }
}
