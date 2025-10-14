using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhachHangController : ControllerBase
    {
        private readonly WebApplication3Context _context;

        public KhachHangController(WebApplication3Context context)
        {
            _context = context;
        }

        // ===================== GET ALL =====================
        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<KhachHang>>> GetAll()
        {
            var list = await _context.KhachHangs.ToListAsync();
            return Ok(new
            {
                message = $"Tổng số {list.Count} khách hàng.",
                data = list
            });
        }

        // ===================== GET BY ID =====================
        [HttpGet("get/{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var kh = await _context.KhachHangs.FindAsync(id);
            if (kh == null)
                return NotFound(new { message = $"Không tìm thấy khách hàng có ID = {id}" });

            return Ok(new
            {
                message = "Lấy thông tin khách hàng thành công!",
                data = kh
            });
        }

        // ===================== CREATE =====================
        [HttpPost("create")]
        public async Task<ActionResult> Create([FromBody] KhachHang kh)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.KhachHangs.Add(kh);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = kh.MaKH }, new
            {
                message = "Thêm khách hàng thành công!",
                data = kh
            });
        }

        // ===================== UPDATE =====================
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] KhachHang kh)
        {
            if (id != kh.MaKH)
                return BadRequest(new { message = "ID không khớp với dữ liệu gửi lên!" });

            var existing = await _context.KhachHangs.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = $"Không tìm thấy khách hàng có ID = {id}" });

            // Cập nhật từng trường
            existing.TenKH = kh.TenKH;
            existing.DienThoai = kh.DienThoai;
            existing.Email = kh.Email;
            existing.DiaChi = kh.DiaChi;

            _context.Entry(existing).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Cập nhật thông tin khách hàng thành công!",
                data = existing
            });
        }

        // ===================== DELETE =====================
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var kh = await _context.KhachHangs.FindAsync(id);
            if (kh == null)
                return NotFound(new { message = $"Không tìm thấy khách hàng có ID = {id}" });

            _context.KhachHangs.Remove(kh);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa khách hàng thành công!" });
        }
    }
}
