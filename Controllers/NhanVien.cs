using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhanVienController : ControllerBase
    {
        private readonly WebApplication3Context _context;

        public NhanVienController(WebApplication3Context context)
        {
            _context = context;
        }

        // 🔹 GET: api/NhanVien/get-all
        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<NhanVien>>> GetAll()
        {
            return await _context.NhanViens.ToListAsync();
        }

        // 🔹 GET: api/NhanVien/get/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<NhanVien>> GetNhanVien(int id)
        {
            var nv = await _context.NhanViens.FindAsync(id);
            if (nv == null)
                return NotFound(new { message = "Không tìm thấy nhân viên" });

            return nv;
        }

        // 🔹 POST: api/NhanVien/create
        [HttpPost("create")]
        public async Task<ActionResult<NhanVien>> Create(NhanVien nv)
        {
            _context.NhanViens.Add(nv);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNhanVien), new { id = nv.MaNV }, nv);
        }

        // 🔹 PUT: api/NhanVien/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, NhanVien nv)
        {
            if (id != nv.MaNV)
                return BadRequest(new { message = "ID không khớp" });

            _context.Entry(nv).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.NhanViens.Any(e => e.MaNV == id))
                    return NotFound(new { message = "Không tìm thấy nhân viên" });
                else
                    throw;
            }

            return Ok(new { message = "Cập nhật thành công" });
        }

        // 🔹 DELETE: api/NhanVien/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var nv = await _context.NhanViens.FindAsync(id);
            if (nv == null)
                return NotFound(new { message = "Không tìm thấy nhân viên" });

            _context.NhanViens.Remove(nv);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa thành công" });
        }
    }
}
