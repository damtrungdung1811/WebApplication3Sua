using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChecklistItemController : ControllerBase
    {
        private readonly WebApplication3Context _context;
        public ChecklistItemController(WebApplication3Context context)
        {
            _context = context;
        }

        // ====================== GET ALL ======================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChecklistItem>>> GetAll()
        {
            var items = await _context.ChecklistItems
                                      .Include(i => i.Checklist)
                                      .ToListAsync();

            if (items == null || items.Count == 0)
                return NotFound(new { message = "Không có dữ liệu ChecklistItem nào!" });

            return Ok(items);
        }

        // ====================== GET BY ID ======================
        [HttpGet("{id}")]
        public async Task<ActionResult<ChecklistItem>> GetById(int id)
        {
            var item = await _context.ChecklistItems
                                     .Include(i => i.Checklist)
                                     .FirstOrDefaultAsync(i => i.ItemID == id);

            if (item == null)
                return NotFound(new { message = $"Không tìm thấy ChecklistItem có ID = {id}" });

            return Ok(item);
        }

        // ====================== CREATE ======================
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ChecklistItem item)
        {
            try
            {
                // Kiểm tra ChecklistID có tồn tại không
                var checklist = await _context.Checklists.FindAsync(item.ChecklistID);
                if (checklist == null)
                    return BadRequest(new { message = $"ChecklistID {item.ChecklistID} không tồn tại!" });

                _context.ChecklistItems.Add(item);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById),
                    new { id = item.ItemID },
                    new { message = "Thêm ChecklistItem thành công!", data = item });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi thêm ChecklistItem!", error = ex.Message });
            }
        }

        // ====================== UPDATE ======================
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] ChecklistItem item)
        {
            if (id != item.ItemID)
                return BadRequest(new { message = "ID không khớp giữa URL và body!" });

            var existing = await _context.ChecklistItems.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = $"Không tìm thấy ChecklistItem có ID = {id}" });

            // Cập nhật từng trường
            existing.NoiDung = item.NoiDung;
            existing.ChecklistID = item.ChecklistID;

            try
            {
                _context.Entry(existing).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(new { message = "Cập nhật ChecklistItem thành công!", data = existing });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật ChecklistItem!", error = ex.Message });
            }
        }

        // ====================== DELETE ======================
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var obj = await _context.ChecklistItems.FindAsync(id);
            if (obj == null)
                return NotFound(new { message = $"Không tìm thấy ChecklistItem có ID = {id}" });

            try
            {
                _context.ChecklistItems.Remove(obj);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Xóa ChecklistItem thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa ChecklistItem!", error = ex.Message });
            }
        }
    }
}
