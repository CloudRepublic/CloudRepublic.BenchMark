using CloudRepublic.BenchMark.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudRepublic.BenchMark.Persistence
{
    public partial class BenchMarkDbContext : DbContext
    {
        public BenchMarkDbContext()
        {
        }

        public BenchMarkDbContext(DbContextOptions<BenchMarkDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BenchMarkResult> BenchMarkResult { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<BenchMarkResult>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("BenchMarkResults_pk")
                    .ForSqlServerIsClustered(false);

                entity.HasIndex(e => e.CreatedAt)
                    .HasName("BenchMarkResults__CreatedAt_index");

                entity.HasIndex(e => e.Id)
                    .HasName("BenchMarkResults_Id_uindex")
                    .IsUnique();

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });
        }
    }
}
