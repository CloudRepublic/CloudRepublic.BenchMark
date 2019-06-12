using Microsoft.EntityFrameworkCore;

namespace CloudRepublic.BenchMark.Persistence
{
   public static class BenchMarkDbContextFactory
    {
        public static BenchMarkDbContext Create(string connectionString)
        {
            var optionBuilder = new DbContextOptionsBuilder<BenchMarkDbContext>();

            optionBuilder.UseSqlServer(connectionString);

            var context = new BenchMarkDbContext(optionBuilder.Options);

            return context;
        }
    }
}
