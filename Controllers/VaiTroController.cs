using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;
using System.Text.Json;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaiTroController : ControllerBase
    {
        private readonly WebApplication3Context _context;

        public VaiTroController(WebApplication3Context context)
        {
            _context = context;
        }

        // ================== CRUD CƠ BẢN ==================

        // GET: api/VaiTro/get-all
        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<VaiTro>>> GetAll()
        {
            return await _context.VaiTros.ToListAsync();
        }

        // GET: api/VaiTro/get/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<VaiTro>> GetById(int id)
        {
            var vt = await _context.VaiTros.FindAsync(id);
            if (vt == null) return NotFound();
            return vt;
        }

        // POST: api/VaiTro/create
        [HttpPost("create")]
        public async Task<ActionResult<VaiTro>> Create([FromBody] VaiTro model)
        {
            _context.VaiTros.Add(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = model.VaiTroID }, model);
        }

        // PUT: api/VaiTro/update/5
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VaiTro model)
        {
            if (id != model.VaiTroID) return BadRequest();

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/VaiTro/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var vt = await _context.VaiTros.FindAsync(id);
            if (vt == null) return NotFound();

            _context.VaiTros.Remove(vt);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ================== NGHIỆP VỤ ==================

        // GET: api/VaiTro/get-by-name/Admin
        [HttpGet("get-by-name/{name}")]
        public async Task<ActionResult<VaiTro>> GetByName(string name)
        {
            var vt = await _context.VaiTros.FirstOrDefaultAsync(x => x.TenVaiTro == name);
            if (vt == null) return NotFound();
            return vt;
        }

        // GET: api/VaiTro/1/permissions
        [HttpGet("{id}/permissions")]
        public async Task<ActionResult<IEnumerable<string>>> GetPermissions(int id)
        {
            var vt = await _context.VaiTros.FindAsync(id);
            if (vt == null) return NotFound();

            if (string.IsNullOrEmpty(vt.QuyenJSON))
                return new List<string>();

            try
            {
                var permissions = JsonSerializer.Deserialize<List<string>>(vt.QuyenJSON);
                return permissions ?? new List<string>();
            }
            catch
            {
                return BadRequest("Dữ liệu quyền (QuyenJSON) không hợp lệ.");
            }
        }

    }
}
