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

        // ===================== 🔹 GET ALL =====================
        // GET: api/NhanVien/get-all
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _context.NhanViens.ToListAsync();
                if (list == null || list.Count == 0)
                    return NotFound(new { message = "Không có nhân viên nào trong hệ thống!" });

                return Ok(new
                {
                    message = "Lấy danh sách nhân viên thành công!",
                    data = list
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách nhân viên!", error = ex.Message });
            }
        }

        // ===================== 🔹 GET BY ID =====================
        // GET: api/NhanVien/get/5
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var nv = await _context.NhanViens.FindAsync(id);
                if (nv == null)
                    return NotFound(new { message = $"Không tìm thấy nhân viên có ID = {id}" });

                return Ok(new { message = "Lấy thông tin nhân viên thành công!", data = nv });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy thông tin nhân viên!", error = ex.Message });
            }
        }

        // ===================== 🔹 CREATE =====================
        // POST: api/NhanVien/create
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] NhanVien nv)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { message = "Dữ liệu không hợp lệ!", errors = ModelState.Values });

                _context.NhanViens.Add(nv);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Thêm nhân viên thành công!",
                    data = nv
                });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi khi thêm nhân viên (có thể trùng khóa hoặc vi phạm ràng buộc)!",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi không xác định khi thêm nhân viên!", error = ex.Message });
            }
        }

        // ===================== 🔹 UPDATE =====================
        // PUT: api/NhanVien/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] NhanVien nv)
        {
            if (id != nv.MaNV)
                return BadRequest(new { message = "ID trong URL không khớp với ID trong dữ liệu gửi lên!" });

            try
            {
                var existing = await _context.NhanViens.FindAsync(id);
                if (existing == null)
                    return NotFound(new { message = $"Không tìm thấy nhân viên có ID = {id}" });

                // Cập nhật thủ công các trường cần thiết (tránh lỗi tracking)
                _context.Entry(existing).CurrentValues.SetValues(nv);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Cập nhật nhân viên thành công!", data = nv });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return StatusCode(500, new { message = "Lỗi xung đột dữ liệu khi cập nhật!", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi không xác định khi cập nhật nhân viên!", error = ex.Message });
            }
        }

        // ===================== 🔹 DELETE =====================
        // DELETE: api/NhanVien/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var nv = await _context.NhanViens.FindAsync(id);
                if (nv == null)
                    return NotFound(new { message = $"Không tìm thấy nhân viên có ID = {id}" });

                _context.NhanViens.Remove(nv);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Xóa nhân viên thành công!", deletedId = id });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new
                {
                    message = "Không thể xóa nhân viên này (có thể đang được tham chiếu ở bảng khác)!",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi không xác định khi xóa nhân viên!", error = ex.Message });
            }
        }

        // ===================== 🔹 KPI 1: Số nhân viên hoạt động =====================
        // GET: api/NhanVien/kpi/hoat-dong
        [HttpGet("kpi/hoat-dong")]
        public async Task<IActionResult> GetSoNhanVienHoatDong()
        {
            try
            {
                var count = await _context.NhanViens
                    .Where(nv => nv.TrangThai == "Hoạt động")
                    .CountAsync();

                return Ok(new
                {
                    message = "Thống kê thành công!",
                    soNhanVienHoatDong = count
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi thống kê nhân viên hoạt động!", error = ex.Message });
            }
        }

        // ===================== 🔹 KPI 2: Phiếu công việc mỗi nhân viên =====================
        // GET: api/NhanVien/kpi/phieu-cong-viec
        [HttpGet("kpi/phieu-cong-viec")]
        public async Task<IActionResult> GetNhanVienPhieuCongViec()
        {
            try
            {
                var result = await _context.NhanViens
                    .Select(nv => new
                    {
                        nv.MaNV,
                        nv.HoTen,
                        SoPhieuCongViec = _context.PhieuCongViecs.Count(pcv => pcv.MaNV_PhanCong == nv.MaNV)
                    })
                    .ToListAsync();

                return Ok(new
                {
                    message = "Thống kê phiếu công việc theo nhân viên thành công!",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi thống kê phiếu công việc!", error = ex.Message });
            }
        }

        // ===================== 🔹 KPI 3: Số phiếu sự cố đã tiếp nhận =====================
        // GET: api/NhanVien/kpi/su-co
        [HttpGet("kpi/su-co")]
        public async Task<IActionResult> GetNhanVienSuCo()
        {
            try
            {
                var result = await _context.NhanViens
                    .Select(nv => new
                    {
                        nv.MaNV,
                        nv.HoTen,
                        SoSuCoTiepNhan = _context.PhieuSuCos.Count(sc => sc.MaNV_TiepNhan == nv.MaNV)
                    })
                    .ToListAsync();

                return Ok(new
                {
                    message = "Thống kê phiếu sự cố theo nhân viên thành công!",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi thống kê phiếu sự cố!", error = ex.Message });
            }
        }
    }
}
