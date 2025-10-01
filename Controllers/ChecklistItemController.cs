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
        public ChecklistItemController(WebApplication3Context context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChecklistItem>>> GetAll()
            => await _context.ChecklistItems.Include(i => i.Checklist).ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<ChecklistItem>> GetById(int id)
        {
            var obj = await _context.ChecklistItems.Include(i => i.Checklist)
                                                   .FirstOrDefaultAsync(i => i.ItemID == id);
            return obj == null ? NotFound() : obj;
        }

        [HttpPost]
        public async Task<ActionResult<ChecklistItem>> Create(ChecklistItem item)
        {
            _context.ChecklistItems.Add(item);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = item.ItemID }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ChecklistItem item)
        {
            if (id != item.ItemID) return BadRequest();
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var obj = await _context.ChecklistItems.FindAsync(id);
            if (obj == null) return NotFound();
            _context.ChecklistItems.Remove(obj);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
