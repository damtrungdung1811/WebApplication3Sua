using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhachHangController : ControllerBase
    {
        private readonly WebApplication3Context _context;

        public KhachHangController(WebApplication3Context context)
        {
            _context = context;
        }

        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<KhachHang>>> GetAll()
        {
            return await _context.KhachHangs.ToListAsync();
        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<KhachHang>> GetById(int id)
        {
            var kh = await _context.KhachHangs.FindAsync(id);
            if (kh == null) return NotFound();
            return kh;
        }

        [HttpPost("create")]
        public async Task<ActionResult<KhachHang>> Create([FromBody] KhachHang kh)
        {
            _context.KhachHangs.Add(kh);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = kh.MaKH }, kh);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] KhachHang kh)
        {
            if (id != kh.MaKH) return BadRequest();

            _context.Entry(kh).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var kh = await _context.KhachHangs.FindAsync(id);
            if (kh == null) return NotFound();

            _context.KhachHangs.Remove(kh);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
