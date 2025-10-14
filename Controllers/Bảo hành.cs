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

        // =====================================================
        // 🔹 GET: api/BaoHanh/get-all
        // =====================================================
        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<BaoHanh>>> GetAll()
        {
            var list = await _context.BaoHanhs
                                     .Include(b => b.NhanVien)
                                     .ToListAsync();

            if (list == null || list.Count == 0)
                return NotFound(new { message = "Không có dữ liệu bảo hành nào." });

            return Ok(new
            {
                message = "Lấy danh sách bảo hành thành công!",
                data = list
            });
        }

        // =====================================================
        // 🔹 GET: api/BaoHanh/get/{id}
        // =====================================================
        [HttpGet("get/{id}")]
        public async Task<ActionResult<BaoHanh>> GetById(int id)
        {
            var bh = await _context.BaoHanhs
                                   .Include(b => b.NhanVien)
                                   .FirstOrDefaultAsync(x => x.MaBaoHanh == id);

            if (bh == null)
                return NotFound(new { message = $"Không tìm thấy bảo hành có ID = {id}" });

            return Ok(new
            {
                message = "Lấy thông tin bảo hành thành công!",
                data = bh
            });
        }

        // =====================================================
        // 🔹 POST: api/BaoHanh/create
        // =====================================================
        [HttpPost("create")]
        public async Task<ActionResult<BaoHanh>> Create([FromBody] BaoHanh bh)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.BaoHanhs.Add(bh);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Thêm mới bảo hành thành công!",
                data = bh
            });
        }

        // =====================================================
        // 🔹 PUT: api/BaoHanh/update/{id}
        // =====================================================
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BaoHanh bh)
        {
            if (id != bh.MaBaoHanh)
                return BadRequest(new { message = "ID không khớp với mã bảo hành gửi lên." });

            var existing = await _context.BaoHanhs.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = $"Không tìm thấy bảo hành có ID = {id}" });

            // ✅ Cập nhật đúng các trường có trong model
            existing.NhaCungCap = bh.NhaCungCap;
            existing.NgayBatDau = bh.NgayBatDau;
            existing.NgayKetThuc = bh.NgayKetThuc;
            existing.DieuKhoan = bh.DieuKhoan;
            existing.MaNV = bh.MaNV;

            _context.Entry(existing).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Cập nhật thông tin bảo hành thành công!",
                data = existing
            });
        }

        // =====================================================
        // 🔹 DELETE: api/BaoHanh/delete/{id}
        // =====================================================
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var bh = await _context.BaoHanhs.FindAsync(id);
            if (bh == null)
                return NotFound(new { message = $"Không tìm thấy bảo hành có ID = {id}" });

            _context.BaoHanhs.Remove(bh);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Xóa bảo hành thành công!",
                data = bh
            });
        }
    }
}
