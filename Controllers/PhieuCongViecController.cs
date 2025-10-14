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

        // ===================== 🔹 GET ALL =====================
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _context.PhieuCongViecs
                    .Include(p => p.TaiSan)
                    .Include(p => p.LichBaoTri)
                    .Include(p => p.NhanVien)
                    .OrderByDescending(p => p.NgayTao)
                    .ToListAsync();

                return Ok(new
                {
                    message = $"Đã tải {list.Count} phiếu công việc thành công!",
                    data = list
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tải dữ liệu phiếu công việc!", error = ex.Message });
            }
        }

        // ===================== 🔹 GET BY ID =====================
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var pcv = await _context.PhieuCongViecs
                    .Include(p => p.TaiSan)
                    .Include(p => p.LichBaoTri)
                    .Include(p => p.NhanVien)
                    .FirstOrDefaultAsync(p => p.MaPhieuCV == id);

                if (pcv == null)
                    return NotFound(new { message = $"Không tìm thấy phiếu công việc có mã {id}!" });

                return Ok(new { message = "Tìm thấy phiếu công việc.", data = pcv });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi truy vấn dữ liệu!", error = ex.Message });
            }
        }

        // ===================== 🔹 CREATE =====================
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] PhieuCongViec model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Dữ liệu không hợp lệ!", error = ModelState });

            try
            {
                _context.PhieuCongViecs.Add(model);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Thêm phiếu công việc thành công!",
                    data = model
                });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi khi thêm phiếu công việc (có thể do ràng buộc khóa ngoại)!",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        // ===================== 🔹 UPDATE =====================
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PhieuCongViec model)
        {
            if (id != model.MaPhieuCV)
                return BadRequest(new { message = "Mã phiếu không khớp với dữ liệu gửi lên!" });

            try
            {
                var existing = await _context.PhieuCongViecs.FindAsync(id);
                if (existing == null)
                    return NotFound(new { message = "Không tìm thấy phiếu công việc cần cập nhật!" });

                _context.Entry(existing).CurrentValues.SetValues(model);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Cập nhật phiếu công việc thành công!", data = model });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi khi cập nhật phiếu công việc!",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        // ===================== 🔹 DELETE =====================
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var obj = await _context.PhieuCongViecs.FindAsync(id);
                if (obj == null)
                    return NotFound(new { message = "Không tìm thấy phiếu công việc cần xóa!" });

                _context.PhieuCongViecs.Remove(obj);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Xóa phiếu công việc thành công!" });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new
                {
                    message = "Không thể xóa phiếu công việc (đang được tham chiếu bởi bảng khác)!",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
    }
}
