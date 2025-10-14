using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaiSanController : ControllerBase
    {
        private readonly WebApplication3Context _context;

        public TaiSanController(WebApplication3Context context)
        {
            _context = context;
        }

        // 🔹 GET: api/TaiSan/get-all
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _context.TaiSans
                    .Include(t => t.BaoHanh)
                    .Include(t => t.KhachHang)
                    .ToListAsync();

                if (data == null || !data.Any())
                    return NotFound(new { message = "Không có tài sản nào trong hệ thống!" });

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tải danh sách tài sản!", error = ex.Message });
            }
        }

        // 🔹 GET: api/TaiSan/get/{id}
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var ts = await _context.TaiSans
                    .Include(t => t.BaoHanh)
                    .Include(t => t.KhachHang)
                    .FirstOrDefaultAsync(t => t.MaTaiSan == id);

                if (ts == null)
                    return NotFound(new { message = "Không tìm thấy tài sản!" });

                return Ok(ts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy dữ liệu tài sản!", error = ex.Message });
            }
        }

        // 🔹 POST: api/TaiSan/create
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] TaiSan model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Dữ liệu tài sản không hợp lệ!" });

            try
            {
                _context.TaiSans.Add(model);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Thêm tài sản thành công!",
                    data = model
                });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new
                {
                    message = "Lỗi khi thêm tài sản (ràng buộc khóa ngoại có thể sai)!",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi không xác định khi thêm tài sản!", error = ex.Message });
            }
        }

        // 🔹 PUT: api/TaiSan/update/{id}
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TaiSan model)
        {
            if (id != model.MaTaiSan)
                return BadRequest(new { message = "ID không khớp với bản ghi cần cập nhật!" });

            try
            {
                var existing = await _context.TaiSans.FindAsync(id);
                if (existing == null)
                    return NotFound(new { message = "Không tìm thấy tài sản cần cập nhật!" });

                _context.Entry(existing).CurrentValues.SetValues(model);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Cập nhật tài sản thành công!" });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new
                {
                    message = "Lỗi khi cập nhật tài sản (có thể do khóa ngoại hoặc dữ liệu sai)!",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi không xác định khi cập nhật tài sản!", error = ex.Message });
            }
        }

        // 🔹 DELETE: api/TaiSan/delete/{id}
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var ts = await _context.TaiSans
                    .Include(t => t.BaoHanh)
                    .Include(t => t.KhachHang)
                    .FirstOrDefaultAsync(t => t.MaTaiSan == id);

                if (ts == null)
                    return NotFound(new { message = "Không tìm thấy tài sản cần xóa!" });

                _context.TaiSans.Remove(ts);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Xóa tài sản thành công!" });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new
                {
                    message = "Không thể xóa tài sản (có thể do ràng buộc dữ liệu)!",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi không xác định khi xóa tài sản!", error = ex.Message });
            }
        }

        // 🔹 GET: api/TaiSan/search?keyword=Canon
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return BadRequest(new { message = "Từ khóa tìm kiếm không được để trống!" });

                var results = await _context.TaiSans
                    .Where(t => t.TenTaiSan.Contains(keyword) || (t.ViTri != null && t.ViTri.Contains(keyword)))
                    .ToListAsync();

                if (!results.Any())
                    return NotFound(new { message = "Không tìm thấy tài sản nào phù hợp!" });

                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tìm kiếm tài sản!", error = ex.Message });
            }
        }
    }
}
