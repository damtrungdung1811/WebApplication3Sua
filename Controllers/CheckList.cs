using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChecklistController : ControllerBase
    {
        private readonly WebApplication3Context _context;

        public ChecklistController(WebApplication3Context context)
        {
            _context = context;
        }

        // ====================== GET ALL ======================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.Checklists
                                     .Include(c => c.Items)
                                     .ToListAsync();
            return Ok(new { message = "Lấy danh sách checklist thành công!", data });
        }

        // ====================== GET BY ID ======================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var checklist = await _context.Checklists
                                          .Include(c => c.Items)
                                          .FirstOrDefaultAsync(c => c.ChecklistID == id);
            if (checklist == null)
                return NotFound(new { message = $"Không tìm thấy checklist có ID = {id}" });

            return Ok(new { message = "Lấy checklist thành công!", data = checklist });
        }

        // ====================== CREATE ======================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Checklist checklist)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Nếu có Items thì gán quan hệ thủ công để tránh lỗi EF
            if (checklist.Items != null)
            {
                foreach (var item in checklist.Items)
                    item.Checklist = checklist;
            }

            _context.Checklists.Add(checklist);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Thêm checklist thành công!",
                data = checklist
            });
        }

        // ====================== UPDATE ======================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Checklist checklist)
        {
            var existing = await _context.Checklists
                                         .Include(c => c.Items)
                                         .FirstOrDefaultAsync(c => c.ChecklistID == id);

            if (existing == null)
                return NotFound(new { message = $"Không tìm thấy checklist có ID = {id}" });

            // Cập nhật thông tin cơ bản
            existing.Ten = checklist.Ten;
            existing.MoTa = checklist.MoTa;

            // Nếu có danh sách item mới -> thay thế
            if (checklist.Items != null)
            {
                // Xóa item cũ
                _context.ChecklistItems.RemoveRange(existing.Items ?? new List<ChecklistItem>());

                // Gắn item mới
                existing.Items = checklist.Items;
                foreach (var item in existing.Items)
                    item.ChecklistID = existing.ChecklistID;
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Cập nhật checklist thành công!",
                data = existing
            });
        }

        // ====================== DELETE ======================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var checklist = await _context.Checklists
                                          .Include(c => c.Items)
                                          .FirstOrDefaultAsync(c => c.ChecklistID == id);

            if (checklist == null)
                return NotFound(new { message = $"Không tìm thấy checklist có ID = {id}" });

            // Xóa item con trước
            if (checklist.Items != null && checklist.Items.Any())
                _context.ChecklistItems.RemoveRange(checklist.Items);

            _context.Checklists.Remove(checklist);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa checklist thành công!" });
        }
    }
}
