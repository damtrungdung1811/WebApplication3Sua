using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PCV_ChecklistController : ControllerBase
    {
        private readonly WebApplication3Context _context;
        public PCV_ChecklistController(WebApplication3Context context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PCV_Checklist>>> GetAll()
            => await _context.PCV_Checklists.Include(p => p.Item).ToListAsync();

        [HttpGet("summary")]
        public async Task<ActionResult<object>> GetChecklistSummary()
        {
            // Nhóm checklist theo Phiếu Công Việc
            var data = await _context.PCV_Checklists
                .GroupBy(p => p.MaPhieuCV)
                .Select(g => new
                {
                    MaPhieuCV = g.Key,
                    TongSo = g.Count(),
                    HoanThanh = g.Count(x => x.DaHoanThanh),
                    ChuaHoanThanh = g.Count(x => !x.DaHoanThanh),
                    DaHoanTat = g.All(x => x.DaHoanThanh) // true nếu tất cả đều hoàn thành
                })
                .ToListAsync();

            // Thống kê tổng thể
            var daHoanTat = data.Count(x => x.DaHoanTat);
            var chuaHoanTat = data.Count(x => !x.DaHoanTat);

            return new
            {
                TongPhieu = data.Count,
                DaHoanTat = daHoanTat,
                ChuaHoanTat = chuaHoanTat,
                ChiTiet = data
            };
        }

    }
}
