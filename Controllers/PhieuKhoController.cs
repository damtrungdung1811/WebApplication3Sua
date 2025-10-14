using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhieuKhoController : ControllerBase
    {
        private readonly WebApplication3Context _context;

        public PhieuKhoController(WebApplication3Context context)
        {
            _context = context;
        }

        // 🔹 GET: api/PhieuKho/get-all
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _context.PhieuKhos.ToListAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tải danh sách phiếu kho!", error = ex.Message });
            }
        }

        // 🔹 GET: api/PhieuKho/get/{id}
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var phieu = await _context.PhieuKhos.FindAsync(id);
                if (phieu == null)
                    return NotFound(new { message = "Không tìm thấy phiếu kho!" });

                return Ok(phieu);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy dữ liệu phiếu kho!", error = ex.Message });
            }
        }

        // 🔹 POST: api/PhieuKho/create
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] PhieuKho model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Dữ liệu phiếu kho không hợp lệ!" });

            try
            {
                _context.PhieuKhos.Add(model);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Thêm phiếu kho thành công!",
                    data = model
                });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new
                {
                    message = "Lỗi khi thêm phiếu kho (ràng buộc hoặc dữ liệu không hợp lệ)!",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi không xác định khi thêm phiếu kho!", error = ex.Message });
            }
        }

        // 🔹 PUT: api/PhieuKho/update/{id}
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PhieuKho model)
        {
            if (id != model.MaPhieuKho)
                return BadRequest(new { message = "ID không khớp với bản ghi cần cập nhật!" });

            try
            {
                var existing = await _context.PhieuKhos.FindAsync(id);
                if (existing == null)
                    return NotFound(new { message = "Không tìm thấy phiếu kho cần cập nhật!" });

                _context.Entry(existing).CurrentValues.SetValues(model);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Cập nhật phiếu kho thành công!" });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new
                {
                    message = "Lỗi khi cập nhật phiếu kho (có thể do ràng buộc dữ liệu)!",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi không xác định khi cập nhật phiếu kho!", error = ex.Message });
            }
        }

        // 🔹 DELETE: api/PhieuKho/delete/{id}
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var phieu = await _context.PhieuKhos.FindAsync(id);
                if (phieu == null)
                    return NotFound(new { message = "Không tìm thấy phiếu kho cần xóa!" });

                _context.PhieuKhos.Remove(phieu);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Xóa phiếu kho thành công!" });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new
                {
                    message = "Không thể xóa phiếu kho (có thể do đang được sử dụng ở bảng khác)!",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa phiếu kho!", error = ex.Message });
            }
        }

        // 🔹 GET: api/PhieuKho/by-type/{loai}
        [HttpGet("by-type/{loai}")]
        public async Task<IActionResult> GetByType(string loai)
        {
            try
            {
                var list = await _context.PhieuKhos
                    .Where(x => x.Loai == loai)
                    .ToListAsync();

                if (!list.Any())
                    return NotFound(new { message = $"Không có phiếu kho nào thuộc loại '{loai}'!" });

                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lọc phiếu kho theo loại!", error = ex.Message });
            }
        }

        // 🔹 KPI: Tổng số phiếu theo loại
        [HttpGet("kpi/summary")]
        public async Task<IActionResult> GetKpiSummary()
        {
            try
            {
                var kpi = await _context.PhieuKhos
                    .GroupBy(p => p.Loai)
                    .Select(g => new
                    {
                        Loai = g.Key,
                        SoLuong = g.Count()
                    })
                    .ToListAsync();

                return Ok(kpi);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi thống kê số lượng phiếu kho!", error = ex.Message });
            }
        }

        // 🔹 KPI: Tổng giá trị từng phiếu (nếu có bảng chi tiết)
        [HttpGet("kpi/tong-gia-tri")]
        public async Task<IActionResult> GetTotalValue()
        {
            try
            {
                var result = await _context.PhieuKhos
                    .Select(p => new
                    {
                        p.MaPhieuKho,
                        p.Loai,
                        p.NgayLap,
                        TongGiaTri = _context.PhieuKho_ChiTiets
                            .Where(ct => ct.MaPhieuKho == p.MaPhieuKho)
                            .Sum(ct => (decimal?)ct.SoLuong * ct.DonGia) ?? 0
                    })
                    .OrderByDescending(p => p.NgayLap)
                    .ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tính tổng giá trị phiếu kho!", error = ex.Message });
            }
        }
    }
}
