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

        // 🔹 GET: api/PhieuSuCo/get-all
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _context.PhieuSuCos
                    .Include(p => p.TaiSan)
                    .Include(p => p.KhachHang)
                    .Include(p => p.NhanVien)
                    .Include(p => p.PhieuCongViec)
                    .ToListAsync();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tải danh sách phiếu sự cố!", error = ex.Message });
            }
        }

        // 🔹 GET: api/PhieuSuCo/get/{id}
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var obj = await _context.PhieuSuCos
                    .Include(p => p.TaiSan)
                    .Include(p => p.KhachHang)
                    .Include(p => p.NhanVien)
                    .Include(p => p.PhieuCongViec)
                    .FirstOrDefaultAsync(p => p.MaSuCo == id);

                if (obj == null)
                    return NotFound(new { message = "Không tìm thấy phiếu sự cố!" });

                return Ok(obj);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy dữ liệu phiếu sự cố!", error = ex.Message });
            }
        }

        // 🔹 POST: api/PhieuSuCo/create
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] PhieuSuCo model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Dữ liệu phiếu sự cố không hợp lệ!" });

            try
            {
                _context.PhieuSuCos.Add(model);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Thêm phiếu sự cố thành công!",
                    data = model
                });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new
                {
                    message = "Lỗi khi thêm phiếu sự cố (ràng buộc dữ liệu hoặc khóa ngoại sai)!",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi không xác định khi thêm phiếu sự cố!", error = ex.Message });
            }
        }

        // 🔹 PUT: api/PhieuSuCo/update/{id}
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PhieuSuCo model)
        {
            if (id != model.MaSuCo)
                return BadRequest(new { message = "ID không khớp với bản ghi cần cập nhật!" });

            try
            {
                var existing = await _context.PhieuSuCos.FindAsync(id);
                if (existing == null)
                    return NotFound(new { message = "Không tìm thấy phiếu sự cố cần cập nhật!" });

                _context.Entry(existing).CurrentValues.SetValues(model);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Cập nhật phiếu sự cố thành công!" });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new
                {
                    message = "Lỗi khi cập nhật phiếu sự cố (có thể do ràng buộc dữ liệu)!",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi không xác định khi cập nhật phiếu sự cố!", error = ex.Message });
            }
        }

        // 🔹 DELETE: api/PhieuSuCo/delete/{id}
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var obj = await _context.PhieuSuCos.FindAsync(id);
                if (obj == null)
                    return NotFound(new { message = "Không tìm thấy phiếu sự cố cần xóa!" });

                _context.PhieuSuCos.Remove(obj);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Xóa phiếu sự cố thành công!" });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new
                {
                    message = "Không thể xóa phiếu sự cố (có thể do liên kết dữ liệu)!",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa phiếu sự cố!", error = ex.Message });
            }
        }

        // 🔹 GET: api/PhieuSuCo/by-tai-san/{maTaiSan}
        [HttpGet("by-tai-san/{maTaiSan}")]
        public async Task<IActionResult> GetByTaiSan(int maTaiSan)
        {
            try
            {
                var list = await _context.PhieuSuCos
                    .Where(p => p.MaTaiSan == maTaiSan)
                    .Include(p => p.TaiSan)
                    .Include(p => p.NhanVien)
                    .ToListAsync();

                if (!list.Any())
                    return NotFound(new { message = $"Không có phiếu sự cố nào cho tài sản #{maTaiSan}!" });

                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy phiếu sự cố theo tài sản!", error = ex.Message });
            }
        }

        // 🔹 KPI: Đếm số phiếu theo trạng thái
        [HttpGet("kpi/summary")]
        public async Task<IActionResult> GetSummaryByStatus()
        {
            try
            {
                var summary = await _context.PhieuSuCos
                    .GroupBy(p => p.TrangThai)
                    .Select(g => new
                    {
                        TrangThai = g.Key,
                        SoLuong = g.Count()
                    })
                    .ToListAsync();

                return Ok(summary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi thống kê số lượng phiếu sự cố!", error = ex.Message });
            }
        }
    }
}
