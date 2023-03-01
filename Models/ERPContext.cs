using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace WebApp.Model
{
    public class ERPContext: DbContext
    {
        public  ERPContext(DbContextOptions<ERPContext> options) : base(options) { }
        public DbSet<T_wb_Poorder> T_wb_Poorder { get; set; } = null!;
        public DbSet<T_wb_PoorderEntry> T_wb_PoorderEntry { get; set; } = null!;
    }
}
