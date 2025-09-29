using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BaoHanhController : ControllerBase
{
    [HttpGet("get-all")]
    public IActionResult GetAll()
    {
        return Ok(new[] {
            new { Id = 1, Ten = "Bảo hành Laptop Dell", TrangThai = "Còn hạn" },
            new { Id = 2, Ten = "Bảo hành Máy in HP", TrangThai = "Hết hạn" }
        });
    }
}
