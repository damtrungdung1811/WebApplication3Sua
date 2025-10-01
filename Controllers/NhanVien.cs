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

        // 🔹 GET: api/NhanVien/get-all
        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<NhanVien>>> GetAll()
        {
            return await _context.NhanViens.ToListAsync();
        }

        // 🔹 GET: api/NhanVien/get/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<NhanVien>> GetNhanVien(int id)
        {
            var nv = await _context.NhanViens.FindAsync(id);
            if (nv == null)
                return NotFound(new { message = "Không tìm thấy nhân viên" });

            return nv;
        }

        // 🔹 POST: api/NhanVien/create
        [HttpPost("create")]
        public async Task<ActionResult<NhanVien>> Create(NhanVien nv)
        {
            _context.NhanViens.Add(nv);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNhanVien), new { id = nv.MaNV }, nv);
        }

        // 🔹 PUT: api/NhanVien/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, NhanVien nv)
        {
            if (id != nv.MaNV)
                return BadRequest(new { message = "ID không khớp" });

            _context.Entry(nv).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.NhanViens.Any(e => e.MaNV == id))
                    return NotFound(new { message = "Không tìm thấy nhân viên" });
                else
                    throw;
            }

            return Ok(new { message = "Cập nhật thành công" });
        }

        // 🔹 DELETE: api/NhanVien/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var nv = await _context.NhanViens.FindAsync(id);
            if (nv == null)
                return NotFound(new { message = "Không tìm thấy nhân viên" });

            _context.NhanViens.Remove(nv);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa thành công" });
        }
        // 🔹 KPI 1: Đếm số nhân viên đang hoạt động
        [HttpGet("api/hoat-dong")]
        public async Task<IActionResult> GetSoNhanVienHoatDong()
        {
            var count = await _context.NhanViens
                                      .Where(nv => nv.TrangThai == "Hoạt động")
                                      .CountAsync();

            return Ok(new { SoNhanVienHoatDong = count });
        }

        // 🔹 KPI 2: Đếm số phiếu công việc được phân công cho từng nhân viên
        [HttpGet("api/phieu-cong-viec")]
        public async Task<IActionResult> GetNhanVienPhieuCongViec()
        {
            var result = await _context.NhanViens
                .Select(nv => new
                {
                    nv.MaNV,
                    nv.HoTen,
                    SoPhieuCongViec = _context.PhieuCongViecs.Count(pcv => pcv.MaNV_PhanCong == nv.MaNV)
                })
                .ToListAsync();

            return Ok(result);
        }

        // 🔹 KPI 3: Đếm số phiếu sự cố mà nhân viên tiếp nhận
        [HttpGet("kpi/su-co")]
        public async Task<IActionResult> GetNhanVienSuCo()
        {
            var result = await _context.NhanViens
                .Select(nv => new
                {
                    nv.MaNV,
                    nv.HoTen,
                    SoSuCoTiepNhan = _context.PhieuSuCos.Count(sc => sc.MaNV_TiepNhan == nv.MaNV)
                })
                .ToListAsync();

            return Ok(result);
        }

    }
}
