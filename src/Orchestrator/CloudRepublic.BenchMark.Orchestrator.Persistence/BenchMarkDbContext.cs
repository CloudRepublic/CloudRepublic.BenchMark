using CloudRepublic.BenchMark.Orchestrator.Application.Interfaces;
using CloudRepublic.BenchMark.Orchestrator.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CloudRepublic.BenchMark.Orchestrator.Persistence
{
    public class BenchMarkDbContext : DbContext,IBenchMarkDbContext
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
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

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

                entity.Property(e => e.Id);

                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });
        }
    }
}
