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

        // ==========================
        // 🔹 GET: api/Quyen/get-all
        // ==========================
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _context.Quyens.ToListAsync();
                if (data == null || !data.Any())
                    return NotFound(new { message = "Không có quyền nào trong hệ thống!" });

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tải danh sách quyền!", error = ex.Message });
            }
        }

        // ==========================
        // 🔹 GET: api/Quyen/get/{id}
        // ==========================
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var quyen = await _context.Quyens.FindAsync(id);
                if (quyen == null)
                    return NotFound(new { message = "Không tìm thấy quyền!" });

                return Ok(quyen);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy dữ liệu quyền!", error = ex.Message });
            }
        }

        // ==========================
        // 🔹 POST: api/Quyen/create
        // ==========================
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Quyen model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Dữ liệu quyền không hợp lệ!" });

            try
            {
                _context.Quyens.Add(model);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Thêm quyền thành công!",
                    data = model
                });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new
                {
                    message = "Lỗi khi thêm quyền (có thể do trùng tên hoặc ràng buộc dữ liệu)!",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi không xác định khi thêm quyền!", error = ex.Message });
            }
        }

        // ==========================
        // 🔹 PUT: api/Quyen/update/{id}
        // ==========================
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Quyen model)
        {
            if (id != model.QuyenID)
                return BadRequest(new { message = "ID không khớp với bản ghi cần cập nhật!" });

            try
            {
                var existing = await _context.Quyens.FindAsync(id);
                if (existing == null)
                    return NotFound(new { message = "Không tìm thấy quyền cần cập nhật!" });

                _context.Entry(existing).CurrentValues.SetValues(model);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Cập nhật quyền thành công!" });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new
                {
                    message = "Lỗi khi cập nhật quyền (ràng buộc dữ liệu có thể sai)!",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi không xác định khi cập nhật quyền!", error = ex.Message });
            }
        }

        // ==========================
        // 🔹 DELETE: api/Quyen/delete/{id}
        // ==========================
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var q = await _context.Quyens.FindAsync(id);
                if (q == null)
                    return NotFound(new { message = "Không tìm thấy quyền cần xóa!" });

                _context.Quyens.Remove(q);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Xóa quyền thành công!" });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new
                {
                    message = "Không thể xóa quyền này (có thể đang được sử dụng ở bảng khác)!",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi không xác định khi xóa quyền!", error = ex.Message });
            }
        }

        // ==========================
        // 🔹 GET: api/Quyen/get-by-group/{nhom}
        // ==========================
        [HttpGet("get-by-group/{nhom}")]
        public async Task<IActionResult> GetByGroup(string nhom)
        {
            try
            {
                var data = await _context.Quyens
                    .Where(x => x.Nhom == nhom)
                    .ToListAsync();

                if (!data.Any())
                    return NotFound(new { message = $"Không có quyền nào thuộc nhóm '{nhom}'!" });

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lọc quyền theo nhóm!", error = ex.Message });
            }
        }

        // ==========================
        // 🔹 GET: api/Quyen/search?keyword=view
        // ==========================
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return BadRequest(new { message = "Từ khóa tìm kiếm không được để trống!" });

                var data = await _context.Quyens
                    .Where(x =>
                        x.TenQuyen.Contains(keyword) ||
                        (x.MoTa != null && x.MoTa.Contains(keyword))
                    )
                    .ToListAsync();

                if (!data.Any())
                    return NotFound(new { message = "Không tìm thấy quyền nào phù hợp!" });

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tìm kiếm quyền!", error = ex.Message });
            }
        }
    }
}
