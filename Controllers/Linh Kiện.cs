using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinhKienController : ControllerBase
    {
        private readonly WebApplication3Context _context;

        public LinhKienController(WebApplication3Context context)
        {
            _context = context;
        }

        // ===================== GET ALL =====================
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.LinhKiens.ToListAsync();
            return Ok(new
            {
                message = "Lấy danh sách linh kiện thành công!",
                total = list.Count,
                data = list
            });
        }

        // ===================== GET BY ID =====================
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var linhKien = await _context.LinhKiens.FindAsync(id);
            if (linhKien == null)
                return NotFound(new { message = $"Không tìm thấy linh kiện có ID = {id}" });

            return Ok(new
            {
                message = "Lấy thông tin linh kiện thành công!",
                data = linhKien
            });
        }

        // ===================== CREATE =====================
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] LinhKien lk)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _context.LinhKiens.Add(lk);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = lk.MaLinhKien }, new
                {
                    message = "Thêm linh kiện thành công!",
                    data = lk
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi thêm linh kiện!", error = ex.Message });
            }
        }

        // ===================== UPDATE =====================
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LinhKien lk)
        {
            if (id != lk.MaLinhKien)
                return BadRequest(new { message = "ID không khớp với dữ liệu gửi lên!" });

            _context.Entry(lk).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    message = "Cập nhật linh kiện thành công!",
                    data = lk
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LinhKienExists(id))
                    return NotFound(new { message = $"Không tìm thấy linh kiện có ID = {id}" });
                else
                    throw;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật linh kiện!", error = ex.Message });
            }
        }

        // ===================== DELETE =====================
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var lk = await _context.LinhKiens.FindAsync(id);
            if (lk == null)
                return NotFound(new { message = $"Không tìm thấy linh kiện có ID = {id}" });

            try
            {
                _context.LinhKiens.Remove(lk);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Xóa linh kiện thành công!" });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new
                {
                    message = "Không thể xóa linh kiện này do đang được sử dụng ở bảng khác!",
                    error = ex.Message
                });
            }
        }

        private bool LinhKienExists(int id)
        {
            return _context.LinhKiens.Any(e => e.MaLinhKien == id);
        }
    }
}
