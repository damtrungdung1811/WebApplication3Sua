using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Areas.Identity.Data;
using WebApplication3.Controllers;
using WebApplication3.Models;

namespace WebApplication3.Data
{
    public class WebApplication3Context : IdentityDbContext<WebApplication3User>
    {
        public WebApplication3Context(DbContextOptions<WebApplication3Context> options)
            : base(options)
        {
        }

        // DbSet cho các bảng
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<LinhKien> LinhKiens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<NhanVien>().ToTable("NhanVien");
            builder.Entity<KhachHang>().ToTable("KhachHang");
            builder.Entity<LinhKien>().ToTable("LinhKien");
        }
    }
}
