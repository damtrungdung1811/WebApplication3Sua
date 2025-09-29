using Microsoft.AspNetCore.Mvc;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaoTriController : ControllerBase
    {
        // GET api/baotri/get-all
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            var data = new[]
            {
                new { Id = 1, Ten = "Bảo trì máy in", TrangThai = "Hoàn thành" },
                new { Id = 2, Ten = "Bảo trì máy lạnh", TrangThai = "Đang xử lý" }
            };
            return Ok(data);
        }

        // POST api/baotri/create
        [HttpPost("create")]
        public IActionResult Create([FromBody] object model)
        {
            return Ok(new { Message = "Đã thêm mới bảo trì", Data = model });
        }

        // PUT api/baotri/update/1
        [HttpPut("update/{id}")]
        public IActionResult Update(int id, [FromBody] object model)
        {
            return Ok(new { Message = $"Đã cập nhật bảo trì {id}", Data = model });
        }

        // DELETE api/baotri/delete/1
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            return Ok(new { Message = $"Đã xóa bảo trì {id}" });
        }
    }
}
