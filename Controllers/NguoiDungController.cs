using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NguoiDungController : ControllerBase
    {
        private readonly WebApplication3Context _context;

        public NguoiDungController(WebApplication3Context context)
        {
            _context = context;
        }

        // ======================= GET ALL =======================
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _context.NguoiDungs
                    .Include(u => u.VaiTro)
                    .Include(u => u.NhanVien)
                    .ToListAsync();

                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách người dùng!", error = ex.Message });
            }
        }

        // ======================= GET BY ID =======================
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var user = await _context.NguoiDungs
                    .Include(u => u.VaiTro)
                    .Include(u => u.NhanVien)
                    .FirstOrDefaultAsync(u => u.MaNguoiDung == id);

                if (user == null)
                    return NotFound(new { message = $"Không tìm thấy người dùng có ID = {id}" });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy người dùng!", error = ex.Message });
            }
        }

        // ======================= CREATE =======================
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] NguoiDung model)
        {
            try
            {
                if (model == null)
                    return BadRequest(new { message = "Dữ liệu gửi lên không hợp lệ!" });

                // Kiểm tra trùng tên đăng nhập
                bool exists = await _context.NguoiDungs.AnyAsync(u => u.TenDangNhap == model.TenDangNhap);
                if (exists)
                    return BadRequest(new { message = "Tên đăng nhập đã tồn tại!" });

                _context.NguoiDungs.Add(model);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Thêm người dùng thành công!", data = model });
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new { message = "Lỗi khi thêm người dùng (ràng buộc FK hoặc trùng khóa)!", error = dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi thêm người dùng!", error = ex.Message });
            }
        }

        // ======================= UPDATE =======================
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] NguoiDung model)
        {
            try
            {
                if (id != model.MaNguoiDung)
                    return BadRequest(new { message = "ID không khớp với dữ liệu gửi lên!" });

                var existing = await _context.NguoiDungs.FindAsync(id);
                if (existing == null)
                    return NotFound(new { message = $"Không tìm thấy người dùng có ID = {id}" });

                // Cập nhật thủ công các trường cần thiết
                existing.TenDangNhap = model.TenDangNhap;
                existing.MatKhauHash = model.MatKhauHash;
                existing.Email = model.Email;
                existing.MaNV = model.MaNV;
                existing.VaiTroID = model.VaiTroID;
                existing.TrangThai = model.TrangThai;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Cập nhật người dùng thành công!", data = existing });
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật người dùng (ràng buộc hoặc dữ liệu không hợp lệ)!", error = dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật người dùng!", error = ex.Message });
            }
        }

        // ======================= DELETE =======================
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var user = await _context.NguoiDungs.FindAsync(id);
                if (user == null)
                    return NotFound(new { message = $"Không tìm thấy người dùng có ID = {id}" });

                // Kiểm tra nếu người dùng đang được tham chiếu (ví dụ ở bảng khác)
                bool isReferenced = false; // Nếu bạn có bảng con, có thể dùng AnyAsync() để kiểm tra

                if (isReferenced)
                    return BadRequest(new { message = "Không thể xóa người dùng này do đang được sử dụng ở bảng khác!" });

                _context.NguoiDungs.Remove(user);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Xóa người dùng thành công!" });
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new { message = "Không thể xóa người dùng này do ràng buộc dữ liệu!", error = dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa người dùng!", error = ex.Message });
            }
        }

        // ======================= GET BY ROLE =======================
        [HttpGet("by-role/{roleId}")]
        public async Task<IActionResult> GetByRole(int roleId)
        {
            try
            {
                var list = await _context.NguoiDungs
                    .Include(u => u.VaiTro)
                    .Where(u => u.VaiTroID == roleId)
                    .ToListAsync();

                if (!list.Any())
                    return NotFound(new { message = $"Không có người dùng nào thuộc vai trò ID = {roleId}" });

                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách người dùng theo vai trò!", error = ex.Message });
            }
        }

        // ======================= GET BY USERNAME =======================
        [HttpGet("by-username/{username}")]
        public async Task<IActionResult> GetByUsername(string username)
        {
            try
            {
                var user = await _context.NguoiDungs
                    .Include(u => u.VaiTro)
                    .Include(u => u.NhanVien)
                    .FirstOrDefaultAsync(u => u.TenDangNhap == username);

                if (user == null)
                    return NotFound(new { message = $"Không tìm thấy người dùng có tên đăng nhập '{username}'" });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tìm người dùng theo tên đăng nhập!", error = ex.Message });
            }
        }
    }
}
