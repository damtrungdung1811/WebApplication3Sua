using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LichBaoTriController : ControllerBase
    {
        private readonly WebApplication3Context _context;

        public LichBaoTriController(WebApplication3Context context)
        {
            _context = context;
        }

        // ====================== GET ALL ======================
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.LichBaoTris
                .Include(l => l.TaiSan)
                .Include(l => l.NhanVien)
                .ToListAsync();

            return Ok(new
            {
                message = "Lấy danh sách lịch bảo trì thành công!",
                total = data.Count,
                data
            });
        }

        // ====================== GET BY ID ======================
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var obj = await _context.LichBaoTris
                .Include(l => l.TaiSan)
                .Include(l => l.NhanVien)
                .FirstOrDefaultAsync(l => l.MaLich == id);

            if (obj == null)
                return NotFound(new { message = $"Không tìm thấy lịch bảo trì có ID = {id}" });

            return Ok(new
            {
                message = "Lấy thông tin lịch bảo trì thành công!",
                data = obj
            });
        }

        // ====================== CREATE ======================
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] LichBaoTri model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.LichBaoTris.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = model.MaLich }, new
            {
                message = "Thêm lịch bảo trì thành công!",
                data = model
            });
        }

        // ====================== UPDATE ======================
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LichBaoTri model)
        {
            if (id != model.MaLich)
                return BadRequest(new { message = "ID không khớp với dữ liệu gửi lên!" });

            var existing = await _context.LichBaoTris.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = $"Không tìm thấy lịch bảo trì có ID = {id}" });

            // Cập nhật các trường
            existing.MaTaiSan = model.MaTaiSan;
            existing.MaNV = model.MaNV;
            existing.TanSuat = model.TanSuat;
            existing.SoNgayLapLai = model.SoNgayLapLai;
            existing.NgayKeTiep = model.NgayKeTiep;
            existing.ChecklistMacDinh = model.ChecklistMacDinh;
            existing.HieuLuc = model.HieuLuc;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Cập nhật lịch bảo trì thành công!",
                data = existing
            });
        }

        // ====================== DELETE ======================
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var lich = await _context.LichBaoTris
                .Include(l => l.TaiSan)
                .FirstOrDefaultAsync(l => l.MaLich == id);

            if (lich == null)
                return NotFound(new { message = $"Không tìm thấy Lịch bảo trì có ID = {id}" });

            // Xóa trước các phiếu công việc liên quan
            var phieuCVs = _context.PhieuCongViecs.Where(p => p.MaLich == id);
            if (phieuCVs.Any())
                _context.PhieuCongViecs.RemoveRange(phieuCVs);

            _context.LichBaoTris.Remove(lich);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa lịch bảo trì thành công (bao gồm các phiếu công việc liên quan)!" });
        }

    }
}
