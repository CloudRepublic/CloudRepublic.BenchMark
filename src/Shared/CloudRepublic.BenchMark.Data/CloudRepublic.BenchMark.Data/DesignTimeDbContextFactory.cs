using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CloudRepublic.BenchMark.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BenchMarkDbContext>
    {
        public BenchMarkDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<BenchMarkDbContext>();
            // TODO REWRITE TO SECRET/INPUT
            builder.UseSqlServer("Server=tcp:sql-srv-benchmark.database.windows.net,1433;Initial Catalog=benchmarkdb;Persist Security Info=False;User ID=benchmarkAdmin;Password=MySuperSecurePassword123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            return new BenchMarkDbContext(builder.Options);
        }
    }
}
