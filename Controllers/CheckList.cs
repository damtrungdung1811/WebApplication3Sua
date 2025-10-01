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
        public ChecklistController(WebApplication3Context context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Checklist>>> GetAll()
            => await _context.Checklists.Include(c => c.Items).ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Checklist>> GetById(int id)
        {
            var obj = await _context.Checklists.Include(c => c.Items)
                                               .FirstOrDefaultAsync(c => c.ChecklistID == id);
            return obj == null ? NotFound() : obj;
        }

        [HttpPost]
        public async Task<ActionResult<Checklist>> Create(Checklist checklist)
        {
            _context.Checklists.Add(checklist);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = checklist.ChecklistID }, checklist);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Checklist checklist)
        {
            if (id != checklist.ChecklistID) return BadRequest();
            _context.Entry(checklist).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var obj = await _context.Checklists.FindAsync(id);
            if (obj == null) return NotFound();
            _context.Checklists.Remove(obj);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
