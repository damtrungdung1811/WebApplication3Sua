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

        public DbSet<BaoHanh> BaoHanhs { get; set; }
        public DbSet<TaiSan> TaiSans { get; set; }
        public DbSet<LichBaoTri> LichBaoTris { get; set; }
        public DbSet<PhieuCongViec> PhieuCongViecs { get; set; }
        public DbSet<PhieuSuCo> PhieuSuCos { get; set; }
        public DbSet<VaiTro> VaiTros { get; set; }
        public DbSet<Checklist> Checklists { get; set; }
        public DbSet<ChecklistItem> ChecklistItems { get; set; }
        public DbSet<PCV_Checklist> PCV_Checklists { get; set; }
        public DbSet<NhatKyHeThong> NhatKyHeThongs { get; set; }
        public DbSet<TepDinhKem> TepDinhKems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<NhanVien>().ToTable("NhanVien");
            builder.Entity<KhachHang>().ToTable("KhachHang");
            builder.Entity<LinhKien>().ToTable("LinhKien");
            builder.Entity<BaoHanh>().ToTable("BaoHanh");
            builder.Entity<TaiSan>().ToTable("TaiSan");
            builder.Entity<LichBaoTri>().ToTable("LichBaoTri");
            builder.Entity<PhieuCongViec>().ToTable("PhieuCongViec");
            builder.Entity<PhieuSuCo>().ToTable("PhieuSuCo");
            builder.Entity<VaiTro>().ToTable("VaiTro");
            builder.Entity<Checklist>().ToTable("Checklist");
            builder.Entity<ChecklistItem>().ToTable("ChecklistItem");
            builder.Entity<PCV_Checklist>().ToTable("PCV_Checklist");
            builder.Entity<NhatKyHeThong>().ToTable("NhatKyHeThong");
            builder.Entity<TepDinhKem>().ToTable("TepDinhKem");





        }
    }
}
