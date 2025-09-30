using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NguoiDungController : ControllerBase
    {
        private readonly WebApplication3Context _context;

        public NguoiDungController(WebApplication3Context context)
        {
            _context = context;
        }

        // GET: api/NguoiDung/get-all
        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<NguoiDung>>> GetAll()
        {
            return await _context.NguoiDungs
                .Include(u => u.VaiTro)
                .Include(u => u.NhanVien)
                .ToListAsync();
        }

        // GET: api/NguoiDung/get/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<NguoiDung>> GetById(int id)
        {
            var user = await _context.NguoiDungs
                .Include(u => u.VaiTro)
                .Include(u => u.NhanVien)
                .FirstOrDefaultAsync(u => u.MaNguoiDung == id);

            if (user == null) return NotFound();
            return user;
        }

        // POST: api/NguoiDung/create
        [HttpPost("create")]
        public async Task<ActionResult<NguoiDung>> Create([FromBody] NguoiDung model)
        {
            _context.NguoiDungs.Add(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = model.MaNguoiDung }, model);
        }

        // PUT: api/NguoiDung/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] NguoiDung model)
        {
            if (id != model.MaNguoiDung) return BadRequest();

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/NguoiDung/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.NguoiDungs.FindAsync(id);
            if (user == null) return NotFound();

            _context.NguoiDungs.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ============= NGHIỆP VỤ BỔ SUNG =============

        // GET: api/NguoiDung/by-role/1
        [HttpGet("by-role/{roleId}")]
        public async Task<ActionResult<IEnumerable<NguoiDung>>> GetByRole(int roleId)
        {
            return await _context.NguoiDungs
                .Include(u => u.VaiTro)
                .Where(u => u.VaiTroID == roleId)
                .ToListAsync();
        }

        // GET: api/NguoiDung/by-username/admin
        [HttpGet("by-username/{username}")]
        public async Task<ActionResult<NguoiDung>> GetByUsername(string username)
        {
            var user = await _context.NguoiDungs
                .Include(u => u.VaiTro)
                .Include(u => u.NhanVien)
                .FirstOrDefaultAsync(u => u.TenDangNhap == username);

            if (user == null) return NotFound();
            return user;
        }
    }
}
