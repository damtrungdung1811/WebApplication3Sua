using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhieuKhoChiTietController : ControllerBase
    {
        private readonly WebApplication3Context _context;

        public PhieuKhoChiTietController(WebApplication3Context context)
        {
            _context = context;
        }

        // 🔹 GET: api/PhieuKhoChiTiet/get-all
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _context.PhieuKho_ChiTiets
                    .Include(c => c.PhieuKho)
                    .Include(c => c.LinhKien)
                    .ToListAsync();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tải danh sách chi tiết phiếu kho!", error = ex.Message });
            }
        }

        // 🔹 GET: api/PhieuKhoChiTiet/get/{id}
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var ct = await _context.PhieuKho_ChiTiets
                    .Include(c => c.PhieuKho)
                    .Include(c => c.LinhKien)
                    .FirstOrDefaultAsync(c => c.MaCT == id);

                if (ct == null)
                    return NotFound(new { message = "Không tìm thấy chi tiết phiếu kho!" });

                return Ok(ct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy dữ liệu chi tiết phiếu kho!", error = ex.Message });
            }
        }

        // 🔹 POST: api/PhieuKhoChiTiet/create
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] PhieuKhoChiTiet model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Dữ liệu chi tiết phiếu kho không hợp lệ!" });

            try
            {
                _context.PhieuKho_ChiTiets.Add(model);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Thêm chi tiết phiếu kho thành công!",
                    data = model
                });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new
                {
                    message = "Lỗi khi thêm chi tiết phiếu kho (có thể do khóa ngoại hoặc dữ liệu sai)!",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi không xác định khi thêm chi tiết phiếu kho!", error = ex.Message });
            }
        }

        // 🔹 PUT: api/PhieuKhoChiTiet/update/{id}
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PhieuKhoChiTiet model)
        {
            if (id != model.MaCT)
                return BadRequest(new { message = "ID không khớp với bản ghi cần cập nhật!" });

            try
            {
                var existing = await _context.PhieuKho_ChiTiets.FindAsync(id);
                if (existing == null)
                    return NotFound(new { message = "Không tìm thấy chi tiết phiếu kho cần cập nhật!" });

                _context.Entry(existing).CurrentValues.SetValues(model);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Cập nhật chi tiết phiếu kho thành công!" });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new
                {
                    message = "Lỗi khi cập nhật chi tiết phiếu kho (có thể do ràng buộc dữ liệu)!",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi không xác định khi cập nhật chi tiết phiếu kho!", error = ex.Message });
            }
        }

        // 🔹 DELETE: api/PhieuKhoChiTiet/delete/{id}
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var ct = await _context.PhieuKho_ChiTiets.FindAsync(id);
                if (ct == null)
                    return NotFound(new { message = "Không tìm thấy chi tiết phiếu kho cần xóa!" });

                _context.PhieuKho_ChiTiets.Remove(ct);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Xóa chi tiết phiếu kho thành công!" });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new
                {
                    message = "Không thể xóa chi tiết phiếu kho (có thể đang được tham chiếu ở nơi khác)!",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa chi tiết phiếu kho!", error = ex.Message });
            }
        }

        // 🔹 NGHIỆP VỤ: Lấy chi tiết theo mã phiếu kho
        // GET: api/PhieuKhoChiTiet/by-phieu/{phieuId}
        [HttpGet("by-phieu/{phieuId}")]
        public async Task<IActionResult> GetByPhieu(int phieuId)
        {
            try
            {
                var list = await _context.PhieuKho_ChiTiets
                    .Include(c => c.LinhKien)
                    .Where(c => c.MaPhieuKho == phieuId)
                    .ToListAsync();

                if (!list.Any())
                    return NotFound(new { message = $"Không có chi tiết nào thuộc phiếu kho #{phieuId}!" });

                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tải chi tiết theo phiếu kho!", error = ex.Message });
            }
        }
    }
}
